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

		private void Awake()
		{
			Init();
		}

		private void OnEnable()
		{
			Vector3 moveDirection = myRigidBody.velocity;

			moveDirection.y = height;
			myRigidBody.velocity = moveDirection;
		}

		private void Update ()
		{
			foreach (Transform trans in feets)
			{
				RaycastHit2D[] ray = Physics2D.RaycastAll(trans.position, -Vector2.up, 0.1f);

				if (ray.Any(r => r.transform.CompareTag("Platform")) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
				{
					var nextState = GetComponent<StateWalk>();
					mystateMachine.SwitchState(nextState);
				}
				else if (ray.Any(r => r.transform.CompareTag("Platform")) && (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)))
				{
					var nextState = GetComponent<StateIdle>();
					mystateMachine.SwitchState(nextState);
				}
			}
		}
	}
}
