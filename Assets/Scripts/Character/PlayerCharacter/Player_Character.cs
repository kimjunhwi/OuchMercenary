using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Player_Character : Character {

	//캐릭터가 데미지를 받았을시에 호출 되는 함수이다.
	public override void TakeDamage(float _fDamage)
	{
		//데미지 계산 처리
		m_fCurrentHp -= _fDamage;

		if(m_fCurrentHp < 0)
		{
			m_fCurrentHp = 0;
		}

		characterUI.ChangeHealth(m_fCurrentHp);

		//캐릭터가 죽을지를 판별 
		if (m_fCurrentHp <= 0 && m_bIsDead == false) {

			//만약 캐릭터가 죽었다면 State를 Dead로 바꿈
			m_bIsDead = true;

			CheckCharacterState (E_CHARACTER_STATE.E_DEAD);
		} 
	}	

	public override void TakeHeal(float _fHeal)
	{
		//데미지 계산 처리
		m_fCurrentHp += _fHeal;

		if(m_fCurrentHp > m_fMaxHp)
		{
			m_fCurrentHp = m_fMaxHp;
		}

		characterUI.ChangeHealth(m_fCurrentHp);
	}
}
