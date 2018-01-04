using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

using ReadOnlys;

public enum StageChapterInfo 
{
	Chapter01 = 0,
	Chapter02 ,
}

public class StageDefensePanel : ToggleUIBase 
{
	public StagePanel stagePanel;
	//각각의 챕터에 대한 정보들
	public Chapter [] chapters;

	void Start()
	{
		togglePanel[0].SetActive (true);
		togglePanel[1].SetActive (false);

		toggle[0].onValueChanged.AddListener((x)=>ActivePanel(StageChapterInfo.Chapter01));
		toggle[1].onValueChanged.AddListener((x)=>ActivePanel(StageChapterInfo.Chapter02));

		chapters [0].Ready_Button.onClick.AddListener (ActivePreBattlePanel);
		chapters [1].Ready_Button.onClick.AddListener (ActivePreBattlePanel);
	}

	public override void ActivePanel<T> (T _chapterIndex) 
	{
		//base.ActivePanel (ref _chapterIndex);

		//Enum.Parse(타입, T의 스트링)
		var eType = Enum.Parse(typeof( StageChapterInfo), _chapterIndex.ToString());


		switch ((StageChapterInfo)eType) 
		{
		case StageChapterInfo.Chapter01:
			Debug.Log ("Active Chapter01 Panel!!");

			togglePanel [1].SetActive (false);
			togglePanel [0].SetActive (true);

			break;
		case StageChapterInfo.Chapter02:
			Debug.Log ("Active Chapter01 Panel!!");

			togglePanel [1].SetActive (true);
			togglePanel [0].SetActive (false);

			break;

		default:
			break;
		}
	}

	public void ActivePreBattlePanel()
	{
		this.gameObject.SetActive (false);
		stagePanel.preBattle_Obj.SetActive (true);
	}



}
