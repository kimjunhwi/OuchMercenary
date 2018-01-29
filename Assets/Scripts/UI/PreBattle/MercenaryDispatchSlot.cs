using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class MercenaryDispatchSlot : MonoBehaviour
{
	public MercenaryDispatchPanel mDispatchPanel;

	public Image CharacterBox_Image;
	public Image CharacterDispatchCancel_Image;

	public PrepareCharacter characterSlot;

	public MercenaryDispatchCancel mDispatchSlotCancel;

	public E_SLOT_STATE eSlot_State;


	public void OnClick()
	{
		//만약 배치준비 상태라면 
		if (eSlot_State == E_SLOT_STATE.E_BATCH_READY) 
		{
			eSlot_State = E_SLOT_STATE.E_BATCH;

			characterSlot = mDispatchPanel.read_Batch_Charic;

			mDispatchPanel.Init ();
		}

		//만약 배치가 돼있다면 
		if (eSlot_State == E_SLOT_STATE.E_BATCH) 
		{
			eSlot_State = E_SLOT_STATE.E_CANCLE;

		}

		if (eSlot_State == E_SLOT_STATE.E_CANCLE) 
		{

		}
	}
}
