using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Character : MonoBehaviour {

	public bool bIsMode = true;	//공격 모드 인지 수비 모드 인지 
	protected bool m_bIsFront ;
	protected bool m_bIsDead = false;

	protected int m_nAttackCount = 0;

	protected float m_fAttackTime = 0.0f;

	protected float m_fMaxHp;
	protected float m_fCurrentHp;

	protected Vector3 m_VecFirstPosition;


	public Character targetCharacter = null;		//공격하려는 오브젝트

	public E_Type E_CHARIC_TYPE = E_Type.E_None;						//0,플레이어 캐릭,적 캐릭 
	public E_CHARACTER_STATE E_CHARIC_STATE = E_CHARACTER_STATE.E_WAIT;	//상태 

	protected Animator animator;										//애니메이션
	protected SpriteRenderer spriteRender;								//스프라이트 

	protected CharacterStats charicStats = null;						//캐릭터에 관한 정보 
	protected CharacterManager characterManager;						//배치된 캐릭터들을 관리
	protected SkillManager skillManager;					
	//LIST<SkillData>

	//캐릭터가 죽운뒤 투명도를 위함
	protected Color alphaColor;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator> ();

		spriteRender = GetComponent<SpriteRenderer> ();
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

	public void TakeDamage(float _fDamage)
	{
		m_fCurrentHp -= _fDamage;

		if (m_fCurrentHp <= 0 && m_bIsDead == false)
		{
			m_bIsDead = true;

			CheckCharacterState(E_CHARACTER_STATE.E_DEAD);
		}
	}

	public void SortingLayer(int _nIndex)
	{
		spriteRender.sortingOrder = _nIndex;
	}

	public bool IsDead() { return m_bIsDead;}
}
