using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;



public class InventoryPanel : ToggleUIBase
{
    //중복 방지 카운트
    int nToggleCount = 0;
    //현재 열려 있는 인벤토리의 타입
    public E_INVENTORY curInventoryType;
    //인벤토리 탭의 종류의 전체 갯수
    private const int nInventoryTotalCount = 5;

    public InventoryEachTopPanel[] inventoryEachTopPanels;
    //아이템 상세정보창
    public InventoryItemInfo inventoryItemInfo;
    //전체 아이템 탭
    public Inventory_TotalItem inventoryTotalItem;
    //무기 탭
    public Inventory_Weapon inventoryWeapon;
    //방어구 탭
    public Inventory_Armor inventoryArmor;
    //장신구 탭
    public Inventory_Accessory inventoryAccessroy;
    //재료 탭
    public Inventory_Ingredient inventoryIngredient;
    //판매 창
    public InventoryItemSellPanel inventoryItemSellPanel;
    //분해 창
    public InventoryDecomposePanel inventoryDecomposePanel;


    public void Init()
    {
        inventoryEachTopPanels = new InventoryEachTopPanel[nInventoryTotalCount];
        togglePanel = new GameObject[nInventoryTotalCount];
        toggle = new Toggle[nInventoryTotalCount];

        //Debug.Log(GameManager.Instance.GetPlayer().LIST_ITEM.Count);

        for (int i=0, j= 5 ; i < nInventoryTotalCount; i++, j--)
        {
            //각각의 인벤토리 종류 탭의 패널
            inventoryEachTopPanels[i] = this.gameObject.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.AddComponent<InventoryEachTopPanel>();
            inventoryEachTopPanels[i] = this.gameObject.transform.GetChild(j).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<InventoryEachTopPanel>();
            //해당 패널의 리스트 컨텐츠 초기화
            inventoryEachTopPanels[i].Init(i);
            //토글을 하기위한 할당
            togglePanel[i] = inventoryEachTopPanels[i].ContentsPanels;
            toggle[i] = this.gameObject.transform.GetChild(j).gameObject.GetComponent<Toggle>();
        }

        inventoryItemInfo = this.gameObject.transform.GetChild(6).gameObject.AddComponent<InventoryItemInfo>();
        inventoryItemInfo = this.gameObject.transform.GetChild(6).GetComponent<InventoryItemInfo>();
        inventoryItemSellPanel = this.gameObject.transform.GetChild(8).gameObject.AddComponent<InventoryItemSellPanel>();
        inventoryItemSellPanel = this.gameObject.transform.GetChild(8).GetComponent<InventoryItemSellPanel>();
        inventoryDecomposePanel = this.gameObject.transform.GetChild(9).gameObject.AddComponent<InventoryDecomposePanel>();
        inventoryDecomposePanel = this.gameObject.transform.GetChild(9).GetComponent<InventoryDecomposePanel>();
    
        inventoryItemSellPanel.Init();
        inventoryDecomposePanel.Init();

        inventoryItemInfo.itemSellPanel = inventoryItemSellPanel;
        inventoryItemInfo.itemDecomposePanel = inventoryDecomposePanel;

        inventoryItemInfo.Init();
        InitToggle();   
    }


