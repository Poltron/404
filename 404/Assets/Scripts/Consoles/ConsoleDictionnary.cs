using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace Adventure
{
	public class ConsoleDictionnary : MonoBehaviour
	{
		[System.Serializable]
		private class Command
		{
			public string value;
			public bool discover;
			public Color color;
			public List<Command> linkCommand;

			public Command()
			{
				linkCommand = new List<Command>();
			}

			public Command FindValue(string toFind)
			{
				if (string.Compare(value, toFind, true) == 0)
					return this;

				if (linkCommand.Count == 0)
					return null;

				Command result = null;
				foreach (Command d in linkCommand)
				{
					result = d.FindValue(toFind);
					if (result != null)
						return result;
				}
				return result;
			}
		}

		private Command commandRoot;
		private Command currentCommand;

		[SerializeField]
		private RectTransform helpDico;
		[SerializeField]
		private Text dicoText;

		private ConsoleLine console;

		private void Awake()
		{
			commandRoot = new Command();
			currentCommand = commandRoot;
			InitDictionnary();
		}

		private void Start()
		{
			console = FindObjectOfType<ConsoleLine>();
			console.AddOnSucessedCommand(OnSucced);
			console.AddOnFailedCommand(OnFailed);
			console.AddOnActiveCommande(ActiveText);
			console.AddOnWriteCommand(AutoCompletion);
			AutoCompletion(new string[] { "" });
			ActiveText(false);
		}

		private void OnDestroy()
		{
			console.RemoveOnSucessedCommand(OnSucced);
			console.RemoveOnFailedCommand(OnFailed);
			console.RemoveOnActiveCommande(ActiveText);
		}

		private void InitDictionnary()
		{
			string pathfile = Application.dataPath + "/StreamingAssets/DictionnaryCommand.xml";
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(pathfile);
				XmlNode currentNode = xmlDoc.FirstChild;

				AddData(commandRoot, currentNode);
			}
			catch (System.NullReferenceException ex)
			{
				Debug.LogError("Le fichier DictionnaryCommand.xml est incomplet : " + ex.ToString());
			}
			catch (XmlException ex)
			{
				Debug.LogError("Le fichier DictionnaryCommand.xml est corrompu : " + ex.ToString());
			}
		}

		private Command AddData(Command currentCmd, XmlNode node)
		{
			XmlNode attributNode = node.SelectSingleNode("value");
			if (attributNode != null)
			{
				currentCmd.value = attributNode.InnerText.ToUpper();
			}
			attributNode = node.SelectSingleNode("discover");
			if (attributNode != null)
			{
				currentCmd.discover = bool.Parse(attributNode.InnerText);
			}
			attributNode = node.SelectSingleNode("color");
			if (attributNode != null)
			{
				ColorUtility.TryParseHtmlString("#" + attributNode.InnerText, out currentCmd.color);
			}
			XmlNodeList cmdLink = node.SelectNodes("command");
			foreach (XmlNode link in cmdLink)
			{
				Command newCmd = new Command();
				newCmd = AddData(newCmd, link);
				currentCmd.linkCommand.Add(newCmd);
			}

			return currentCmd;
		}

		public string[] GetCommand()
		{
			string[] allCommand = new string[commandRoot.linkCommand.Count];

			int i = 0;
			foreach (Command root in commandRoot.linkCommand)
			{
				allCommand[i++] = root.value;
			}

			return allCommand;
		}

		public void SetPositionDico(Vector3 pos)
		{
			helpDico.position = pos;
		}

		private void ActiveText(bool active)
		{
			helpDico.gameObject.SetActive(active);
		}

		public ConsoleLine.ECommandResult AutoCompletion(string[] words)
		{
			string lastArg = words.Length > 0 ? words[words.Length - 1] : "";

			currentCommand = commandRoot;
			FindLink(words);
			dicoText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(currentCommand.color) + ">";
			WriteAutoCompletion(lastArg);
			return ConsoleLine.ECommandResult.Successed;
		}

		private void FindLink(string[] args)
		{
			for (int i = 0 ; i < args.Length ; ++i)
			{
				Command commandFound = currentCommand.FindValue(args[i]);

				if (commandFound == null)
					return;
				currentCommand = commandFound;
			}
		}

		private void WriteAutoCompletion(string lastArg)
		{
			bool find = false;
			bool showAll = lastArg == "";

			foreach (Command nextData in currentCommand.linkCommand)
			{
				if (showAll || nextData.value.StartsWith(lastArg, System.StringComparison.InvariantCultureIgnoreCase))
				{
					dicoText.text += nextData.value + "\n";
					find = true;
				}
			}
			if (dicoText.text.Length > 0)
				dicoText.text = dicoText.text.Remove(dicoText.text.Length - 1);
			dicoText.text += "</color>";
			ActiveText(find);
		}

		private ConsoleLine.ECommandResult OnSucced(string[] args)
		{
			Command currentData = commandRoot;

			foreach (string str in args)
			{
				Command dataFound = currentData.FindValue(str);

				if (dataFound == null)
					continue;
				dataFound.discover = true;
				currentData = dataFound;
			}

			helpDico.gameObject.SetActive(false);
			return ConsoleLine.ECommandResult.Successed;
		}

		private ConsoleLine.ECommandResult OnFailed(string[] args)
		{
			helpDico.gameObject.SetActive(false);
			return ConsoleLine.ECommandResult.Failed;
		}
	}
}