using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adventure
{
	public class VisualVariable : MonoBehaviour
	{
		[SerializeField]
		private Text nameText;
		[SerializeField]
		private Text valueText;
		private EntityVariable variable;

		private string variableName;
		private string variableValue;

		private string nameColor;
		private string valueColor;

		public void Init(EntityVariable reference)
		{
			variable = reference;
			reference.AddOnSetValue(UpdateText);
			nameColor = ColorUtility.ToHtmlStringRGBA(Color.white);
			valueColor = ColorUtility.ToHtmlStringRGBA(Color.white);
			ResetColor();
			UpdateText("");
		}

		private void OnDestroy()
		{
			variable.RemoveOnSetValue(UpdateText);
		}

		private void UpdateText(string value)
		{
			variableName = Crypt(EViewable.CryptName) ? variable.Name.Crypt() : variable.Name;
			variableValue = Crypt(EViewable.CryptValue) ? variable.Value.Crypt() : variable.Value;
			UpdateVisual();
		}

		private void UpdateVisual()
		{
			nameText.text = "<color=#" + nameColor + ">" + variableName + "</color>\n";
			valueText.text = "<color=#" + valueColor + ">" + variableValue + "</color>\n";
		}

		public void WriteCommand(ConsoleDictionnary.Command cmd)
		{
			nameColor = ColorUtility.ToHtmlStringRGBA(Color.white);
			valueColor = ColorUtility.ToHtmlStringRGBA(Color.white);

			foreach (ConsoleDictionnary.Command link in cmd.linkCommand)
			{
				if (!link.discover)
					continue;

				if (string.Compare(link.value, variableName, true) == 0)
				{
					nameColor = ColorUtility.ToHtmlStringRGBA(cmd.color);
				}
				if (string.Compare(link.value, variableValue, true) == 0)
				{
					valueColor = ColorUtility.ToHtmlStringRGBA(cmd.color);
				}
			}
			UpdateVisual();
		}

		public void ResetColor()
		{
			nameColor = ColorUtility.ToHtmlStringRGBA(Color.white);
			valueColor = ColorUtility.ToHtmlStringRGBA(Color.white);
			UpdateVisual();
		}

		private bool Crypt(EViewable crypt)
		{
			return (variable.Viewable & crypt) == crypt;
		}
	}
}