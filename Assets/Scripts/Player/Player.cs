using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Player : MonoBehaviour {


	private List<CharacterStats> LIST_HERO = new List<CharacterStats>();

	public void Init()
	{
		CharacterStats _charic = new CharacterStats ();

		_charic.m_nIndex = 0;
		_charic.m_strName = "Warrior";
		_charic.m_strJob = "assassin";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_MELEE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 10;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 10;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 1;
		_charic.m_fAttack_Area = 0;
		_charic.m_nTargetNumber = 0;
		_charic.m_nAttack_Priority = 0;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 10;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;

		LIST_HERO.Add (_charic);

		_charic = new CharacterStats ();

		_charic.m_nIndex = 0;
		_charic.m_strName = "basicArcher";
		_charic.m_strJob = "archer";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_RANGE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 10;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 10;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 1;
		_charic.m_fAttack_Area = 0;
		_charic.m_nTargetNumber = 0;
		_charic.m_nAttack_Priority = 0;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 10;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;

		LIST_HERO.Add (_charic);
	}

	public void AddCharacter(int _nIndex)
	{
		//Character _charic = GameManager.instance.GetCharicData(_nIndex);

		//if(_charic != null){
		//	LIST_HERO.Add(Character);
		//}
	}
}
