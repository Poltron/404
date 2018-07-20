using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{
	public class StateJump : State
	{
		[SerializeField] private float height;
		[SerializeField]
		private List<Transform> feets;

		private float veloX;

		private void Awake()
		{
			Init();
		}

		private void OnEnable()
		{
			veloX = myRigidBody.velocity.x;

			Vector3 moveDirection = myRigidBody.velocity;

			moveDirection.y = height;
			myRigidBody.velocity = moveDirection;
		}

		private void Update ()
		{
			foreach (Transform trans in feets)
			{
				RaycastHit2D[] ray = Physics2D.RaycastAll(trans.position, -Vector2.up, 0.1f);

				if (ray.Any(r => r.transform.CompareTag("Platform")) && Input.GetKey(Key))
				{
					var nextState = GetComponent<StateWalk>();
					stateMachine.SwitchState(nextState);
				}
				else if (ray.Any(r => r.transform.CompareTag("Platform")) && )
				{
					var nextState = GetComponent<StateIdle>();
					stateMachine.SwitchState(nextState);
				}
			}
		}

		private void FixedUpdate()
		{
			Jump();
		}

		public void Jump()
		{
			
		}
	}
}
