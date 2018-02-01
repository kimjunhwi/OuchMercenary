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
	public GameObject PrepareBattleTotalPanel;

	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();
	}

	void Start()
	{
		StartButton.onClick.AddListener(NextStage);

		//GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_BATTLE, ReadOnlys.E_SCENE_INDEX.E_STAGE,false));

		AllDisableDefenceButtons ();

		forestButtons [player.nDefenceChapterOne].SetUpSprite (true);
	}

	public void NextStage()
	{
		gameObject.SetActive (false);

		PrepareBattleTotalPanel.SetActive (true);
	}

	public void AllDisableDefenceButtons()
	{
		for (int nIndex = 0; nIndex < forestButtons.Length; nIndex++) 
		{
			forestButtons [nIndex].SetUpSprite (false);	
		}
	}
}
