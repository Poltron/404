using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public abstract class AVariable : ScriptableObject
	{
		[SerializeField]
		protected string visualName;

		public string VisualName => visualName;

		public abstract bool Set(string newValue);
	}
}
