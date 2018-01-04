using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

//자기가 원하고자 하는 오브젝트 이외에는 모두 SetActive(False)

public class NotFirstObjDisable : ButtonUIBase 
{
	public override void OnPointerDown(PointerEventData eventData)
	{
		
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		//Debug.Log ("클릭함");
		if (eventData.pointerEnter.gameObject.name == "MercenaryEquipmentChange_Button") {
			ActivateObj.SetActive (true);
			for (int i = 0; i < DeActivateObj.Length; i++) {
				DeActivateObj [i].SetActive (false);
			}
		}

		else if (eventData.pointerEnter.gameObject.name == "MercenarySkillChange_Button") {
			ActivateObj.SetActive (true);
			DeActivateObj [0].SetActive (false);
		}

		else if (eventData.pointerEnter.gameObject.name == "EquipmentClose_Button") {
			ActivateObj.SetActive (true);
			DeActivateObj [0].SetActive (false);
		}
	}
}
