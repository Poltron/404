using UnityEngine;

namespace StateMachine
{
	public class StateIdle : State
	{
		[SerializeField] private KeyCode MoveRight;
		[SerializeField] private KeyCode MoveLeft;
		[SerializeField] private KeyCode Jump;

		private void Awake ()
		{
			Init();
			mystateMachine.SwitchState(this);
		}

		private void OnEnable()
		{
			myRigidBody.velocity = new Vector2(0.0f, myRigidBody.velocity.y);
		}

		private void Update ()
		{
			CheckInput();
		}

		protected override void CheckInput()
		{
			if (Input.GetKey(MoveRight) || Input.GetKey(MoveLeft))
			{
				var nextState = GetComponent<StateWalk>();
				mystateMachine.SwitchState(nextState);
			}
			else if (Input.GetKey(Jump))
			{
				var nextState = GetComponent<StateJump>();
				mystateMachine.SwitchState(nextState);
			}
		}
	}
}
