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
	void Start()
	{
		togglePanel[0].SetActive (true);
		togglePanel[1].SetActive (false);

		toggle[0].onValueChanged.AddListener((x)=>ActivePanel(StageChapterInfo.Chapter01));
		toggle[1].onValueChanged.AddListener((x)=>ActivePanel(StageChapterInfo.Chapter02));
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

			togglePanel[1].SetActive (false);
			togglePanel[0].SetActive (true);

			break;
		case StageChapterInfo.Chapter02:
			Debug.Log ("Active Chapter01 Panel!!");

			togglePanel[1].SetActive (true);
			togglePanel[0].SetActive (false);

			break;

		default:
			break;
		}
	
	}



}
