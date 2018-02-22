using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDecompose : ButtonUIBase
{
    public Image Yes_Button;
    public Image No_Button;
    public Image Cancel_Button;
    public InventoryDecomposeList decomposeList;
    public InventoryDecomposePanel decomposePanel;

    public void Init()
    {
        Yes_Button = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        No_Button = this.gameObject.transform.GetChild(1).GetComponent<Image>();
        Cancel_Button = this.gameObject.transform.GetChild(2).GetComponent<Image>();
        decomposeList = this.gameObject.transform.GetChild(3).gameObject.AddComponent<InventoryDecomposeList>();
        decomposeList = this.gameObject.transform.GetChild(3).GetComponent<InventoryDecomposeList>();
        decomposeList.Init();
    }


    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //확인 버튼
        if (eventData.pointerCurrentRaycast.gameObject.name == Yes_Button.name)
        {
            Debug.Log("분해 확인!");
            decomposePanel.gameObject.SetActive(false);
        }
        //아니요 버튼
        else if (eventData.pointerCurrentRaycast.gameObject.name == No_Button.name)
        {
            decomposePanel.gameObject.SetActive(false);
        }
        //장착 버튼 
        else if (eventData.pointerCurrentRaycast.gameObject.name == Cancel_Button.name)
        {
            decomposePanel.gameObject.SetActive(false);
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
}
