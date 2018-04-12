using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public enum EViewable
	{
		Show = 1 << 0,
		CryptName = 1 << 1,
		CryptValue = 1 << 2,
		Hide = 1 << 3,

		FullCrypt = CryptName | CryptValue
	}

	[System.Serializable]
	public class EntityVariable
	{
		[SerializeField]
		private string value;
		[SerializeField]
		private AVariable reference;
		[SerializeField]
		private EViewable viewable;

		public string Value => value;
		public string Name => reference.VisualName;
		public string TrueName => reference.name;
		public EViewable Viewable => viewable;

		public bool Set(string newValue)
		{
			newValue = newValue.ToUpper();
			if (reference.Set(newValue))
			{
				value = newValue;
				InvokeOnSetValue(value);
				return true;
			}
			return false;
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