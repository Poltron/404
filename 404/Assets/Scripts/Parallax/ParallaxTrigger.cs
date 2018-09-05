using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrincessNotFound.Parallax
{
	public class ParallaxTrigger : MonoBehaviour
	{
		[SerializeField]
		private ParallaxManager man;

		[SerializeField]
		private ParallaxPool pool;

		private void Start()
		{
			Init();
		}

		public bool Init()
		{
			if (man == null)
				man = FindObjectOfType<ParallaxManager>();
			return true;
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (man.CurrentPool != pool)
				man.SwitchPool(pool);
		}
	}
}