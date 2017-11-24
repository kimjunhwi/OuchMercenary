using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Warrior : Character {


	protected override void Awake ()
	{
		base.Awake ();

	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	public override void Setup (CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		charicStats = new CharacterStats (_charic);

		m_fCurrentHp = charicStats.m_fHealth;
		m_fMaxHp = m_fCurrentHp;

		skillManager = _skillManager;
		characterManager = _charicManager;

		E_CHARIC_TYPE = _E_TYPE;

		m_VecFirstPosition = _vecPosition;

		gameObject.transform.position = m_VecFirstPosition;

		animator.runtimeAnimatorController = ObjectCashing.Instance.LoadAnimationController("Animation/" + charicStats.m_strJob);
	}


	protected override void Update ()
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
			case E_CHARACTER_STATE.E_DEAD:
			{
				spriteRender.flipX = false;

				animator.SetBool("Dead",true);
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

				//찾은 캐릭터가 없고 자신이 적일경우 왼쪽으로 이동
				if (E_CHARIC_TYPE == E_Type.E_Enemy)
					transform.Translate (Vector3.left * charicStats.m_fMoveSpeed * Time.deltaTime);
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

				//null 일 경우 재탐색
				if (targetCharacter == null) 
				{
					ResetTargetCharacter(this,charicStats.m_fAttack_Range);
				} 
				else 
				{
					//공격 하려던 캐릭터가 이미 죽어있을경우 타겟을 갱신 
					if (targetCharacter.IsDead ()) 
					{
						ResetTargetCharacter(this,charicStats.m_fAttack_Range);
					}

					//만약 현재 공격중인 적이 자신의 공격범위에서 벗어날 경우
					if (Vector3.Distance (transform.position, targetCharacter.transform.position) > charicStats.m_fAttack_Range) 
					{
						m_fAttackTime = 0.0f;

						ResetTargetCharacter(this,charicStats.m_fAttack_Range);
					}
				}

				if(targetCharacter == null)
					yield break;

				//공격 시
				if (m_fAttackTime >= charicStats.m_fAttackSpeed) {
					m_fAttackTime = 0.0f;

					animator.SetTrigger ("Attack");

					//활성화 시킬 스킬을 null
					activeSkill = null;

					//1.캐릭터가 현재 가지고 있는 스킬 만큼 돈다.
					//2.쿨타임 중일 경우 다음 인덱스로
					//3.발동 확률을 체크 후 됐을 경우 활성화 시킬 스킬에 넣어준다.
					for (int nIndex = 0; nIndex < charicStats.activeSkill.Count; nIndex++) 
					{
						if(charicStats.activeSkill[nIndex].m_bIsCooltime)
							continue;

						if(IsUseSkill(nIndex)) 
						{
							activeSkill = charicStats.activeSkill [nIndex];
							break;
						}
					}

					if (activeSkill == null) {

						Debug.Log ("BaseAttack");

						for (int nAttackCount = 0; nAttackCount < charicStats.basicSkill.nAttackNumber; nAttackCount++) {

							if(targetCharacter.IsDead())
							{ 
								ResetTargetCharacter(this,charicStats.m_fAttack_Range);

								if(targetCharacter == null)
									yield break;
							}
							nAttackCount++;

							//1.사거리안에 들어온 캐릭터가 1개 이상일 경우
							//2.적 캐릭터에서 공격 범위 만큼의 리스트를 구한 후
							//3.그 안에 있는 캐릭터에게 공격 횟수 만큼 데미지를 준다.
							ArrayList targetLists = characterManager.FindTarget(targetCharacter,charicStats.basicSkill.fAttackArea);

							for (int nIndex = 0; nIndex < charicStats.basicSkill.nMaxTargetNumber; nIndex++) {
								//만약 타겟수가 공격 해야할 캐릭터 수보다 적을 경우 종료
								if (targetLists.Count <= nIndex) {
									break;
								}

								skillManager.BasicAttack (this, (Character)targetLists [nIndex]);
							}
						}
					} 
					else 
					{
						Debug.Log (activeSkill.m_strName);

						StartCoroutine(SkillCoolTime());
					}
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
}
