using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ReadOnlys;



public class MainSceneManager : MonoBehaviour 
{
	public Transform canvas;
	//Test용 이미지
	public Image someImage;

	//에셋 번들로 부터 초기화 됬는지 아닌지 체크
	public bool bInitCheck; 

	public Button[] activeButton;
	public GameObject[] activeButtonPanel;

	public FadeInOut fadeInOutTest;

	void Awake () 
	{
		fadeInOutTest.mainSceneManager = this;
		//MercenaryManage_Button.onClick.AddListener(() => GameManager.Instance.SetUpMercenaryManage (canvas));
		//Debug.Log ("총 캐릭터 개수 : " +  GameManager.Instance.lDbBasicCharacter.Count);

		//for (int i = 0; i < GameManager.Instance.getSpriteArray.Count; i++)
		//	Debug.Log (GameManager.Instance.getSpriteArray[i]);
	
		//메인 UI 버튼 초기화
		InitActiveButton();
		if (!bInitCheck) 
		{
			//LoadSprtieAndDispatch ();
		}
	}
	//메인 UI 버튼 초기화
	void InitActiveButton()
	{
		//StageButton
		activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE].onClick.AddListener(() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_STAGE , E_SCENE_INDEX.E_MENU , true));
		//Mercenary Manage(용병관리)
		activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_MANAGEMENT].onClick.AddListener(() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_MERMANAGE , E_SCENE_INDEX.E_MENU , true));

		activeButton [(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING].onClick.AddListener (() => StartCoroutine (fadeInOutTest.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING,false)));

		activeButton [(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING].onClick.AddListener (() => StartCoroutine (fadeInOutTest.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING,false)));

	}


	void LoadSprtieAndDispatch()
	{
		someImage.sprite = GameManager.Instance.getSpriteArray [0];
	}

	public void ActivePanelBack(E_ACTIVEBUTTON _button , bool _isback)
	{
		StartCoroutine (fadeInOutTest.FadeInOutOnce (_button, _isback));

	}
}
