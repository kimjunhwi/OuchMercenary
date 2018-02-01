using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class MercenaryDispatchPanel : MonoBehaviour
{
	public const int nMaxBatchAmount = 16;

	public Sprite dispatching_Image; 
	public Sprite transparent_Image;
	public Sprite changedispatch_Image;

	public MercenaryDispatchSlot[] mDispatchSlot;

	public CharacterSlot characterSlot;

	public PrepareCharacter read_Batch_Charic = null;

	public void Awake()
	{
		for (int nIndex = 0; nIndex < mDispatchSlot.Length; nIndex++) 
		{
			mDispatchSlot [nIndex].m_nIndex = nIndex;
		}
	}

	public void Init()
	{
		read_Batch_Charic = null;

		ChangeSpriteToDispatchingImage (false);
	}

	public void ChangeSpriteToDispatchingImage(bool _isDispatchOn)
	{
		if (_isDispatchOn) 
		{
			for (int i = 0; i < nMaxBatchAmount; i++) {
				//이미 배치가 되어있는 이미지가 있다면 건너 뛴다 characterSlot (빈 슬롯일때 만 바꿈)
				if (mDispatchSlot[i].eSlot_State != E_SLOT_STATE.E_BATCH && mDispatchSlot[i].eSlot_State != E_SLOT_STATE.E_CANCLE ) 
				{
					mDispatchSlot [i].eSlot_State = E_SLOT_STATE.E_BATCH_READY;
					mDispatchSlot [i].CharacterDispatchCancel_Image.gameObject.SetActive (false);
					mDispatchSlot [i].CharacterBox_Image.sprite = dispatching_Image;
				}
			
			}
		}
		else 
		{
			for (int i = 0; i < nMaxBatchAmount; i++) 
			{
				//이미 배치가 되어있는 이미지가 있다면 건너 뛴다 (다시 원래 빈 이미지로 바꿔 준다)
				if (mDispatchSlot [i].eSlot_State == E_SLOT_STATE.E_BATCH || mDispatchSlot[i].eSlot_State == E_SLOT_STATE.E_CANCLE) continue;

				mDispatchSlot [i].eSlot_State = E_SLOT_STATE.E_NONE;
				mDispatchSlot [i].CharacterDispatchCancel_Image.gameObject.SetActive (false);
				mDispatchSlot [i].CharacterBox_Image.sprite = transparent_Image;

			}
		}
	}

	public int GetCancleIndex()
	{
		for (int nIndex = 0; nIndex < nMaxBatchAmount; nIndex++) 
		{
			if (mDispatchSlot [nIndex].eSlot_State == E_SLOT_STATE.E_CANCLE)
				return nIndex;
		}

		return -1;
	}

	public void SuccessedBatch(int nIndex)
	{
		mDispatchSlot [nIndex].characterSlot.bIsBatch = true;
	}

	public void IsCheckCompare(int _nIndex)
	{
		for(int nIndex = 0; nIndex < nMaxBatchAmount; nIndex++)
		{
			if (nIndex == _nIndex || mDispatchSlot[nIndex] == null)
				continue;

			if (mDispatchSlot [nIndex].characterSlot == read_Batch_Charic) 
			{
				mDispatchSlot [nIndex].E_SWITCH (E_SLOT_STATE.E_NONE);
				break;
			}
		}
	}

	public void IsDifferentCheckCancel(int _nIndex)
	{
		for (int nIndex = 0; nIndex < nMaxBatchAmount; nIndex++) 
		{
			if (nIndex == _nIndex || mDispatchSlot[nIndex].characterSlot == null)
				continue;

			if (mDispatchSlot [nIndex].characterSlot == read_Batch_Charic) 
			{
				mDispatchSlot [nIndex].E_SWITCH (E_SLOT_STATE.E_BATCH);

				return;
			}
		}
	}
}
