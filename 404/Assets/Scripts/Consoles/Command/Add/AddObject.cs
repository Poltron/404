﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adventure
{
	public class AddObject : MonoBehaviour
	{
		[SerializeField]
		private List<InteractiveBehaviour> objectsToInstantiate;
		private ConsoleLine console;
		[Header("Columns")]
		[SerializeField]
		private Camera worldCamera;
		[SerializeField]
		private Transform contentText;
		[SerializeField]
		private ColumnCamera textColumn;
		[SerializeField]
		private int nbrColumn;
		private List<Transform> columns;
		private List<ColumnCamera> visualColumns;

		private void Awake()
		{
			columns = new List<Transform>();
			visualColumns = new List<ColumnCamera>();
		}

		void Start()
		{
			console = GetComponentInParent<ConsoleLine>();
			console.AddOnSendCommand(Constantes.Command.Add, Do);
			console.AddOnWriteCommand(ShowVisual);
			console.AddOnActiveCommande(ShowVisual);
			DivideScreen();
		}

		private void OnDestroy()
		{
			console.RemoveOnSendCommand(Constantes.Command.Add, Do);
			console.RemoveOnActiveCommande(ShowVisual);
			console.RemoveOnWriteCommand(ShowVisual);
		}

		private void DivideScreen()
		{
			Vector3 topLeft = worldCamera.ViewportToWorldPoint(Vector3.up);
			Vector3 topRight = worldCamera.ViewportToWorldPoint(Vector3.one);
			topLeft.z = 0.0f;
			topRight.z = 0.0f;

			float distance = Vector3.Distance(topLeft, topRight);
			float stepSize = distance / nbrColumn;

			Vector3 pos = topLeft;
			pos.x += stepSize * 0.5f;

			for (int i = 0 ; i < nbrColumn ; ++i)
			{
				GameObject newColumn = new GameObject("Column " + i);
				newColumn.transform.SetParent(worldCamera.transform);
				newColumn.transform.position = pos;

				ColumnCamera textObj = Instantiate(textColumn, contentText);
				textObj.Init(i + 1);

				pos.x += stepSize;
				columns.Add(newColumn.transform);
				visualColumns.Add(textObj);
			}
		}

		private ConsoleLine.ECommandResult ShowVisual(string[] words)
		{
			int numColumn = -1;
			ShowVisual(false);

			if (words.Length != 3 || !int.TryParse(words[words.Length - 1], out numColumn)
				|| numColumn < 1 || numColumn > nbrColumn)
				return ConsoleLine.ECommandResult.Failed;
			if (words[0].CompareTo(Constantes.Command.Add) != 0)
				return ConsoleLine.ECommandResult.Failed;


			visualColumns[numColumn - 1].ActiveVisual(true);
			return ConsoleLine.ECommandResult.Successed;
		}

		private void ShowVisual(bool show)
		{
			foreach (ColumnCamera col in visualColumns)
			{
				col.ActiveVisual(false);
			}
		}

		ConsoleLine.ECommandResult Do(string[] args)
		{
			if (args.Length != 2)
			{
				return ConsoleLine.ECommandResult.Failed;
			}
			string objName = args[0];
			int numColumn = -1;

			if (!int.TryParse(args[1], out numColumn))
			{
				return ConsoleLine.ECommandResult.Failed;
			}

			if (numColumn < 1 || numColumn > nbrColumn)
			{
				return ConsoleLine.ECommandResult.Failed;
			}

			InteractiveBehaviour toSpawn = objectsToInstantiate.Find(obj => string.Compare(obj.Name, objName, true) == 0);
			if (toSpawn == null)
			{
				return ConsoleLine.ECommandResult.Failed;
			}

			Instantiate(toSpawn, columns[numColumn - 1].position, Quaternion.identity);

			return ConsoleLine.ECommandResult.Successed;
		}
	}
}