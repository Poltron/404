using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class InteractiveBehaviour : MonoBehaviour
	{
		[SerializeField]
		private string objName;
		public string Name { get { return objName; } }

		private void Awake()
		{
			GameObject.FindGameObjectWithTag("MainCamera");
		}

		private void OnBecameVisible()
		{

		}

		private void OnBecameInvisible()
		{

		}
	}
}