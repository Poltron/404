using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;

namespace Adventure
{
	public class CommandWindow : EditorWindow
	{
		private Command commandRoot;
		private Vector2 scrollPos;
		private Vector2 scrollView;
		private Vector2 space;

		private Rect visualCommand;
		private Rect separatorRect;
		private float sizeCommand;

		public class Command
		{
			public string value;
			public bool discover;
			public Color color;
			public List<Command> linkCommand;
			public bool showLink;

			public Command()
			{
				value = "";
				color = Color.white;
				discover = false;
				linkCommand = new List<Command>();
				showLink = true;
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

		[MenuItem("404Adventure/CommandManager")]
		private static void ShowWindow()
		{
			CommandWindow window = (CommandWindow)GetWindow(typeof(CommandWindow));
			window.Show();
			window.Init();
		}

		private void Init()
		{
			space = new Vector2(5f, 5f);
			sizeCommand = 15f + 150f + 40 + 15 + 15;
			string pathfile = Application.dataPath + "/StreamingAssets/DictionnaryCommand.xml";
			XmlDocument xmlDoc = new XmlDocument();

			commandRoot = new Command();

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

		private void Save()
		{
			XmlDocument xmlDoc = new XmlDocument();
			XmlNode currentCommand = xmlDoc.CreateElement("command");

			xmlDoc.AppendChild(Save(commandRoot, currentCommand));

			xmlDoc.Save(Application.dataPath + "/StreamingAssets/DictionnaryCommand.xml");
		}

		private XmlNode Save(Command cmd, XmlNode parent)
		{
			XmlNode currentCommand = parent.OwnerDocument.CreateElement("command");
			XmlNode dataNode = parent.OwnerDocument.CreateElement("value");
			dataNode.InnerText = cmd.value;
			currentCommand.AppendChild(dataNode);

			dataNode = parent.OwnerDocument.CreateElement("discover");
			dataNode.InnerText = cmd.discover.ToString();
			currentCommand.AppendChild(dataNode);

			dataNode = parent.OwnerDocument.CreateElement("color");
			dataNode.InnerText = ColorUtility.ToHtmlStringRGBA(cmd.color);
			currentCommand.AppendChild(dataNode);

			foreach (Command link in cmd.linkCommand)
			{
				currentCommand.AppendChild(Save(link, dataNode));
			}

			return currentCommand;
		}

		private void OnGUI()
		{
			if (commandRoot == null)
				Init();
			visualCommand = new Rect(space.x, 15f, 15f, 15);
			separatorRect = new Rect(5f, 0f, position.width - 5f, 15f);

			scrollPos = GUI.BeginScrollView(new Rect(0f, 0f, position.width, position.height), scrollPos, new Rect(0f, 0f, scrollView.x, scrollView.y));

			Header();

			ViewCommand();
			scrollView.y = separatorRect.y;
			GUI.EndScrollView();
		}

		private void Header()
		{
			visualCommand.width = 95f;
			bool addCmd = EditorGUI.Toggle(visualCommand, false, GUI.skin.button);
			visualCommand.x += 5f;
			EditorGUI.LabelField(visualCommand, "Add command");

			visualCommand.x += visualCommand.width + 5f;
			visualCommand.width = 70f;
			bool ReloadCommand = EditorGUI.Toggle(visualCommand, false, GUI.skin.button);
			visualCommand.x += 2f;
			EditorGUI.LabelField(visualCommand, "Reload file");

			visualCommand.x += visualCommand.width + 5f;
			visualCommand.width = 57f;
			bool saveFile = EditorGUI.Toggle(visualCommand, false, GUI.skin.button);
			visualCommand.x += 2f;
			EditorGUI.LabelField(visualCommand, "Save file");

			if (addCmd)
			{
				Command newCmd = new Command();
				commandRoot.linkCommand.Add(newCmd);
			}
			if (ReloadCommand)
			{
				Init();
			}
			if (saveFile)
			{
				Save();
				AssetDatabase.Refresh();
			}

			separatorRect.y = visualCommand.y + visualCommand.height + space.y;
			EditorGUI.LabelField(separatorRect, "", GUI.skin.horizontalSlider);
			visualCommand.y = (visualCommand.height + space.y) * 3.0f;
			visualCommand.x = space.x;
		}

		private void ViewCommand()
		{
			for (int i = 0 ; i < commandRoot.linkCommand.Count ; ++i)
			{
				Command cmd = commandRoot.linkCommand[i];
				ViewCommand(cmd, commandRoot);
				separatorRect.y = visualCommand.y + visualCommand.height + space.y;
				EditorGUI.LabelField(separatorRect, "", GUI.skin.horizontalSlider);
				visualCommand.y += (visualCommand.height + space.y) * 2.0f;
				visualCommand.x = space.x;
			}
		}

		private void ViewCommand(Command cmd, Command parent)
		{
			float positionX = visualCommand.x;
			visualCommand.width = 25f + (cmd.linkCommand.Count.ToString().Length * 10f);
			cmd.showLink = EditorGUI.Foldout(visualCommand, cmd.showLink, "(" + cmd.linkCommand.Count + ")");

			visualCommand.x += visualCommand.width;
			visualCommand.width = 57f;
			cmd.discover = EditorGUI.Toggle(visualCommand, cmd.discover, GUI.skin.button);
			EditorGUI.LabelField(visualCommand, "Discover");

			visualCommand.x += visualCommand.width + space.x;
			visualCommand.width = 150f;
			cmd.value = EditorGUI.TextField(visualCommand, cmd.value).ToUpper();

			visualCommand.x += visualCommand.width + space.x;
			visualCommand.width = 40f;
			cmd.color = EditorGUI.ColorField(visualCommand, cmd.color);

			visualCommand.x += visualCommand.width + space.x;
			visualCommand.width = 88f;
			bool addCmd = EditorGUI.Toggle(visualCommand, false, GUI.skin.button);
			//visualCommand.x += 5f;
			EditorGUI.LabelField(visualCommand, "Add command");

			visualCommand.x += visualCommand.width + 5f;
			visualCommand.width = 12f;
			bool deleteCmd = EditorGUI.Toggle(visualCommand, false, GUI.skin.button);
			//visualCommand.x += 2f;
			EditorGUI.LabelField(visualCommand, "X");

			if (addCmd)
			{
				Command newCmd = new Command();
				cmd.linkCommand.Add(newCmd);
			}
			if (deleteCmd)
			{
				parent.linkCommand.Remove(cmd);
			}
			if (cmd.showLink)
			{
				for (int i = 0 ; i < cmd.linkCommand.Count ; ++i)
				{
					Command link = cmd.linkCommand[i];
					visualCommand.y += visualCommand.height + space.y;
					visualCommand.x = positionX + sizeCommand * .25f;
					ViewCommand(link, cmd);
				}
			}
		}
	}
}