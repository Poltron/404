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
    private Animator myAnimator;
    private Rigidbody2D myRigidBody;


	#endregion

	private void Awake()
	{
		myTransform = transform;
		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myAnimator = gameObject.GetComponent<Animator>();
		gravity = defaulGravity;
	}

	private void Start()
	{
	}

	private void Update()
	{
		KeyUpdate();
        UpdateAnimator();
    }
	private void FixedUpdate()
	{
		Move();
		Jump();
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

    private void UpdateAnimator()
    {
        // si le joueur est sur le sol
        if (IsOnGround())
        { 
            if (myRigidBody.velocity.x < 0) // et qu'il bouge vers la gauche
            {
                if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    myAnimator.SetTrigger("isWalking");
                }

                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(myRigidBody.velocity.x > 0) // et qu'il bouge vers la droite
            {
                if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    myAnimator.SetTrigger("isWalking");
                }

                GetComponent<SpriteRenderer>().flipX = false;
            }
            else // et qu'il bouge ne bouge pas
            {
                if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    myAnimator.SetTrigger("isIdle");
                }
            }
        }
        else // si il n'est pas sur le sol
        {
            if (myRigidBody.velocity.y < 0) // et qu'il tombe
            {
                if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                {
                    myAnimator.SetTrigger("isFalling");
                }
            }
            else // et qu'il monte
            {
                if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    myAnimator.SetTrigger("isJumping");
                }
            }
        }
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
		if (isKeyJump)
		{
			if (IsOnGround())
			{
				//Debug.Log("Tata");
				Vector3 moveDirection = myRigidBody.velocity;

				moveDirection.y = JumpForce(jumpHeight, jumpTime);
				myRigidBody.velocity = moveDirection;
                myAnimator.SetTrigger("isJumping");
			}
		}
	}

	public bool IsOnGround()
	{
	//Debug.Log("Titi");
		foreach (var trans in feets)
		{
			RaycastHit2D[] ray = Physics2D.RaycastAll(trans.position, -Vector2.up, 0.2f);

			foreach (RaycastHit2D r in ray)
			{
				if (r.transform.tag == "Platform")
				{
					//Debug.Log("Toto");
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
