using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrincessNotFound.Parallax
{
	public class ParallaxLayer : MonoBehaviour
	{
		public enum EState { In, Out, Full }

		[SerializeField]
		private ParallaxManager man;

		[SerializeField]
		private float appearSpeed;
		[SerializeField]
		private float speed;
		[SerializeField]
		private float tileSizeZ;

		[SerializeField]
		private SpriteRenderer sprite1;
		[SerializeField]
		private SpriteRenderer sprite2;
		[SerializeField]
		private SpriteRenderer sprite3;

		private Vector3 startPosition;
		private EState state;

		public void Init(ParallaxPool.Element element, ParallaxManager manager)
		{
			man = manager;

			startPosition = transform.position;
			state = EState.In;

			sprite1.sprite = element.img;
			sprite2.sprite = element.img;
			sprite3.sprite = element.img;
			speed = element.speed;
		}

		public void Disapear()
		{
			state = EState.Out;
		}

		public void UpdateBehaviour(int sens)
		{
			if (sens != 0)
			{
				float x = Mathf.Repeat(transform.localPosition.x + (-sens * Time.deltaTime * speed * 0.5f), 19);
				transform.localPosition = new Vector3(x, 0, 0);
			}

			switch (state)
			{
				case EState.In:
					sprite1.color += new Color(0, 0, 0, appearSpeed * Time.deltaTime);
					sprite2.color += new Color(0, 0, 0, appearSpeed * Time.deltaTime);
					sprite3.color += new Color(0, 0, 0, appearSpeed * Time.deltaTime);
					if (sprite3.color.a >= 1.0f)
						state = EState.Full;
					break;
				case EState.Out:
					sprite1.color -= new Color(0, 0, 0, appearSpeed * Time.deltaTime);
					sprite2.color -= new Color(0, 0, 0, appearSpeed * Time.deltaTime);
					sprite3.color -= new Color(0, 0, 0, appearSpeed * Time.deltaTime);
					if (sprite3.color.a <= 0.0f)
						man.CallBackDisapear(this);
					break;
			}
		}
	}
}