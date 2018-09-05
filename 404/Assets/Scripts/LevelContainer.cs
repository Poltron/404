using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class LevelContainer : MonoBehaviour
	{
		private HashSet<IRespawnable> allEntities;
		private GameObject player;
		private Camera cam;

		public GameObject Player
		{
			get
			{
				if (!player)
					player = GameObject.FindGameObjectWithTag(Constantes.Tag.Player);
				return player;
			}
		}
		public Camera Cam
		{
			get
			{
				if (!cam)
					cam = GameObject.FindGameObjectWithTag(Constantes.Tag.MainCamera)?.GetComponent<Camera>();
				return cam;
			}
		}

		private void Awake()
		{
			allEntities = new HashSet<IRespawnable>();
			InteractiveBehaviour.AddOnCreateEntity(UpdateEntities);
		}

		private void Start()
		{
			var at = FindObjectsOfType<AudioTrigger>();
			foreach (var t in at)
			{
				allEntities.Add(t);
			}
		}

		private void OnDestroy()
		{
			InteractiveBehaviour.RemoveOnCreateEntity(UpdateEntities);
		}

		private void UpdateEntities(InteractiveBehaviour obj, bool isCreated)
		{
			IRespawnable[] respawnablesComp = obj.GetComponentsInChildren<IRespawnable>();

			foreach (IRespawnable comp in respawnablesComp)
			{
				if (isCreated)
					allEntities.Add(comp);
				else
					allEntities.Remove(comp);
			}
		}

		public IReadOnlyCollection<IRespawnable> GetAllEntities()
		{
			return allEntities.ToList().AsReadOnly();
		}
	}
}