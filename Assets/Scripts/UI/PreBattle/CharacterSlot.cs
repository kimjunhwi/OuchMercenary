using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public Image characterBox_Image;
	public Image characterTribe_Image;
	public Text characterName_Text;
	public Text characterHealth_Text;
	public Text characterLevel_Text;

	public Button characterSpecific_Button;
	public Slider hp_Slider;
	public Slider levelExp_Slider;

	public GameObject skillPanel_Obj;
	public Image[] skillImage;
	public Image[] dispatch_ImageButton;

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log ("OnPointerUp");
		if (eventData.pointerCurrentRaycast.gameObject.name == "CharacterDispatch_ImageButton") 
		{
			Debug.Log ("CharacterDispatch_ImageButton Touch!!");
		} 
		else if (eventData.pointerCurrentRaycast.gameObject.name == "CharacterCancel_ImageButton ") {
			Debug.Log ("CharacterCancel_ImageButton  Touch!!");
		} 
		else if (eventData.pointerCurrentRaycast.gameObject.name == "CharacterDispatching_ImageButton") {
			Debug.Log ("CharacterDispatching_ImageButton Touch!!");
		} 
		else {
			Debug.Log ("Not Click");
		}
	}
}
