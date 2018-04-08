using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	[System.Serializable]
	public class EntityVariable
	{
		[SerializeField]
		private string value;
		[SerializeField]
		private AVariable checker;
		[SerializeField]
		private bool viewable;

		public string Value => value;
		public string Name => checker.VisualName;

		public void Set(string newValue)
		{
			newValue = newValue.ToUpper();
			if (checker.Set(newValue))
			{
				value = newValue;
				InvokeOnSetValue(value);
			}
		}

		#region Event
		public delegate void SetValueDelegate(string value);
		private event SetValueDelegate OnSetValue;

		public void AddOnSetValue(SetValueDelegate func)
		{
			OnSetValue += func;
		}

		public void RemoveOnSetValue(SetValueDelegate func)
		{
			OnSetValue -= func;
		}

		private void ResetOnSetValue()
		{
			OnSetValue = null;
		}

		private void InvokeOnSetValue(string value)
		{
			OnSetValue?.Invoke(value);
		}
		#endregion
	}
}