using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

//각각의 탭들의 중간다리 역할
public class InventoryEachTopPanel : MonoBehaviour
{
    public GameObject ContentsPanels;   //실제 내용을 담는 오브젝트
    //현재 열려 있는 인벤토리의 타입
    public E_INVENTORY curInventoryType;
    public void Init(int _type)
    {
        ContentsPanels = this.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject;
        curInventoryType = (E_INVENTORY)_type;
    }
    
}
