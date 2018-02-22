using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ReadOnlys;

public enum  BREAKMATERIALORDER
{
    IRON = 0,
    FABRIC,
    WOOD ,
    WEAPONSTONE,
    ARMORSTONE,
    ACCESSORY,
}


public class InventoryDecomposeList : MonoBehaviour
{
    public InventoryDecomposeSlot[] DecomposeSlots;
    public bool[] bMaterialCheck;

    public void Init()
    {
        DecomposeSlots = new InventoryDecomposeSlot[this.gameObject.transform.childCount];
        bMaterialCheck = new bool[this.gameObject.transform.childCount];
        for (int i=0; i< this.gameObject.transform.childCount; i++)
        {
            DecomposeSlots[i] = this.gameObject.transform.GetChild(i).gameObject.AddComponent<InventoryDecomposeSlot>();
            DecomposeSlots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventoryDecomposeSlot>();
            DecomposeSlots[i].Init();
        }
    }

    public void SetSlot(Equipment _equip ,int _count)
    {
      
        //꺼져있는 슬롯을 다시 킨다.
        for (int i = 0; i < 6; i++)
        {
            DecomposeSlots[i].gameObject.SetActive(true);
            bMaterialCheck[i] = false;
        }

        float fInitXPosition = 0;

        switch (_count)
        {
            case 1:
                fInitXPosition = 0;
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    if (i < _count)
                    {
                        DecomposeSlots[i].gameObject.SetActive(true);
                        RectTransform rectTransform = DecomposeSlots[i].transform.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(fInitXPosition, rectTransform.anchoredPosition.y);

                        DBBreakMaterial breakMaterial = GameManager.Instance.lDBBreakMaterial[_equip.nBreakMaterialIndex];
                        SetMaterailImage(DecomposeSlots[i],breakMaterial);
                    }
                    else
                        DecomposeSlots[i].gameObject.SetActive(false);
                }
                break;
            case 2:
                fInitXPosition = -65f;
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    if (i < _count)
                    {
                        DecomposeSlots[i].gameObject.SetActive(true);
                        RectTransform rectTransform = DecomposeSlots[i].transform.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(fInitXPosition, rectTransform.anchoredPosition.y);

                        DBBreakMaterial breakMaterial = GameManager.Instance.lDBBreakMaterial[_equip.nBreakMaterialIndex];
                        SetMaterailImage(DecomposeSlots[i], breakMaterial);

                        fInitXPosition += 130f;
                    }
                    else
                        DecomposeSlots[i].gameObject.SetActive(false);
                }
                break;
            case 3:
                fInitXPosition = -130f;
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    if (i < _count)
                    {
                        DecomposeSlots[i].gameObject.SetActive(true);
                        RectTransform rectTransform = DecomposeSlots[i].transform.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(fInitXPosition, rectTransform.anchoredPosition.y);

                        DBBreakMaterial breakMaterial = GameManager.Instance.lDBBreakMaterial[_equip.nBreakMaterialIndex];
                        SetMaterailImage(DecomposeSlots[i], breakMaterial);

                        fInitXPosition += 130f;
                    }
                    else
                        DecomposeSlots[i].gameObject.SetActive(false);
                }
                break;
            case 4:
                fInitXPosition = -195f;
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    if (i < _count)
                    {
                        DecomposeSlots[i].gameObject.SetActive(true);
                        RectTransform rectTransform = DecomposeSlots[i].transform.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(fInitXPosition, rectTransform.anchoredPosition.y);
                        DBBreakMaterial breakMaterial = GameManager.Instance.lDBBreakMaterial[_equip.nBreakMaterialIndex];
                        SetMaterailImage(DecomposeSlots[i], breakMaterial);
                        fInitXPosition += 130f;
                    }
                    else
                        DecomposeSlots[i].gameObject.SetActive(false);
                }
                break;
            case 5:
                fInitXPosition = -195f;
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    if (i < _count)
                    {
                        DecomposeSlots[i].gameObject.SetActive(true);
                        RectTransform rectTransform = DecomposeSlots[i].transform.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(fInitXPosition, rectTransform.anchoredPosition.y);
                        DBBreakMaterial breakMaterial = GameManager.Instance.lDBBreakMaterial[_equip.nBreakMaterialIndex];
                        SetMaterailImage(DecomposeSlots[i], breakMaterial);
                        fInitXPosition += 130f;
                    }
                    else
                        DecomposeSlots[i].gameObject.SetActive(false);
                }
                break;
            case 6:
                fInitXPosition = -260f;
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    if (i < _count)
                    {
                        DecomposeSlots[i].gameObject.SetActive(true);
                        RectTransform rectTransform = DecomposeSlots[i].transform.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(fInitXPosition, rectTransform.anchoredPosition.y);
                        DBBreakMaterial breakMaterial = GameManager.Instance.lDBBreakMaterial[_equip.nBreakMaterialIndex];
                        SetMaterailImage(DecomposeSlots[i], breakMaterial);
                        fInitXPosition += 130f;
                    }
                    else
                        DecomposeSlots[i].gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }

    }
   
    public void SetMaterailImage(InventoryDecomposeSlot _slot, DBBreakMaterial _material)
    {
        if(_material.nIron != 0 && bMaterialCheck[0] == false)
        {
            _slot.SetInfo(_material.nIron, GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == "Iron"));
            bMaterialCheck[0] = true;
        }
        else if(_material.nFabric != 0 && bMaterialCheck[1] == false)
        {
            _slot.SetInfo(_material.nFabric, GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == "Fabric"));
            bMaterialCheck[1] = true;
        }
        else if (_material.nWood != 0 && bMaterialCheck[2] == false)
        {
            _slot.SetInfo(_material.nWood, GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == "Wood"));
            bMaterialCheck[2] = true;
        }
          
        else if (_material.nWeaponStone != 0 && bMaterialCheck[3] == false)
        {
            _slot.SetInfo(_material.nWeaponStone, GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == "WeaponStone"));
            bMaterialCheck[3] = true;
        }
        else if (_material.nArmorStone != 0 && bMaterialCheck[4] == false)
        {
            _slot.SetInfo(_material.nArmorStone, GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == "ArmorStone"));
            bMaterialCheck[4] = true;
        }
    
        else if(_material.nAccessoryStone != 0 && bMaterialCheck[5] == false)
        {
            _slot.SetInfo(_material.nAccessoryStone, GameManager.Instance.Equipment_MaterialSpriteList.Find(x => x.name == "AccessoryStone"));
            bMaterialCheck[5] = true;
        }
           

    }
}
