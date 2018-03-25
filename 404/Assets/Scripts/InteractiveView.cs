using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class InteractiveView : MonoBehaviour
	{
		private List<InteractiveBehaviour> objectView;

		private void Awake()
		{
			objectView = new List<InteractiveBehaviour>();
		}

		private void UpdateObjectInView(InteractiveBehaviour obj, bool inView)
		{
			if (inView && objectView.Contains(obj) == false)
				objectView.Add(obj);
			else if (!inView && objectView.Contains(obj))
				objectView.Remove(obj);
		}
	}
}