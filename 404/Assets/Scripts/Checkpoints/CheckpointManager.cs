using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class CheckpointManager : MonoBehaviour
	{
		public delegate void RespawnPlayerDelegate();
		private event RespawnPlayerDelegate OnRespawnPlayer;

		[SerializeField]
		private List<Checkpoint> allCheckpoints;
		private Checkpoint currentCheckpoint;

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			if (allCheckpoints.Count == 0)
				yield break;
			currentCheckpoint = allCheckpoints[0];
			currentCheckpoint.Init();
		}

		public void OnTriggerCheckpoint(Checkpoint newCheckpoint)
		{
			currentCheckpoint.DisableCheckpoint();
			currentCheckpoint = newCheckpoint;
			newCheckpoint.Init();
		}

		#region Events
		#region OnRespawnPlayer
		public void AddOnRespawnPlayer(RespawnPlayerDelegate func)
		{
			OnRespawnPlayer += func;
		}

		public void RemoveOnRespawnPlayer(RespawnPlayerDelegate func)
		{
			OnRespawnPlayer -= func;
		}

		private void InvokeOnRespawnPlayer()
		{
			OnRespawnPlayer?.Invoke();
		}
		#endregion
		#endregion
	}
}