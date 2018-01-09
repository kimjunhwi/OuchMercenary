using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadOnlys
{
	//메인씬 UI버튼의 인덱스들
	public enum E_ACTIVEBUTTON
	{
		E_ACTIVEBUTTON_BLACKSMITH = 0,
		E_ACTIVEBUTTON_BOOK,
		E_ACTIVEBUTTON_EMPLOYMENT,
		E_ACTIVEBUTTON_HEALING,
		E_ACTIVEBUTTON_INVEN,
		E_ACTIVEBUTTON_MANAGEMENT,
		E_ACTIVEBUTTON_SHOP,
		E_ACTIVEBUTTON_STAGE,
		E_ACTIVEBUTTON_TRAINNING,
		E_ACTIVEBUTTON_OPTION,
	}

	public enum E_PRECHARACTERSLOT_STATE : int
	{
		E_PRECSLOT_STATE_SCROOLUP = 0,
		E_PRECSLOT_STATE_SCROOLDOWN = 1,
		E_PRECSLOT_STATE_SCROOLSTOP = 2,
	}

	public enum E_STAGE_INDEX : int
	{
		E_STAGE_INDEX_DEFENSE = 0,
		E_STAGE_INDEX_ATTACK,
		E_STAGE_INDEX_INFINITE,
	}

	//전투 준비 씬에서의 캐릭터 타입 분류
	public enum E_PREPAREBATTLE_CHARCTERTYPE : int
	{
		E_PREPAREBATTLE_CHARCTERTYPE_TOTAL = 0,
		E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER,
		E_PREPAREBATTLE_CHARCTERTYPE_MELEE,
		E_PREPAREBATTLE_CHARCTERTYPE_RANGE,
		E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE,
	}

	public enum E_LOAD_STATE : int
	{
		E_LOAD_GET_BASICCHARACTERDATA = 0,

	}

	public enum E_LOGIN_PORVIDER_INDEX : int
	{
		E_GOOGLE = 0,
		E_FACEBOOK,
		E_GUEST,
	};

	//각 씬에 대한 인덱스
	public enum E_SCENE_INDEX : int
	{
		E_NONE = -1,
		E_LOGO = 0,
		E_START,
		E_MENU,
		E_BATTLE,
		E_STAGE,
		E_MERMANAGE,
		E_LOADING,
		E_LOADING_SHORT,
		E_STAGE_HEALING,
		E_STAGE_TRAINNIG,
		E_EMPLOYER,
	};

	//물리 타입, 마법 타입, 무
	enum E_ATTRIBUTE : int
	{
		E_PHYSICAL = 0,
		E_MAGIC,
		E_NONE,
	};

	//공격 타입 
	enum E_ATTACK_TYPE : int
	{
		E_MELEE = 0,
		E_RANGE,
	};

	//종족
	enum E_TRIBE : int
	{
		E_HUMAN = 0,

	};

	// Type ------------------------------------
	public enum E_Type
	{
		E_None = 0,
		E_Hero,
		E_Enemy,
	};

	//각 캐릭터들의 대한 상태들
	public enum E_CHARACTER_STATE
	{
		E_WAIT = 0,
		E_WALK,
		E_TARGET_MOVE,
		E_TARGET_CHARACTER_MOVE,
		E_ATTACK,
		E_CAST,
		E_DEAD,
	};

	public enum E_SKILL_TYPE
	{
		E_NONE = -1,
	}

	//패시브 스킬의 타입 종류
	public enum E_PASSIVE_TYPE
	{
		E_HP,						//HP 증가 감소
		E_ACCURACY,					//명중률 증가 감소
		E_PHYSICAL_ATTACK_RATING,	//물리 공격령 증가 감소
		E_MAGIC_ATTACK_RATING,		//마법 공격력 증가 감소
		E_PHYSICAL_DEFENCE,			//물리 방어력 증가 감소
		E_MAGIC_DEFENCE,			//마법 방어력 증가 감소
		E_DODGE,					//회피율 증가 감소
		E_CRITICAL_RATING,			//크리티컬 확률 증가 감소
		E_CRITICAL_DAMAGE,			//크리티컬 데미지 확률 증가 감소
		E_ATTACK_SPEED,				//공격 속도 증가 감소
	}


	//
	public enum E_TARGET : int
	{
		E_TARGET_ENEMY = 0,
		E_TARGET_ALLAY,
		E_TARGET_SELF,
		E_TARGET_ALLY_MIN_HEALTH,
	}

	public enum E_ACTIVE_TYPE : int
	{
		E_ATTACK = 0,
		E_BLEED,
		E_BURN,
		E_POISON,
		E_BUFF_HP,
		E_BUFF_ACCURACY,
		E_BUFF_ATTACK_RANGE,
		E_BUFF_P_ATTACK_RATING,
		E_BUFF_M_ATTACK_RATING,
		E_BUFF_ATTACK_SPEED,
		E_BUFF_P_DEFENCE,
		E_BUFF_M_DEFENCE,
		E_BUFF_DODGE,
		E_BUFF_CRITICAL_RATING,
		E_BUFF_CRITICAL_DAMAGE,
		E_BUFF_P_PENETRATE,
		E_BUFF_M_PENETRATE,
		E_BUFF_COOLTIME,
		E_STRUN,
		E_MIN_HEAL,
		E_HEAL,
	}

	public enum E_CHARACTER_TYPE
	{
		E_ALL = 0,
		E_ASSASIN,
		E_WARRIOR,
		E_PRIEST,
		E_WIZZARD,
		E_ARCHER,
		E_COMMAND,
		E_KNIGHT,
	}


	#region Class 


[System.Serializable]
	public class CharacterStats
	{

		public int m_nIndex;				//인덱스
		
		public string m_strJobName; 		//주 직업
		
		public string m_strCharicName;		//캐릭 이름
		
		public int m_nEnhace;				//강화 횟수
		
		public string m_strJob;				//직업 
		
		public int m_nLevel;				//레벨
		
		public int m_nTier;					//티어 

		public int m_nAttribute;			//공격 속성 (물리 , 마법 , x)
		
		public int m_nAttackType;			//공격 타입 (근거리, 원거리) 
		
		public int m_nTribe;				//종족 (인간 등..)

		public float m_fSite;				//적 인식 범위 
		
		public float m_fHealth;				//체력
		
		public float m_fAccuracy;			//명중률

		public float m_fAttack_Range;		//공격 사거리

		public float m_fPhyiscal_Rating;	//물리 공격력(%)
		
		public float m_fMagic_Rating;		//마법 공격력(%)

		public float m_fAttackSpeed;		//공격 속도

		public float m_fMoveSpeed;			//이동 속도

		public float m_fPhysical_Defence;	//물리 방어력
		
		public float m_fMasic_Defence;		//마법 방어력

		public float m_fDodge;				//회피율
		
		public float m_fCritical_Rating;	//크리 확률
		
		public float m_fCritical_Damage;	//크리 데미지

		public float m_fPhysicalPenetrate;	//물리 관통

		public float m_fMagicPenetrate;		//마법 관통

		public float m_fCC_Reg;				//??

		public float m_fExp;				//경험치 (다음 레벨 경험치는 따로 저장)

		public float m_fMaxExp;				//임시

		public int m_nBatchIndex;			//배치 인덱스 

		public List<BasicSkill> basicSkill = new List<BasicSkill>();

		public List<ActiveSkill> activeSkill = new List<ActiveSkill>();

		public List<PassiveSkill> passiveSkill = new List<PassiveSkill>();

		public CharacterStats(CharacterStats _charic)
		{
			m_nIndex 			= _charic.m_nIndex;
			m_strJobName 		= _charic.m_strJobName;
			m_strCharicName 	= _charic.m_strCharicName;
			m_nEnhace 			= _charic.m_nEnhace;
			m_strJob 			= _charic.m_strJob;
			m_nLevel 			= _charic.m_nLevel;
			m_nTier 			= _charic.m_nTier;
			m_nAttribute 		= _charic.m_nAttribute;
			m_nAttackType		= _charic.m_nAttackType;
			m_nTribe 			= _charic.m_nTribe;
			m_fSite 			= _charic.m_fSite;
			m_fHealth 			= _charic.m_fHealth;
			m_fAccuracy 		= _charic.m_fAccuracy;
			m_fPhyiscal_Rating 	= _charic.m_fPhyiscal_Rating;
			m_fMagic_Rating 	= _charic.m_fMagic_Rating;
			m_fAttack_Range 	= _charic.m_fAttack_Range;
			m_fAttackSpeed 		= _charic.m_fAttackSpeed;
			m_fMoveSpeed 		= _charic.m_fMoveSpeed;
			m_fPhysical_Defence = _charic.m_fPhysical_Defence;
			m_fMasic_Defence 	= _charic.m_fMasic_Defence;
			m_fDodge 			= _charic.m_fDodge;
			m_fCritical_Rating 	= _charic.m_fCritical_Rating;
			m_fCritical_Damage 	= _charic.m_fCritical_Damage;
			m_nBatchIndex 		= _charic.m_nBatchIndex;
			basicSkill = _charic.basicSkill;
			activeSkill = _charic.activeSkill;
			passiveSkill = _charic.passiveSkill;
		}

		public CharacterStats()
		{

		}
	}

	[System.Serializable]
	public class ActiveSkill
	{
		public int m_nIndex;
		public string m_strName;
		public int m_nCharacterIndex;
		public string m_strAttackType;
		public int m_nSkillClass;
		public int m_nTier;
		public string m_strJob;
		public int m_nAttribute;						//공격 속성
		public int m_nAttackType;						//공격 타입
		public int m_nActivePriority;					//공격 우선 순위
		public float m_fAttack_ActvieRating;			//공격시 발동 확률
		public float m_fCriticalAttack_ActiveRating;	//크리티컬 공격시 발동 확률
		public int m_nAttackCount_ActiveRating;			//n번 공격시 발동 확률
		public float m_fMiss_ActiveRating;				//Miss시 발동 확률 
		public float m_fDodgy_ActiveRating;				//회피시 발동 확률
		public float m_fHit_ActiveRating;				//데미지를 받았을때 발동 확률
		public float m_fCoolTime;				//쿨타임
		public float m_fCastTime;				//캐스팅
		public float m_fPhysicalMagnification;  //물리 공격 배율
		public float m_fMagicMagnification;		//마법 공격 배율
		public int m_nAttackNumber;				//공격 횟수
		public float m_fAttackRange;			//공격 범위
		public float m_fAttackArea;				//공격 스킬 범위
		public int m_nMaxTargetNumber; 			//최대 공격 개수
		public string m_strAttackPriority;
		public float m_fKnockback_Power;
		public float m_fDuration;				//지속시간
		public string m_strEffectName;
		public string m_strAnimationClip;
		public string m_strExplanation; 			//설명
		public bool m_bIsCooltime;				//사용 할 수 있는가 

		public ActiveSkill(int _nIndex,int _nCharacterIndex,string _strName,string _strAttackType , int _nSkillClass,int _nTier,string _strJob,
							int _nAttribute,int _nAttackType, int _nActivePriority,float _fAttack_ActiveRating,float _fCriticalAttack_ActiveRating,
			int _nAttackCount_ActiveRating,float _fMiss_ActiveRating, float _fDodgy_ActiveRating,float _fHit_ActiveRating,float _fCoolTime,float _fCastTime,
							float _fPhysicalMagnification,float _fMagicMagnification,int _nAttackNumber,float _fAttackRange,float _fAttackArea, 
			int _nMaxTargetNumber,string _strAttackPriority,float  _fKnockback_Power, float _fDuration,string _strEffectName,string _strAnimationClip, string _strExplanation,bool _bIsCooltime)
		{
			m_nIndex = _nIndex;
			m_strName = _strName;
			m_nCharacterIndex = _nCharacterIndex;
			m_strAttackType = _strAttackType;
			m_nSkillClass = _nSkillClass;
			m_nTier = _nTier;
			m_strJob = _strJob;
			m_nAttribute = _nAttribute;
			m_nAttackType = _nAttackType;
			m_nActivePriority = _nActivePriority;
			m_fAttack_ActvieRating = _fAttack_ActiveRating;
			m_fCriticalAttack_ActiveRating = _fCriticalAttack_ActiveRating;
			m_nAttackCount_ActiveRating = _nAttackCount_ActiveRating;
			m_fMiss_ActiveRating = _fMiss_ActiveRating;
			m_fDodgy_ActiveRating = _fDodgy_ActiveRating;
			m_fHit_ActiveRating = _fHit_ActiveRating;
			m_fCoolTime = _fCoolTime;
			m_fCastTime = _fCastTime;
			m_nMaxTargetNumber = _nMaxTargetNumber;
			m_strAttackPriority = _strAttackPriority;
			m_fKnockback_Power = _fKnockback_Power;
			m_fDuration = _fDuration;
			m_strEffectName = _strEffectName;
			m_strAnimationClip = _strAnimationClip;
			m_strExplanation = _strExplanation;
			m_bIsCooltime = _bIsCooltime;
		}
	}

	[System.Serializable]
	public class BasicSkill {

    public int nIndex;             //스킬에 대한 인덱스
    public int nCharacterIndex;    //소유가능한 캐릭터 인덱스
    public string strSkillName;    //스킬 이름
    public string strSkillType;    //스킬 타입 0 =basic attack,1= formation, 2 = active attack, 3 =  buff, 4 = debuff,
    public int nTier;              //캐릭터 전직 단계
    public int nSkillClass;        //스킬 분류
    public string strJob;          //소유 가능한 직업
    public int nAttribute;         //속성 물리,마법인지
    public int nAttackType;        //공격 타입 (근접, 원거리, 0)
    public float fPhsyicMagnification;   //물리 속성 공격
    public float fMagicMagnification;    //마법 속성 공격
	public float fAttackArea;
    public string strSkillTarget;      //대상
    public int nMaxTargetNumber;       //최대 공격 개수
    public int nAttackNumber;          //공격 횟수
    public string strAttackPriority;   //공격 우선순위
    public string strExplanation;      //스킬 설명

    public BasicSkill(int _nindex,int _nCharacterIndex,string _strSkillName,string _strSkillType,int _nTier, int _nSkillClass,
                        string _strJob,int _nAttribute, int _nAttackType, int _nPhsyicMagnification, int _nMagicMagnification,float _fAttackArea,
						string _strSkillTarget, int _nMaxTargetNumber, int _nAttackNumber,string _strAttackPriority,string _strExplain)
                        {
                                nIndex = _nindex;
                                nCharacterIndex = _nCharacterIndex;
                                strSkillName = _strSkillName;
                                strSkillType = _strSkillType;
                                nTier = _nTier;
                                nSkillClass = _nSkillClass;
                                strJob = _strJob;
                                nAttribute = _nAttribute;
                                nAttackType = _nAttackType;
                                fPhsyicMagnification = _nPhsyicMagnification;
                                fMagicMagnification = _nMagicMagnification;
								fAttackArea = _fAttackArea;
                                strSkillTarget = _strSkillTarget;
                                nMaxTargetNumber = _nMaxTargetNumber;
                                nAttackNumber = _nAttackNumber;
                                strAttackPriority = _strAttackPriority;
                                strExplanation = _strExplain;

                        }
	}


	[System.Serializable]
	public class AllPassiveSkillData {
		public int nIndex;             //스킬에 대한 인덱스
		public int nCharacterIndex;    //소유가능한 캐릭터 인덱스
		public string strSkillName;    //스킬 이름
		public string strSkillType;    //스킬 타입 0 =basic attack,1= formation, 2 = active attack, 3 =  buff, 4 = debuff,
		public int nTier;              //캐릭터 전직 단계
		public int nSkillClass;        //스킬 분류
		public string strJob;          //소유 가능한 직업
		public int nAttribute;         //속성 물리,마법인지,두개다
		public int nAttackType;        //공격 타입 (근접, 원거리, 두개 =다)
		public string strOption_List;  //옵션 인덱스 
		public string strExplanation;  //스킬 설명
	}

	[System.Serializable]
	public class PassiveSkill
	{
		public int nIndex;

		public int nOptionIndex;

		public int nEnhaceValue;

		public float fValue;

		public float fPlus;

		public int nCalculate;

		public string strExplain;

		public PassiveSkill(int _nSkillIndex, AllPassiveSkillOptionData _passive)
		{
			nIndex = _nSkillIndex;

			nOptionIndex = _passive.nOptionIndex;

			fValue = _passive.fValue;

			fPlus = _passive.fPlus;

			nCalculate = _passive.nCalculate;

			strExplain = _passive.strExplain;
		}
	}

	[System.Serializable]
	public class AllPassiveSkillOptionData
	{
		public int nIndex;

		public int nOptionIndex;
		public float fValue;

		public float fPlus;

		public int nCalculate;

		public string strExplain;
	}


	public class TestCharacter
	{
		public int Index { get; set; }
		public List<CharacterSkill> characterSkills { get;set; }
	}


	public class CharacterSkill
	{
		public string skillName { get; set; }
		public int skillPower { get; set;}
	}

	[System.Serializable]
	public class DBBasicCharacter
	{
		public int Index { get; set; } 						// Hash key.

		public int C_Index { get; set; }					// CharacterIndex 

		public string C_JobNames { get; set;}				// Character 직업 이름

		public int C_JobIndex { get; set; }

		public string C_Name { get; set;}					// Character Name

		public int C_Enhance { get; set; }					// Character 강화 단계

		public string Jobs {get;set;}						// 직업 이름

		public int Levels { get; set;}						// Character Level

		public int Tier { get; set;}						// Chracter Tier

		public int Attribute { get; set; }					// Character 특성 (물리, 마법, None)

		public int AttackType { get; set;}					// 근거리, 원거리 타입

		public int Tribe { get; set;}						// Character 종족

		public float Site { get; set; }						// Character 인지범위

		public float Health { get; set;}					// Character 체력

		public float Accurancy { get; set;}					// Character 정확도

		public float AttackRange { get; set;}				// Character 공격 사거리

		public float Physic_AttackRating { get; set;}		// Character 물리 공격력

		public float Magic_AttackRating { get; set;}		// Character 마법 공격력

		public float AttackSpeed { get; set;}				// Character 공격 속도

		public float MoveSpeed { get; set;}					// Character 이동 속도

		public float Physic_Defense { get; set;}			// Character 물리 방어력

		public float Magic_Defense { get; set; }			// Character 마법 방어력

		public float Dodge { get; set;}						// Character 회피력

		public float Crit_Rating { get; set; }				// Character 크리 확률

		public float Crit_Dmg { get; set; }					// Character 크리 데미지

		public float Physic_Penetrate {get; set;}			// Character 물리 관통

		public float Magic_Penetrate {get; set;}			// Character 마법 관통

		public float CC_Registance {	get; set; }			// Character 상태이상 저항

		public float Exp {	get; set; }						// Character 현재 경험치

		public float ExpMax {	get; set; }					// Character 최대 경험치

		public float Betch_Index {	get; set; }				// Character 배치위치 
					
		public List<BasicSkill> basicSkill {get; set;}

		public List<ActiveSkill> activeSkills {get; set;}

		public DBBasicCharacter()
		{
		}

		public DBBasicCharacter(DBBasicCharacter _Charic)
		{
			C_Index = _Charic.C_Index;
			C_JobNames = _Charic.C_JobNames;
			C_JobIndex = _Charic.C_JobIndex;
			C_Name = _Charic.C_Name;
			C_Enhance = _Charic.C_Enhance;
			Jobs = _Charic.Jobs;
			Levels = _Charic.Levels;
			Tier = _Charic.Tier;
			Attribute = _Charic.Attribute;
			AttackType = _Charic.AttackType;
			Tribe = _Charic.Tribe;
			Site = _Charic.Site;
			Health = _Charic.Health;
			Accurancy = _Charic.Accurancy;
			AttackRange = _Charic.AttackRange;
			Physic_AttackRating = _Charic.Physic_AttackRating;
			Magic_AttackRating = _Charic.Magic_AttackRating;
			AttackSpeed = _Charic.AttackSpeed;
			MoveSpeed = _Charic.MoveSpeed;
			Physic_Defense = _Charic.Physic_Defense;
			Magic_Defense = _Charic.Magic_Defense;
			Dodge = _Charic.Dodge;
			Crit_Rating = _Charic.Crit_Rating;
			Crit_Dmg = _Charic.Crit_Dmg;
			Physic_Penetrate = _Charic.Physic_Penetrate;
			Magic_Penetrate = _Charic.Magic_Penetrate;
			CC_Registance = _Charic.CC_Registance;
			Exp = _Charic.Exp;
			ExpMax = _Charic.ExpMax;
			Betch_Index = _Charic.Betch_Index;

		}
	}

	public class DBPlayersCharacter
	{
		//해당플레이어의 캐릭터가 맞는지 체크 하는 2개의 변수
		public string UserEamil { get; set; } 				// Hash key.
		public string UserNick	{ get; set; }
						
		public List<DBBasicCharacter> Characters {get; set;}
	}
	[System.Serializable]
	public class DBBasicCharacter_Sealized
	{
		public int Index { get; set; } 						// Hash key.

		public int C_Index { get; set; }					// CharacterIndex 

		public string C_JobNames { get; set;}				// Character 직업 이름

		public string C_Name { get; set;}					// Character Name

		public int C_Enhance { get; set; }					// Character 강화 단계

		public string Jobs {get;set;}						// 직업 이름

		public int Levels { get; set;}						// Character Level

		public int Tier { get; set;}						// Chracter Tier

		public int Attribute { get; set; }					// Character 특성 (물리, 마법, None)

		public int AttackType { get; set;}					// 근거리, 원거리 타입

		public int Tribe { get; set;}						// Character 종족

		public float Site { get; set; }						// Character 인지범위

		public float Health { get; set;}					// Character 체력

		public float Accurancy { get; set;}					// Character 정확도

		public float AttackRange { get; set;}				// Character 공격 사거리

		public float Physic_AttackRating { get; set;}		// Character 물리 공격력

		public float Magic_AttackRating { get; set;}		// Character 마법 공격력

		public float AttackSpeed { get; set;}				// Character 공격 속도

		public float MoveSpeed { get; set;}					// Character 이동 속도

		public float Physic_Defense { get; set;}			// Character 물리 방어력

		public float Magic_Defense { get; set; }			// Character 마법 방어력

		public float Dodge { get; set;}						// Character 회피력

		public float Crit_Rating { get; set; }				// Character 크리 확률

		public float Crit_Dmg { get; set; }					// Character 크리 데미지

		public float Physic_Penetrate {get; set;}			// Character 물리 관통

		public float Magic_Penetrate {get; set;}			// Character 마법 관통

		public float CC_Registance {	get; set; }			// Character 상태이상 저항

		public float Exp {	get; set; }						// Character 현재 경험치

		public float ExpMax {	get; set; }					// Character 최대 경험치

		public float Betch_Index {	get; set; }				// Character 배치위치 

		public List<BasicSkill> basicSkill {get; set;}

		public List<ActiveSkill> activeSkills {get; set;}
	}

	[System.Serializable]
	public class AllActiveSkillType
	{
		public int nIndex;			//인덱스
		public int nActiveType;	//스킬 종류
		public int nTargetIndex;	//타겟인덱스(0 :적, 1: 우리팀, 2: 자신, 3: 최소 체력)
	}

	[System.Serializable]
	public class Equipment
	{
		public int nIndex;
		public string strName;
		public int nTier;
		public int nQulity;
		public string strPossibleJob;
		public int nEnhance;
		public int nEquimnetType;
		public int nPhysical_Attack_Rating;
		public int nMagic_Attack_Rating;
	}


	#endregion
}
