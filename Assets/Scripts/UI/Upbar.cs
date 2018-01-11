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

	public Sprite[] upbarSprites;
	public Image[] upbarImages;

	public MainSceneManager mainSceneManager;

	public void SetUpHomeButton()
	{
		GameManager.Instance.InitUpbar ();
		GameManager.Instance.LoadScene( E_SCENE_INDEX.E_MENU , E_SCENE_INDEX.E_NONE, false);
	}

	public void SetSprite()
	{
		upbarImages [0].sprite = upbarSprites [1];
		upbarImages [1].sprite = upbarSprites [0];
		upbarImages [2].sprite = upbarSprites [3];
		upbarImages [3].sprite = upbarSprites [3];
		upbarImages [4].sprite = upbarSprites [2];
	}


	//뒤로가기 버튼 씬마다 재세팅
	public void SetBackButtonLoadScene(E_SCENE_INDEX _curSceneIndex , E_SCENE_INDEX _prevSceneIndex )
	{
		GameManager.Instance.isPrevLoad = true;
	
		switch (_curSceneIndex) 
		{
		case E_SCENE_INDEX.E_STAGE:
			//SetUpBar 초기화
		
			GameManager.Instance.LoadScene( _curSceneIndex , _prevSceneIndex, true);
			//GameManager.Instance.InitUpbar ();
			break;

		case E_SCENE_INDEX.E_MERMANAGE:
			//GameManager.Instance.InitUpbar ();
			GameManager.Instance.LoadScene (_curSceneIndex, _prevSceneIndex , true);
			break;
		case E_SCENE_INDEX.E_STAGE_HEALING:
	
			mainSceneManager.ActivePanelBack (E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING, true);
			break;
		case E_SCENE_INDEX.E_EMPLOYER:
			//GameManager.Instance.InitUpbar ();
			GameManager.Instance.LoadScene (_curSceneIndex, _prevSceneIndex , true);
			break;
		case E_SCENE_INDEX.E_STAGE_TRAINNIG:

			mainSceneManager.ActivePanelBack (E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING, true);
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
		//StopCoroutine (mainSceneManager.fadeInOutTest.FadeInOutOnce (E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING, true));
		switch (_eSceneIndex) 
		{
		//스테이지 씬
		case E_SCENE_INDEX.E_STAGE:
			//이전 씬의 정보
			ePrev_SceneIndex = GameManager.Instance.prevSceneIndex;
			//현재 씬의 정보
			stageInfo_Text.text = _stageInfo;
			//셋팅되는 씬을 입력해야됨 (해당씬, 이전씬)
			Back_Button.onClick.AddListener (()=> SetBackButtonLoadScene(E_SCENE_INDEX.E_STAGE, ePrev_SceneIndex));
			break;
		//용병관리 씬
		case E_SCENE_INDEX.E_MERMANAGE:
			//이전 씬의 정보
			ePrev_SceneIndex = GameManager.Instance.prevSceneIndex;
			//현재 씬의 정보
			stageInfo_Text.text = _stageInfo;
			//셋팅되는 씬을 입력해야됨 (해당씬, 이전씬)
			//Back_Button.onClick.AddListener (GameManager.Instance.InitMercenaryManage);
			Back_Button.onClick.AddListener (()=> SetBackButtonLoadScene(E_SCENE_INDEX.E_MERMANAGE, ePrev_SceneIndex));
			break;

		case E_SCENE_INDEX.E_STAGE_HEALING:

			//이전 씬의 정보
			ePrev_SceneIndex = GameManager.Instance.prevSceneIndex;
			//현재 씬의 정보
			stageInfo_Text.text = _stageInfo;
			Back_Button.onClick.AddListener (()=> SetBackButtonLoadScene(E_SCENE_INDEX.E_STAGE_HEALING, ePrev_SceneIndex));
			break;

		case E_SCENE_INDEX.E_STAGE_TRAINNIG:

			//이전 씬의 정보
			ePrev_SceneIndex = GameManager.Instance.prevSceneIndex;
			//현재 씬의 정보
			stageInfo_Text.text = _stageInfo;
			Back_Button.onClick.AddListener (()=> SetBackButtonLoadScene(E_SCENE_INDEX.E_STAGE_TRAINNIG, ePrev_SceneIndex));
			break;


		default:
			break;
		}
	}
}
