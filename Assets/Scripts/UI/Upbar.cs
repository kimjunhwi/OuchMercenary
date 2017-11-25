using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ReadOnlys;

public class Upbar : MonoBehaviour 
{
	public Text stageInfo_Text;
	public Button Back_Button;
	public Button Home_Button;

	public E_SCENE_INDEX ePrev_SceneIndex;


	public void SetUpHomeButton()
	{
		GameManager.Instance.InitUpbar ();
		SceneManager.LoadScene ((int)E_SCENE_INDEX.E_MENU);
	}



	//뒤로가기 버튼 씬마다 재세팅
	public void SetBackButtonLoadScene(E_SCENE_INDEX _curSceneIndex , E_SCENE_INDEX _prevSceneIndex )
	{
		switch (_curSceneIndex) 
		{
		case E_SCENE_INDEX.E_STAGE:
			//SetUpBar 초기화
			GameManager.Instance.InitUpbar ();
			SceneManager.LoadScene ((int)E_SCENE_INDEX.E_MENU);
			break;
		case E_SCENE_INDEX.E_STAGE_DEFENSE:
			GameManager.Instance.InitUpbar ();
			SceneManager.LoadScene ((int)E_SCENE_INDEX.E_STAGE);
			break;
		case E_SCENE_INDEX.E_STAGE_PREBATTLE:
			GameManager.Instance.InitUpbar ();

			if (_prevSceneIndex == E_SCENE_INDEX.E_STAGE_DEFENSE)
				SceneManager.LoadScene ((int)E_SCENE_INDEX.E_STAGE_DEFENSE);
			else if (_prevSceneIndex == E_SCENE_INDEX.E_STAGE_ATTACK)
				SceneManager.LoadScene ((int)E_SCENE_INDEX.E_STAGE_ATTACK);
			else if (_prevSceneIndex == E_SCENE_INDEX.E_STAGE_INFINITE)
				SceneManager.LoadScene ((int)E_SCENE_INDEX.E_STAGE_INFINITE);
		
			else
				Debug.Log ("Load Scene Failed");

			break;
		default:
			break;
		}
	}

	//해당 씬에 대한 정보 설정
	public void UpbarChangeInfo(E_SCENE_INDEX _eSceneIndex, string _stageInfo)
	{
		//BackButton 초기화 
		Back_Button.onClick.RemoveAllListeners ();
		gameObject.SetActive (true);

		switch (_eSceneIndex) 
		{
		case E_SCENE_INDEX.E_STAGE:
			//이전 씬의 정보
			ePrev_SceneIndex = GameManager.Instance.prevSceneIndex;
			//현재 씬의 정보
			stageInfo_Text.text = _stageInfo;
			//셋팅되는 씬을 입력해야됨
			Back_Button.onClick.AddListener (()=> SetBackButtonLoadScene(E_SCENE_INDEX.E_STAGE, ePrev_SceneIndex));
			break;
		case E_SCENE_INDEX.E_STAGE_DEFENSE:
			ePrev_SceneIndex = GameManager.Instance.prevSceneIndex;
			stageInfo_Text.text = _stageInfo;
			Back_Button.onClick.AddListener (()=> SetBackButtonLoadScene(E_SCENE_INDEX.E_STAGE_DEFENSE, ePrev_SceneIndex));
			break;

		case E_SCENE_INDEX.E_STAGE_PREBATTLE:
			ePrev_SceneIndex = GameManager.Instance.prevSceneIndex;
			stageInfo_Text.text = _stageInfo;
			Back_Button.onClick.AddListener (()=> SetBackButtonLoadScene(E_SCENE_INDEX.E_STAGE_PREBATTLE, ePrev_SceneIndex));
			break;

		default:
			break;
		}
	}
}
