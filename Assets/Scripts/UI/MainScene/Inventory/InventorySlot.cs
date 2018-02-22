using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;



//각각의 아이템의 정보를 담는 역할
public class InventorySlot : ButtonUIBase
{
    public Item item;
    public Equipment equipment;
    public Ingredient ingredient;
    public Image ActiveImage;                                   //선택된 이미지
    public Image itemImage;                                     //아이템 이미지
    public Image Quality_Image;                                 //퀄리티 이미지
    public Text Enhanced_Text;                                  //강화정도 텍스트
    public Text Count_Text;                                     //개수 텍스트
    public InventoryItemInfo inventoryItemInfo;
    public Vector2 initSlotPosition;

    public string sItemType;

    public void Init()
    {
        equipment = new Equipment();
        ingredient = new Ingredient();
        ActiveImage = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        itemImage = this.gameObject.transform.GetChild(1).GetComponent<Image>();
        Quality_Image = this.gameObject.transform.GetChild(2).GetComponent<Image>();
        Enhanced_Text = this.gameObject.transform.GetChild(3).GetComponent<Text>();
        Count_Text = this.gameObject.transform.GetChild(4).GetComponent<Text>();
        initSlotPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    public void SetItemInfo_ALLItem(Item _item)
    {
     
        //재료 이외의 것.
        if (_item.strItemType != "10")
        {
            Equipment equipment_Get = (Equipment)_item;

            equipment.nIndex = equipment_Get.nIndex;
            equipment.strName = equipment_Get.strName;
            equipment.nTier = equipment_Get.nTier;
            equipment.nQulity = equipment_Get.nQulity;
            equipment.strPossibleJob = equipment_Get.strPossibleJob;
            equipment.nEnhance = equipment_Get.nEnhance;
            equipment.strItemType = equipment_Get.strItemType;
            equipment.fPhysical_Attack_Rating = equipment_Get.fPhysical_Attack_Rating;
            equipment.fMagic_Attack_Rating = equipment_Get.fMagic_Attack_Rating;
            equipment.fHp = equipment_Get.fHp;
            equipment.fAccuracy = equipment_Get.fAccuracy;
            equipment.fAll_Attack_RatingPlus = equipment_Get.fAll_Attack_RatingPlus;
            equipment.fPysical_Attack_RatingPlus = equipment_Get.fPysical_Attack_RatingPlus;
            equipment.fMagic_Attack_RatingPlus = equipment_Get.fMagic_Attack_RatingPlus;
            equipment.fAll_Penetrate = equipment_Get.fAll_Penetrate;
            equipment.fPhysical_Penetrate = equipment_Get.fPhysical_Penetrate;
            equipment.fMagic_Penetrate = equipment_Get.fMagic_Penetrate;
            equipment.fAll_Defense = equipment_Get.fAll_Defense;
            equipment.fPhysical_Defense = equipment_Get.fPhysical_Defense;
            equipment.fMagic_Defense = equipment_Get.fMagic_Defense;
            equipment.fDodge = equipment_Get.fDodge;
            equipment.fCritical_Rating = equipment_Get.fCritical_Rating;
            equipment.fCritical_Damage = equipment_Get.fCritical_Damage;
            equipment.fAttack_Speed = equipment_Get.fAttack_Speed;
            equipment.fCoolTime = equipment_Get.fCoolTime;
            equipment.fExpBoost = equipment_Get.fExpBoost;
            equipment.nSellCost = equipment_Get.nSellCost;
            equipment.nMakeMaterialIndex = equipment_Get.nMakeMaterialIndex;
            equipment.nBreakMaterialIndex = equipment_Get.nBreakMaterialIndex;
            equipment.sImage = equipment_Get.sImage;
            equipment.nSelected = equipment_Get.nSelected;
            equipment.nListIndex = equipment_Get.nListIndex;

            //이미지 할당.
            ////무기
            if (equipment_Get.strItemType == "0" || equipment_Get.strItemType == "1" || equipment_Get.strItemType == "2")
                //임시로 4가지 이미지를 돌려쓰자.
                itemImage.sprite = GameManager.Instance.Equipment_WeaponSpriteList.Find(x => x.name == equipment_Get.sImage);

            //방어구
            else if (equipment_Get.strItemType == "3" || equipment_Get.strItemType == "4" || equipment_Get.strItemType == "5")
                itemImage.sprite = GameManager.Instance.Equipment_ArmorSpriteList.Find(x => x.name == equipment_Get.sImage);

            //장갑
            else if (equipment_Get.strItemType == "6" || equipment_Get.strItemType == "7" || equipment_Get.strItemType == "8")
                itemImage.sprite = GameManager.Instance.Equipment_GloveSpriteList.Find(x => x.name == equipment_Get.sImage);

            //장신구
            else
                itemImage.sprite = GameManager.Instance.Equipment_AccessorySpriteList.Find(x => x.name == equipment_Get.sImage);

            //퀄리티 등급에 따른 스프라이트 할당.
            Quality_Image.sprite = GameManager.Instance.Equipment_QualitySpriteList[equipment_Get.nQulity];

            Quality_Image.gameObject.SetActive(true);
            //강화가 0이라면 표시를 하지 않는다.
            if (equipment_Get.nEnhance == 0)
                Enhanced_Text.text = "";
            else
                Enhanced_Text.text = string.Format("+{0}", equipment_Get.nEnhance);

            Count_Text.text = "";

            sItemType = equipment_Get.strItemType;
        }
        else
        {
            Ingredient ingredient_Get = (Ingredient)_item;

            ingredient.nIndex = ingredient_Get.nIndex;
            ingredient.strName = ingredient_Get.strName;
            ingredient.sImage = ingredient_Get.sImage;
            ingredient.nCount = ingredient_Get.nCount;
            ingredient.sExplanation = ingredient_Get.sExplanation;
            ingredient.nSelected = ingredient_Get.nSelected;
            ingredient.nSellCost = ingredient_Get.nSellCost;
            ingredient.nListIndex = ingredient_Get.nListIndex;


            itemImage.sprite = GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == ingredient_Get.sImage);
            Quality_Image.gameObject.SetActive(false);
            Enhanced_Text.text = "";
            Count_Text.text = string.Format("{0}",ingredient_Get.nCount);
            sItemType = ingredient_Get.strItemType;
        }

    }

