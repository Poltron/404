using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region Private

	private bool isMovingRight;
	private bool isMovingLeft;
	private bool isKeyJump;
	private bool isAttack;

	private const float DefaulGravity = 9.81f;
	private float gravity;

	private Transform myTransform;
	private Animator myAnimator;
	private Rigidbody2D myRigidBody;

	#endregion

	#region Inspector

	[Header("Keys")]
	[SerializeField]
	private KeyCode keyRight;
	[SerializeField]
	private KeyCode keyLeft;
	[SerializeField]
	private KeyCode keyJump;
	[SerializeField]
	private KeyCode keyAttack;
	[Header("Jump")]
	[SerializeField]
	private float jumpHeight;
	[SerializeField]
	private float jumpTime;
	[Header("Variables")]
	[SerializeField]
	private List<Transform> feets;
	[SerializeField]
	private float speed;
	[Header("Parallax")]
	[SerializeField]
	private PrincessNotFound.Parallax.ParallaxManager parallax;

	#endregion

	private void Awake()
	{
		myTransform = transform;
		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myAnimator = gameObject.GetComponent<Animator>();
		gravity = DefaulGravity;

	}

	private void Update()
	{
        UpdateAnimator();
		KeyUpdate();
	}
	private void FixedUpdate()
	{
		Move();
		Jump();
	}

	public void KeyUpdate()
	{
		if (Input.GetKey(keyRight))
		{
			isMovingRight = true;
			parallax.SetSens(1);
		}
		if (Input.GetKey(keyLeft))
		{
			isMovingLeft = true;
			parallax.SetSens(-1);
		}
		if (Input.GetKeyDown(keyJump))
		{
			isKeyJump = true;
		}
		if (Input.GetKeyDown(keyAttack))
		{
			isAttack = true;
		}
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
				if (isAttack)
				{
					myAnimator.SetTrigger("isAttack");
				}
                else if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
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

	private void StopAttack()
	{
		isAttack = false;
	}

	public void Move()
	{
		if (isMovingLeft)
		{
			Vector3 moveDirection = myRigidBody.velocity;

			moveDirection.x = -1.0f * speed * 100f * Time.deltaTime;

			myRigidBody.velocity = moveDirection;

			isMovingLeft = false;
			parallax.SetSens(0);

			return;
		}

		if (isMovingRight)
		{
			Vector3 moveDirection = myRigidBody.velocity;

			moveDirection.x = 1.0f * speed * 100f * Time.deltaTime;

			myRigidBody.velocity = moveDirection;

			isMovingRight = false;
			parallax.SetSens(0);

			return;
		}

		myRigidBody.velocity = new Vector2(0.0f, myRigidBody.velocity.y);
	}

	public void Jump()
	{
		if (!isKeyJump)
			return;
		if (!IsOnGround())
			return;

		Vector3 moveDirection = myRigidBody.velocity;

		//Debug.Log("JE SUIS SUR LE SOL");
		moveDirection.y = JumpForce(jumpHeight, jumpTime);
		myRigidBody.velocity = moveDirection;
		myAnimator.SetTrigger("isJumping");
		isKeyJump = false;
	}

	public bool IsOnGround()
	{
		foreach (var trans in feets)
		{
			var ray = Physics2D.RaycastAll(trans.position, -Vector2.up, 0.1f);

			if (ray.Any(r => r.transform.CompareTag("Platform")))
			{
				return true;
			}
		}
		return false;
	}

	public float JumpForce(float height, float time)
	{
		gravity = CalculeGravity(height, time);
		return (4.0f * height) / time;
	}

	private static float CalculeGravity(float height, float time)
	{
		return (8.0f * height) / (time * time);
	}
}
