using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


using ReadOnlys;

#region enums
//메인씬 UI버튼의 인덱스들
public enum E_ACTIVEBUTTON
{
    E_ACTIVEBUTTON_MERCENARY_MANAGEMENT = 0,
    E_ACTIVEBUTTON_INVEN,
    E_ACTIVEBUTTON_BLACKSMITH,
    E_ACTIVEBUTTON_TRAINNING,
    E_ACTIVEBUTTON_HEALING,
    E_ACTIVEBUTTON_EMPLOYMENT,
    E_ACTIVEBUTTON_BOOK,
    E_ACTIVEBUTTON_SHOP,
    E_ACTIVEBUTTON_STAGE,
    E_ACTIVEBUTTON_OPTION,
    E_ACTIVEBUTTON_POST,
    E_ACTIVEBUTTON_CALENDER,
}

enum E_CANVAS_UI_ORDER
{
    E_CANVAS_UI_BACKGROUND = 0,             //배경
    E_CANVAS_UI_ACTIVEBUTTON,               //엑티브 버튼
    E_CANVAS_UI_MERCENARYHEAL,              //치료소
    E_CANVAS_UI_MERCENARYTRAINNING,         //훈련소
    E_CANVAS_UI_MERCENARYEMPLOY,            //용병 소환
    E_CANVAS_UI_STAGE,                      //스테이지
    E_CANVAS_UI_INFO,                       //플레이어 정보
    E_CANVAS_UI_POST,                       //우편
    E_CANVAS_UI_CALENDER,                   //출석 보상
    E_CANVAS_UI_FADEPANEL,                  //사라지는 효과
    E_CANVAS_UI_POSTGETPANEL,               //우편 얻을때 슬롯
}

