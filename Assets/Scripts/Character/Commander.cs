using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ReadOnlys;

public class Commander : Character {

	protected override void Awake ()
	{
		base.Awake ();

	}

	//초기화
	public override void Setup (CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		skillManager = _skillManager;
		characterManager = _charicManager;

		E_CHARIC_TYPE = _E_TYPE;

		m_VecFirstPosition = _vecPosition;
		gameObject.transform.position = m_VecFirstPosition;

		charicStats = new CharacterStats (_charic);

		animator.runtimeAnimatorController = ObjectCashing.Instance.LoadAnimationController("Animation/" + charicStats.m_strJob);
	}


	protected override void Update ()
	{
		StartCoroutine(this.CharacterAction());


		if(Input.GetMouseButtonDown(0))
		{
			//UI가 아닐경우
			if (EventSystem.current.IsPointerOverGameObject () == false) {
				//오브젝트 피킹
				GameObject pickingObject = GameManager.Instance.GetRaycastObject2D ();

				if(pickingObject == null)
					return;
			
				//만약 바닥일 경우
				if (pickingObject.CompareTag ("Ground")) {
					//누른 위치를 대입후 그 위치로 이동하는 액션을 취함
					m_VecFirstPosition = GameManager.Instance.GetScreenPosition ();

					m_VecFirstPosition.z = 0.0f;

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

					CheckCharacterState (E_CHARACTER_STATE.E_TARGET_MOVE);
				}
			}
		}
	}

	//각 액션에 대한 초기화를 진행함
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
				
			}
			break;
		case E_CHARACTER_STATE.E_WALK:
			{
				
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

		case E_CHARACTER_STATE.E_ATTACK:
			{
				
			}
			break;
		}
	}
}
