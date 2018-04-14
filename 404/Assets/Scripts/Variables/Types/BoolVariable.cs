using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	[CreateAssetMenu(fileName = "BoolVariable", menuName = "Variables/Bool")]
	public class BoolVariable : AVariable
	{
		public override bool Set(string newValue)
		{
			bool b = false;
			return bool.TryParse(newValue, out b);
		}
	}
}