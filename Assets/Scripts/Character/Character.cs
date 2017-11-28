using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	protected GameObject HealthActiveObject;	 						//피격시 체력 게이지를 보여줬다 사라지게 할 오브젝트

	protected Transform HealthSizeTransform;							//Sprite Render이기 때문에 ScaleX를 잡아서 조절 함  
	protected Transform damageTextTransform;

	public Character targetCharacter = null;							//공격하려는 오브젝트

	public E_Type E_CHARIC_TYPE = E_Type.E_None;						//0,플레이어 캐릭,적 캐릭 
	public E_CHARACTER_STATE E_CHARIC_STATE = E_CHARACTER_STATE.E_WAIT;	//상태 

	protected Animator animator;										//애니메이션
	protected SpriteRenderer spriteRender;								//스프라이트 

	protected CharacterStats charicStats = null;						//캐릭터에 관한 정보 

	protected BattleManager battleManager;
	protected CharacterManager characterManager;						//배치된 캐릭터들을 관리
	protected SkillManager skillManager;					
	//LIST<SkillData>
	protected int nActiveSkillIndex = -1;

	//캐릭터가 죽운뒤 투명도를 위함
	protected Color alphaColor;

	protected CharacterUI characterUI;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator> ();

		spriteRender = GetComponent<SpriteRenderer> ();

		HealthActiveObject = transform.GetChild (0).gameObject;

		HealthSizeTransform = HealthActiveObject.transform.GetChild (0).transform;

		damageTextTransform = transform.GetChild (1).transform;
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
	public virtual void Setup(CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager,BattleManager _BattleManager,  E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		Debug.Log("1");

		charicStats = new CharacterStats (_charic);

		//스킬 매니저와 캐릭터 매니저를 등록
		skillManager = _skillManager;
		characterManager = _charicManager;
		battleManager = _BattleManager;

		//타입을 저장
		E_CHARIC_TYPE = _E_TYPE;

		//캐릭터 타입이 자신 캐릭일 경우
		if(E_CHARIC_TYPE == E_Type.E_Hero)
		{
			//패시브 스킬들을 적용
			ActivePassiveSkill();

			//처음 위치를 미리 저장해둔다.
			m_VecFirstPosition = _vecPosition;
			gameObject.transform.position = m_VecFirstPosition;

			//캐릭터 UI들을 생성
			GameObject Charic_UI_Object= Instantiate(Resources.Load("Prefabs/Battle_Charic_Info") as GameObject);

			//생성된 캐릭터를 부모를 UI로 해줌
			Charic_UI_Object.transform.SetParent(battleManager.characterUI_Parent,false);

			//캐릭터 UI를 받아와서 세팅
			characterUI = Charic_UI_Object.GetComponent<CharacterUI>();
			characterUI.SetUp(charicStats.m_fHealth);
		}

		//캐릭터의 현재 체력을 셋팅
		m_fCurrentHp = charicStats.m_fHealth;
		m_fMaxHp = m_fCurrentHp;

		animator.runtimeAnimatorController = ObjectCashing.Instance.LoadAnimationController("Animation/" + charicStats.m_strJob);
	 }

	//액션이 변경이 됐을때 초기화를 진행
	public virtual void CheckCharacterState(E_CHARACTER_STATE _E_STATE){ }

	//FSM머신 처리
	protected virtual IEnumerator CharacterAction (){ yield return null; }

	public CharacterStats GetStats(){ return charicStats;}

	public float GetCurrentHealth() { return m_fCurrentHp; }

	//캐릭터가 데미지를 받았을시에 호출 되는 함수이다.
	public void TakeDamage(float _fDamage)
	{
		//데미지 계산 처리
		m_fCurrentHp -= _fDamage;

		if(m_fCurrentHp < 0)
		{
			m_fCurrentHp = 0;
		}

		if(E_CHARIC_TYPE == E_Type.E_Hero)
		{
			characterUI.ChangeHealth(m_fCurrentHp);
		}
	
		HealthSizeTransform.localScale = new Vector3 (m_fCurrentHp / m_fMaxHp, 1.0f, 1.0f);

		//캐릭터가 죽을지를 판별 
		if (m_fCurrentHp <= 0 && m_bIsDead == false) {

			//만약 캐릭터가 죽었다면 State를 Dead로 바꿈
			m_bIsDead = true;

			CheckCharacterState (E_CHARACTER_STATE.E_DEAD);
		} 
		else 
		{
			if (E_CHARIC_TYPE == E_Type.E_Enemy) {

				ShowDamage(_fDamage.ToString("F0"));

				//만약 체력바가 활성화 되지않았다면 활성화를 시킨후 코루틴을 호출
				if (!HealthActiveObject.activeSelf) {
					HealthActiveObject.SetActive (true);

					StartCoroutine (ShowHealthBar ());
				}
				//만약 호출 되지 않았다면 시간을 0으로 바꿔줌
				else 
				{
					m_fActiveHealthTime = 0.0f;
				}
			}
		}
	}

	public void TakeHeal(float _fHeal)
	{
		//데미지 계산 처리
		m_fCurrentHp += _fHeal;

		if(m_fCurrentHp < m_fMaxHp)
		{
			m_fCurrentHp = m_fMaxHp;
		}

		if(E_CHARIC_TYPE == E_Type.E_Hero)
		{
			characterUI.ChangeHealth(m_fCurrentHp);
		}

		HealthSizeTransform.localScale = new Vector3 (m_fCurrentHp / m_fMaxHp, 1.0f, 1.0f);
	}

	public void Dodge()
	{
		ShowDamage ("빗나감");

		for(int nIndex = 0; nIndex< charicStats.activeSkill.Count; nIndex++)
		{
			if(charicStats.activeSkill[nIndex].m_bIsCooltime)
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

	//패시브 스킬 적용 함수이다. 
	//1.현재 자신이 가진 패시브 개수만큼 돈다
	//2.패시브 옵션에 따라 증가되는 방식이 다르며 (% , +),증가되는 종류도 다르기 때문에 switch로 나눔
	protected void ActivePassiveSkill()
	{
		for(int nIndex = 0; nIndex < charicStats.passiveSkill.Count; nIndex++)
		{
			switch(charicStats.passiveSkill[nIndex].nOptionIndex)
			{
				case (int)E_PASSIVE_TYPE.E_HP:						
				
				charicStats.m_fHealth += charicStats.m_fHealth * charicStats.passiveSkill[nIndex].fValue * 0.01f;	
				
				break;
				
				case (int)E_PASSIVE_TYPE.E_ACCURACY:					
				
				charicStats.m_fAccuracy += charicStats.passiveSkill[nIndex].fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_PHYSICAL_ATTACK_RATING:		
				
				charicStats.m_fPhyiscal_Rating += charicStats.m_fPhyiscal_Rating * charicStats.passiveSkill[nIndex].fValue * 0.01f;	

				break;
				case (int)E_PASSIVE_TYPE.E_MAGIC_ATTACK_RATING:			
				
				charicStats.m_fMagic_Rating += charicStats.m_fMagic_Rating * charicStats.passiveSkill[nIndex].fValue * 0.01f;	
				
				break;
				case (int)E_PASSIVE_TYPE.E_PHYSICAL_DEFENCE:			
				
				charicStats.m_fPhysical_Defence += charicStats.passiveSkill[nIndex].fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_MAGIC_DEFENCE:				
				
				charicStats.m_fMasic_Defence += charicStats.passiveSkill[nIndex].fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_DODGE:						
				
				charicStats.m_fDodge += charicStats.passiveSkill[nIndex].fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_CRITICAL_RATING:				
				
				charicStats.m_fCritical_Rating += charicStats.passiveSkill[nIndex].fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_CRITICAL_DAMAGE:				
				
				charicStats.m_fCritical_Damage += charicStats.passiveSkill[nIndex].fValue;
				
				break;
				case (int)E_PASSIVE_TYPE.E_ATTACK_SPEED:				
				
				charicStats.m_fAttackSpeed += charicStats.m_fAttackSpeed * charicStats.passiveSkill[nIndex].fValue * 0.01f;
				
				break;
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
		if(nActiveSkillIndex == -1)
			yield break;

		nSkillIndex = nActiveSkillIndex;

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

	protected void ResetTargetCharacter(float _fRange)
	{
		
		ArrayList targetLists = characterManager.FindTarget (this,_fRange);

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
}
