using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;


public class StagePanel : MonoBehaviour 
{
	public Transform canvas;

	private const string sStageInfo_Defense 	= "스테이지 - 방어";
	private const string sStageInfo_Attack 		= "스테이지 - 공격";
	private const string sStageInfo_Infinite 	= "무한던전";

	public GameObject preBattle_Obj;

	//Stage
	public GameObject[] Stage;

	public Button stageDefense_Button;
	public Button stageAttack_Button;
	public Button stageInfinite_Button;

	void Start()
	{
		stageDefense_Button.onClick.RemoveAllListeners ();
		stageAttack_Button.onClick.RemoveAllListeners ();
		stageInfinite_Button.onClick.RemoveAllListeners ();

		stageDefense_Button.onClick.AddListener(()=> StageActive(E_STAGE_INDEX.E_STAGE_INDEX_DEFENSE));
		stageAttack_Button.onClick.AddListener(()=> StageActive(E_STAGE_INDEX.E_STAGE_INDEX_ATTACK));
		stageInfinite_Button.onClick.AddListener(()=> StageActive(E_STAGE_INDEX.E_STAGE_INDEX_INFINITE));
	}

	public void StageActive(E_STAGE_INDEX _index)
	{
		this.gameObject.SetActive (false);
		switch (_index) 
		{
		case E_STAGE_INDEX.E_STAGE_INDEX_DEFENSE:
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_DEFENSE].SetActive (true);
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_ATTACK].SetActive (false);
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_INFINITE].SetActive (false);
			break;
		case E_STAGE_INDEX.E_STAGE_INDEX_ATTACK:
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_DEFENSE].SetActive (false);
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_ATTACK].SetActive (true);
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_INFINITE].SetActive (false);
			break;
		case E_STAGE_INDEX.E_STAGE_INDEX_INFINITE:
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_DEFENSE].SetActive (false);
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_ATTACK].SetActive (false);
			Stage [(int)E_STAGE_INDEX.E_STAGE_INDEX_INFINITE].SetActive (true);
			break;
		default:
			break;
		}
	}
}
