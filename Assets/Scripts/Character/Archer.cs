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

	public override void Setup (CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		skillManager = _skillManager;
		characterManager = _charicManager;

		E_CHARIC_TYPE = _E_TYPE;

		m_VecFirstPosition = _vecPosition;
		gameObject.transform.position = m_VecFirstPosition;

		charicStats = new CharacterStats (_charic);

		//임시 베이직 스킬을 부여함 ---------------------------------------------------
		charicStats.basicSkill = new BasicSkill(2,1002,"a","attack",0,1,"archer",1,1,100,100,"enemy",1,1,"close","p_attack rating의 100%로 공격");

		ActiveSkill active = new ActiveSkill(2,"ChargingShot",1002,"attack",2,1,"archer",1,2,0,0,0,5,0,0,0,0,120,120,1,5,3,"enemy",1,"close",0," 5회 공격시 마다 적 1명에게 p_AttackRating 150%의 피해를 입힌다",false);

		charicStats.activeSkill.Add(active);

		animator.runtimeAnimatorController = ObjectCashing.Instance.LoadAnimationController ("Animation/" + charicStats.m_strJob);

		if (_E_TYPE == E_Type.E_Enemy)
		{
			CheckCharacterState (E_CHARACTER_STATE.E_WALK);

			spriteRender.flipX = true;
		}

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

				if (targetCharacter == null) 
				{
					CheckCharacterState (E_CHARACTER_STATE.E_WALK);

					break;
				} 
				else 
				{
					//공격 하려던 캐릭터가 이미 죽어있을경우 대기 상태로 바꿈 
					if (targetCharacter.IsDead ()) 
					{
						CheckCharacterState (E_CHARACTER_STATE.E_WAIT);

						break;
					}

					//만약 현재 공격중인 적이 자신의 공격범위에서 벗어날 경우
					if (Vector3.Distance (transform.position, targetCharacter.transform.position) > charicStats.m_fAttack_Range) 
					{
						m_fAttackTime = 0.0f;

						targetCharacter = null;

						CheckCharacterState (E_CHARACTER_STATE.E_WALK);

						break;
					}

					//현재 활성화 된 캐릭터들 중에서 공격 범위 안에 들어온 리스트 들을 반환 
					ArrayList targetLists = characterManager.FindTarget (this, charicStats.m_fAttack_Range);

					//만약 범위안에 들어온 캐릭터가 1개 이상일 경우 
					if (targetLists.Count > 0) {
						//제일 가까운 캐릭터를 반환한다.
						targetCharacter = (Character)targetLists [0];
					}
				}

				if (m_fAttackTime >= charicStats.m_fAttackSpeed) {
					m_fAttackTime = 0.0f;

					animator.SetTrigger ("Attack");

					GameObject Arrow = arrowPool.GetObject();

					Projectile projectile = Arrow.GetComponent<Projectile> ();

					StartCoroutine(projectile.Shoot(arrowPool,transform.position,targetCharacter.transform.position,1.0f));

					Debug.Log ("Attack");
				}
			}
			break;

			case E_CHARACTER_STATE.E_DEAD:
			{
				alphaColor.a = Mathf.Lerp(spriteRender.color.a,0,1 * Time.deltaTime);

				spriteRender.color = alphaColor;
			}
			break;
		}
	}
}
