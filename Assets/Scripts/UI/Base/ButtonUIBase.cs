using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class ButtonUIBase : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{ 
	public GameObject ActivateObj;
	public GameObject[] DeActivateObj;



	public virtual void OnPointerDown(PointerEventData eventData)	{}
	public virtual void OnPointerUp(PointerEventData eventData) {}
    public virtual void OnPointerExit(PointerEventData eventData) { }


}
