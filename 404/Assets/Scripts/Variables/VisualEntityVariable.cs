using System.Collections;
using System.Collections.Generic;
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
		private Camera mainCamera;
		private Transform owner;

		private ConsoleDictionnary dico;
		private ConsoleLine console;

		public void Init(InteractiveBehaviour entity)
		{
			owner = entity.transform;
			mainCamera = GameObject.FindGameObjectWithTag(Constantes.Tag.MainCamera).GetComponent<Camera>();
			objName.text = entity.Name;

			console = FindObjectOfType<ConsoleLine>();
			dico = console.GetComponent<ConsoleDictionnary>();

			InitTransform();
			InitVariable(entity);
		}

		private void Update()
		{
			transform.position = mainCamera.WorldToScreenPoint(owner.position);
		}

		private void InitTransform()
		{
			Transform content = GameObject.FindGameObjectWithTag(Constantes.Tag.ViewerVariable).transform;
			transform.SetParent(content);
			transform.localScale = Vector3.one;
			transform.localEulerAngles = Vector3.zero;
		}

		private void InitVariable(InteractiveBehaviour entity)
		{
			allVariables = new List<VisualVariable>();

			foreach (EntityVariable variable in entity.GetAllVariable())
			{
				if (variable.Viewable == EViewable.Hide)
					continue;
				VisualVariable viewVariable = Instantiate(prefabVisualVariable, contentVariable);
				viewVariable.Init(variable);
				allVariables.Add(viewVariable);
			}
		}
	}
}