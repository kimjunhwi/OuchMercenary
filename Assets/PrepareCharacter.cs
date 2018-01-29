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

	CharacterStats charicData;
	PreBattleScenePanel preBattleClass;

	void Awake()
	{
		ActiveImage = gameObject.GetComponent<Image> ();
	}

	public void Active(bool _bIsActvie)
	{
		ActiveImage.sprite = (_bIsActvie == true) ? ActiveSprite : UnActiveSprite;

	}

	public void SetUp(PreBattleScenePanel _preBattleClass, CharacterStats _charicData)
	{
		charicData = _charicData;
		preBattleClass = _preBattleClass;

		Active (false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (preBattleClass.InfoGameObject.activeSelf) 
		{
			preBattleClass.stageInfoPanel.charicStats = charicData;
		} 
		else 
		{
			preBattleClass.mercenaryDispath.read_Batch_Charic = this;

			preBattleClass.mercenaryDispath.ChangeSpriteToDispatchingImage (true);
		}
	}
}
