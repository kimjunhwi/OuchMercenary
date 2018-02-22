using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class MercenaryDispatchSlot : MonoBehaviour
{
	public int m_nIndex ;

	public MercenaryDispatchPanel mDispatchPanel;

	public Image CharacterBox_Image;
	public Image CharacterDispatchCancel_Image;

	public PrepareCharacter characterSlot;

	public MercenaryDispatchCancel mDispatchSlotCancel;

	public E_SLOT_STATE eSlot_State;


	public void OnClick()
	{
		switch (eSlot_State) 
		{
		case E_SLOT_STATE.E_BATCH: 			E_SWITCH (E_SLOT_STATE.E_CANCLE); 	break;
		case E_SLOT_STATE.E_BATCH_READY: 	E_SWITCH (E_SLOT_STATE.E_BATCH); 	break;
		case E_SLOT_STATE.E_CANCLE: 		E_SWITCH (E_SLOT_STATE.E_BATCH); 	break;
		}
	}

	public void E_SWITCH(E_SLOT_STATE _State)
	{
		eSlot_State = _State;

		switch (_State) 
		{
		case E_SLOT_STATE.E_BATCH:
			{
				characterSlot = (mDispatchPanel.read_Batch_Charic == null) ? characterSlot : mDispatchPanel.read_Batch_Charic;

				mDispatchPanel.IsCheckCompare (m_nIndex);

				characterSlot.charicData.m_nBatchIndex = m_nIndex;

				characterSlot.IsBatch (true);

				CharacterBox_Image.sprite = ObjectCashing.Instance.LoadSpriteFromCache("UI/BoxImages/Character/" + characterSlot.charicData.m_sImage);

				CharacterDispatchCancel_Image.gameObject.SetActive (false);

				mDispatchPanel.Init ();
			}
			break;
		case E_SLOT_STATE.E_BATCH_READY:
			{
			}
			break;
		case E_SLOT_STATE.E_CANCLE:
			{
				mDispatchPanel.IsDifferentCheckCancel (m_nIndex);

				mDispatchPanel.read_Batch_Charic = characterSlot;

				characterSlot.Active (true);

				CharacterBox_Image.sprite = ObjectCashing.Instance.LoadSpriteFromCache("UI/BoxImages/Character/" + characterSlot.charicData.m_sImage);

				CharacterDispatchCancel_Image.gameObject.SetActive (true);

				mDispatchPanel.ChangeSpriteToDispatchingImage (true);
			}
			break;
		case E_SLOT_STATE.E_NONE:
			{
				characterSlot.charicData.m_nBatchIndex = -1;

				characterSlot.IsBatch (false);

				characterSlot.Active (false);

				CharacterDispatchCancel_Image.gameObject.SetActive (false);

				characterSlot = null;

				mDispatchPanel.Init ();
			}
			break;
		}
	}
}
