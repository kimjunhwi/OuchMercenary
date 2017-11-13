using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Enemy_Archer : Character {

	protected override void Awake ()
	{
		base.Awake ();

	}

	public override void Setup (CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		//캐릭터에 대한 정보를 갱신
		charicStats = new CharacterStats (_charic);

		//스킬 매니저 및 캐릭터 매니저를 대입
		skillManager = _skillManager;
		characterManager = _charicManager;

		//(적 or 자신) 캐릭터를 구분하기 위해 대입
		E_CHARIC_TYPE = _E_TYPE;


		//지정해준 위치로 대입후 갱신
		m_VecFirstPosition = _vecPosition;
		gameObject.transform.position = m_VecFirstPosition;

		animator.runtimeAnimatorController = ObjectCashing.Instance.LoadAnimationController("Animation/" + charicStats.m_strJob);

		CheckCharacterState (E_CHARACTER_STATE.E_WALK);

		spriteRender.flipX = true;
	}


	protected override void Update ()
	{
		StartCoroutine(this.CharacterAction());
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

		case E_CHARACTER_STATE.E_WALK:
			{
				animator.SetTrigger ("Walk");


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
		}
	}

	protected override IEnumerator CharacterAction()
	{
		yield return new WaitForSeconds (0.1f);

		switch (E_CHARIC_STATE) {

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
		case E_CHARACTER_STATE.E_TARGET_CHARACTER_MOVE:
			{
				if (targetCharacter == null) {
					CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
					break;
				}

				transform.position = Vector3.MoveTowards (transform.position, targetCharacter.transform.position, Time.deltaTime * charicStats.m_fMoveSpeed);

				//캐릭터 레이어를 재정렬
				characterManager.SortingCharacterLayer();

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

					Debug.Log ("Attack");
				}
			}
			break;
		}
	}
}
