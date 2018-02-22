using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;


public enum E_INVENTORYG_ITEMINFO_ORDER
{
    ITEM_NAME = 0,
    ITEM_ENHANCED,
    SELL_BUTTON,
    COMPOSE_BUTTON,
    EQUIP_BUTTON,
    ABILITY_PANEL,
    INGREDIENT_PANEL,
    DECOMPOSEBLOCK_IMAGE,
}


public class InventoryItemInfo : ButtonUIBase
{
    public Text ItemName_Text;
    public Text Enhanced_Text;
    public Image Sell_Button;
    public Image Compose_Button;
    public Image Equip_Button;
    public InventoryItemAbilityInfoPanel itemAbilityPanel;
    public InventoryItemSellPanel itemSellPanel;
    public InventoryIngredientPanel itemIngredientPanel;
    public InventoryDecomposePanel itemDecomposePanel;

    public Image DecomposeBlock_Image;

    public InventorySlot CurInventorySlot;

    public void Init()
    {
        ItemName_Text = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.ITEM_NAME).GetComponent<Text>();
        Enhanced_Text = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.ITEM_ENHANCED).GetComponent<Text>();
        Sell_Button = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.SELL_BUTTON).GetComponent<Image>();
        Compose_Button = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.COMPOSE_BUTTON).GetComponent<Image>();
        Equip_Button = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.EQUIP_BUTTON).GetComponent<Image>();

        itemAbilityPanel = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.ABILITY_PANEL).gameObject.AddComponent<InventoryItemAbilityInfoPanel>();
        itemAbilityPanel = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.ABILITY_PANEL).GetComponent<InventoryItemAbilityInfoPanel>();
        itemIngredientPanel = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.INGREDIENT_PANEL).gameObject.AddComponent<InventoryIngredientPanel>();
        itemIngredientPanel = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.INGREDIENT_PANEL).GetComponent<InventoryIngredientPanel>();
        DecomposeBlock_Image = this.gameObject.transform.GetChild((int)E_INVENTORYG_ITEMINFO_ORDER.DECOMPOSEBLOCK_IMAGE).GetComponent<Image>();
        itemAbilityPanel.Init();
        itemIngredientPanel.Init();

    }
    
    public void SetInfo(InventorySlot _invetorySlot)
    {
        DecomposeBlock_Image.gameObject.SetActive(false);
        if (CurInventorySlot != null)
        {
            //이전에 선택된 슬롯의 체크 이미지 off
            if(CurInventorySlot != _invetorySlot)
                CurInventorySlot.ActiveImage.gameObject.SetActive(false);

            CurInventorySlot = _invetorySlot;

        }
        else
        {
            CurInventorySlot = _invetorySlot;
        }
        
        //재료 이외의 장비들
        if(_invetorySlot.sItemType != "10")
        {
            //아이템 이름
            ItemName_Text.text = _invetorySlot.equipment.strName;
            //강화 수치
            Enhanced_Text.text = "+ " + _invetorySlot.equipment.nEnhance;
            itemAbilityPanel.gameObject.SetActive(true);
            itemIngredientPanel.gameObject.SetActive(false);
            //옵션 갯수 계산
            int nOptionCount = 0;
            //무기 고정옵 2개
            if (_invetorySlot.equipment.strItemType == "0" || _invetorySlot.equipment.strItemType == "1" ||
                _invetorySlot.equipment.strItemType == "2")
            {
                nOptionCount += 2;
                nOptionCount += GetQualityOptionCount(_invetorySlot);
                itemAbilityPanel.SetAbilitySlot(nOptionCount, CurInventorySlot);
            }
            //방어구 고정옵 3개
            else if (_invetorySlot.equipment.strItemType == "3" || _invetorySlot.equipment.strItemType == "4" ||
                _invetorySlot.equipment.strItemType == "5")
            {
                nOptionCount += 3;
                nOptionCount += GetQualityOptionCount(_invetorySlot);
                itemAbilityPanel.SetAbilitySlot(nOptionCount, CurInventorySlot);
            }
            //장갑 고정옵 2개
            else if (_invetorySlot.equipment.strItemType == "3" || _invetorySlot.equipment.strItemType == "4" ||
                _invetorySlot.equipment.strItemType == "5")
            {
                nOptionCount += 2;
                nOptionCount += GetQualityOptionCount(_invetorySlot);
                itemAbilityPanel.SetAbilitySlot(nOptionCount, CurInventorySlot);
            }
            //장신구 고정옵 1개
            else
            {
                nOptionCount += 1;
                nOptionCount += GetQualityOptionCount(_invetorySlot);
                itemAbilityPanel.SetAbilitySlot(nOptionCount, CurInventorySlot);
            }
        }
        //재료
        else
        {
            //재료가 아니면 아에 안되게 막아놓는다.
            DecomposeBlock_Image.gameObject.SetActive(true);
            //아이템 이름
            ItemName_Text.text = _invetorySlot.ingredient.strName;
            //강화 수치
            Enhanced_Text.text = "";

            itemAbilityPanel.gameObject.SetActive(false);
            itemIngredientPanel.gameObject.SetActive(true);
            itemIngredientPanel.SetInfo(CurInventorySlot.ingredient.sExplanation);
        }
       

       

    }


    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //판매 버튼 
        if (eventData.pointerCurrentRaycast.gameObject.name == Sell_Button.name)
        {
            itemSellPanel.SetUpItemSellPanel(CurInventorySlot);
        }
        //분해 버튼 
        else if (eventData.pointerCurrentRaycast.gameObject.name == Compose_Button.name)
        {
            if (CurInventorySlot.equipment.strItemType != "10")
            {
                itemDecomposePanel.gameObject.SetActive(true);
                itemDecomposePanel.inventoryDecompose.decomposeList.SetSlot(CurInventorySlot.equipment, GetDecomposeSlotCount(CurInventorySlot.equipment.nBreakMaterialIndex));
            }

        }
        //장착 버튼 
        else if (eventData.pointerCurrentRaycast.gameObject.name == Equip_Button.name)
        {

        }
        else
        {
            Debug.Log("Cliked");
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

    }
    #endregion
    //퀄리티에 따른 옵션 개수
    public int GetQualityOptionCount(InventorySlot _slot)
    {
        if (_slot.equipment.nQulity == 0)
            return 0;
        else if (_slot.equipment.nQulity == 1)
            return 1;
        else
            return 2;
    }
    //분해 아이템 슬롯 개수
    public int GetDecomposeSlotCount(int _index)
    {
        int nSlotCount = 0;

        if (GameManager.Instance.lDBBreakMaterial[_index].nIron != 0)
            nSlotCount++;
        if (GameManager.Instance.lDBBreakMaterial[_index].nFabric != 0)
            nSlotCount++;
        if (GameManager.Instance.lDBBreakMaterial[_index].nWood != 0)
            nSlotCount++;
        if (GameManager.Instance.lDBBreakMaterial[_index].nWeaponStone != 0)
            nSlotCount++;
        if (GameManager.Instance.lDBBreakMaterial[_index].nArmorStone != 0)
            nSlotCount++;
        if (GameManager.Instance.lDBBreakMaterial[_index].nAccessoryStone != 0)
            nSlotCount++;

        return nSlotCount;
    }

}
