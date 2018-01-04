using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class ButtonUIBase : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{

	public GameObject ActivateObj;
	public GameObject[] DeActivateObj;



	public virtual void OnPointerDown(PointerEventData eventDataa)	{}
	public virtual void OnPointerUp(PointerEventData eventData) {}


}
