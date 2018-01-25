using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CalenderPanel : ButtonUIBase
{
    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
        if (eventData.pointerCurrentRaycast.gameObject.name == "Close_Button")
        {
            eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Clicked");
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

    }

    #endregion
}