enum E_MAINSCENE_OBJECTPOOL
{
    E_MAINSCENE_OBJECTPOOL_POSTSLOT = 0,    //우편슬롯
    E_MAINSCENE_OBJECTPOOL_POSTGETSLOT,     //우편 얻을때 사용되는 슬롯
}
#endregion


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

    //SimpleObjectPools
    public SimpleObjectPool[] simpleObjectpools;
    //우편 알림
    public GameObject postExpression;
    private PostPanel postPanel;
    private PostGetPanel postGetPanel;
    //용병고용
    public EmployPanel employPanel;
    //메인메뉴에 있는 아이콘의 개수
    private const int nMenuIconCount = 10;
    //CustomWindow
    private CustomWindowYesNo customWindowYesNo;
    //InfoUI
    public GameObject InfoUI_Obj;
  

    void Awake () 
	{
        //메인 씬 프리팹 배치
        DispatchMenuScenePrefab();
        //저장된 데이터를 제대로 불러왔는지 체크
        //CheckSaveDataIsSure();
    

        //Debug.Log(GameManager.Instance.enemy_archerList);
        
    

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
        Debug.Log("lDBMaterialData : " + GameManager.Instance.lDBMaterialData.Count);
        Debug.Log("lDBCalendar : " + GameManager.Instance.lDBCalendar.Count);
        Debug.Log("lDBCharacterTicket : " + GameManager.Instance.lDBCharacterTicket.Count);
        Debug.Log("lDBWeaponTicket : " + GameManager.Instance.lDBWeaponTicket.Count);
        Debug.Log("lDEmployGacha : " + GameManager.Instance.lDBEmployGacha.Count);

    }

    IEnumerator MainSceneInit()
    {
        yield return new WaitForSeconds(0.1f);

        //페이드 인 아웃 효과를 위한 할당
        fadeInOut = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_FADEPANEL).GetComponent<FadeInOut>();
        fadeInOut.mainSceneManager = this;


        //버튼 할당
        for (int i = 0; i < 12; i++)
            activeButton[i] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_ACTIVEBUTTON).GetChild(i).GetComponent<Button>();


        //Active될 panel Object 할당
        //치료소
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_MERCENARYHEAL).gameObject;
        //훈련소
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_MERCENARYTRAINNING).gameObject;
        //용병 고용소
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_MERCENARYEMPLOY).gameObject;
        //스테이지
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_STAGE).gameObject;
        //우편
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_POST] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_POST).gameObject;
        //달력
        activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_CALENDER] = canvas.transform.GetChild((int)E_CANVAS_UI_ORDER.E_CANVAS_UI_CALENDER).gameObject;


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        //해당 버튼에 따른 function 할당
        //Mercenary Manage(용병관리)
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_MERCENARY_MANAGEMENT].onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_MERMANAGE, E_SCENE_INDEX.E_MENU, true));
        //치료소
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING].onClick.AddListener(() => StartCoroutine(fadeInOut.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING, false)));
        //훈련소
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING].onClick.AddListener(() => StartCoroutine(fadeInOut.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING, false)));
        //용병고용
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT].onClick.AddListener(() => StartCoroutine(fadeInOut.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT, false)));
        //스테이지
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE].onClick.AddListener(() => StartCoroutine(fadeInOut.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE, false)));

        //우편
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_POST].onClick.AddListener(() => ActivePanelNoEffect(activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_POST], E_ACTIVEBUTTON.E_ACTIVEBUTTON_POST));
        postExpression = activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_POST].transform.GetChild(0).gameObject;
        //용병고용
        //activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_SHOP].onClick.AddListener(() => StartCoroutine(fadeInOut.FadeInOutOnce(E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING, false)));


        //달력
        activeButton[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_CALENDER].onClick.AddListener(() => ActivePanelNoEffect(activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_CALENDER], E_ACTIVEBUTTON.E_ACTIVEBUTTON_CALENDER));

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //ObjectPool 할당
       
        for(E_MAINSCENE_OBJECTPOOL objectPool =0; (int)objectPool < 2; objectPool++)
        {
            switch (objectPool)
            {
                case E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTSLOT:
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTSLOT].strPrefabName = "PostSlot";
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTSLOT].prefab = simpleObjectpools[0].gameObject.transform.GetChild(0).gameObject;
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTSLOT].PreloadPool();
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTSLOT].nPoolSize = 20;
                    //유동적으로 바뀔수 있음.
                    postPanel.m_PostSimpleObject = simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTSLOT];
                    break;

                case E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTGETSLOT:
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTGETSLOT].strPrefabName = "PostGetSlot";
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTGETSLOT].prefab = simpleObjectpools[1].gameObject.transform.GetChild(0).gameObject;
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTGETSLOT].PreloadPool();
                    simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTGETSLOT].nPoolSize = 20;
                    postGetPanel.m_PostGetSimpleObject = simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTGETSLOT];
                    break;
                default:
                    break;
            }

        }

        //우편 체크
        postPanel.postGetPanel = postGetPanel;
        //우편 체크 표시 활성화
        if (GameManager.Instance.GetPlayer().mail != null)
        {
            postExpression.SetActive(true);
        }

    }
    //메인 씬 프리팹 배치
    void DispatchMenuScenePrefab()
    {
        int nPrefabHoldCount = GameManager.Instance.prefabHold_Obj.transform.childCount;
        int nEmployCharacterHoldCount = GameManager.Instance.employCharacterHold_Obj.transform.childCount;
        for (int i = 0; i < nPrefabHoldCount; i++)
        {
            GameObject prefab = GameManager.Instance.prefabHold_Obj.transform.GetChild(0).gameObject;
            prefab.transform.SetParent(canvas);
            prefab.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            prefab.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            if (prefab.gameObject.name == "MercenaryEmployPanel")
            {
                employPanel = prefab.gameObject.GetComponent<EmployPanel>();
                employPanel.mainSceneManager = this;
                employPanel.employFinishPanel.mainSceneManager = this;
            }
            if (prefab.gameObject.name == "InfoUI")
                InfoUI_Obj = prefab;

            if (prefab.gameObject.name == "PostPanel")
            {
                postPanel = prefab.gameObject.GetComponent<PostPanel>();
                postGetPanel = prefab.gameObject.transform.GetChild(4).gameObject.GetComponent<PostGetPanel>();
                postGetPanel.postPanel = postPanel;
            }

            if (prefab.gameObject.name == "PostSlot")
            {
                prefab.transform.SetParent(simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTSLOT].transform);
            }

            if (prefab.gameObject.name == "PostGetSlot")
                prefab.transform.SetParent(simpleObjectpools[(int)E_MAINSCENE_OBJECTPOOL.E_MAINSCENE_OBJECTPOOL_POSTGETSLOT].transform);

            if (prefab.gameObject.name == "Custom_YesNo")
            {
                customWindowYesNo = prefab.gameObject.GetComponent<CustomWindowYesNo>();
                customWindowYesNo.employPanel = employPanel;
            }

<<<<<<< HEAD
=======
        }

        for (int i = 0; i < nEmployCharacterHoldCount; i++)
        {
            GameObject prefab = GameManager.Instance.employCharacterHold_Obj.transform.GetChild(0).gameObject;
            prefab.transform.SetParent(employPanel.employFinishPanel.employCharacterHold_Obj.transform);
            prefab.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            prefab.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
>>>>>>> 541b302419a6df2beb66b970948fecc6a12b366b
        }
        //용병 고용에 쓰일 캐릭터 프리팹 처리
        
        for (int i = 0; i < nEmployCharacterHoldCount; i++)
        {
            GameObject prefab = GameManager.Instance.employCharacterHold_Obj.transform.GetChild(0).gameObject;
            prefab.transform.SetParent(employPanel.employFinishPanel.employCharacterHold_Obj.transform);
            prefab.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            prefab.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
        
    }

    //패널을 활성화 할때 아무런 효과 없이 그냥 띄울때 
    public void ActivePanelNoEffect(GameObject _obj , E_ACTIVEBUTTON _activeButton)
    {
        _obj.SetActive(true);  

        switch (_activeButton)
        {
            case E_ACTIVEBUTTON.E_ACTIVEBUTTON_POST:
                postPanel.CheckDataAndDispatchPostSlot();
                break;

            default:
                break;
        }
    }

    //활성화되어 있는 패널이 뒤로 갈때의 효과
    public void ActivePanelBack(E_ACTIVEBUTTON _button , bool _isback)
	{
		StartCoroutine (fadeInOut.FadeInOutOnce (_button, _isback));
	}

    //팝업 창에 대한 셋팅
    public void SetCustomWindow(E_CUSTOMWINDOW _state, string _Title)
    {
        customWindowYesNo.eCurState = _state;
        customWindowYesNo.contents_Text.text = _Title;
        customWindowYesNo.gameObject.SetActive(true);

    }
}
