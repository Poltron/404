﻿using System.Collections;
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

		private struct SLineHistory
		{
			public string line;
			public Color color;

			public SLineHistory(string text, Color color)
			{
				line = text;
				this.color = color;
			}
		}

		#region Event
		public delegate ECommandResult SendCommandDelegate(string[] args);
		private Dictionary<string, List<SendCommandDelegate>> allCommand;
		private event SendCommandDelegate OnWriteCommand;
		private event SendCommandDelegate OnFailedCommand;
		private event SendCommandDelegate OnSucessedCommand;

		public delegate void ActiveCommandDelegate(bool isActive);
		private event ActiveCommandDelegate OnActiveCommande;
		#endregion

		[Header("Console")]
		[SerializeField]
		private GameObject consoleLine;
		private InputField consoleField;
		[SerializeField]
		private List<string> cmdNames;
		[SerializeField]
		private ConsoleDictionnary dico;

		[Header("History")]
		[SerializeField]
		private Color sucessCmd;
		[SerializeField]
		private Color failedCmd;
		[SerializeField]
		private int historyNumber;
		[SerializeField]
		private Text historiesLine;
		private Queue<SLineHistory> historic;

		private void Awake()
		{
			allCommand = new Dictionary<string, List<SendCommandDelegate>>();
			historic = new Queue<SLineHistory>();
			cmdNames = new List<string>();
		}

		private void Start()
		{
			consoleField = consoleLine.GetComponent<InputField>();
			consoleField.onValueChanged.AddListener(WriteCommande);
			consoleLine.SetActive(false);
			historiesLine.text = "";
			InitCommandList();
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
			ResetOnSendCommand();
			ResetOnActiveCommande();
			ResetOnFailedCommand();
			ResetOnSendCommand();
			ResetOnSucessedCommand();
			ResetOnWriteCommand();
		}

		private void InitCommandList()
		{
			ConsoleDictionnary dico = GetComponent<ConsoleDictionnary>();
			cmdNames = new List<string>(dico.GetCommand());
			return;
		}

		private void SendCommande()
		{
			// event inputManager KeyCode.Return
			consoleLine.SetActive(!consoleLine.activeSelf);

			if (consoleLine.activeSelf)
			{
				consoleField.ActivateInputField();
			}
			else
			{
				SendCommande(consoleField.text);
			}
			consoleField.text = "";
			InvokeOnActiveCommande(consoleLine.activeSelf);
		}

		private string[] GetArguments(string text, out string cmd)
		{
			cmd = "";
			string[] words = text.Split(' ');
			if (words.Length < 2)
				return new string[0];

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
			if (consoleText == "")
				return;
			consoleText = consoleText.ToUpper();
			string cmd = "";
			string[] args = GetArguments(consoleText, out cmd);
			ECommandResult result = ECommandResult.Failed;

			if (cmdNames.Contains(cmd))
				result = InvokeOnSendCommand(cmd, args);
			Color color = result == ECommandResult.Successed ? sucessCmd : failedCmd;

			string[] type = new string[args.Length + 1];
			args.CopyTo(type, 1);
			type[0] = cmd;
			if (result == ECommandResult.Successed)
			{
				InvokeOnSucessedCommand(type);
			}
			else
			{
				InvokeOnFailedCommand(type);
			}

			historic.Enqueue(new SLineHistory(consoleText, color));
			ViewHistory();
		}

		private void WriteCommande(string cmd)
		{
			consoleField.text = consoleField.text.ToUpper();

			RectTransform rt = consoleField.GetComponent<RectTransform>();
			Vector3 posCarret = rt.position;

			posCarret.x += 10 + consoleField.text.Length * 10f;
			posCarret.y += 10 + rt.sizeDelta.y;
			dico.SetPositionDico(posCarret);

			string[] words = consoleField.text.Split(' ');

			InvokeOnWriteCommand(words);
		}

		private void ViewHistory()
		{
			if (historic.Count > historyNumber)
			{
				historic.Dequeue();
			}

			historiesLine.text = "";
			foreach (SLineHistory line in historic)
			{
				historiesLine.text += "<color=#" + ColorUtility.ToHtmlStringRGBA(line.color) + ">" + line.line + "</color>\n";
			}
			historiesLine.text = historiesLine.text.Remove(historiesLine.text.Length - 1);
		}

		#region Event
		#region OnWriteCommand
		public void AddOnWriteCommand(SendCommandDelegate func)
		{
			OnWriteCommand += func;
		}

		public void RemoveOnWriteCommand(SendCommandDelegate func)
		{
			OnWriteCommand -= func;
		}

		private void ResetOnWriteCommand()
		{
			OnWriteCommand = null;
		}

		private void InvokeOnWriteCommand(string[] args)
		{
			OnWriteCommand?.Invoke(args);
		}
		#endregion

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

		private ECommandResult InvokeOnSendCommand(string cmd, string[] args)
		{
			ECommandResult result = ECommandResult.Failed;

			if (string.IsNullOrEmpty(cmd) || !allCommand.ContainsKey(cmd))
			{
				return ECommandResult.Failed;
			}

			foreach (SendCommandDelegate func in allCommand[cmd])
			{
				if (func(args) == ECommandResult.Successed)
				{
					result = ECommandResult.Successed;
				}
			}
			return result;
		}
		#endregion

		#region OnSucessedCommand
		public void AddOnSucessedCommand(SendCommandDelegate func)
		{
			OnSucessedCommand += func;
		}

		public void RemoveOnSucessedCommand(SendCommandDelegate func)
		{
			OnSucessedCommand -= func;
		}

		private void ResetOnSucessedCommand()
		{
			OnSucessedCommand = null;
		}

		private void InvokeOnSucessedCommand(string[] args)
		{
			OnSucessedCommand?.Invoke(args);
		}
		#endregion

		#region OnFailedCommand
		public void AddOnFailedCommand(SendCommandDelegate func)
		{
			OnFailedCommand += func;
		}

		public void RemoveOnFailedCommand(SendCommandDelegate func)
		{
			OnFailedCommand -= func;
		}

		private void ResetOnFailedCommand()
		{
			OnFailedCommand = null;
		}

		private void InvokeOnFailedCommand(string[] args)
		{
			OnFailedCommand?.Invoke(args);
		}
		#endregion

		#region OnActiveCommande
		public void AddOnActiveCommande(ActiveCommandDelegate func)
		{
			OnActiveCommande += func;
		}

		public void RemoveOnActiveCommande(ActiveCommandDelegate func)
		{
			OnActiveCommande -= func;
		}

		private void ResetOnActiveCommande()
		{
			OnActiveCommande = null;
		}

		private void InvokeOnActiveCommande(bool isActive)
		{
			OnActiveCommande?.Invoke(isActive);
		}
		#endregion
		#endregion
	}
}