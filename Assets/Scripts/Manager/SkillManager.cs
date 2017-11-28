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

		fDamage = 0.0f;
		fCritical = (_bIsCritical == true) ? _skiller.GetStats().m_fCritical_Damage : 100f;

		fPhsicalAttackDamage = (_skiller.GetStats().m_fPhyiscal_Rating * (_skiller.GetStats().basicSkill[0].fPhsyicMagnification * 0.01f) * (fCritical * 0.01f));
		fMagicAttackDamage = (_skiller.GetStats().m_fMagic_Rating * (_skiller.GetStats().basicSkill[0].fMagicMagnification * 0.01f) * (fCritical * 0.01f));

		fPhsicalDefence = (_skillee.GetStats().m_fPhysical_Defence * 0.002f);
		fMagicDefence = (_skillee.GetStats().m_fMasic_Defence * 0.02f);

		fDamage = fPhsicalAttackDamage - (fPhsicalAttackDamage * fPhsicalDefence);
		fDamage += fMagicAttackDamage - (fMagicAttackDamage * fMagicDefence);
		
		_skillee.TakeDamage (fDamage);
	}

	public void ActiveAttack(ActiveSkill _skiller, Character _skillee,bool _bIsCritical)
	{
		
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
