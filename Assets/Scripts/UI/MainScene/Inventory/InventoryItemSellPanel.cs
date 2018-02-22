using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ReadOnlys;

public class InventoryItemSellPanel : ButtonUIBase
{

    public Image YesButton_Image;
    public Image NoButton_Image;
    public Image CloseButton_Image;
    public Text SellCost_Text;

    InventorySlot curSlot;
    public InventoryPanel inventoryPanel;

    public void Init()
    {
        YesButton_Image = this.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        NoButton_Image = this.gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        CloseButton_Image = this.gameObject.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>();
        SellCost_Text = this.gameObject.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
        inventoryPanel = this.gameObject.transform.GetComponentInParent<InventoryPanel>();
    }

    public void SetUpItemSellPanel(InventorySlot _slot)
    {
        curSlot = _slot;
        this.gameObject.SetActive(true);
        SellCost_Text.text = string.Format("{0}G", curSlot.equipment.nSellCost) ;
    }

    public void ReSortItem()
    {
        if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_TOTAL)
            inventoryPanel.inventoryTotalItem.ContentsListInit();
        else if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_WEAPON)
            inventoryPanel.inventoryWeapon.ContentsListInit();
        else if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_ARMOR)
            inventoryPanel.inventoryArmor.ContentsListInit();
        else if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_ACCESSORY)
            inventoryPanel.inventoryAccessroy.ContentsListInit();
        else 
            inventoryPanel.inventoryIngredient.ContentsListInit();
    }

    #region events
    public override void OnPointerDown(PointerEventData eventData)
    {}
    public override void OnPointerUp(PointerEventData eventData)
    {
        //확인버튼
        if(eventData.pointerCurrentRaycast.gameObject.name == YesButton_Image.name)
        {
            if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_TOTAL)
            {
                //삭제한 아이템 이후 부터 리스트를 이어붙여서 보여준다.
                List<Item> GetAddList = new List<Item>();
                //삭제할 개체 부터 리스트에 임시로 넣는다.
                for (int i = curSlot.equipment.nListIndex + 1; i < GameManager.Instance.GetPlayer().LIST_ITEM.Count; i++)
                    GetAddList.Add(GameManager.Instance.GetPlayer().LIST_ITEM[i]);
                GameManager.Instance.GetPlayer().LIST_ITEM.RemoveRange(curSlot.equipment.nListIndex, GetAddList.Count);
                GameManager.Instance.GetPlayer().LIST_ITEM.RemoveAt(curSlot.equipment.nListIndex);
                GameManager.Instance.GetPlayer().LIST_ITEM.InsertRange(curSlot.equipment.nListIndex, GetAddList);
            }
            else if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_WEAPON)
            {
                //삭제한 아이템 이후 부터 리스트를 이어붙여서 보여준다.
                List<Equipment> GetAddList = new List<Equipment>();
                //삭제할 개체 부터 리스트에 임시로 넣는다.
                for (int i = curSlot.equipment.nListIndex + 1; i < GameManager.Instance.GetPlayer().LIST_ITEM_WEAPON.Count; i++)
                    GetAddList.Add(GameManager.Instance.GetPlayer().LIST_ITEM_WEAPON[i]);
                GameManager.Instance.GetPlayer().LIST_ITEM_WEAPON.RemoveRange(curSlot.equipment.nListIndex, GetAddList.Count);
                GameManager.Instance.GetPlayer().LIST_ITEM_WEAPON.RemoveAt(curSlot.equipment.nListIndex);
                GameManager.Instance.GetPlayer().LIST_ITEM_WEAPON.InsertRange(curSlot.equipment.nListIndex, GetAddList);
            }
            else if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_ARMOR)
            {
                //삭제한 아이템 이후 부터 리스트를 이어붙여서 보여준다.
                List<Equipment> GetAddList = new List<Equipment>();
                //삭제할 개체 부터 리스트에 임시로 넣는다.
                for (int i = curSlot.equipment.nListIndex + 1; i < GameManager.Instance.GetPlayer().LIST_TIEM_ARMOR.Count; i++)
                    GetAddList.Add(GameManager.Instance.GetPlayer().LIST_TIEM_ARMOR[i]);
                GameManager.Instance.GetPlayer().LIST_TIEM_ARMOR.RemoveRange(curSlot.equipment.nListIndex, GetAddList.Count);
                GameManager.Instance.GetPlayer().LIST_TIEM_ARMOR.RemoveAt(curSlot.equipment.nListIndex);
                GameManager.Instance.GetPlayer().LIST_TIEM_ARMOR.InsertRange(curSlot.equipment.nListIndex, GetAddList);
            }
            else if (inventoryPanel.curInventoryType == ReadOnlys.E_INVENTORY.E_INVENTORY_ACCESSORY)
            {
                //삭제한 아이템 이후 부터 리스트를 이어붙여서 보여준다.
                List<Equipment> GetAddList = new List<Equipment>();
                //삭제할 개체 부터 리스트에 임시로 넣는다.
                for (int i = curSlot.equipment.nListIndex + 1; i < GameManager.Instance.GetPlayer().LIST_ITEM_ACCESSORY.Count; i++)
                    GetAddList.Add(GameManager.Instance.GetPlayer().LIST_ITEM_ACCESSORY[i]);
                GameManager.Instance.GetPlayer().LIST_ITEM_ACCESSORY.RemoveRange(curSlot.equipment.nListIndex, GetAddList.Count);
                GameManager.Instance.GetPlayer().LIST_ITEM_ACCESSORY.RemoveAt(curSlot.equipment.nListIndex);
                GameManager.Instance.GetPlayer().LIST_ITEM_ACCESSORY.InsertRange(curSlot.equipment.nListIndex, GetAddList);
            }
            else
            {
                //삭제한 아이템 이후 부터 리스트를 이어붙여서 보여준다.
                List<Ingredient> GetAddList = new List<Ingredient>();
                //삭제할 개체 부터 리스트에 임시로 넣는다.
                for (int i = curSlot.equipment.nListIndex + 1; i < GameManager.Instance.GetPlayer().LIST_ITEM_ACCESSORY.Count; i++)
                    GetAddList.Add(GameManager.Instance.GetPlayer().LIST_ITEM_INGREDIENT[i]);
                GameManager.Instance.GetPlayer().LIST_ITEM_INGREDIENT.RemoveRange(curSlot.equipment.nListIndex, GetAddList.Count);
                GameManager.Instance.GetPlayer().LIST_ITEM_INGREDIENT.RemoveAt(curSlot.equipment.nListIndex);
                GameManager.Instance.GetPlayer().LIST_ITEM_INGREDIENT.InsertRange(curSlot.equipment.nListIndex, GetAddList);
            }

            curSlot = null;
            ReSortItem();
            this.gameObject.SetActive(false);
        }
        //아니요 버튼(캔슬)
        else if(eventData.pointerCurrentRaycast.gameObject.name == NoButton_Image.name)
        {
            this.gameObject.SetActive(false);
        }
        //캔슬 버튼
        else if(eventData.pointerCurrentRaycast.gameObject.name == CloseButton_Image.name)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Clicked");
        }
    }
    public override void OnPointerExit(PointerEventData eventData) { }
    #endregion

}
