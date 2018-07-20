using UnityEngine;

namespace StateMachine
{
	public abstract class State : MonoBehaviour
	{
		protected StateMachine stateMachine;
		protected Rigidbody2D myRigidBody;
		[SerializeField]
		protected string stateName;

		protected void Init()
		{
			stateMachine = GetComponent<StateMachine>();
			myRigidBody = GetComponent<Rigidbody2D>();
		}

		protected virtual void CheckInput()
		{
		}
	}
}
