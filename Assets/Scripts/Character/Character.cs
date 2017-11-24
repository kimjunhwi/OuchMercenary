using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Character : MonoBehaviour {

	public bool bIsMode = true;					//공격 모드 인지 수비 모드 인지 
	protected bool m_bIsFront ;
	protected bool m_bIsDead = false;			//캐릭터가 죽었는지

	protected int m_nAttackCount = 0;			//n횟수 공격시 발동하는 엑티브 스킬을 위함

	protected float m_fAttackTime = 0.0f;		//
	protected float m_fActiveHealthTime = 0.0f; //체력 오브젝트의 활성화 되는 시간
	protected float m_fMaxHealthTime = 1.0f;	//체력 오브젝트가 활성화 되는 최대 시간
	protected float m_fMaxHp;					//캐릭터의 최대 체력을 저장
	protected float m_fCurrentHp;				//현재 캐릭터의 남은 체력
	protected readonly float m_fDisableSpeed = 2f;			//죽었을 때 사라지는 시간

	protected Vector3 m_VecFirstPosition;

	protected GameObject HealthActiveObject;							//피격시 체력 게이지를 보여줬다 사라지게 할 오브젝트
	protected Transform HealthSizeTransform;							//Sprite Render이기 때문에 ScaleX를 잡아서 조절 함  

	public Character targetCharacter = null;							//공격하려는 오브젝트

	public E_Type E_CHARIC_TYPE = E_Type.E_None;						//0,플레이어 캐릭,적 캐릭 
	public E_CHARACTER_STATE E_CHARIC_STATE = E_CHARACTER_STATE.E_WAIT;	//상태 

	protected Animator animator;										//애니메이션
	protected SpriteRenderer spriteRender;								//스프라이트 

	protected CharacterStats charicStats = null;						//캐릭터에 관한 정보 
	protected CharacterManager characterManager;						//배치된 캐릭터들을 관리
	protected SkillManager skillManager;					
	//LIST<SkillData>
	protected ActiveSkill activeSkill = null;

	//캐릭터가 죽운뒤 투명도를 위함
	protected Color alphaColor;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator> ();

		spriteRender = GetComponent<SpriteRenderer> ();

		HealthActiveObject = transform.GetChild (0).gameObject;

		HealthSizeTransform = HealthActiveObject.transform.GetChild (0).transform;
	}

	protected virtual void OnEnable()
	{
		m_nAttackCount = 0;

		alphaColor = new Color(255,255,255,255);

		spriteRender.color = alphaColor;
	}

	protected virtual void Start() { }

	protected virtual void Update(){ }

	//캐릭터에 대한 초기화 및 배치를 함
	public virtual void Setup(CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0){ }

	//액션이 변경이 됐을때 초기화를 진행
	public virtual void CheckCharacterState(E_CHARACTER_STATE _E_STATE){ }

	//FSM머신 처리
	protected virtual IEnumerator CharacterAction (){ yield return null; }

	//캐릭터가 데미지를 받았을시에 호출 되는 함수이다.
	public void TakeDamage(float _fDamage)
	{
		//데미지 계산 처리
		m_fCurrentHp -= _fDamage;
	
		HealthSizeTransform.localScale = new Vector3 (m_fCurrentHp / m_fMaxHp, 1.0f, 1.0f);

		//캐릭터가 죽을지를 판별 
		if (m_fCurrentHp <= 0 && m_bIsDead == false) {

			//만약 캐릭터가 죽었다면 State를 Dead로 바꿈
			m_bIsDead = true;

			CheckCharacterState (E_CHARACTER_STATE.E_DEAD);
		} 
		else 
		{
			//만약 체력바가 활성화 되지않았다면 활성화를 시킨후 코루틴을 호출
			if (!HealthActiveObject.activeSelf) 
			{
				HealthActiveObject.SetActive (true);

				StartCoroutine (ShowHealthBar());
			}
			//만약 호출 되지 않았다면 시간을 0으로 바꿔줌
			else 
			{
				m_fActiveHealthTime = 0.0f;
			}
		}

	}

	IEnumerator ShowHealthBar()
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

	
	//스킬을 사용 한지 체크한다.
	protected bool IsUseSkill(int nIndex)
	{
		//공격시 발동 되는 확률이다.
		if (Random.Range (0, 100) < charicStats.activeSkill [nIndex].m_fAttack_ActvieRating) 
		{
			return true;
		} 

		//n회 공격시 발동되는 스킬이다.
		else if (charicStats.activeSkill [nIndex].m_nAttackCount_ActiveRating != 0 &&
				charicStats.activeSkill [nIndex].m_nAttackCount_ActiveRating < m_nAttackCount) 
		{
			return true;
		}

		return false;
	}

	//스킬 쿨타임을 위한 함수이며 
	//만약 현재 쿨타임을 스킬 스킬이 없거나 캐릭터 내에 없을 경우 종료 시킨다.
	protected IEnumerator SkillCoolTime()
	{
		int nSkillIndex = -1;

		//쿨타임 시킬 스킬이 없을 경우
		if(activeSkill == null)
			yield break;

		//캐릭터가 가진 액티브 스킬의 개수 만큼 돌려서 확인
		for(int nIndex = 0; nIndex < charicStats.activeSkill.Count; nIndex++)
		{
			if(charicStats.activeSkill[nIndex] == activeSkill)
			{
				nSkillIndex = nIndex;
				break;
			}
		}

		//만약 없었을 경우 종료
		if(nSkillIndex == -1)
			yield break;


		//스킬이 있을 경우 쿨타임을 돌림
		charicStats.activeSkill [nSkillIndex].m_bIsCooltime = true;

		float fTime = 0.0f;

		//스킬 내부에 쿨타임 만큼 돌림
		while (fTime < charicStats.activeSkill [nSkillIndex].m_fCoolTime) 
		{
			fTime += Time.deltaTime;

			yield return null;
		}

		//만약 쿨타임이 다 됐다면 스킬을 False로 바꿔줌
		charicStats.activeSkill [nSkillIndex].m_bIsCooltime = false;
	}

	protected void ResetTargetCharacter(Character _StartCharacter, float _fRange)
	{
		//현재 활성화 된 캐릭터들 중에서 지정한 사거리 안에 들어온 리스트 들을 반환 
		ArrayList targetLists = characterManager.FindTarget (_StartCharacter,_fRange);

		//만약 0보다 크면 넣어주고 찾지 못했을 경우 대기 상태로 변경
		if(targetLists.Count > 0)
		{
			targetCharacter = (Character)targetLists[0];
		}
		else
		{
			targetCharacter = null;
			CheckCharacterState(E_CHARACTER_STATE.E_WAIT);
		}
	}

	//캐릭터의 재정렬을 위함 Y축이 낮을 수록 앞으로 나와야 함 (쿼터뷰 식)
	public void SortingLayer(int _nIndex)
	{
		spriteRender.sortingOrder = _nIndex;
	}

	public bool IsDead() { return m_bIsDead;}
}
