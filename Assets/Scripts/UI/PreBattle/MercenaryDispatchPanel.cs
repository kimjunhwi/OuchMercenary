using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MercenaryDispatchPanel : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public Sprite dispatching_Image; 
	public Sprite transparent_Image;

	public Sprite changedispatch_Image;

	public MercenaryDispatchSlot[] mDispatchSlot;



	public CharacterSlot characterSlot;

	public void ChangeSpriteToDispatchingImage(bool _isDispatchOn)
	{
		if (_isDispatchOn) 
		{
			for (int i = 0; i < 16; i++) {
				//이미 배치가 되어있는 이미지가 있다면 건너 뛴다 characterSlot (빈 슬롯일때 만 바꿈)
				if (mDispatchSlot [i].CharacterBox_Image.sprite.name == "non") 
				{
					mDispatchSlot [i].CharacterBox_Image.sprite = dispatching_Image;
				}
			
			}
		}
		else 
		{
			for (int i = 0; i < 16; i++) 
			{
				//이미 배치가 되어있는 이미지가 있다면 건너 뛴다 (다시 원래 빈 이미지로 바꿔 준다)
				if (mDispatchSlot [i].CharacterBox_Image.sprite.name == "ready_charactor_icon_active") {
					mDispatchSlot [i].CharacterBox_Image.sprite = transparent_Image;
				}
			}
		}
	}


	//Event Call
	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log ("Mercenary Panel!!");
	}
	public void OnPointerDown(PointerEventData eventData)
	{

	}

}
