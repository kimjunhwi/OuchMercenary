using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Enemy_Character : Character {

	//김준휘
	public override void TakeDamage(float _fDamage)
	{
		//데미지 계산 처리
		m_fCurrentHp -= _fDamage;

		if(m_fCurrentHp < 0)
		{
			m_fCurrentHp = 0;
		}

		HealthSizeTransform.localScale = new Vector3 (m_fCurrentHp / m_fMaxHp, 1.0f, 1.0f);

		//캐릭터가 죽을지를 판별 
		if (m_fCurrentHp <= 0 && m_bIsDead == false) {

			//만약 캐릭터가 죽었다면 State를 Dead로 바꿈
			m_bIsDead = true;

			CheckCharacterState (E_CHARACTER_STATE.E_DEAD);
		} 
		else 
		{
			ShowDamage(_fDamage.ToString("F0"));

			//만약 체력바가 활성화 되지않았다면 활성화를 시킨후 코루틴을 호출
			if (!HealthActiveObject.activeSelf) {
				HealthActiveObject.SetActive (true);

				StartCoroutine (ShowHealthBar ());
			}
			//만약 호출 되지 않았다면 시간을 0으로 바꿔줌
			else 
			{
				m_fActiveHealthTime = 0.0f;
			}	
		}
	}

	protected IEnumerator ShowHealthBar()
	{
		yield return new WaitForSeconds (0.1f);

		while (m_fActiveHealthTime < m_fMaxHealthTime) 
		{
			m_fActiveHealthTime += Time.deltaTime;

			yield return null;
		}

		m_fActiveHealthTime = 0.0f;

		HealthActiveObject.SetActive (false);
	}


	public override void TakeHeal(float _fHeal)
	{
		//데미지 계산 처리
		m_fCurrentHp += _fHeal;

		if(m_fCurrentHp > m_fMaxHp)
		{
			m_fCurrentHp = m_fMaxHp;
		}

		HealthSizeTransform.localScale = new Vector3 (m_fCurrentHp / m_fMaxHp, 1.0f, 1.0f);
	}
}
