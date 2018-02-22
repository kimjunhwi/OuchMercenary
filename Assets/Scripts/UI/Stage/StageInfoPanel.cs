using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;
using UnityEngine.UI;

public class StageInfoPanel : MonoBehaviour {

	public CharacterStats charicStats = null;

	Transform AbilityContentsTransform;

	enum E_ABILITY_TYPE
	{
		E_ATTACK_TYPE = 0,
		E_TRIBE,
		E_SITE,
		E_HP,
		E_ACCURACY,
		E_ATTACKRANGE,
		E_PHYSICAL_ATTACK_RATING,
		E_MAGIC_ATTACK_RATING,
		E_ATTACK_SPEED,
		E_MOVE_SPEED,
		E_PHYSICAL_DEFENCE,
		E_MAGIC_DEFENCE,
		E_DODGE,
		E_CRITICAL_RATING,
		E_CRITICAL_DAMAGE,
		E_PHYSICAL_PENETRATE,
		E_MAGIC_PENETRATE,
	}

	PreBattleScenePanel preBattleScene;

	Button ExitButton;
	GameObject backgroundPanel;

	Text AttackTypeText;
	Text TribeText;
	Text SiteText;
	Text HpText;
	Text AccuracyText;
	Text AttackRangeText;
	Text PhysicalAttackRatingText;
	Text MagicAttackRatingText;
	Text AttackSpeedText;
	Text MoveSpeedText;
	Text PhysicalDefenceText;
	Text MagicDefenceText;
	Text DodgeText;
	Text CriticalRatingText;
	Text CriticalDamageText;
	Text PhysicalPenetrateText;
	Text MagicPenetrateText;

	public void Init(PreBattleScenePanel _preBattleScene)
	{
		preBattleScene = _preBattleScene;

		ExitButton = transform.GetChild (1).GetComponent<Button> ();
		ExitButton.onClick.AddListener (Exit);

		AbilityContentsTransform = transform.GetChild (2).GetChild (0).GetChild (0).transform;

		AttackTypeText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_ATTACK_TYPE).GetChild(1).GetChild(0).GetComponent<Text>();
		TribeText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_TRIBE).GetChild(1).GetChild(0).GetComponent<Text>();
		SiteText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_SITE).GetChild(1).GetChild(0).GetComponent<Text>();
		HpText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_HP).GetChild(1).GetChild(0).GetComponent<Text>();
		AccuracyText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_ACCURACY).GetChild(1).GetChild(0).GetComponent<Text>();
		AttackRangeText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_ATTACKRANGE).GetChild(1).GetChild(0).GetComponent<Text>();
		PhysicalAttackRatingText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_PHYSICAL_ATTACK_RATING).GetChild(1).GetChild(0).GetComponent<Text>();
		MagicAttackRatingText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_MAGIC_ATTACK_RATING).GetChild(1).GetChild(0).GetComponent<Text>();
		AttackSpeedText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_ATTACK_SPEED).GetChild(1).GetChild(0).GetComponent<Text>();
		MoveSpeedText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_MOVE_SPEED).GetChild(1).GetChild(0).GetComponent<Text>();
		PhysicalDefenceText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_PHYSICAL_DEFENCE).GetChild(1).GetChild(0).GetComponent<Text>();
		MagicDefenceText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_MAGIC_DEFENCE).GetChild(1).GetChild(0).GetComponent<Text>();
		DodgeText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_DODGE).GetChild(1).GetChild(0).GetComponent<Text>();
		CriticalRatingText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_CRITICAL_RATING).GetChild(1).GetChild(0).GetComponent<Text>();
		CriticalDamageText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_CRITICAL_DAMAGE).GetChild(1).GetChild(0).GetComponent<Text>();
		PhysicalPenetrateText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_PHYSICAL_PENETRATE).GetChild(1).GetChild(0).GetComponent<Text>();
		MagicPenetrateText = AbilityContentsTransform.GetChild((int)E_ABILITY_TYPE.E_MAGIC_PENETRATE).GetChild(1).GetChild(0).GetComponent<Text>();
	}

	void Exit()
	{
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		if(preBattleScene != null)
			preBattleScene.mercenaryDispath.ChangeSpriteToDispatchingImage (true);
	}

	public void SetUp(CharacterStats _stats)
	{
		AttackTypeText.text = _stats.m_nAttackType.ToString();
		TribeText.text = _stats.m_nTribe.ToString();
		SiteText.text = _stats.m_fSite.ToString("F1");
		HpText.text = _stats.m_fHealth.ToString("F1");
		AccuracyText.text = _stats.m_fAccuracy.ToString("F1");
		AttackRangeText.text = _stats.m_fAttack_Range.ToString("F1");
		PhysicalAttackRatingText.text = _stats.m_fPhyiscal_Rating.ToString("F1");
		MagicAttackRatingText.text = _stats.m_fMagic_Rating.ToString("F1");
		AttackSpeedText.text = _stats.m_fAttackSpeed.ToString("F1");
		MoveSpeedText.text = _stats.m_fMoveSpeed.ToString("F1");
		PhysicalDefenceText.text = _stats.m_fPhysical_Defence.ToString("F1");
		MagicDefenceText.text = _stats.m_fMagic_Defence.ToString("F1");
		DodgeText.text = _stats.m_fDodge.ToString("F1");
		CriticalRatingText.text = _stats.m_fCritical_Rating.ToString("F1");
		CriticalDamageText.text = _stats.m_fCritical_Damage.ToString("F1");
		PhysicalPenetrateText.text = _stats.m_fPhysicalPenetrate.ToString("F1");
		MagicPenetrateText.text = _stats.m_fMagicPenetrate.ToString("F1");
	}
}
