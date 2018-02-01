using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class PrepareCharacter : MonoBehaviour, IPointerClickHandler
{
	Image ActiveImage;
	public Image characterImage;

	public Sprite ActiveSprite;
	public Sprite UnActiveSprite;

	public Text NameText;
	public Text levelText;

	public Slider staminaSlider;

	public GameObject BatchObject;

	public CharacterStats charicData;
	PreBattleScenePanel preBattleClass;

	public bool bIsBatch = false;

	void Awake()
	{
		ActiveImage = gameObject.GetComponent<Image> ();

		BatchObject.SetActive (false);
	}

	public void OnEnable()
	{
		if (charicData != null) 
		{
			IsBatch (charicData.m_nBatchIndex != -1); 

			Active (false);
		}
	}

	public void Active(bool _bIsActvie)
	{
		ActiveImage.sprite = (_bIsActvie == true) ? ActiveSprite : UnActiveSprite;
	}

	public void SetUp(PreBattleScenePanel _preBattleClass, CharacterStats _charicData)
	{
		charicData = _charicData;
		preBattleClass = _preBattleClass;

		characterImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache("UI/BoxImages/Character/" + charicData.m_sImage);

		NameText.text = charicData.m_strCharicName;
		levelText.text = string.Format("Lv.{0}", charicData.m_nEnhace);

		IsBatch (_charicData.m_nBatchIndex != -1); 

		Active (false);
	}

	public void IsBatch(bool _bIsCheck)
	{
		bIsBatch = _bIsCheck;

		if (bIsBatch) 
		{
			Active (false);
			BatchObject.SetActive (true);
		} 
		else 
		{
			Active (false);
			BatchObject.SetActive (false);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (preBattleClass.InfoGameObject.activeSelf) 
		{
			Active (true);

			preBattleClass.stageInfoPanel.charicStats = charicData;
		} 
		else 
		{
			//Cancle버튼이 활성화했는지 체크 
			int nCheckIndex = preBattleClass.mercenaryDispath.GetCancleIndex ();

			if (nCheckIndex != -1)  preBattleClass.mercenaryDispath.mDispatchSlot [nCheckIndex].E_SWITCH (E_SLOT_STATE.E_BATCH);

			if (bIsBatch) 
			{

				return;
			}

			Active (true);

			preBattleClass.mercenaryDispath.read_Batch_Charic = this;

			preBattleClass.mercenaryDispath.ChangeSpriteToDispatchingImage (true);
		}
	}
}
