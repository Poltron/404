﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adventure
{
	public class VisualEntityVariable : MonoBehaviour
	{
		[SerializeField]
		private Text objName;
		[SerializeField]
		private RectTransform contentVariable;
		[SerializeField]
		private VisualVariable prefabVisualVariable;

		private List<VisualVariable> allVariables;

		[SerializeField]
		private Camera mainCamera;
		private InteractiveBehaviour owner;
		private Transform target;

		[SerializeField]
		private bool hideName;
		[SerializeField, Range(0.0f, 60.0f)]
		private float timeToCrypt;

		private ConsoleDictionnary dico;
		private ConsoleLine console;

		public void Init(InteractiveBehaviour entity, IReadOnlyCollection<EntityVariable> variables, Transform target)
		{
			owner = entity;
			this.target = target;

			if (hideName)
			{
				if (timeToCrypt > 0.0f)
					InvokeRepeating("CryptName", 0.0f, timeToCrypt);
				else
					objName.text = "<color=#ffffffff>" + owner.Name.Crypt() + "</color>";
			}
			else
				objName.text = "<color=#ffffffff>" + owner.Name + "</color>";

			console = FindObjectOfType<ConsoleLine>();
			dico = console.GetComponent<ConsoleDictionnary>();

			// temporary fix to add objects without having a ref to the main camera
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}

			console.AddOnWriteCommand(SetColor);
			console.AddOnActiveCommande(OnActiveConsole);

			InitTransform();
			InitVariable(variables);
		}

		private void OnDestroy()
		{
			console.RemoveOnWriteCommand(SetColor);
			console.RemoveOnActiveCommande(OnActiveConsole);
		}

		private void Update()
		{
			transform.position = mainCamera.WorldToScreenPoint(target.position);
		}

		private void CryptName()
		{
			objName.text = "<color=#ffffffff>" + owner.Name.Crypt() + "</color>";
		}

		private void InitTransform()
		{
			Transform content = GameObject.FindGameObjectWithTag(Constantes.Tag.ViewerVariable).transform;
			transform.SetParent(content);
			transform.localScale = Vector3.one;
			transform.localEulerAngles = Vector3.zero;
		}

		private void InitVariable(IReadOnlyCollection<EntityVariable> variables)
		{
			allVariables = new List<VisualVariable>();

			foreach (EntityVariable variable in variables)
			{
				if (variable.Viewable == EViewable.Hide)
					continue;
				VisualVariable viewVariable = Instantiate(prefabVisualVariable, contentVariable);
				viewVariable.Init(variable);
				allVariables.Add(viewVariable);
			}
		}

		public ConsoleLine.ECommandResult SetColor(string[] words)
		{
			int depth;
			ConsoleDictionnary.Command command = dico.FindLink(words, out depth);
			if (!command.discover || depth < words.Length)
			{
				foreach (VisualVariable visual in allVariables)
					visual.ResetColor();
				if (!hideName)
					objName.text = "<color=#ffffffff>" + owner.Name + "</color>";
				return ConsoleLine.ECommandResult.Failed;
			}

			ConsoleDictionnary.Command t = command.FindValue(owner.Name, 1);
			if (!hideName)
			{
				if (t == null || !t.discover)
					objName.text = "<color=#ffffffff>" + owner.Name + "</color>";
				else
					objName.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(command.color) + ">" + owner.Name + "</color>";
			}

			foreach (VisualVariable visual in allVariables)
			{
				visual.WriteCommand(command);
			}

			return ConsoleLine.ECommandResult.Successed;
		}

		public void OnActiveConsole(bool b)
		{
			foreach (VisualVariable visual in allVariables)
				visual.ResetColor();
			if (!hideName)
				objName.text = "<color=#ffffffff>" + owner.Name + "</color>";
		}
	}
}