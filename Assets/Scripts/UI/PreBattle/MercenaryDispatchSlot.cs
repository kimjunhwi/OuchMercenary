using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MercenaryDispatchSlot : MonoBehaviour , IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public MercenaryDispatchPanel mDispatchPanel;

	public Image CharacterBox_Image;
	public Image CharacterDispatchCancel_Image;

	public CharacterSlot characterSlot;

	public MercenaryDispatchCancel mDispatchSlotCancel;

	//Event Call
	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log ("mDispatchSlot OnPointerUp");
		//이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
		if (eventData.pointerCurrentRaycast.gameObject.name == "MercenarySlot_Image" && CharacterBox_Image.sprite.name == "ready_charactor_icon_active" &&
			characterSlot == null) 
		{
			CharacterBox_Image.sprite = mDispatchPanel.characterSlot.characterBox_Image.sprite;
			characterSlot = mDispatchPanel.characterSlot;
			//배치중인 이미지 활성화
			characterSlot.SetDipathcingState ();
			characterSlot.ActiveBackground_Obj.SetActive (true);
			//나머지 칸을 다시 빈칸으로 만듬
			mDispatchPanel.ChangeSpriteToDispatchingImage (false);
		} 
		//캐릭터 이미지가 있다면 캔슬버튼을 활성화
		else if (eventData.pointerCurrentRaycast.gameObject.name == "MercenarySlot_Image" && CharacterBox_Image.sprite.name != "ready_charactor_icon_active" &&
			characterSlot != null) 
		{
			CharacterDispatchCancel_Image.gameObject.SetActive (true);
			mDispatchSlotCancel.characterSlot = characterSlot;
			mDispatchSlotCancel.mercenaryDispatchSlot = this;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

}
