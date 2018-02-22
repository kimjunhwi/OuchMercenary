using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;



public class MercenaryManageCharacterInfo : ToggleUIBase
{

    //중복 방지 카운트
    int nToggleCount = 0;
    //현재 열려 있는 인벤토리의 타입
    public E_MERCENARYMANAGE curMercenaryType;
    //용병 탭의 종류의 전체 갯수
    private const int nMercenaryTotalCount = 5;

    //용병 탭에있는 각각의 중간 패널.
    public MercenaryEachTopPanel[] mercenaryEachTopPanel;
    //각각의 탭들
    //전체 캐릭터 탭
    public MercenaryTotalCharacter mercenaryTotalCharacter;
    //캐릭터 상세 정보 창.
    public MercenaryDetailInfoPanel mercenaryDetailInfoPanel;

    


    public void Init()
    {
        mercenaryEachTopPanel = new MercenaryEachTopPanel[nMercenaryTotalCount];
        togglePanel = new GameObject[nMercenaryTotalCount];
        toggle = new Toggle[nMercenaryTotalCount];


        for (int i = 0, j = 4; i < nMercenaryTotalCount; i++, j--)
        {
            //각각의 인벤토리 종류 탭의 패널
            //mercenaryEachTopPanel[i] = this.gameObject.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.AddComponent<MercenaryEachTopPanel>();
            mercenaryEachTopPanel[i] = this.gameObject.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<MercenaryEachTopPanel>();
            //해당 패널의 리스트 컨텐츠 초기화
            mercenaryEachTopPanel[i].Init(i);
            //토글을 하기위한 할당
            togglePanel[i] = mercenaryEachTopPanel[i].ContentsPanels;
            toggle[i] = this.gameObject.transform.GetChild(j).gameObject.GetComponent<Toggle>();
        }

        mercenaryDetailInfoPanel = this.gameObject.transform.GetChild(6).GetComponent<MercenaryDetailInfoPanel>();
        mercenaryDetailInfoPanel.Init();

        InitToggle();
    }


	//각각의 캐릭터 패널들 초기화
	public void InitToggle()
	{
		//1.전체, 2. 지휘관, 3. 근거리, 4. 원거리, 5. 즐겨찾기
		
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER);
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE);
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE);
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE);

		toggle[(int)E_MERCENARYMANAGE.TOTAL].onValueChanged.AddListener(
			(x)=>ActivePanel(E_MERCENARYMANAGE.TOTAL));

		toggle[(int)E_MERCENARYMANAGE.COMMANDER].onValueChanged.AddListener(
			(x)=>ActivePanel(E_MERCENARYMANAGE.COMMANDER));

		toggle[(int)E_MERCENARYMANAGE.MELEE].onValueChanged.AddListener(
			(x)=>ActivePanel(E_MERCENARYMANAGE.MELEE));

		toggle[(int)E_MERCENARYMANAGE.RANGE].onValueChanged.AddListener(
			(x)=>ActivePanel(E_MERCENARYMANAGE.RANGE));

		toggle[(int)E_MERCENARYMANAGE.FAVOIRTE].onValueChanged.AddListener(
			(x)=>ActivePanel(E_MERCENARYMANAGE.FAVOIRTE));
        

		Debug.Log("init Toggle!!");


        //실질적인 아이템 탭 할당
        //전체
        //mercenaryTotalCharacter = mercenaryEachTopPanel[(int)E_MERCENARYMANAGE.TOTAL].gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.AddComponent<MercenaryTotalCharacter>();
        mercenaryTotalCharacter = mercenaryEachTopPanel[(int)E_INVENTORY.E_INVENTORY_TOTAL].gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<MercenaryTotalCharacter>();
        mercenaryTotalCharacter.Init();
        ActivePanel(E_MERCENARYMANAGE.TOTAL);
        
    }


    //각각의 패널 활성화
    public override void ActivePanel<T>(T _chapterIndex)
    {
        //중복 호출 방지
        if (nToggleCount == 1)
        {
            nToggleCount = 0;
            return;
        }

        var eType = Enum.Parse(typeof(E_MERCENARYMANAGE), _chapterIndex.ToString());
        nToggleCount++;

        switch ((E_MERCENARYMANAGE)eType)
        {
            case E_MERCENARYMANAGE.TOTAL:


                curMercenaryType = E_MERCENARYMANAGE.TOTAL;
                ExceptSpecificPanelAllDeActive(curMercenaryType);
                Debug.Log("Active TotalCharacter Panel!!");
               
                break;

            case E_MERCENARYMANAGE.COMMANDER:


                curMercenaryType = E_MERCENARYMANAGE.COMMANDER;
                ExceptSpecificPanelAllDeActive(curMercenaryType);
                Debug.Log("Active Commander Panel!!");
             


                break;

            case E_MERCENARYMANAGE.MELEE:

                curMercenaryType = E_MERCENARYMANAGE.MELEE;
                ExceptSpecificPanelAllDeActive(curMercenaryType);
                Debug.Log("Active Melee Panel!!");
              
                break;

            case E_MERCENARYMANAGE.RANGE:

                curMercenaryType = E_MERCENARYMANAGE.RANGE;
                ExceptSpecificPanelAllDeActive(curMercenaryType);
                Debug.Log("Active Range Panel!!");
           

                break;

            case E_MERCENARYMANAGE.FAVOIRTE:

                curMercenaryType = E_MERCENARYMANAGE.FAVOIRTE;
                ExceptSpecificPanelAllDeActive(curMercenaryType);
                Debug.Log("Active Favorite Panel!!");
           
                break;

            default:
                break;

        }

    }
    //특정 패널을 제외하고 모든 탭 비활성화
    public void ExceptSpecificPanelAllDeActive(E_MERCENARYMANAGE _mercenaryManage)
    {
        for (int nTogglePanelIndex = 0; nTogglePanelIndex < nMercenaryTotalCount; nTogglePanelIndex++)
        {
            if (nTogglePanelIndex == (int)_mercenaryManage)
                mercenaryEachTopPanel[nTogglePanelIndex].gameObject.SetActive(true);
            else
                mercenaryEachTopPanel[nTogglePanelIndex].gameObject.SetActive(false);
        }
    }

}
