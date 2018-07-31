using UnityEngine;

namespace StateMachine
{
	public class StateWalk : State
	{
		[SerializeField] private KeyCode MoveRight;
		[SerializeField] private KeyCode MoveLeft;
		[SerializeField] private KeyCode Jump;

		[SerializeField] private float speed;

		private bool isMovingRight;
		private bool isMovingLeft;

		private void Awake()
		{
			Init();
			
		}

		private void OnEnable()
		{
			if (Input.GetKey(MoveRight))
			{
				isMovingRight = true;
			}
			else if (Input.GetKey(MoveLeft))
			{
				isMovingLeft = true;
			}
		}

		private void Update ()
		{
			CheckInput();
		}

		private void FixedUpdate()
		{
			Move();
		}

		protected override void CheckInput()
		{
			if (Input.GetKey(MoveRight))
			{
				isMovingRight = true;
				isMovingLeft = false;
			}
			else if (Input.GetKey(MoveLeft))
			{
				isMovingLeft = true;
				isMovingRight = false;
			}

			if (Input.GetKeyUp(MoveRight) || Input.GetKeyUp(MoveLeft))
			{
				if (Input.GetKey(MoveRight) || Input.GetKey(MoveLeft))
					return;

				var nextState = GetComponent<StateIdle>();
				mystateMachine.SwitchState(nextState);
			}
			else if (Input.GetKeyDown(Jump))
			{
				Debug.Log("HAUT");
				var nextState = GetComponent<StateJump>();
				mystateMachine.SwitchState(nextState);
			}
		}

		private void Move()
		{
			if (isMovingLeft)
			{
				Vector3 moveDirection = myRigidBody.velocity;

				moveDirection.x = -1.0f * speed * 100f * Time.deltaTime;

				myRigidBody.velocity = moveDirection;

				isMovingLeft = false;

			}
			else if (isMovingRight)
			{
				Vector3 moveDirection = myRigidBody.velocity;

				moveDirection.x = 1.0f * speed * 100f * Time.deltaTime;

				myRigidBody.velocity = moveDirection;

				isMovingRight = false;
			}
		}
	}
}
