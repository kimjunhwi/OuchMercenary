using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryManagePanel : MonoBehaviour 
{
    //용병 탭 전체 관리 패널
    public MercenaryManageCharacterInfo merCharacterInfoPanel;



    public void Awake()
    {
        merCharacterInfoPanel = this.gameObject.transform.GetChild(2).GetChild(2).GetComponent<MercenaryManageCharacterInfo>();
        merCharacterInfoPanel.Init();
    }
}
