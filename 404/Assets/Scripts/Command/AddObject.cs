using System.Collections;
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
		private Text textColumn;
		[SerializeField]
		private int nbrColumn;
		private List<Transform> columns;

		private void Awake()
		{
			columns = new List<Transform>();
		}

		void Start()
		{
			console = FindObjectOfType<ConsoleLine>();
			console.AddOnSendCommand(Constantes.Command.Add, Do);
			DivideScreen();
		}

		private void OnDestroy()
		{
			console.RemoveOnSendCommand(Constantes.Command.Add, Do);
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

				Text textObj = Instantiate(textColumn, contentText);
				textObj.transform.position = worldCamera.WorldToScreenPoint(pos);
				textObj.text = (i + 1).ToString();

				if (i == nbrColumn - 1)
				{
					int childs = textObj.transform.childCount;
					for (int j = childs - 1 ; j >= 0 ; --j)
					{
						Destroy(textObj.transform.GetChild(j).gameObject);
					}
				}
				pos.x += stepSize;
				columns.Add(newColumn.transform);
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

			if (numColumn < 0 || numColumn >= nbrColumn)
			{
				return ConsoleLine.ECommandResult.Failed;
			}

			InteractiveBehaviour toSpawn = objectsToInstantiate.Find(obj => obj.Name == args[0]);
			if (toSpawn == null)
			{
				return ConsoleLine.ECommandResult.Failed;
			}

			Instantiate(toSpawn, columns[numColumn].position, Quaternion.identity);

			return ConsoleLine.ECommandResult.Successed;
		}
	}
}