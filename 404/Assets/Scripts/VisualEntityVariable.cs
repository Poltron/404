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
		private List<Text> allVariables;
		private Camera mainCamera;
		private Transform owner;

		public void Init(InteractiveBehaviour entity)
		{
			owner = entity.transform;
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			objName.text = entity.Name;

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
			allVariables = new List<Text>();

			foreach (EntityVariable variable in entity.GetAllVariable())
			{
				Text viewVariable = Instantiate(objName, contentVariable);
				viewVariable.resizeTextForBestFit = false;
				viewVariable.alignment = TextAnchor.MiddleLeft;
				Destroy(viewVariable.GetComponent<LayoutElement>());

				variable.AddOnSetValue(delegate {
					UpdateVariable(viewVariable, variable);
				});

				viewVariable.text = variable.Name + " : " + variable.Value;
				allVariables.Add(viewVariable);
			}
		}

		private void UpdateVariable(Text textView, EntityVariable variable)
		{
			textView.text = variable.Name + " : " + variable.Value;
		}
	}
}