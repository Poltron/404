using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class SetCommand : MonoBehaviour
	{
		private InteractiveView cameraView;
		private ConsoleLine console;

		private void Start()
		{
			console = GetComponentInParent<ConsoleLine>();
			console.AddOnSendCommand(Constantes.Command.Set, Do);

			cameraView = GameObject.FindGameObjectWithTag(Constantes.Tag.MainCamera).GetComponent<InteractiveView>();

		}

		private void OnDestroy()
		{
			console.RemoveOnSendCommand(Constantes.Command.Set, Do);
		}

		private ConsoleLine.ECommandResult Do(string[] words)
		{
			if (words.Length != 2)
			{
				return ConsoleLine.ECommandResult.Failed;
			}
			string variableName = words[0];
			string variableValue = words[1];
			ConsoleLine.ECommandResult result = ConsoleLine.ECommandResult.Failed;

			foreach (InteractiveBehaviour obj in cameraView.GetAllObjectInView())
			{
				if (obj.SetVariable(variableName, variableValue, false))
					result = ConsoleLine.ECommandResult.Successed;
			}

			return result;
		}
	}
}