using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class LevelContainer : MonoBehaviour
	{
		private HashSet<InteractiveBehaviour> allEntities;
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
			allEntities = new HashSet<InteractiveBehaviour>();
			InteractiveBehaviour.AddOnCreateEntity(UpdateEntities);
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{
			InteractiveBehaviour.RemoveOnCreateEntity(UpdateEntities);
		}

		private void UpdateEntities(InteractiveBehaviour obj, bool isCreated)
		{
			if (isCreated)
				allEntities.Add(obj);
			else
				allEntities.Remove(obj);
		}

		public IReadOnlyCollection<InteractiveBehaviour> GetAllEntities()
		{
			return allEntities.ToList().AsReadOnly();
		}
	}
}