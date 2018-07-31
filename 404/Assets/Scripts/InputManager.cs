using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	#region Singleton
	private static InputManager instance;

	public static InputManager GetInputManager()
	{
		return instance;
	}
	#endregion

	[SerializeField] private KeyCode moveLeftKeyCode;
	[SerializeField] private KeyCode moveRightKeyCode;
	[SerializeField] private KeyCode jumpKeyCode;
	[SerializeField] private KeyCode attackKeyCode;


	private void Awake()
	{
		instance = this;
	}

	public bool IsMovingLeftOrRight()
	{
		return Input.GetKey(moveLeftKeyCode) || Input.GetKey(moveRightKeyCode);
	}

	public bool IsMovingLeft()
	{
		return Input.GetKey(moveLeftKeyCode);
	}

	public bool IsMovingRight()
	{
		return Input.GetKey(moveRightKeyCode);
	}

	public bool IsJumping()
	{
		return Input.GetKey(jumpKeyCode);
	}

	public bool IsAttacking()
	{
		return Input.GetKey(attackKeyCode);
	}
}
