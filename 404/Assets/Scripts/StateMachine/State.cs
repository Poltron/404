using UnityEngine;

namespace StateMachine
{
	public abstract class State : MonoBehaviour
	{
		protected StateMachine mystateMachine;
		protected Rigidbody2D myRigidBody;
		protected InputManager myInputManager;
		[SerializeField]
		protected string stateName;

		protected void Init()
		{
			mystateMachine = GetComponent<StateMachine>();
			myRigidBody = GetComponent<Rigidbody2D>();
			myInputManager = GetComponent<InputManager>();
		}

		protected virtual void CheckInput()
		{
		}
	}
}
