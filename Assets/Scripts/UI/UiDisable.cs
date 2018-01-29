using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiDisable : MonoBehaviour, IPointerDownHandler
{
	GameObject getInfoGameObject;

	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;
		
		if (getInfoGameObject.gameObject.name == "BackGroundPanel") {
			getInfoGameObject.transform.parent.gameObject.SetActive (false);
		}
	}
}
