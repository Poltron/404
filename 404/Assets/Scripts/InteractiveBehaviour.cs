using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class InteractiveBehaviour : MonoBehaviour
	{
		[SerializeField]
		private string objName;
		public string Name => objName;

		[SerializeField]
		private List<EntityVariable> variables;

		private void Awake()
		{
			GameObject.FindGameObjectWithTag("MainCamera");

			foreach (EntityVariable v in variables)
			{
				v.Set(v.Value);
			}

			GetComponentInChildren<VisualEntityVariable>().Init(this);
		}

		public List<EntityVariable> GetAllVariable()
		{
			return variables;
		}

		private void OnBecameVisible()
		{

		}

		private void OnBecameInvisible()
		{

		}
	}
}