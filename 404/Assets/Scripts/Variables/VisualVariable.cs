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

		public void Init(EntityVariable reference)
		{
			variable = reference;
			reference.AddOnSetValue(UpdateVisual);
			UpdateVisual("");
		}

		private void OnDestroy()
		{
			variable.RemoveOnSetValue(UpdateVisual);
		}

		private void UpdateVisual(string value)
		{
			nameText.text = Crypt(EViewable.CryptName) ? variable.Name.Crypt() : variable.Name;
			valueText.text = Crypt(EViewable.CryptValue) ? variable.Value.Crypt() : variable.Value;
		}

		private bool Crypt(EViewable crypt)
		{
			return (variable.Viewable & crypt) == crypt;
		}
	}
}