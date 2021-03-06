﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class Character : MonoBehaviour {

	protected float fBetween;
	protected Vector3 movePosition;


	public bool bIsMode = true;					//공격 모드 인지 수비 모드 인지 
	protected bool m_bIsFront ;
	protected bool m_bIsDead = false;			//캐릭터가 죽었는지

	//각 도트딜 및 상태에 따른 체크를 위함---------------------------------------
	protected bool m_bIsBleed = false;			//출혈상태
	protected bool m_bIsBurn = false;			//화상상태
	protected bool m_bIsPoison = false;			//중독상태
	protected bool m_bIsSturn = false;			//스턴상태

	protected float m_fDestoryTime = 0.0f;
	protected const float m_fConstDestroyTime = 0.5f;

	protected float m_fBleedTime = 0.0f;
	protected float m_fBurnTime = 0.0f;
	protected float m_fPoisonTime = 0.0f;
	protected float m_fSturnTime = 0.0f;
	protected float m_fCastTime = 0.0f;
	protected float m_fMaxCastTime = 0.0f;
	protected float m_fCastSuccessedTime = 0.0f;

	protected GameObject BleedObject;
	protected GameObject BurnObject;
	protected GameObject PoisonObject;
	protected GameObject SturnObject;
	protected GameObject CastObject;

	public ParticleSystem PhysicalParticle;
	public ParticleSystem MagicParticle;
	public ParticleSystem HealParticle;
	public ParticleSystem MechanicParticle;

	//------------------------------------------------------------------

	protected int m_nAttackCount = 0;			//n횟수 공격시 발동하는 엑티브 스킬을 위함

	protected float m_fAttackTime = 0.0f;		//
	protected float m_fActiveHealthTime = 0.0f; //체력 오브젝트의 활성화 되는 시간
	protected float m_fMaxHealthTime = 1.0f;	//체력 오브젝트가 활성화 되는 최대 시간
	protected float m_fMaxHp;					//캐릭터의 최대 체력을 저장
	protected float m_fCurrentHp;				//현재 캐릭터의 남은 체력
	protected readonly float m_fDisableSpeed = 2f;			//죽었을 때 사라지는 시간

	protected Vector3 m_VecFirstPosition;

	protected GameObject HealthActiveObject;	 						//피격시 체력 게이지를 보여줬다 사라지게 할 오브젝트

	protected Transform HealthSizeTransform;							//Sprite Render이기 때문에 ScaleX를 잡아서 조절 함  
	protected Transform damageTextTransform;

	public Character targetCharacter = null;							//공격하려는 오브젝트

	public ArrayList targetCharacter_LIST = new ArrayList();

	public E_Type E_CHARIC_TYPE;										//플레이어 캐릭,적 캐릭 
	public E_CHARACTER_STATE E_CHARIC_STATE = E_CHARACTER_STATE.E_WAIT;	//상태 

	protected Animator animator;										//애니메이션
	protected SpriteRenderer spriteRender;								//스프라이트 

	protected CharacterStats baseCharicStats = null;					//캐릭터 원본 데이터(얕은 복사)
	protected CharacterStats charicStats = null;						//캐릭터에 관한 정보 (깊은 복사)

	protected BattleManager battleManager;
	protected CharacterManager characterManager;						//배치된 캐릭터들을 관리
	protected SkillManager skillManager;					
	//LIST<SkillData>
	protected int nActiveSkillIndex = -1;

	//캐릭터가 죽운뒤 투명도를 위함
	protected Color alphaColor;
	protected CharacterUI characterUI;

	protected Transform arrowPosition;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator> ();

		spriteRender = GetComponent<SpriteRenderer> ();

		HealthActiveObject = transform.GetChild (0).gameObject;

		HealthSizeTransform = HealthActiveObject.transform.GetChild (0).transform;

		damageTextTransform = transform.GetChild (1).transform;

		BleedObject = transform.GetChild (2).gameObject;
		BurnObject = transform.GetChild (3).gameObject;
		PoisonObject = transform.GetChild (4).gameObject;
		SturnObject = transform.GetChild (5).gameObject;
		CastObject = transform.GetChild (6).gameObject;
		PhysicalParticle = transform.GetChild (7).GetComponent<ParticleSystem> ();
		MagicParticle = transform.GetChild (8).GetComponent<ParticleSystem> ();
		HealParticle = transform.GetChild (9).GetComponent<ParticleSystem> ();
		MechanicParticle = transform.GetChild (10).GetComponent<ParticleSystem> ();

		arrowPosition = transform.GetChild (11).GetComponent<Transform> ();
	}

	protected virtual void OnEnable()
	{
		m_nAttackCount = 0;

		alphaColor = new Color(255,255,255,255);

		spriteRender.color = alphaColor;
	}

	protected virtual void Start() { }

	public virtual void ActionUpdate(){ }

	//캐릭터에 대한 초기화 및 배치를 함
	public virtual void Setup(CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager,BattleManager _BattleManager,  E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		baseCharicStats = _charic;
		charicStats = new CharacterStats (_charic);

		//스킬 매니저와 캐릭터 매니저를 등록
		skillManager = _skillManager;
		characterManager = _charicManager;
		battleManager = _BattleManager;

		//타입을 저장
		E_CHARIC_TYPE = _E_TYPE;

		//캐릭터 타입이 플레이어 캐릭일 경우
		if (E_CHARIC_TYPE == E_Type.E_Hero) {
			//패시브 스킬들을 적용
			ActivePassiveSkill ();

			//처음 위치를 미리 저장해둔다.
			m_VecFirstPosition = _vecPosition;
			gameObject.transform.position = m_VecFirstPosition;

			//캐릭터 UI들을 생성
			GameObject Charic_UI_Object = Instantiate (Resources.Load ("Prefabs/Battle_Charic_Info") as GameObject);

			//생성된 캐릭터를 부모를 UI로 해줌
			Charic_UI_Object.transform.SetParent (battleManager.characterUI_Parent, false);

			//캐릭터 UI를 받아와서 세팅
			characterUI = Charic_UI_Object.GetComponent<CharacterUI> ();
			characterUI.SetUp (charicStats.m_fHealth);
		} else {

			gameObject.transform.position = _vecPosition;

			spriteRender.flipX = true;
		}
	
		//캐릭터의 현재 체력을 셋팅
		m_fCurrentHp = charicStats.m_fHealth;
		m_fMaxHp = m_fCurrentHp;

		animator.runtimeAnimatorController = ObjectCashing.Instance.LoadAnimationController("Animation/Character/"  + charicStats.m_strJob);

		CheckCharacterState (E_CHARACTER_STATE.E_WAIT);
	 }

	//액션이 변경이 됐을때 초기화를 진행
	public virtual void CheckCharacterState(E_CHARACTER_STATE _E_STATE){ }

	//FSM머신 처리
	protected virtual IEnumerator CharacterAction (){ yield return null; }

	public CharacterStats GetStats(){ return charicStats;}
	public CharacterStats GetBasicStats(){ return baseCharicStats; }

	public float GetCurrentHealth() { return m_fCurrentHp; }

	//캐릭터가 데미지를 받았을시에 호출 되는 함수이다.
	public virtual void TakeDamage(float _fDamage){ }

	//캐릭터가 체력을 회복할 경우 호출 함
	public virtual void TakeHeal(float _fHeal){ }

	public virtual void Attack(){ }

	public int CheckActiveAttack()
	{ 
		int nResultSkillIndex = (int)E_SKILL_TYPE.E_NONE;

		//1.캐릭터가 현재 가지고 있는 스킬 만큼 돈다.
		//2.쿨타임 중일 경우 다음 인덱스로
		//3.발동 확률을 체크 후 됐을 경우 활성화 시킬 스킬에 넣어준다.
		for (int nIndex = 0; nIndex < charicStats.activeSkill.Count; nIndex++) 
		{
			if(charicStats.activeSkill[nIndex].m_bIsCooltime == (int)E_COOLTIME.E_COOLTIME)
				continue;

			if(IsUseSkill(nIndex)) 
			{
				return nIndex;
			}
		}
	
		return nResultSkillIndex;
	}

	public void Dodge()
	{
		ShowDamage ("빗나감");

		for(int nIndex = 0; nIndex< charicStats.activeSkill.Count; nIndex++)
		{
			if(charicStats.activeSkill[nIndex].m_bIsCooltime == (int)E_COOLTIME.E_COOLTIME)
				continue;

			if(Random.Range(0,100) < charicStats.activeSkill[nIndex].m_fDodgy_ActiveRating)
			{
				nActiveSkillIndex = nIndex;

				StartCoroutine( SkillCoolTime());

				return;
			}
		}
		return;
	}

	protected IEnumerator TweenMove(Vector3 end, float time)
	{
		var t = 0f;
		var start = transform.position;
		while(t < 1f)
		{
			t += Time.deltaTime/time;
			transform.position = Vector3.Lerp(start, end, t);
			yield return null;
		}
	}



	//Skilll 관련 ------------------------------------------------------------

	//패시브 스킬 적용 함수이다. 
	//1.현재 자신이 가진 패시브 개수만큼 돈다
	//2.패시브 옵션에 따라 증가되는 방식이 다르며 (% , +),증가되는 종류도 다르기 때문에 switch로 나눔
	protected void ActivePassiveSkill()
	{
		for(int nIndex = 0; nIndex < charicStats.passiveSkill.Count; nIndex++)
		{
			switch(charicStats.passiveSkill[nIndex].optionData.nOptionIndex)
			{
				case (int)E_PASSIVE_TYPE.E_HP:						
				
				charicStats.m_fHealth += charicStats.m_fHealth * charicStats.passiveSkill[nIndex].optionData.fValue * 0.01f;	
				
				break;
				
				case (int)E_PASSIVE_TYPE.E_ACCURACY:					
				
				charicStats.m_fAccuracy += charicStats.passiveSkill[nIndex].optionData.fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_PHYSICAL_ATTACK_RATING:		
				
				charicStats.m_fPhyiscal_Rating += charicStats.m_fPhyiscal_Rating * charicStats.passiveSkill[nIndex].optionData.fValue * 0.01f;	

				break;
				case (int)E_PASSIVE_TYPE.E_MAGIC_ATTACK_RATING:			
				
				charicStats.m_fMagic_Rating += charicStats.m_fMagic_Rating * charicStats.passiveSkill[nIndex].optionData.fValue * 0.01f;	
				
				break;
				case (int)E_PASSIVE_TYPE.E_PHYSICAL_DEFENCE:			
				
				charicStats.m_fPhysical_Defence += charicStats.passiveSkill[nIndex].optionData.fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_MAGIC_DEFENCE:				
				
				charicStats.m_fMagic_Defence += charicStats.passiveSkill[nIndex].optionData.fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_DODGE:						
				
				charicStats.m_fDodge += charicStats.passiveSkill[nIndex].optionData.fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_CRITICAL_RATING:				
				
				charicStats.m_fCritical_Rating += charicStats.passiveSkill[nIndex].optionData.fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_CRITICAL_DAMAGE:				
				
				charicStats.m_fCritical_Damage += charicStats.passiveSkill[nIndex].optionData.fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_ATTACK_SPEED:				
				
				charicStats.m_fAttackSpeed += charicStats.m_fAttackSpeed * charicStats.passiveSkill[nIndex].optionData.fValue * 0.01f;
				
				break;
			}
		}
	}

	//Active Skill관련 -----------------------
	#region  ActiveSkill
	public void PlayActiveSkill(int _nActiveSkillIndex,bool _bIsCritical)
	{
		//Parsing된 스킬에서 SkillType과 Target을 받아옴
		//cf

		if(charicStats.activeSkill[_nActiveSkillIndex].m_strAnimationClip != "-1")
			animator.SetTrigger (charicStats.activeSkill [_nActiveSkillIndex].m_strAnimationClip);

		string[] strActiveTypes = charicStats.activeSkill[_nActiveSkillIndex].m_strSkillType.Split(',');

		for(int nIndex = 0; nIndex < strActiveTypes.Length; nIndex++)
		{
			targetCharacter_LIST.Clear ();

			AllActiveSkillType skillData = GameManager.Instance.cAllActiveType[int.Parse(strActiveTypes[nIndex])];

			if(skillData == null)
				return;

			targetCharacter_LIST = GetTargetLIST(skillData.nTargetIndex,_nActiveSkillIndex);

			if(targetCharacter_LIST.Count == 0)
				continue;

			skillManager.ActiveSkill(skillData.nActiveType,charicStats.activeSkill[_nActiveSkillIndex],this,targetCharacter_LIST,_bIsCritical);
		}		
	}
	
	ArrayList GetTargetLIST(int _nActiveSkillType,int _nActiveSkillIndex)
	{
		switch(_nActiveSkillType)
		{
			case (int)E_TARGET.E_TARGET_ENEMY:
			{
				return characterManager.FindTargetArea(this,targetCharacter,charicStats.activeSkill[_nActiveSkillIndex].m_fAttackRange);
			}
			case (int)E_TARGET.E_TARGET_ALLAY:
			{
				return characterManager.FindMyCharacterArea(this,targetCharacter,charicStats.activeSkill[_nActiveSkillIndex].m_fAttackArea);
			}
			case (int)E_TARGET.E_TARGET_ALLY_MIN_HEALTH:
			{
				return characterManager.FindMyMinHealthTarget(targetCharacter,charicStats.activeSkill[_nActiveSkillIndex].m_fAttackArea);
			}
			case (int)E_TARGET.E_TARGET_SELF:
			{
				ArrayList selfList = new ArrayList ();

				selfList.Add (this);

				return selfList;
			}
		}

		return null;
	}

	public void SetDetrimental(E_ACTIVE_TYPE _TYPE,float _fDamage,float _fTime)
	{
		switch(_TYPE)
		{
			case E_ACTIVE_TYPE.E_BLEED:
			{
				SetBleed(_fDamage,_fTime);
			}
			break;
			case E_ACTIVE_TYPE.E_BURN:
			{
				SetBurn(_fDamage,_fTime);
			}
			break;
			case E_ACTIVE_TYPE.E_POISON:
			{
				SetPoison(_fDamage,_fTime);
			}
			break;
		case E_ACTIVE_TYPE.E_STURN:
			{
				SetSturn(_fDamage,_fTime);
			}
			break;
		}
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
			m_nAttackCount = 0;

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
		if(nActiveSkillIndex == -1)
			yield break;

		nSkillIndex = nActiveSkillIndex;

		//스킬이 있을 경우 쿨타임을 돌림
		charicStats.activeSkill [nSkillIndex].m_bIsCooltime = (int)E_COOLTIME.E_COOLTIME;

		float fTime = 0.0f;

		//스킬 내부에 쿨타임 만큼 돌림
		while (fTime < charicStats.activeSkill [nSkillIndex].m_fCoolTime) 
		{
			fTime += Time.deltaTime;

			yield return null;
		}

		//만약 쿨타임이 다 됐다면 스킬을 False로 바꿔줌
		charicStats.activeSkill [nSkillIndex].m_bIsCooltime = (int)E_COOLTIME.E_NONE_COOLTIME;
	}

	#endregion


	//각 상태들(중독,화상,출혈,스턴)을 갱신하기 위한 코루틴들
	#region Detrimental 

	public void SetBleed(float _fValue,float _fTime)
	{
		if (m_bIsBleed)
			m_fBleedTime = (m_fBleedTime > _fTime) ? m_fBleedTime : _fTime;
		else {
		
			m_fBleedTime = _fTime;

			StartCoroutine (DelayBleedObject ());

		}
		StartCoroutine(SustainedDamage(_fValue,_fTime));
	}

	public IEnumerator DelayBleedObject()
	{
		yield return new WaitForSeconds(0.1f);

		m_bIsBleed =true;

		BleedObject.SetActive(true);

		while(m_fBleedTime > 0)
		{
			m_fBleedTime -= Time.deltaTime;

			yield return null;
		}

		BleedObject.SetActive(false);

		m_bIsBleed = false;
	}

	public void SetPoison(float _fValue,float _fTime)
	{
		if (m_bIsPoison)
			m_fPoisonTime = (m_fPoisonTime > _fTime) ? m_fPoisonTime : _fTime;
		else {
		
			m_fPoisonTime = _fTime;

			StartCoroutine (DelayPoisonObject ());
		}

		StartCoroutine(SustainedDamage(_fValue,_fTime));
	}

	public IEnumerator DelayPoisonObject()
	{
		yield return new WaitForSeconds(0.1f);

		m_bIsPoison =true;

		PoisonObject.SetActive(true);

		while(m_fPoisonTime > 0)
		{
			m_fPoisonTime -= Time.deltaTime;

			yield return null;
		}

		PoisonObject.SetActive(false);

		m_bIsPoison = false;
	}

	public void SetBurn(float _fValue,float _fTime)
	{
		if (m_bIsBurn)
			m_fBurnTime = (m_fBurnTime > _fTime) ? m_fBurnTime : _fTime;
		else {

			m_fBurnTime = _fTime;

			StartCoroutine (DelayBurnObject ());
		}
		StartCoroutine(SustainedDamage(_fValue,_fTime));
	}

	public IEnumerator DelayBurnObject()
	{
		yield return new WaitForSeconds(0.1f);

		m_bIsBurn =true;

		BurnObject.SetActive(true);

		while(m_fBurnTime > 0)
		{
			m_fBurnTime -= Time.deltaTime;

			yield return null;
		}

		BurnObject.SetActive(false);

		m_bIsBurn = false;
	}

	public void SetSturn(float _fValue,float _fTime)
	{
		if (m_bIsSturn)
			m_fSturnTime = (m_fSturnTime > _fTime) ? m_fSturnTime : _fTime;
		else {

			m_fSturnTime = _fTime;

			StartCoroutine (DelaySturnObject ());
		}

		StartCoroutine(SustainedDamage(_fValue,_fTime));
	}

	public IEnumerator DelaySturnObject()
	{
		yield return new WaitForSeconds(0.1f);

		m_bIsSturn =true;

		SturnObject.SetActive(true);

		while(m_fSturnTime > 0)
		{
			m_fSturnTime -= Time.deltaTime;

			yield return null;
		}

		SturnObject.SetActive(false);

		m_bIsSturn = false;
	}

	//지속 데미지를 주는 코루틴 1초마다 n데미지
	public IEnumerator SustainedDamage(float _fValue,float _fTime)
	{
		 yield return new WaitForSeconds(0.1f);

		int nAttackTime = (int)_fTime;

        while (nAttackTime > 0)
        {
            yield return new WaitForSeconds(1f);

			if(m_bIsDead)
				yield break;

			TakeDamage(_fValue);

			nAttackTime--;
        }
	}

	#endregion
	protected void ResetTargetCharacter(float _fRange)
	{
		
		ArrayList targetLists;

		if (charicStats.basicSkill [0].strSkillTarget != "enemy") 
		{
			//현재 활성화 된 캐릭터들 중 가장 체력이 적은 우리팀 캐릭터 리스트를 반환
			targetLists = characterManager.FindMyMinHealthTarget (this,_fRange);
		} 
		else 
		{
			//현재 활성화 된 캐릭터들 중에서 지정한 사거리 안에 들어온 리스트 들을 반환 
			targetLists = characterManager.FindTarget (this,_fRange);
		}

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

	public void ShowDamage(string _strDamage)
	{
		Camera camera = Camera.main;

		GameObject damageText = battleManager.damageTextPool.GetObject ();

		damageText.transform.SetParent (battleManager.damagetParentTransfrom,false);
		damageText.transform.localScale = Vector3.one;
		damageText.transform.position = camera.WorldToScreenPoint(damageTextTransform.position);
		damageText.name = "Damage";

		DamageTextPool damagePool = damageText.GetComponent<DamageTextPool> ();
		damagePool.Damage (_strDamage);
		damagePool.textObjPool = battleManager.damageTextPool;
		damagePool.leftSecond = 1f;

		battleManager.SortDamageTextLayer ();
	}

	//캐릭터의 재정렬을 위함 Y축이 낮을 수록 앞으로 나와야 함 (쿼터뷰 식)
	public void SortingLayer(int _nIndex)
	{
		spriteRender.sortingOrder = _nIndex;
	}

	public bool IsDead() { return m_bIsDead;}

	public int UpCheck(Vector3 _vecStart, Vector3 _vecEnd)
	{
		if (GetAngle (_vecStart, _vecEnd) >= 75 && GetAngle (_vecStart, _vecEnd) <= 115) {
			return 1;
		} else if (GetAngle (_vecStart, _vecEnd) <= -75 && GetAngle (_vecStart, _vecEnd) >= -115) {
			return -1;
		}
		return 0;
	}

	public static float GetAngle (Vector3 vStart, Vector3 vEnd)
	{
		Vector3 v = vEnd - vStart;

		return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
	}
}
