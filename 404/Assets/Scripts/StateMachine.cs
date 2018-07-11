using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
	public enum State
	{
		Idle,
		MoveRight,
		MoveLeft,
		Jump,
		Fall
	}

	private State state;

	public State GetState()
	{
		return state;
	}
	public void SetState(State tmp)
	{
		state = tmp;
	}

	private Dictionary<State, string> allMethods;


	private void Start()
	{
		allMethods = new Dictionary<State, string>
		{
			{State.Idle, "IdleState"},
			{State.MoveRight, "MoveRightState"},
			{State.MoveLeft, "MoveLeftState"},
			{State.Jump, "JumpState"},
			{State.Fall, "FallState"}
		};

		NextState();
	}

	private void NextState()
	{
		System.Reflection.MethodInfo info = GetType().GetMethod(allMethods[state], System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		if (info != null)
			StartCoroutine((IEnumerator) info.Invoke(this, null));
	}

	private IEnumerator IdleState()
	{
		Debug.Log("Idle: Enter");
		while (state == State.Idle)
		{
			yield return 0;
		}
		Debug.Log("Idle: Exit");
		NextState();
	}
	private IEnumerator MoveRightState()
	{
		Debug.Log("MoveRight: Enter");
		while (state == State.MoveRight)
		{
			yield return 0;
		}
		Debug.Log("MoveRight: Exit");
		NextState();
	}
	private IEnumerator MoveLeftState()
	{
		Debug.Log("MoveLeft: Enter");
		while (state == State.MoveLeft)
		{
			yield return 0;
		}
		Debug.Log("MoveLeft: Exit");
		NextState();
	}
	private IEnumerator JumpState()
	{
		Debug.Log("Jump: Enter");
		while (state == State.Jump)
		{
			yield return 0;
		}
		Debug.Log("Jump: Exit");
		NextState();
	}
	private IEnumerator FallState()
	{
		Debug.Log("Fall: Enter");
		while (state == State.Fall)
		{
			yield return 0;
		}
		Debug.Log("Fall: Exit");
		NextState();
	}

}
