using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class SkillManager : MonoBehaviour {

	float fDamage = 0.0f;
	float fCritical = 0.0f;

	float fPhsicalAttackDamage = 0.0f;
	float fMagicAttackDamage = 0.0f;

	float fPhsicalDefence = 0.0f;
	float fMagicDefence = 0.0f;

    public void BasicAttack(Character _skiller,Character _skillee,bool _bIsCritical)
	{
		if(_skiller.IsDead() || _skillee.IsDead())
			return;

		if (Random.Range (0, 100) < _skillee.GetStats ().m_fDodge) 
		{
			_skillee.Dodge ();

			return;
		}
		
		_skillee.TakeDamage (GetResultBasicDamage(_skiller,_skillee,_bIsCritical));
	}

	public void ActiveAttack(ActiveSkill _activeSkill, Character _skiller, Character _skillee,bool _bIsCritical)
	{
		if(_skillee.IsDead())
			return;

		if (Random.Range (0, 100) < _skillee.GetStats ().m_fDodge) 
		{
			_skillee.Dodge ();

			return;
		}
		
		_skillee.TakeDamage (GetResultActiveDamage(_activeSkill,_skiller,_skillee,_bIsCritical));
	}

	public float GetResultBasicDamage(Character _skiller, Character _skillee,bool _bIsCritical)
	{
		fDamage = 0.0f;
		fCritical = (_bIsCritical == true) ? _skiller.GetStats().m_fCritical_Damage : 100f;

		fPhsicalAttackDamage = (_skiller.GetStats().m_fPhyiscal_Rating * (_skiller.GetStats().basicSkill[0].fPhsyicMagnification * 0.01f) * (fCritical * 0.01f));
		fMagicAttackDamage = (_skiller.GetStats().m_fMagic_Rating * (_skiller.GetStats().basicSkill[0].fMagicMagnification * 0.01f) * (fCritical * 0.01f));

		fPhsicalDefence = (_skillee.GetStats().m_fPhysical_Defence * 0.002f);
		fMagicDefence = (_skillee.GetStats().m_fMasic_Defence * 0.02f);

		fDamage = fPhsicalAttackDamage - (fPhsicalAttackDamage * fPhsicalDefence);
		fDamage += fMagicAttackDamage - (fMagicAttackDamage * fMagicDefence);

		return fDamage;
	}

	public float GetResultActiveDamage(ActiveSkill _activeSkill, Character _skiller, Character _skillee,bool _bIsCritical)
	{
		fDamage = 0.0f;
		fCritical = (_bIsCritical == true) ? _skiller.GetStats().m_fCritical_Damage : 100f;

		fPhsicalAttackDamage = (_skiller.GetStats().m_fPhyiscal_Rating * (_activeSkill.m_fPhysicalMagnification * 0.01f) * (fCritical * 0.01f));
		fMagicAttackDamage = (_skiller.GetStats().m_fMagic_Rating * (_activeSkill.m_fMagicMagnification * 0.01f) * (fCritical * 0.01f));

		fPhsicalDefence = (_skillee.GetStats().m_fPhysical_Defence * 0.002f);
		fMagicDefence = (_skillee.GetStats().m_fMasic_Defence * 0.02f);

		fDamage = fPhsicalAttackDamage - (fPhsicalAttackDamage * fPhsicalDefence);
		fDamage += fMagicAttackDamage - (fMagicAttackDamage * fMagicDefence);

		return fDamage;
	}


	public void ActiveSkill(int _nSkillType,ActiveSkill _nActiveSkillIndex,Character _skiller, ArrayList _targetCharacter_LIST,bool _bIsCritical)
	{
		switch (_nSkillType) 
		{
			case (int)E_ACTIVE_TYPE.E_ATTACK:
			{
				for(int nIndex = 0; nIndex < _nActiveSkillIndex.m_nMaxTargetNumber; nIndex++)
				{
					if (_targetCharacter_LIST.Count <= nIndex) {
						return;
					}

					ActiveAttack(_nActiveSkillIndex,_skiller, (Character)_targetCharacter_LIST[nIndex],_bIsCritical);
				}
			}
			break;

			case (int)E_ACTIVE_TYPE.E_BLEED:
			case (int)E_ACTIVE_TYPE.E_BURN:
			case (int)E_ACTIVE_TYPE.E_POISON:
			{
				for(int nIndex = 0; nIndex < _nActiveSkillIndex.m_nMaxTargetNumber; nIndex++)
				{
					if (_targetCharacter_LIST.Count <= nIndex) {
						return;
					}

					Character charic = (Character)_targetCharacter_LIST[nIndex];

					//배율은 수치가 나온 후 적용
					fDamage = GetResultActiveDamage(_nActiveSkillIndex,_skiller,charic,_bIsCritical);

					charic.SetDetrimental((E_ACTIVE_TYPE)_nSkillType,fDamage,_nActiveSkillIndex.m_fDuration);
				}
			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_HP:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_ACCURACY:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_ATTACK_RANGE:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_P_ATTACK_RATING:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_M_ATTACK_RATING:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_ATTACK_SPEED:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_P_DEFENCE:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_M_DEFENCE:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_DODGE:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_CRITICAL_RATING:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_CRITICAL_DAMAGE:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_P_PENETRATE:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_M_PENETRATE:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_BUFF_COOLTIME:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_STRUN:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_MIN_HEAL:
			{

			}
			break;
			case (int)E_ACTIVE_TYPE.E_HEAL:
			{

			}
			break;
		}
	}

	public void TargetHeal(Character _skiller,Character _skillee,bool _bIsCritical)
	{
		if (_skiller.IsDead () || _skillee.IsDead ())
			return;

		fDamage = 0.0f;
		fCritical = (_bIsCritical == true) ? _skiller.GetStats().m_fCritical_Damage : 100f;

		fPhsicalAttackDamage = (_skiller.GetStats().m_fPhyiscal_Rating * (_skiller.GetStats().basicSkill[0].fPhsyicMagnification * 0.01f) * (fCritical * 0.01f));
		fMagicAttackDamage = (_skiller.GetStats().m_fMagic_Rating * (_skiller.GetStats().basicSkill[0].fMagicMagnification * 0.01f) * (fCritical * 0.01f));

		fDamage = fPhsicalAttackDamage + fMagicAttackDamage;

		_skillee.TakeHeal (fDamage);
	}
}
