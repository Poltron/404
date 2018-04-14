using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public static class ExtensionMethode
	{
		public static string Crypt(this string s)
		{
			string crypted = "";
			string value = "&#$£!?%@";
			int nbrValue = value.Length;

			for (int i = 0 ; i < s.Length ; ++i)
			{
				crypted += value[Random.Range(0, nbrValue)];
			}

			return crypted;
		}
	}
}