    //각각의 캐릭터 패널들 초기화
    public void InitToggle()
    {
        //1.전체, 2. 무기, 3. 방어구, 4. 장갑, 5. 장신구


        toggle[(int)E_INVENTORY.E_INVENTORY_TOTAL].onValueChanged.AddListener(
            (x) => ActivePanel(E_INVENTORY.E_INVENTORY_TOTAL));

        toggle[(int)E_INVENTORY.E_INVENTORY_WEAPON].onValueChanged.AddListener(
            (x) => ActivePanel(E_INVENTORY.E_INVENTORY_WEAPON));

        toggle[(int)E_INVENTORY.E_INVENTORY_ARMOR].onValueChanged.AddListener(
            (x) => ActivePanel(E_INVENTORY.E_INVENTORY_ARMOR));

        toggle[(int)E_INVENTORY.E_INVENTORY_ACCESSORY].onValueChanged.AddListener(
            (x) => ActivePanel(E_INVENTORY.E_INVENTORY_ACCESSORY));

        toggle[(int)E_INVENTORY.E_INVENTORY_INGREDIENT].onValueChanged.AddListener(
            (x) => ActivePanel(E_INVENTORY.E_INVENTORY_INGREDIENT));

        //실질적인 아이템 탭 할당
        //전체
        inventoryTotalItem = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_TOTAL].gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.AddComponent<Inventory_TotalItem>();
        inventoryTotalItem = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_TOTAL].gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Inventory_TotalItem>();
        inventoryTotalItem.inventoryItemInfo = inventoryItemInfo;
        inventoryTotalItem.Init();
        ActivePanel(E_INVENTORY.E_INVENTORY_TOTAL);
        //무기
        inventoryWeapon = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_WEAPON].gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.AddComponent<Inventory_Weapon>();
        inventoryWeapon = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_WEAPON].gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Inventory_Weapon>();
        inventoryWeapon.inventoryItemInfo = inventoryItemInfo;
        //방어구
        inventoryArmor = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_ARMOR].gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.AddComponent<Inventory_Armor>();
        inventoryArmor = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_ARMOR].gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Inventory_Armor>();
        inventoryArmor.inventoryItemInfo = inventoryItemInfo;
        //장신구
        inventoryAccessroy = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_ACCESSORY].gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.AddComponent<Inventory_Accessory>();
        inventoryAccessroy = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_ACCESSORY].gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Inventory_Accessory>();
        inventoryAccessroy.inventoryItemInfo = inventoryItemInfo;
        //재료
        inventoryIngredient = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_INGREDIENT].gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.AddComponent<Inventory_Ingredient>();
        inventoryIngredient = inventoryEachTopPanels[(int)E_INVENTORY.E_INVENTORY_INGREDIENT].gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Inventory_Ingredient>();
        inventoryIngredient.inventoryItemInfo = inventoryItemInfo;

        //inventoryTotalItem.StartItemListScrolling();
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

        var eType = Enum.Parse(typeof(E_INVENTORY), _chapterIndex.ToString());
        nToggleCount++;

        switch ((E_INVENTORY)eType)
        {
            case E_INVENTORY.E_INVENTORY_TOTAL:
               

                curInventoryType = E_INVENTORY.E_INVENTORY_TOTAL;

                Debug.Log("Active TotalItem Panel!!");
                ExceptSpecificPanelAllDeActive(curInventoryType);
                if (inventoryTotalItem.bIsInit == true)
                    inventoryTotalItem.ContentsListInit();
                //탭을 할때 첫번째 아이템을 무조건적으로 오른쪽에 보여준다.

                break;

            case E_INVENTORY.E_INVENTORY_WEAPON:


                curInventoryType = E_INVENTORY.E_INVENTORY_WEAPON;

                Debug.Log("Active Weapon Panel!!");
                ExceptSpecificPanelAllDeActive(curInventoryType);
                if (inventoryWeapon.bIsInit == true)
                    inventoryWeapon.ContentsListInit();
                else
                    inventoryWeapon.Init();
             
                

                break;

            case E_INVENTORY.E_INVENTORY_ARMOR:

                curInventoryType = E_INVENTORY.E_INVENTORY_ARMOR;

                Debug.Log("Active Armor Panel!!");
                ExceptSpecificPanelAllDeActive(curInventoryType);
          
                if (inventoryArmor.bIsInit == true)
                    inventoryArmor.ContentsListInit();
                else
                    inventoryArmor.Init();

                break;

            case E_INVENTORY.E_INVENTORY_ACCESSORY:

                curInventoryType = E_INVENTORY.E_INVENTORY_ACCESSORY;

                Debug.Log("Active Accessory Panel!!");
                ExceptSpecificPanelAllDeActive(curInventoryType);
                if (inventoryAccessroy.bIsInit == true)
                    inventoryAccessroy.ContentsListInit();
                else
                    inventoryAccessroy.Init();

                break;

            case E_INVENTORY.E_INVENTORY_INGREDIENT:

                curInventoryType = E_INVENTORY.E_INVENTORY_INGREDIENT;

                Debug.Log("Active Ingredient Panel!!");
                ExceptSpecificPanelAllDeActive(curInventoryType);
                if (inventoryIngredient.bIsInit == true)
                    inventoryIngredient.ContentsListInit();
                else
                    inventoryIngredient.Init();

                break;

            default:
                break;

        }

    }
    //특정 패널을 제외하고 모든 탭 비활성화
    public void ExceptSpecificPanelAllDeActive(E_INVENTORY _inventoryType)
    {
        for(int nTogglePanelIndex=0; nTogglePanelIndex < nInventoryTotalCount; nTogglePanelIndex++)
        {
            if (nTogglePanelIndex == (int)_inventoryType)
                inventoryEachTopPanels[nTogglePanelIndex].gameObject.SetActive(true);
            else
                inventoryEachTopPanels[nTogglePanelIndex].gameObject.SetActive(false);
        }
    }

}
