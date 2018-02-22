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
	Toggle One_Chapter_Toggle;
	Toggle Two_Chapter_Toggle;

	public Transform One_Chapter_Parent;
	public Transform Two_Chapter_Parent;


	public Button StartButton;

	public Player player;

	public DefenceStageButton[] forestButtons;
	public GameObject PrepareBattleTotalPanel;

	public GameObject StageButton;
	public StageInfoPanel stageInfoPanel;


	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();
	}

	public void SetUp(GameObject _obj)
	{
		One_Chapter_Toggle = transform.GetChild(0).GetComponent<Toggle> ();
		Two_Chapter_Toggle = transform.GetChild(1).GetComponent<Toggle> ();

		One_Chapter_Parent = transform.GetChild (2).GetChild (0).GetChild (0).GetChild (0).transform;
		Two_Chapter_Parent = transform.GetChild (2).GetChild (1).GetChild (0).GetChild (0).transform;

		int nStageIndex = 0;

//		for(nStageIndex < 5; nStageIndex++;)
//		{
//			GameObject stageObject = Instantiate(_obj);
//			transform.SetParent(One_Chapter_Parent,false);
//
//			DefenceStageButton buttonScript = stageObject.AddComponent<DefenceStageButton>();
//
//
//				
//		}
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
