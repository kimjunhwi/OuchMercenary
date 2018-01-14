﻿using System.Collections;
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

    //씬을 나누지 않은 것들에 대한 중간 효과 페이드 테스트(완료)
    public FadeInOut fadeInOutTest;

    //메인메뉴에 있는 아이콘의 개수
    private const int nMenuIconCount = 10;

    void Awake () 
	{
		fadeInOutTest.mainSceneManager = this;
        //MercenaryManage_Button.onClick.AddListener(() => GameManager.Instance.SetUpMercenaryManage (canvas));
        //Debug.Log ("총 캐릭터 개수 : " +  GameManager.Instance.lDbBasicCharacter.Count);

        //for (int i = 0; i < GameManager.Instance.getSpriteArray.Count; i++)
        //	Debug.Log (GameManager.Instance.getSpriteArray[i]);

        //저장된 데이터를 제대로 불러왔는지 체크
        //CheckSaveDataIsSure();

        StartCoroutine(GameManager.Instance.LoadAnimationFromAssetBundel("Assets/AssetBundles/character"));


        //메인 UI 버튼 초기화
        InitActiveButton();
		if (!bInitCheck) 
		{
			LoadSprtieAndDispatch ();
		}
	}

    //저장된 데이터를 제대로 불러왔는지 체크
    void CheckSaveDataIsSure()
    {
        Debug.Log("BasicCharacterCount : " + GameManager.Instance.lDbBasicCharacter.Count);
        Debug.Log("ActiveSkillCount : " + GameManager.Instance.lDbActiveSkill.Count);
        Debug.Log("ActiveSkillTypeCount : " + GameManager.Instance.lDbActiveSkillType.Count);
        Debug.Log("PassiveSkillCount : " + GameManager.Instance.lDbPassiveSkill.Count);
        Debug.Log("PassiveSkillOptionIndexCount : " + GameManager.Instance.lDbPassiveSkillOptionIndex.Count);
        Debug.Log("lDbWeapon : " + GameManager.Instance.lDbWeapon.Count);
        Debug.Log("lDBArmor : " + GameManager.Instance.lDBArmor.Count);
        Debug.Log("lDBGlove : " + GameManager.Instance.lDBGlove.Count);
        Debug.Log("lDBAccessory : " + GameManager.Instance.lDBAccessory.Count);
        Debug.Log("lDBEquipmentRandomOption : " + GameManager.Instance.lDBEquipmentRandomOption.Count);
        Debug.Log("lDBStage : " + GameManager.Instance.lDBStageData.Count);
        Debug.Log("lDBCraftMaterial : " + GameManager.Instance.lDBCraftMaterial.Count);
        Debug.Log("lDBBreakMaterial : " + GameManager.Instance.lDBBreakMaterial.Count);
        Debug.Log("lDBFormationSkill : " + GameManager.Instance.lDBFomationSkill.Count);
    }


    //메인 UI 버튼 초기화
    void InitActiveButton()
    {
        //StageButton
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE].onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_STAGE, E_SCENE_INDEX.E_MENU, true));
        //Mercenary Manage(용병관리)
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_MERCENARY_MANAGEMENT].onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_MERMANAGE, E_SCENE_INDEX.E_MENU, true));
        //Mercenary Manage(용병고용)
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT].onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_EMPLOYER, E_SCENE_INDEX.E_MENU, false));
        //치료소
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING].onClick.AddListener(() => StartCoroutine(fadeInOutTest.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING, false)));
        //훈련소
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING].onClick.AddListener(() => StartCoroutine(fadeInOutTest.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING, false)));
    }

    void LoadSprtieAndDispatch()
    {
        someImage.sprite = GameManager.Instance.getSpriteArray[0];
        for (int i = 0; i < nMenuIconCount; i++)
            activeButton[i].image.sprite = GameManager.Instance.getSpriteArray[i];
    }

    public void ActivePanelBack(E_ACTIVEBUTTON _button , bool _isback)
	{
		StartCoroutine (fadeInOutTest.FadeInOutOnce (_button, _isback));

	}
}
