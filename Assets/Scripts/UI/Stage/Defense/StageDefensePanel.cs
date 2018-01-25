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

public class StageDefensePanel :MonoBehaviour
{
	public Button StartButton;

	public Player player;

	public DefenceStageButton[] forestButtons;


	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();
	}

	void Start()
	{
		StartButton.onClick.AddListener(() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_BATTLE, ReadOnlys.E_SCENE_INDEX.E_STAGE,false));

		AllDisableDefenceButtons ();

		forestButtons [player.nDefenceChapterOne].SetUpSprite (true);
	}

	public void AllDisableDefenceButtons()
	{
		for (int nIndex = 0; nIndex < forestButtons.Length; nIndex++) 
		{
			forestButtons [nIndex].SetUpSprite (false);	
		}
	}
}
