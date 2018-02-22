using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ReadOnlys;

public class MercenaryDetailInfoPanel : ButtonUIBase
{
    public Image DetailShow_Button;
    public MercenaryDetailInfo detailInfo;

    public void Init()
    {
        DetailShow_Button = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        detailInfo = this.gameObject.transform.GetChild(1).GetComponent<MercenaryDetailInfo>();
    }
        
    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {

        if (eventData.pointerCurrentRaycast.gameObject.name == DetailShow_Button.name)
            detailInfo.gameObject.SetActive(true);
        else
            Debug.Log("Cliked");


    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Cliked");
    }




    #endregion
}
