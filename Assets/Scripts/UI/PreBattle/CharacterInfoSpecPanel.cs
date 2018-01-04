using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class CharacterInfoSpecPanel : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{

	public void OnPointerDown(PointerEventData eventData)
	{

	}
	public void OnPointerUp(PointerEventData eventData)
	{
		//Debug.Log ("OnPointerUp");
		if (eventData.pointerCurrentRaycast.gameObject.name == "cSpecificClose_Button") {
			//Debug.Log ("CharacterDispatch_ImageButton Touch!!");
			this.gameObject.SetActive (false);

		} else {
			Debug.Log ("캐릭터 상세창 활성");

		}
	}
}
