using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

enum DispatchImage
{
	Dispatch = 0,
	DispatchCancel,
	Dispatching,
}

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
	public GameObject ActiveBackground_Obj;
	public Image[] skillImage;
	public Image[] dispatch_ImageButton;

	public RectTransform cSlotRect;

	public MercenaryDispatchPanel mDispatchPanel;

	//이 캐릭터 슬롯에 있는 정보
	public DBBasicCharacter characterInfo;

	public float fRectAnchorYPos = 0f;

	public Mask mask;


	public CharacterInfoPanel cInfoPanel;

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void SetDispatchState()
	{
		//맨뒤로 배치 버튼
		dispatch_ImageButton [(int)DispatchImage.Dispatch].gameObject.transform.SetAsFirstSibling ();
		//맨 앞으로 취소버튼
		dispatch_ImageButton [(int)DispatchImage.DispatchCancel].gameObject.transform.SetAsLastSibling ();

	}

	public void SetDispatchCancelState()
	{
		//맨뒤로 배치 버튼
		dispatch_ImageButton [(int)DispatchImage.Dispatch].gameObject.transform.SetAsLastSibling ();
		//맨 앞으로 취소버튼
		dispatch_ImageButton [(int)DispatchImage.DispatchCancel].gameObject.transform.SetAsFirstSibling ();

	}

	public void SetDipathcingState()
	{
		dispatch_ImageButton [(int)DispatchImage.Dispatching].gameObject.transform.SetAsLastSibling ();

		//cInfoPanel.specificWindow_Obj.SetActive (true);
	}



	public void OnPointerUp(PointerEventData eventData)
	{
		//Debug.Log ("OnPointerUp");
		if (eventData.pointerCurrentRaycast.gameObject.name == "CharacterDispatch_ImageButton") 
		{
			Debug.Log ("CharacterDispatch_ImageButton Touch!!");
			SetDispatchState ();
			mDispatchPanel.ChangeSpriteToDispatchingImage (true);
			mDispatchPanel.characterSlot = this;

		} 
		else if (eventData.pointerCurrentRaycast.gameObject.name == "CharacterCancel_ImageButton") {
			Debug.Log ("CharacterCancel_ImageButton  Touch!!");

			SetDispatchCancelState ();
			mDispatchPanel.ChangeSpriteToDispatchingImage (false);

		} 
		else if(eventData.pointerCurrentRaycast.gameObject.name == "CharacterSpecific_Button")
		{
			Debug.Log ("캐릭터 상세창 활성");
			cInfoPanel.specificWindow_Obj.SetActive (true);
		}

		else {
			
		}
	}

	public float GetRectAnchorPosition()
	{
		return this.gameObject.GetComponent<RectTransform> ().anchoredPosition.y;
	}


}
