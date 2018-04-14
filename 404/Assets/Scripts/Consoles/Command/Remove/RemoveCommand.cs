using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class RemoveCommand : MonoBehaviour
	{
		private InteractiveView cameraView;
		private ConsoleLine console;

		void Start()
		{
			console = GetComponentInParent<ConsoleLine>();
			console.AddOnSendCommand(Constantes.Command.Remove, Do);

			cameraView = GameObject.FindGameObjectWithTag(Constantes.Tag.MainCamera).GetComponent<InteractiveView>();
		}

		private void OnDestroy()
		{
			console.RemoveOnSendCommand(Constantes.Command.Remove, Do);
		}

		private ConsoleLine.ECommandResult Do(string[] words)
		{
			if (words.Length != 1)
			{
				return ConsoleLine.ECommandResult.Failed;
			}
			string entityName = words[0];
			ConsoleLine.ECommandResult result = ConsoleLine.ECommandResult.Failed;
			List<GameObject> toDestroy = new List<GameObject>();

			foreach (InteractiveBehaviour obj in cameraView.GetAllObjectInView())
			{
				if (string.Compare(obj.Name, entityName, true) == 0)
					toDestroy.Add(obj.gameObject);
			}

			while (toDestroy.Count > 0)
			{
				GameObject objToDestroy = toDestroy[0];
				toDestroy.RemoveAt(0);
				Destroy(objToDestroy);
			}

			return result;
		}
	}
}