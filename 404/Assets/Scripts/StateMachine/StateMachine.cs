using UnityEngine;

namespace StateMachine
{
	public class StateMachine : MonoBehaviour
	{
		private State state;

		public State GetState()
		{
			return state;
		}

		public void SwitchState(State nextState)
		{
			if (state)
			{
				state.enabled = false;
			}
			state = nextState;
			nextState.enabled = true;
		}
	}
}
