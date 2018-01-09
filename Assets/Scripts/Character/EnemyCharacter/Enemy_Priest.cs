using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Enemy_Priest : Enemy_Character {

	protected override void Awake ()
	{
		base.Awake ();

	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	public override void Setup (CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager,BattleManager _BattleManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		base.Setup(_charic,_charicManager,_skillManager,_BattleManager, _E_TYPE,_vecPosition);		
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

				transform.Translate (Vector3.left * charicStats.m_fMoveSpeed * Time.deltaTime);
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

					animator.SetTrigger ("Attack");

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
					m_fCastTime = 0.0f;

					base.PlayActiveSkill (nActiveSkillIndex, false);

					CastObject.SetActive (false);

					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
				}
			}
			break;
		case E_CHARACTER_STATE.E_DEAD:
			{
				alphaColor.a = Mathf.Lerp(spriteRender.color.a,0,m_fDisableSpeed * Time.deltaTime);

				spriteRender.color = alphaColor;

				if(spriteRender.color.a == 0.0f)
				{
					characterManager.Remove(this);

				}
			}
			break;
		}
	}

	public override void Attack ()
	{
		nActiveSkillIndex = CheckActiveAttack ();

		if (nActiveSkillIndex == (int)E_SKILL_TYPE.E_NONE) 
		{
			Debug.Log ("BaseAttack");

			for (int nAttackCount = 0; nAttackCount < charicStats.basicSkill[0].nAttackNumber; nAttackCount++) {

				bool bIsCritical = false;

				//만약 공격 하려던 캐릭터가 죽어있다면
				if(targetCharacter.IsDead())
				{ 
					//다시 공격범위 안에 있는 캐릭터를 탐색
					ResetTargetCharacter(charicStats.m_fAttack_Range);

					//공격 범위안에 있는 캐릭이 null일 경우 종료
					if (targetCharacter == null)
						return;
				}

				nAttackCount++;

				//
				if(Random.Range(0,100) < charicStats.m_fCritical_Rating)
				{
					bIsCritical = true;

					for(int nIndex = 0; nIndex < charicStats.activeSkill.Count;nIndex++)
					{
						if(Random.Range(0,100) < charicStats.activeSkill[nIndex].m_fCriticalAttack_ActiveRating)
						{
							nActiveSkillIndex = nIndex;

							break;
						}
					}

					if(nActiveSkillIndex != (int)E_SKILL_TYPE.E_NONE)
					{
						StartCoroutine(SkillCoolTime());

						if (charicStats.activeSkill [nActiveSkillIndex].m_fCastTime == 0) 
						{
							base.PlayActiveSkill (nActiveSkillIndex, false);

						} 
						else 
						{
							CheckCharacterState (E_CHARACTER_STATE.E_CAST);
						}

						continue;
					}
				}

				//1.사거리안에 들어온 캐릭터가 1개 이상일 경우
				//2.적 캐릭터에서 공격 범위 만큼의 리스트를 구한 후
				//3.그 안에 있는 캐릭터에게 공격 횟수 만큼 데미지를 준다.
				ArrayList targetLists = characterManager.FindMyCharacterArea(this,targetCharacter,charicStats.basicSkill[0].fAttackArea);

				for (int nIndex = 0; nIndex < charicStats.basicSkill[0].nMaxTargetNumber; nIndex++) 
				{
					//만약 타겟수가 공격 해야할 캐릭터 수보다 적을 경우 종료
					if (targetLists.Count <= nIndex) {
						break;
					}

					skillManager.TargetHeal (this, (Character)targetLists [nIndex], bIsCritical);
				}
			}
		} 
		else 
		{
			StartCoroutine(SkillCoolTime());

			if (charicStats.activeSkill [nActiveSkillIndex].m_fCastTime == 0) 
			{
				base.PlayActiveSkill (nActiveSkillIndex, false);

			} 
			else 
			{
				CheckCharacterState (E_CHARACTER_STATE.E_CAST);
			}
		}
	}
}
