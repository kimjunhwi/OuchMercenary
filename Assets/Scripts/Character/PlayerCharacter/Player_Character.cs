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

	public override void ActionUpdate ()
	{
		StartCoroutine(this.CharacterAction());
	}

	public override void CheckCharacterState(E_CHARACTER_STATE _E_STATE)
	{
		if (E_CHARIC_STATE == _E_STATE)
			return;

		animator.Rebind();

		//액션 변경
		E_CHARIC_STATE = _E_STATE;
		animator.speed = 1.0f;


		//초기화가 필요한 부분
		m_fCastTime = 0.0f;
		m_fMaxCastTime = 0.0f;


		//추후 사용 될 수 있을 부분이 있기에 만들어둠
		switch(E_CHARIC_STATE)
		{
		case E_CHARACTER_STATE.E_WAIT:
			{
				animator.SetTrigger ("Idle");

			}
			break;
		case E_CHARACTER_STATE.E_WALK:
			{
				animator.SetTrigger ("Walk");


			}
			break;
		case E_CHARACTER_STATE.E_TARGET_MOVE:
			{
				animator.SetTrigger ("Walk");

				//자신 보다 오른쪽에 있을 경우 
				if (transform.position.x <= m_VecFirstPosition.x) 
				{
					spriteRender.flipX = false;
				} 
				//자신 보다 왼쪽에 있을 경우
				else 
				{
					spriteRender.flipX = true;
				}
			}
			break;


		case E_CHARACTER_STATE.E_TARGET_CHARACTER_MOVE:
			{
				animator.SetTrigger ("Walk");

				if (targetCharacter == null)
					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);

				//자신 보다 오른쪽에 있을 경우 
				if (transform.position.x < targetCharacter.transform.position.x) {
					spriteRender.flipX = false;
				} 
				//자신 보다 왼쪽에 있을 경우
				else 
				{
					spriteRender.flipX = true;
				}
			}
			break;
		case E_CHARACTER_STATE.E_ATTACK:
			{
				animator.SetTrigger ("Idle");

			}
			break;
		case E_CHARACTER_STATE.E_CAST:
			{
				animator.SetTrigger ("Cast");

				m_fMaxCastTime = charicStats.activeSkill [nActiveSkillIndex].m_fCastTime;

				CastObject.SetActive (true);
			}
			break;
		case E_CHARACTER_STATE.E_CAST_SUCCESSED:
			{
				animator.SetTrigger ("CastSuccessed");

				m_fCastSuccessedTime = 0.6f;

				m_fCastTime = 0.0f;
			}
			break;
		case E_CHARACTER_STATE.E_DEAD:
			{
				if (spriteRender.flipX == true)
					StartCoroutine(TweenMove(new Vector3(transform.position.x + 1.5f,transform.position.y,transform.position.z),1.0f));

				else 
					StartCoroutine(TweenMove(new Vector3(transform.position.x - 1.5f,transform.position.y,transform.position.z),1.0f));

				animator.SetTrigger("Die");
			}
			break;
		}
	}

	protected override IEnumerator CharacterAction()
	{
		yield return new WaitForSeconds (0.1f);

		switch (E_CHARIC_STATE) {
		case E_CHARACTER_STATE.E_WAIT:
			{
				//공격 모드
				if (bIsMode) 
				{
					//현재 활성화 된 캐릭터들 중에서 인식 범위 안에 들어온 리스트 들을 반환 
					ArrayList targetLists = characterManager.FindTarget (this, charicStats.m_fSite);

					//만약 범위안에 들어온 캐릭터가 1개 이상일 경우 
					if (targetLists.Count > 0) {

						//제일 가까운 캐릭터를 반환한다.
						targetCharacter = (Character)targetLists [0];

						//찾은 캐릭터로 이동 
						CheckCharacterState (E_CHARACTER_STATE.E_TARGET_CHARACTER_MOVE);

						break;
					}
				} 
				//자유 모드
				else 
				{
					//현재 활성화 된 캐릭터들 중에서 인식 범위 안에 들어온 리스트 들을 반환 
					ArrayList targetLists = characterManager.FindTarget (this, charicStats.m_fAttack_Range);

					//만약 범위안에 들어온 캐릭터가 1개 이상일 경우 
					if (targetLists.Count > 0) {

						//제일 가까운 캐릭터를 반환한다.
						targetCharacter = (Character)targetLists [0];

						//찾은 캐릭터로 이동 
						CheckCharacterState (E_CHARACTER_STATE.E_ATTACK);

						break;
					}
				}
			}
			break;
		case E_CHARACTER_STATE.E_WALK:
			{
				//현재 활성화 된 캐릭터들 중에서 인식 범위 안에 들어온 리스트 들을 반환 
				ArrayList targetLists = characterManager.FindTarget (this, charicStats.m_fSite);

				//만약 범위안에 들어온 캐릭터가 1개 이상일 경우 
				if (targetLists.Count > 0) {

					//제일 가까운 캐릭터를 반환한다.
					targetCharacter = (Character)targetLists [0];

					//찾은 캐릭터로 이동 
					CheckCharacterState (E_CHARACTER_STATE.E_TARGET_CHARACTER_MOVE);

					break;
				}
			}
			break;

		case E_CHARACTER_STATE.E_TARGET_MOVE:
			{
				if (transform.position != m_VecFirstPosition) 
				{
					//캐릭터 레이어를 재정렬
					characterManager.SortingCharacterLayer();

					transform.position = Vector3.MoveTowards (transform.position, m_VecFirstPosition, Time.deltaTime * charicStats.m_fMoveSpeed);
				} 
				else 
				{
					spriteRender.flipX = false;
					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
				}
			}
			break;

		case E_CHARACTER_STATE.E_TARGET_CHARACTER_MOVE:
			{
				//현재 활성화 된 캐릭터들 중에서 인식 범위 안에 들어온 리스트 들을 반환 
				ArrayList targetLists = characterManager.FindTarget (this, charicStats.m_fSite);

				//만약 범위안에 들어온 캐릭터가 1개 이상일 경우 
				if (targetLists.Count > 0) {

					//제일 가까운 캐릭터를 반환한다.
					targetCharacter = (Character)targetLists [0];
				} 
				else 
				{
					targetCharacter = null;
				}

				//타겟 캐릭터가 도중에 없어졌을 경우 
				if (targetCharacter == null) {

					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
					break;
				}

				//캐릭터 레이어를 재정렬
				characterManager.SortingCharacterLayer();

				transform.position = Vector3.MoveTowards (transform.position, targetCharacter.transform.position, Time.deltaTime * charicStats.m_fMoveSpeed);

				//공격 범위안에 들어왔을 경우 
				if (Vector3.Distance (transform.position, targetCharacter.transform.position) < charicStats.m_fAttack_Range) {
					CheckCharacterState (E_CHARACTER_STATE.E_ATTACK);
				}
			}
			break;

		case E_CHARACTER_STATE.E_ATTACK:
			{
				m_fAttackTime += Time.deltaTime;

				//현재 활성화 된 캐릭터들 중에서 인식 범위 안에 들어온 리스트 들을 반환 
				ArrayList targetLists = characterManager.FindTarget (this, charicStats.m_fAttack_Range);

				//만약 범위안에 들어온 캐릭터가 1개 이상일 경우 
				if (targetLists.Count > 0) {

					//제일 가까운 캐릭터를 반환한다.
					targetCharacter = (Character)targetLists [0];
				} 
				else 
				{
					targetCharacter = null;
				}

				//타겟 캐릭터가 도중에 없어졌을 경우 
				if (targetCharacter == null) {

					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
					yield break;
				}

				//공격 시
				if (m_fAttackTime >= charicStats.m_fAttackSpeed) {

					m_fAttackTime = 0.0f;

					nActiveSkillIndex = -1;

					Attack ();
				}
			}
			break;
		case E_CHARACTER_STATE.E_CAST:
			{
				m_fCastTime += Time.deltaTime;

				if (m_fMaxCastTime < m_fCastTime) 
				{
					CheckCharacterState (E_CHARACTER_STATE.E_CAST_SUCCESSED);
				}
			}
			break;
		case E_CHARACTER_STATE.E_CAST_SUCCESSED:
			{
				m_fCastSuccessedTime -= Time.deltaTime;

				if (m_fCastSuccessedTime < 0) 
				{
					base.PlayActiveSkill (nActiveSkillIndex, false);

					CastObject.SetActive (false);

					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
				}
			}
			break;


		case E_CHARACTER_STATE.E_DEAD:
			{
				m_fDestoryTime += Time.deltaTime;

				if (m_fDestoryTime > m_fConstDestroyTime) 
				{
					characterManager.Remove (this);
					battleManager.CharacterDie (E_Type.E_Hero);
				}
			}
			break;
		}
	}
}
