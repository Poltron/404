using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrincessNotFound.Parallax
{
	public class ParallaxManager : MonoBehaviour
	{
		[SerializeField]
		private Transform player;

		[SerializeField]
		private List<ParallaxPool> pools;
		[SerializeField]
		private ParallaxPool currentPool;
		public ParallaxPool CurrentPool { get { return currentPool; } }

		[SerializeField]
		private ParallaxLayer prefabLayer;

		[SerializeField]
		private List<ParallaxLayer> layers;
		[SerializeField]
		private List<ParallaxLayer> layersOld;

		[SerializeField]
		private int sens;

		private void Start()
		{
			if (!Init())
				Debug.LogError("Error : ParallaxManager can't be init.");
		}

		public bool Init()
		{
			if (pools.Count == 0)
				return false;

			currentPool = pools[0];
			SwitchPool(currentPool);
			return enabled = true;
		}

		public void SetSens(int newSens)
		{
			sens = newSens;
		}

		private void Update()
		{
			//Debug
			/*if (Input.GetKeyDown(KeyCode.LeftArrow))
				sens = -1;
			if (Input.GetKeyDown(KeyCode.RightArrow))
				sens = 1;
			if (Input.GetKeyUp(KeyCode.LeftArrow))
				sens = 0;
			if (Input.GetKeyUp(KeyCode.RightArrow))
				sens = 0;*/

			foreach (ParallaxLayer layer in layers)
			{
				layer.UpdateBehaviour(sens);
			}

			for (int i = 0; i < layersOld.Count; i++)
			{
				layersOld[i].UpdateBehaviour(sens);
			}
		}

		public void CallBackDisapear(ParallaxLayer layer)
		{
			layersOld.Remove(layer);
			Destroy(layer.gameObject);
		}

		public void SwitchPool(ParallaxPool pool)
		{
			currentPool = pool;

			foreach (ParallaxLayer layer in layers)
			{
				layersOld.Add(layer);
				layer.Disapear();
			}

			layers.Clear();

			int order = pool.elements.Count;

			foreach (ParallaxPool.Element element in pool.elements)
			{
				ParallaxLayer layer = Instantiate(prefabLayer, player) as ParallaxLayer;
				layer.GetComponent<SpriteRenderer>().sortingOrder = order--;
				layer.Init(element, this);
				layers.Add(layer);
			}
		}
	}
}