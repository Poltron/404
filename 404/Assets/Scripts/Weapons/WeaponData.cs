using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData")]
	public class WeaponData : ScriptableObject
	{
		[SerializeField]
		protected string visualName;
		[SerializeField]
		protected int damage;
		[SerializeField]
		protected float range;
		[SerializeField]
		protected AudioClip sound;

		public string VisualName => visualName;
		public int Damage => damage;
		public float Range => range;
		public AudioClip Sound => sound;

		private void OnValidate()
		{
			visualName = visualName.ToUpper();
		}
	}
}
