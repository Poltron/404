using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnCamera : MonoBehaviour
{
	[SerializeField]
	private Text columnText;
	[SerializeField]
	private GameObject visual;

	public void Init(int num)
	{
		columnText.text = num.ToString();
		ActiveVisual(false);
	}

	public void ActiveVisual(bool active)
	{
		visual.SetActive(active);
	}
}
