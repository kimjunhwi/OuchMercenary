using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class SkillManager : MonoBehaviour {

	public void BasicAttack(Character _skiller,Character _skillee)
	{
		//나중에 수식이 정해지면 그 수치를 적용
		float fDamage = 50;

		_skillee.TakeDamage (fDamage);
	}

	public void ActiveAttack(ActiveSkill _skiller, Character _skillee)
	{
		
	}
}
