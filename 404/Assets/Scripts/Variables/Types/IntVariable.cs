using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	[CreateAssetMenu(fileName = "IntVariable", menuName = "Variables/Int")]
	public class IntVariable : AVariable
	{
		[SerializeField]
		private int min;
		[SerializeField]
		private int max;

		public override bool Set(string newValue)
		{
			int i = 0;
			if (!int.TryParse(newValue, out i))
				return false;

			if (i >= min && i <= max)
				return true;
			return false;
		}
	}
}