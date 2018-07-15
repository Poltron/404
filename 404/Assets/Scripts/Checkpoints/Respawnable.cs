using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	[RequireComponent(typeof(InteractiveBehaviour))]
	public class Respawnable : MonoBehaviour
	{
		[System.Serializable]
		private class SaveValue
		{
			public string name;
			public string value;
		}

		[SerializeField]
		private List<SaveValue> variablesToSave;
		private InteractiveBehaviour interactiveVariable;

		private Vector3 position;
		private Quaternion rotation;

		private void Awake()
		{
			interactiveVariable = gameObject.GetComponent<InteractiveBehaviour>();
		}

		public void SaveCheckpoint()
		{
			foreach (SaveValue v in variablesToSave)
			{
				switch (v.name)
				{
					case "ACTIVE":
						v.value = gameObject.activeInHierarchy.ToString();
						break;
					case "POSITION":
						v.value = transform.position.ToString();
						break;
					case "ROTATION":
						v.value = transform.rotation.ToString();
						break;
					default:
						EntityVariable entVariable = interactiveVariable.GetVariable(v.name, true);
						if (entVariable == null)
						{
							Debug.LogWarning("Try to save a wrong variable (" + v + ") in gameObject : " + name, gameObject);
							continue;
						}
						v.value = entVariable.Value;
						break;
				}
			}
		}

		public void LoadCheckpoint()
		{
			foreach (SaveValue v in variablesToSave)
			{
				switch (v.name)
				{
					case "ACTIVE":
						gameObject.SetActive(bool.Parse(v.value));
						break;
					case "POSITION":
						transform.position = ToVector3(v.value);
						break;
					case "ROTATION":
						transform.rotation = ToQuaternion(v.value);
						break;
					default:
						EntityVariable entVariable = interactiveVariable.GetVariable(v.name, true);
						if (entVariable == null)
						{
							Debug.LogWarning("Try to load a wrong variable (" + v + ") in gameObject : " + name, gameObject);
							continue;
						}
						entVariable.Set(v.value);
						break;
				}
			}
		}

		private Vector3 ToVector3(string value)
		{
			Vector3 result = Vector3.zero;
			int count = 0;
			int index = 1;
			bool coma = false;

			for (int i = index ; i < value.Length ; ++i)
			{
				if (value[i] == ',' || value[i] == ')')
				{
					if (coma)
					{
						switch (count)
						{
							case 0:
								result.x = float.Parse(value.Substring(index, i - index));
								break;
							case 1:
								result.y = float.Parse(value.Substring(index, i - index));
								break;
							case 2:
								result.z = float.Parse(value.Substring(index, i - index));
								break;
						}
						++count;
						index = i + 1;
					}
					coma = !coma;
				}
			}

			return result;
		}

		private Quaternion ToQuaternion(string value)
		{
			Quaternion result = Quaternion.identity;
			int count = 0;
			int index = 1;
			bool coma = false;

			for (int i = index ; i < value.Length ; ++i)
			{
				if (value[i] == ',' || value[i] == ')')
				{
					if (coma)
					{
						switch (count)
						{
							case 0:
								result.x = float.Parse(value.Substring(index, i - index));
								break;
							case 1:
								result.y = float.Parse(value.Substring(index, i - index));
								break;
							case 2:
								result.z = float.Parse(value.Substring(index, i - index));
								break;
							case 3:
								result.w = float.Parse(value.Substring(index, i - index));
								break;
						}
						++count;
						index = i + 1;
					}
					coma = !coma;
				}
			}

			return result;
		}
	}
}