using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class MercenaryDispatchCancel : MonoBehaviour , IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public CharacterSlot characterSlot;
	public MercenaryDispatchSlot mercenaryDispatchSlot;

	public void OnPointerUp(PointerEventData eventData)
	{
		//이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
		if (eventData.pointerCurrentRaycast.gameObject.name == "Cancel_Image") 
		{
			//배치 슬롯 초기화
			//Cancel 버튼 삭제
			mercenaryDispatchSlot.CharacterDispatchCancel_Image.gameObject.SetActive (false);
			//배치 이미지 빈 이미지로
			mercenaryDispatchSlot.CharacterBox_Image.sprite = mercenaryDispatchSlot.mDispatchPanel.transparent_Image;
			//모든 배치 이미지를 빈 슬롯으로
			mercenaryDispatchSlot.mDispatchPanel.ChangeSpriteToDispatchingImage (false);
			//해당 칸에 나타내는 캐릭터 슬롯정보 초기화
			mercenaryDispatchSlot.characterSlot = null;
			//캐릭터 슬롯 초기화
			characterSlot.ActiveBackground_Obj.SetActive (false);
			characterSlot.SetDispatchCancelState ();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

}
