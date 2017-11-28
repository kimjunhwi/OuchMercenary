using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Archer : Character {

	SimpleObjectPool arrowPool;

	protected override void Awake ()
	{
		base.Awake ();

		arrowPool = GameObject.Find("ArrowObjectPool").GetComponent<SimpleObjectPool>();
	}

	public override void Setup (CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager,BattleManager _BattleManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		base.Setup(_charic,_charicManager,_skillManager,_BattleManager, _E_TYPE,_vecPosition);
	}

	protected override void Update ()
	{
		StartCoroutine(this.CharacterAction());
	}

	protected override void OnEnable()
	{
		base.OnEnable();
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
				if (transform.position.x <= targetCharacter.transform.position.x) {
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
				//방어  모드
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
					transform.position = Vector3.MoveTowards (transform.position, m_VecFirstPosition, Time.deltaTime * charicStats.m_fMoveSpeed);

					//캐릭터 레이어를 재정렬
					characterManager.SortingCharacterLayer();
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
				if (targetCharacter == null) {
					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
					break;
				}

				//캐릭터 레이어를 재정렬
				characterManager.SortingCharacterLayer();

				transform.position = Vector3.MoveTowards (transform.position, targetCharacter.transform.position, Time.deltaTime * charicStats.m_fMoveSpeed);

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
					ResetTargetCharacter(charicStats.m_fAttack_Range);
				} 
				else 
				{
					//공격 하려던 캐릭터가 이미 죽어있을경우 타겟을 갱신 
					if (targetCharacter.IsDead ()) 
					{
						ResetTargetCharacter(charicStats.m_fAttack_Range);

						if(targetCharacter == null)
							yield break;
					}

					//만약 현재 공격중인 적이 자신의 공격범위에서 벗어날 경우
					if (Vector3.Distance (transform.position, targetCharacter.transform.position) > charicStats.m_fAttack_Range) 
					{
						m_fAttackTime = 0.0f;

						ResetTargetCharacter(charicStats.m_fAttack_Range);

						if(targetCharacter == null)
							yield break;
					}
				}

				if(targetCharacter == null)
					yield break;


				if (m_fAttackTime >= charicStats.m_fAttackSpeed) {
					m_fAttackTime = 0.0f;

					animator.SetTrigger ("Attack");

					nActiveSkillIndex = (int)E_SKILL_TYPE.E_NONE;

					//1.캐릭터가 현재 가지고 있는 스킬 만큼 돈다.
					//2.쿨타임 중일 경우 다음 인덱스로
					//3.발동 확률을 체크 후 됐을 경우 활성화 시킬 스킬에 넣어준다.
					for (int nIndex = 0; nIndex < charicStats.activeSkill.Count; nIndex++) 
					{
						if(charicStats.activeSkill[nIndex].m_bIsCooltime)
							continue;

						if(IsUseSkill(nIndex)) 
						{
							nActiveSkillIndex = nIndex;
							break;
						}
					}

					if (nActiveSkillIndex == (int)E_SKILL_TYPE.E_NONE) {

						Debug.Log ("BaseAttack");

						for (int nAttackCount = 0; nAttackCount < charicStats.basicSkill[0].nAttackNumber; nAttackCount++) {

							bool bIsCritical = false;

							//만약 공격 하려던 캐릭터가 죽어있다면
							if(targetCharacter.IsDead())
							{ 
								//다시 공격범위 안에 있는 캐릭터를 탐색
								ResetTargetCharacter(charicStats.m_fAttack_Range);

								//공격 범위안에 있는 캐릭이 null일 경우 종료
								if(targetCharacter == null)
									yield break;
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
									
									continue;
								}
							}

							//1.사거리안에 들어온 캐릭터가 1개 이상일 경우
							//2.적 캐릭터에서 공격 범위 만큼의 리스트를 구한 후
							//3.그 안에 있는 캐릭터에게 공격 횟수 만큼 데미지를 준다.
							ArrayList targetLists = characterManager.FindTargetArea(this,targetCharacter,charicStats.basicSkill[0].fAttackArea);

							for (int nIndex = 0; nIndex < charicStats.basicSkill[0].nMaxTargetNumber; nIndex++) {
								//만약 타겟수가 공격 해야할 캐릭터 수보다 적을 경우 종료
								if (targetLists.Count <= nIndex) {
									break;
								}
								
								//데미지 계산을 미리 한 후 화살에 데미지를 인자로 보낸후 데미지 처리를 한다. 
								GameObject Arrow = arrowPool.GetObject();

								Projectile projectile = Arrow.GetComponent<Projectile> ();

								StartCoroutine(projectile.Shoot(arrowPool,skillManager,this,targetCharacter,bIsCritical));

								//skillManager.BasicAttack (this, (Character)targetLists [nIndex],bIsCritical);
							}
						}
					} 
					else 
					{
						Debug.Log (charicStats.activeSkill[nActiveSkillIndex].m_strName);

						StartCoroutine(SkillCoolTime());
					}
				}
			}
			break;

			case E_CHARACTER_STATE.E_DEAD:
			{
				alphaColor.a = Mathf.Lerp(spriteRender.color.a,0,1 * Time.deltaTime);

				spriteRender.color = alphaColor;

				if(spriteRender.color.a == 0.0f)
				{

				}
			}
			break;
		}
	}
}
