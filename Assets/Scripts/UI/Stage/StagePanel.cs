using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour 
{
	private const string sStageInfo_Defense 	= "스테이지 - 방어";
	private const string sStageInfo_Attack 	= "스테이지 - 공격";
	private const string sStageInfo_Infinite = "무한던전";



	public Button stageDefense_Button;
	public Button stageAttack_Button;
	public Button stageInfinite_Button;

	void Start()
	{
		stageDefense_Button.onClick.RemoveAllListeners ();
		stageAttack_Button.onClick.RemoveAllListeners ();
		stageInfinite_Button.onClick.RemoveAllListeners ();

		stageDefense_Button.onClick.AddListener (() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_STAGE_DEFENSE));
		//GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_STAGE, canvas, sStageInfo_Defense);
	}


}
