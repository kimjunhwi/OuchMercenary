using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ReadOnlys;

enum E_CANVAS_UI_ORDER
{
    E_CANVAS_UI_BACKGROUND = 0,
    E_CANVAS_UI_ACTIVEBUTTON,
    E_CANVAS_UI_MERCENARYHEAL,
    E_CANVAS_UI_MERCENARYTRAINNING,
    E_CANVAS_UI_INFO,
    E_CANVAS_UI_FADEPANEL,
}


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
    public FadeInOut fadeInOut;

    //메인메뉴에 있는 아이콘의 개수
    private const int nMenuIconCount = 10;

    void Awake () 
	{
        //메인 씬 프리팹 배치
        DispatchMenuScenePrefab();
        //저장된 데이터를 제대로 불러왔는지 체크
        //CheckSaveDataIsSure();

        //테스트용(캐릭터들 임시로 불러옴)
        //StartCoroutine(GameManager.Instance.LoadAnimationFromAssetBundel("Assets/AssetBundles/character"));
	}

    private void Start()
    {
        //MainScene에 있는 각종 변수 할당
        StartCoroutine(MainSceneInit());
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

    IEnumerator MainSceneInit()
    {
        yield return new WaitForSeconds(0.1f);
           

        //페이드 인 아웃 효과를 위한 할당
        fadeInOut = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_FADEPANEL).GetComponent<FadeInOut>();
        fadeInOut.mainSceneManager = this;

        //버튼 할당
        for (int i = 0; i < 10; i++)
            activeButton[i] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_ACTIVEBUTTON).GetChild(i).GetComponent<Button>();

        //해당 버튼에 따른 function 할당
        //StageButton
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE].onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_STAGE, E_SCENE_INDEX.E_MENU, true));
        //Mercenary Manage(용병관리)
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_MERCENARY_MANAGEMENT].onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_MERMANAGE, E_SCENE_INDEX.E_MENU, true));
        //Mercenary Manage(용병고용)
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT].onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_EMPLOYER, E_SCENE_INDEX.E_MENU, false));
        //치료소
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING].onClick.AddListener(() => StartCoroutine(fadeInOut.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING, false)));
        //훈련소
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING].onClick.AddListener(() => StartCoroutine(fadeInOut.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING, false)));

        //Active될 panel 할당
        //치료소
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_MERCENARYHEAL).gameObject;

        //훈련소
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_MERCENARYTRAINNING).gameObject;


    }
    //메인 씬 프리팹 배치
    void DispatchMenuScenePrefab()
    {
        int nPrefabHoldCount = GameManager.Instance.prefabHold_Obj.transform.childCount;
        for (int i = 0; i < nPrefabHoldCount; i++)
        {
            GameObject prefab = GameManager.Instance.prefabHold_Obj.transform.GetChild(0).gameObject;
            prefab.transform.SetParent(canvas);
            prefab.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            prefab.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }
    

    public void ActivePanelBack(E_ACTIVEBUTTON _button , bool _isback)
	{
		StartCoroutine (fadeInOut.FadeInOutOnce (_button, _isback));
	}
}
