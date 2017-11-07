using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Character : MonoBehaviour {

	protected bool m_bIsFront ;
	protected bool m_bIsDead = false;

	protected float m_fAttackTime = 0.0f;

	protected float m_fMaxHp;
	protected float m_fCurrentHp;


	public Character targetCharacter = null;		//공격하려는 오브젝트

	public E_Type E_CHARIC_TYPE = E_Type.E_None;						//0,플레이어 캐릭,적 캐릭 
	public E_CHARACTER_STATE E_CHARIC_STATE = E_CHARACTER_STATE.E_WAIT;	//상태 

	protected Animator animator;										//애니메이션
	protected SpriteRenderer spriteRender;								//스프라이트 

	protected CharacterStats charicStats = null;						//캐릭터에 관한 정보 
	protected CharacterManager characterManager;						//배치된 캐릭터들을 관리
	protected SkillManager skillManager;					


	//LIST<SkillData>

	protected virtual void Awake()
	{
		animator = GetComponent<Animator> ();

		spriteRender = GetComponent<SpriteRenderer> ();
	}

	protected virtual void Start() { }

	protected virtual void Update(){ }

	protected virtual void Setup(CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager, E_Type _E_TYPE, int _nBatchIndex= 0){ }

	protected virtual void CheckCharacterState(E_CHARACTER_STATE _E_STATE){ }

	protected virtual IEnumerator CharacterAction (){ yield return null; }

	public void TakeDamage(float _fDamage)
	{
		m_fCurrentHp -= _fDamage;

		if (m_fCurrentHp <= 0)
			m_bIsDead = true;
	}

	public bool IsDead() { return m_bIsDead;}
}
