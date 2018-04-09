using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	[CreateAssetMenu(fileName = "StringVariable", menuName = "Variables/String")]
	public class StringVariable : AVariable
	{
		[SerializeField]
		protected List<string> acceptedValue;

		public override bool Set(string newValue)
		{
			if (acceptedValue.Count == 0 || Contain(newValue))
				return true;
			return false;
		}

		private bool Contain(string v)
		{
			for (int i = 0 ; i < acceptedValue.Count ; ++i)
			{
				if (string.Compare(acceptedValue[i], v, true) == 0)
					return true;
			}

			return false;
		}
	}
}