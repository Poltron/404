using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class InteractiveBehaviour : MonoBehaviour
	{
		#region Event
		public delegate void CameraViewDelegate(InteractiveBehaviour obj, bool visible);
		private static event CameraViewDelegate OnCameraView;

		public delegate void CreateEntity(InteractiveBehaviour obj, bool isCreated);
		private static event CreateEntity OnCreateEntity;
		#endregion

		[SerializeField]
		private string objName;
		public string Name => objName;

		[SerializeField]
		private Transform targetVisual;

		[SerializeField]
		private List<EntityVariable> defaultVariables;
		private Dictionary<string, EntityVariable> allVariables;

		private VisualEntityVariable visualVariable;

		private void Awake()
		{
			allVariables = new Dictionary<string, EntityVariable>();

			foreach (EntityVariable v in defaultVariables)
			{
				v.Set(v.Value);
				allVariables.Add(v.Name, v);
			}

			visualVariable = GetComponentInChildren<VisualEntityVariable>();
			visualVariable.Init(this, GetAllVariable(), targetVisual);
		}

		private void Start()
		{
			InvokeOnCreateEntity(this, true);
		}

		private void OnDestroy()
		{
			if (visualVariable)
				Destroy(visualVariable.gameObject);
			InvokeOnCreateEntity(this, false);
		}

		private void OnEnable()
		{
			visualVariable?.gameObject.SetActive(true);
		}

		private void OnDisable()
		{
			if (visualVariable)
				visualVariable.gameObject.SetActive(false);
		}

		private void OnBecameVisible()
		{
			InvokeOnCameraView(this, true);
		}

		private void OnBecameInvisible()
		{
			InvokeOnCameraView(this, false);
		}

		#region Get&Set
		public EntityVariable GetVariable(string name)
		{
			return GetVariable(name, true);
		}

		public EntityVariable GetVariable(string name, bool force)
		{
			name = name.ToUpper();
			if (allVariables.ContainsKey(name, true) == false)
				return null;
			if (allVariables[name].Viewable == EViewable.Hide)
				return null;
			return allVariables[name];
		}

		public bool SetVariable(string name, string value)
		{
			return SetVariable(name, value, true);
		}

		public bool SetVariable(string name, string value, bool force)
		{
			if (allVariables.ContainsKey(name) == false)
				return false;
			if (allVariables[name].Viewable == EViewable.Hide)
				return false;
			return allVariables[name].Set(value);
		}
		#endregion

		public IReadOnlyCollection<EntityVariable> GetAllVariable()
		{
			return defaultVariables.AsReadOnly();
		}

		#region Event
		#region OnCameraView
		public static void AddOnCameraView(CameraViewDelegate func)
		{
			OnCameraView += func;
		}

		public static void RemoveOnCameraView(CameraViewDelegate func)
		{
			OnCameraView -= func;
		}

		private static void InvokeOnCameraView(InteractiveBehaviour obj, bool visible)
		{
			OnCameraView?.Invoke(obj, visible);
		}
		#endregion
		#region OnCreateEntity
		public static void AddOnCreateEntity(CreateEntity func)
		{
			OnCreateEntity += func;
		}

		public static void RemoveOnCreateEntity(CreateEntity func)
		{
			OnCreateEntity -= func;
		}

		private static void InvokeOnCreateEntity(InteractiveBehaviour obj, bool isCreated)
		{
			OnCreateEntity?.Invoke(obj, isCreated);
		}
		#endregion

		#endregion
	}
}