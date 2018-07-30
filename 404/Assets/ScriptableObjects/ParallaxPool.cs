using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PrincessNotFound.Parallax
{

	[CreateAssetMenu(fileName = "ParallaxPool", menuName = "Parallax/ParallaxPool", order = 1)]
	public class ParallaxPool : ScriptableObject
	{
		[System.Serializable]
		public struct Element
		{
			public string elementName;
			public Sprite img;
			public float speed;
		}

		public List<Element> elements;
	}
}