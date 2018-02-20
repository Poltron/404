using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adventure
{
	public class ConsoleLine : MonoBehaviour
	{
		public enum ECommandResult
		{
			Failed,
			Successed,
		}

		#region Event
		public delegate ECommandResult SendCommandDelegate(string[] args);
		private Dictionary<string, List<SendCommandDelegate>> allCommand;
		#endregion

		[SerializeField]
		private GameObject consoleLine;
		private InputField consoleField;

		private void Awake()
		{
			allCommand = new Dictionary<string, List<SendCommandDelegate>>();
		}

		private void Start()
		{
			consoleField = consoleLine.GetComponent<InputField>();
			consoleLine.SetActive(false);
		}

		private void Update()
		{
			//* link to inputManager
			if (Input.GetKeyDown(KeyCode.Return))
			{
				SendCommande();
			}
			/**/
		}

		private void OnDestroy()
		{
			allCommand.Clear();
		}

		private void SendCommande()
		{
			// event inputManager KeyCode.Return
			consoleLine.SetActive(!consoleLine.activeSelf);
			consoleField.text = "";

			if (consoleLine.activeSelf)
			{
				consoleField.ActivateInputField();
			}
			else
			{
				SendCommande(consoleField.text);
			}
		}

		private string[] GetArguments(string text, out string cmd)
		{
			cmd = "";
			string[] words = text.Split(' ');
			if (words.Length < 2)
				return null;

			cmd = words[0];
			string[] args = new string[words.Length - 1];

			for (int i = 1 ; i < words.Length ; ++i)
			{
				args[i - 1] = words[i];
			}

			return args;
		}

		private void SendCommande(string consoleText)
		{
			string cmd = "";
			string[] args = GetArguments(consoleText, out cmd);
			InvokeOnSendCommand(cmd, args);
		}

		#region Event
		#region OnSendCommand
		public void AddOnSendCommand(string cmd, SendCommandDelegate func)
		{
			if (string.IsNullOrEmpty(cmd))
				return;

			if (!allCommand.ContainsKey(cmd) || allCommand[cmd] == null)
			{
				allCommand.Add(cmd, new List<SendCommandDelegate>());
			}
			allCommand[cmd].Add(func);
		}

		public void RemoveOnSendCommand(string cmd, SendCommandDelegate func)
		{
			if (allCommand.ContainsKey(cmd))
			{
				allCommand[cmd].Remove(func);
			}
		}

		private void ResetOnSendCommand(string cmd)
		{
			if (allCommand.ContainsKey(cmd))
				allCommand[cmd].Clear();
		}

		private void ResetOnSendCommand()
		{
			allCommand.Clear();
		}

		private void InvokeOnSendCommand(string cmd, string[] args)
		{
			ECommandResult result = ECommandResult.Successed;

			if (string.IsNullOrEmpty(cmd) || !allCommand.ContainsKey(cmd))
			{
				result = ECommandResult.Failed;
				return;
			}

			foreach (SendCommandDelegate func in allCommand[cmd])
			{
				if (func(args) == ECommandResult.Failed)
				{
					result = ECommandResult.Failed;
				}
			}

			if (result == ECommandResult.Failed)
			{
				// do stuff error
			}
		}
		#endregion
		#endregion
	}
}