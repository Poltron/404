using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class Checkpoint : MonoBehaviour
	{
		private CheckpointManager manager;
		private List<IRespawnable> associatedRespawnable;
		private InteractiveBehaviour player;
		private LevelContainer levelContainer;

		private bool active;

		private void Awake()
		{
			levelContainer = FindObjectOfType<LevelContainer>();
			manager = GetComponentInParent<CheckpointManager>();
			associatedRespawnable = new List<IRespawnable>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag != Constantes.Tag.Player || active)
				return;
			manager.OnTriggerCheckpoint(this);
		}

		public void Init()
		{
			if (active)
				return;

			associatedRespawnable.Clear();
			foreach (var obj in levelContainer.GetAllEntities())
			{
				associatedRespawnable.Add(obj);
			}

			player = levelContainer.Player?.GetComponent<InteractiveBehaviour>();
			if (player)
				player.GetVariable("LIFE").AddOnSetValue(RespawnPlayer);
			else
				Debug.LogError("There is no player");

			foreach (IRespawnable obj in associatedRespawnable)
			{
				obj.SaveCheckpoint();
			}
			active = true;
		}

		public void DisableCheckpoint()
		{
			if (player)
				player.GetVariable("LIFE").RemoveOnSetValue(RespawnPlayer);
			active = false;
		}

		private void RespawnPlayer(string value)
		{
			int life;
			if (!int.TryParse(value, out life))
				return;
			if (life == 0)
				StartCoroutine(RespawnPlayer());
		}

		private IEnumerator RespawnPlayer()
		{
			foreach (IRespawnable obj in associatedRespawnable)
			{
				obj.LoadCheckpoint();
			}
			yield return null;

			foreach (IRespawnable obj in levelContainer.GetAllEntities())
			{
				if (!associatedRespawnable.Contains(obj))
				{
					(obj as Respawnable)?.gameObject.SetActive(false);
				}
			}

			yield return null;

			player.transform.position = transform.position;
			player.GetVariable("LIFE").Set("10");
		}
	}
}