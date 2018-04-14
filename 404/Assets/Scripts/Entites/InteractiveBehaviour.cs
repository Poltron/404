﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
	public class InteractiveBehaviour : MonoBehaviour
	{
		#region Event
		public delegate void CameraViewDelegate(InteractiveBehaviour obj, bool visible);
		private static event CameraViewDelegate OnCameraView;
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

		private void OnDestroy()
		{
			if (visualVariable)
				Destroy(visualVariable.gameObject);
		}

		public IReadOnlyCollection<EntityVariable> GetAllVariable()
		{
			return defaultVariables.AsReadOnly();
		}

		public EntityVariable GetVariable(string name)
		{
			return GetVariable(name, true);
		}

		public EntityVariable GetVariable(string name, bool force)
		{
			if (allVariables.ContainsKey(name) == false)
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

		private void OnBecameVisible()
		{
			InvokeOnCameraView(this, true);
		}

		private void OnBecameInvisible()
		{
			InvokeOnCameraView(this, false);
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
		#endregion
	}
}