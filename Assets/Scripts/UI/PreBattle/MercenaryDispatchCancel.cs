using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class MercenaryDispatchCancel : MonoBehaviour , IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public MercenaryDispatchSlot mercenaryDispatchSlot;

	public void OnPointerUp(PointerEventData eventData)
	{
		//이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
		if (eventData.pointerCurrentRaycast.gameObject.name == "Cancel_Image") 
		{
			mercenaryDispatchSlot.E_SWITCH (ReadOnlys.E_SLOT_STATE.E_NONE);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

}
