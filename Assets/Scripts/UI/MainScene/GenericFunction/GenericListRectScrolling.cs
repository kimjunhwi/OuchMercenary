using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class GenericListRectScrolling : ButtonUIBase
{
    //Update를 하면서 계속해서 스크롤을 내릴때 마다 아이템 업데이트
    //Contents 크기 조절 
    protected float fInitRectYValue = 0f;
    protected float fSpencingValueX = 15f;                  //X 간격
    protected float fSpencingValueY = 15f;                  //Y 간격

    protected int nRowCount = 6;                            //행 갯수
    protected int nColCount = 7;                            //열 갯수

    protected float fSlotSizeX = 190;                       //한 슬롯의 크기
    protected float fSlotSizeY = 0;
    protected int nInitItemSlotCount = 42;                  //초기 슬롯 갯수 
    //인벤토리 과련
    public InventorySlot[] inventoryslots;                  //인벤 슬롯
    public InventoryItemInfo inventoryItemInfo;             //인벤 슬롯의 정보를 보여주는 패널
                                                            //List
    protected List<Item> ItemSortInLevel = new List<Item>();
    protected List<Equipment> EquipmentSortInLevel = new List<Equipment>();
    protected List<Ingredient> IngreidentSortInLevel = new List<Ingredient>();
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //용병관리 과련
    public MercenaryCharacterSlot[] mercenaryCharacterSlots;//용병관리 캐릭 슬롯
   
    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int nPlayerTotalItemCount = 0;                   //현재탭에 플레이어의 아이템 갯수

    //아이템 인덱스들
    public int nLowItemCount = 0;                           //스크롤을 올릴때 현재 인덱스를 체크하는 변수
    public int nHighItemCount = 0;                          //스크롤을 내릴때 현재 인덱스를 체크하는 변수

    protected ScrollRect scrollRect;                        
    protected RectTransform rectTransform;

    //스크롤 계산이 필요없을때는 하지 않는다.
    public bool bIsNeedScrolling = false;
    //내리는지 올리는지 체크하는 변수
    public float fLastValue;
    //내릴때 사용되는 변수들 
    public float fPrevHeight;
    public float fCurHeight;
    public int nNextIndex;
    //올릴때 사용되는 변수들
    public bool bIsLastLine = false;                         //마지막줄인지 아닌지 체크하는 변수

    public bool bIsInit = false;                             //초기화 했는지 안했는지.

 
    //초기화
    public virtual void Init() { }
    //패널을 켰을때 처음 아이템의 정보를 설정한다.
    protected virtual void SetFirstItemInfoTo_ItemInfoPanel(InventorySlot _invenSlot) { }
    //ScrollRect에 있는 OnValueChanged에 넣는 함수. (실질적인 스크롤링 체크 함수)
    protected virtual void StartItemScrollingTest(Vector2 value) { }
    //스크롤링 할 리스트 처음에 초기화
    protected virtual void InitList() { }
    //슬롯 위치 재설정(위로올릴지 아닌지, 다음줄에 해당하는 아이템 인덱스)
    protected virtual void RePositionSlots(bool _isUp, int _itemIndex) { }
    //탭 할때마다 맨위로 올라오게 위치 초기화
    public virtual void ContentsListInit() { }
    //정렬
    //레벨
    protected virtual void SortListInLevelOrder() { }
    //강화
    protected virtual void SortListInEnhancedOrder() { }



}