    public void SetItemInfo_Ingredient(Ingredient _item)
    {
        ingredient.nIndex = _item.nIndex;
        ingredient.strName = _item.strName;
        ingredient.sImage = _item.sImage;
        ingredient.nCount = _item.nCount;
        ingredient.sExplanation = _item.sExplanation;
        ingredient.nSelected = _item.nSelected;
        ingredient.nSellCost = _item.nSellCost;
        ingredient.nListIndex = _item.nListIndex;

        
        itemImage.sprite = GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == _item.sImage);
        Quality_Image.gameObject.SetActive(false);
        Enhanced_Text.text = "";
        Count_Text.text = string.Format("{0}", _item.nCount);
        sItemType = _item.strItemType;
    }

    public void SetItemInfo_Equipment(Equipment _item)
    {
        equipment.nIndex = _item.nIndex;
        equipment.strName = _item.strName;
        equipment.nTier = _item.nTier;
        equipment.nQulity = _item.nQulity;
        equipment.strPossibleJob = _item.strPossibleJob;
        equipment.nEnhance = _item.nEnhance;
        equipment.strItemType = _item.strItemType;
        equipment.fPhysical_Attack_Rating = _item.fPhysical_Attack_Rating;
        equipment.fMagic_Attack_Rating = _item.fMagic_Attack_Rating;
        equipment.fHp = _item.fHp;
        equipment.fAccuracy = _item.fAccuracy;
        equipment.fAll_Attack_RatingPlus = _item.fAll_Attack_RatingPlus;
        equipment.fPysical_Attack_RatingPlus = _item.fPysical_Attack_RatingPlus;
        equipment.fMagic_Attack_RatingPlus = _item.fMagic_Attack_RatingPlus;
        equipment.fAll_Penetrate = _item.fAll_Penetrate;
        equipment.fPhysical_Penetrate = _item.fPhysical_Penetrate;
        equipment.fMagic_Penetrate = _item.fMagic_Penetrate;
        equipment.fAll_Defense = _item.fAll_Defense;
        equipment.fPhysical_Defense = _item.fPhysical_Defense;
        equipment.fMagic_Defense = _item.fMagic_Defense;
        equipment.fDodge = _item.fDodge;
        equipment.fCritical_Rating = _item.fCritical_Rating;
        equipment.fCritical_Damage = _item.fCritical_Damage;
        equipment.fAttack_Speed = _item.fAttack_Speed;
        equipment.fCoolTime = _item.fCoolTime;
        equipment.fExpBoost = _item.fExpBoost;
        equipment.nSellCost = _item.nSellCost;
        equipment.nMakeMaterialIndex = _item.nMakeMaterialIndex;
        equipment.nBreakMaterialIndex = _item.nBreakMaterialIndex;
        equipment.sImage = _item.sImage;
        equipment.nSelected = _item.nSelected;
        equipment.nListIndex = _item.nListIndex;

        Quality_Image.gameObject.SetActive(true);
        //이미지 할당.
        ////무기
        if (equipment.strItemType == "0" || equipment.strItemType == "1" || equipment.strItemType == "2")
            //임시로 4가지 이미지를 돌려쓰자.
            itemImage.sprite = GameManager.Instance.Equipment_WeaponSpriteList.Find(x => x.name == equipment.sImage);
     
        //방어구
        else if (equipment.strItemType == "3" || equipment.strItemType == "4" || equipment.strItemType == "5")
            itemImage.sprite = GameManager.Instance.Equipment_ArmorSpriteList.Find(x => x.name == equipment.sImage);
    
        //장갑
        else if (equipment.strItemType == "6" || equipment.strItemType == "7" || equipment.strItemType == "8")
            itemImage.sprite = GameManager.Instance.Equipment_GloveSpriteList.Find(x => x.name == equipment.sImage);
      
        //장신구
        else
            itemImage.sprite = GameManager.Instance.Equipment_AccessorySpriteList.Find(x => x.name == equipment.sImage);

        //퀄리티 등급에 따른 스프라이트 할당.
        Quality_Image.sprite = GameManager.Instance.Equipment_QualitySpriteList[equipment.nQulity];

        //강화가 0이라면 표시를 하지 않는다.
        if (equipment.nEnhance == 0)
            Enhanced_Text.text = "";
        else
            Enhanced_Text.text = string.Format("+{0}", equipment.nEnhance);

        Count_Text.text = "";
        sItemType = _item.strItemType;
    }
   

    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.dragging == false)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == itemImage.name)
            {

                ActiveImage.gameObject.SetActive(true);
                inventoryItemInfo.SetInfo(this);
            }
            else
            {
                Debug.Log("Cliked");
            }
        }
        //해당 슬롯의 이미지를 누를시.
      
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Cliked");
    }

  


    #endregion
}
