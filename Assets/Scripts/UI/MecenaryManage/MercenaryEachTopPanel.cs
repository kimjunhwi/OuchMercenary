using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class MercenaryEachTopPanel : MonoBehaviour
{
    public GameObject ContentsPanels;   //실제 내용을 담는 오브젝트
    //현재 열려 있는 인벤토리의 타입
    public E_MERCENARYMANAGE curMercenaryType;
    public void Init(int _type)
    {
        ContentsPanels = this.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject;
        curMercenaryType = (E_MERCENARYMANAGE)_type;
    }


}
