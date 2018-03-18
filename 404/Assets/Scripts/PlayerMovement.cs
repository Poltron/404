using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region Private
	#region isMovingRight
	private bool isMovingRight = false;

	public bool GetIsMovingRight()
	{
		return isMovingRight;
	}
	#endregion
	#region isMovingLeft
	private bool isMovingLeft = false;

	public bool GetIsMovingLeft()
	{
		return isMovingLeft;
	}
	#endregion
	#region isJumping
	private bool isKeyJump = false;

	public bool GetIsJumping()
	{
		return isKeyJump;
	}
	#endregion
	#region Inspector
	#region Keys
	[Header("Keys")]
	[SerializeField]
	private KeyCode keyRight;
	[SerializeField]
	private KeyCode keyLeft;
	[SerializeField]
	private KeyCode keyJump;
	#endregion
	#region Jump
	[Header("Jump")]
	[SerializeField]
	private float jumpHeight;
	[SerializeField]
	private float jumpTime;
	#endregion
	[Header("Variables")]
	[SerializeField]
	private List<Transform> feets;
	[SerializeField]
	private float speed;
	#endregion

	private float defaulGravity = 9.81f;
	private float gravity;

	private Transform myTransform;
	private Rigidbody2D myRigidBody;


	#endregion

	private void Awake()
	{
		myTransform = transform;
		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
		gravity = defaulGravity;
	}

	private void Start()
	{
	}

	private void Update()
	{
		KeyUpdate();
	}
	private void FixedUpdate()
	{
		Move();
	}

	public void KeyUpdate()
	{
		if (Input.GetKey(keyRight))
			isMovingRight = true;
		else
			isMovingRight = false;

		if (Input.GetKey(keyLeft))
			isMovingLeft = true;
		else
			isMovingLeft = false;

		if (Input.GetKeyDown(keyJump))
			isKeyJump = true;
		else
			isKeyJump = false;
	}

	public void Move()
	{
		if (isMovingLeft)
		{
			Vector3 moveDirection = myRigidBody.velocity;

			moveDirection.x = -1.0f * speed * 100f * Time.deltaTime;

			myRigidBody.velocity = moveDirection;

			return;
		}
		else if (isMovingRight)
		{
			Vector3 moveDirection = myRigidBody.velocity;

			moveDirection.x = 1.0f * speed * 100f * Time.deltaTime;

			myRigidBody.velocity = moveDirection;

			return;
		}
		else
		{
			myRigidBody.velocity = new Vector2(0.0f, myRigidBody.velocity.y);
		}
	}

	public void Jump()
	{
		if (IsOnGround())
		{
			if (isKeyJump)
			{
				Vector3 moveDirection = myRigidBody.velocity;

				moveDirection.y = JumpForce(jumpHeight, jumpTime);
				myRigidBody.velocity = moveDirection;
			}
		}
	}

	public bool IsOnGround()
	{
		foreach (var trans in feets)
		{
			RaycastHit2D[] ray = Physics2D.RaycastAll(trans.position, -Vector2.up, 0.2f);

			foreach (RaycastHit2D r in ray)
			{
				if (r.transform.tag == "Platform")
				{
					return true;
				}
			}
		}
		return false;
	}

	public float JumpForce(float height, float time)
	{
		gravity = CalculeGravity(height, time);
		return (4.0f * height) / time;
	}

	private float CalculeGravity(float height, float time)
	{
		return (8.0f * height) / (time * time);
	}
}
