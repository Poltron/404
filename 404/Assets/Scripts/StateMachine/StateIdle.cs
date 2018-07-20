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
				Debug.Log("GAUCHE OU DROITE");
				var nextState = GetComponent<StateWalk>();
				stateMachine.SwitchState(nextState);
			}
			else if (Input.GetKey(Jump))
			{
				Debug.Log("HAUT");
				var nextState = GetComponent<StateJump>();
				stateMachine.SwitchState(nextState);
			}
		}
	}
}
