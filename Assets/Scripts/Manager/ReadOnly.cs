using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadOnlys
{

	//각 씬에 대한 인덱스
	enum E_SCENE_INDEX : int
	{
		E_LOGO = 0,
		E_START,
		E_MENU,
		E_BATTLE,
	}

	//물리 타입, 마법 타입, 무
	enum E_ATTRIBUTE : int
	{
		E_PHYSICAL = 0,
		E_MAGIC,
		E_NONE,
	}

	//공격 타입 
	enum E_ATTACK_TYPE : int
	{
		E_MELEE = 0,
		E_RANGE,
	}

	//종족
	enum E_TRIBE : int
	{
		E_HUMAN = 0,

	}

	// Type ------------------------------------
	public enum E_Type
	{
		E_None = 0,
		E_Hero,
		E_Enemy,
	};

	public enum E_CHARACTER_STATE
	{
		E_WAIT = 0,
		E_WALK,
		E_TARGET_MOVE,
		E_TARGET_CHARACTER_MOVE,
		E_ATTACK,
		E_DEAD,
	}

	public enum E_SKILL_TYPE
	{
		E_BASIC = 0,
		E_PASSIVE,
		E_ACTIVE,
	}


	#region Class 


[System.Serializable]
	public class CharacterStats{

		public int m_nIndex;				//인덱스
		public string m_strName; 			//주 직업
		public string m_strCharicName;		//캐릭 이름
		public int m_nEnhace;				//강화 횟수
		public string m_strJob;				//직업 
		public int m_nLevel;				//레벨
		public int m_nTier;					//티어 

		public int m_nAttribute;			//공격 속성 (물리 , 마법 , x)
		public int m_nAttackType;			//공격 타입 (근거리, 원거리) 
		public int m_nTribe;				//종족 (인간 등..)

		public float m_fSite;					//적 인식 범위 
		public float m_fHealth;				//체력
		public float m_fAccuracy;			//명중률

		public float m_fPhyiscal_Rating;	//물리 공격력(%)
		public float m_fMagic_Rating;		//마법 공격력(%)

		public float m_fAttack_Range;		//공격 사거리 
		public float m_fAttack_Area;		//공격 범위

		public int m_nMaxTargetNumber;			//대상 개수
		public int m_nAttack_Priority;		//공격 우선 순위

		public float m_fAttackSpeed;		//공격 속도
		public float m_fMoveSpeed;			//이동 속도

		public float m_fPhysical_Defence;	//물리 방어력
		public float m_fMasic_Defence;		//마법 방어력 

		public float m_fDodge;				//회피율
		public float m_fCritical_Rating;	//크리 확률
		public float m_fCritical_Damage;	//크리 데미지

		public int m_nBatchIndex;			//배치 인덱스 

		public BasicSkill basicSkill;

		public List<ActiveSkill> activeSkill = new List<ActiveSkill>();

		public CharacterStats(CharacterStats _charic)
		{
			m_nIndex 			= _charic.m_nIndex;
			m_strName 			= _charic.m_strName;
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
			m_fAttack_Area 		= _charic.m_fAttack_Area;
			m_nMaxTargetNumber 	= _charic.m_nMaxTargetNumber;
			m_nAttack_Priority 	= _charic.m_nAttack_Priority;
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
		public float m_fDodgy_ActiveRating;				//회피시 발동 확률
		public float m_fHit_ActiveRating;				//데미지를 받았을때 발동 확률
		public float m_fCoolTime;				//쿨타임
		public float m_fCastSpeed;				//캐스팅 속도
		public float m_fPhysicalMagnification;  //물리 공격 배율
		public float m_fMagicMagnification;		//마법 공격 배율
		public int m_nAttackNumber;				//공격 횟수
		public float m_fAttackRange;			//공격 범위
		public float m_fAttackArea;				//공격 스킬 범위
		public string m_strSkillTarget;			//스킬 타겟 (적 or 자신)
		public int m_nMaxTargetNumber; 			//최대 공격 개수
		public string m_strAttackPriority;
		public float m_fDuration;				//지속시간
		public string m_strExplanation; 			//설명
		public bool m_bIsActive;				//활성화 할 수 있는가

		public ActiveSkill(int _nIndex,string _strName,int _nCharacterIndex,string _strAttackType , int _nSkillClass,int _nTier,string _strJob,
							int _nAttribute,int _nAttackType, int _nActivePriority,float _fAttack_ActiveRating,float _fCriticalAttack_ActiveRating,
							int _nAttackCount_ActiveRating,float _fDodgy_ActiveRating,float _fHit_ActiveRating,float _fCoolTime,float _fCastTime,
							float _fPhysicalMagnification,float _fMagicMagnification,int _nAttackNumber,float _fAttackRange,float _fAttackArea, 
							string _strSkillTarget,int _nMaxTargetNumber,string _strAttackPriority, float _fDuration,string _strExplanation,bool _bIsActive)
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
			m_fDodgy_ActiveRating = _fDodgy_ActiveRating;
			m_fHit_ActiveRating = _fHit_ActiveRating;
			m_fCoolTime = _fCoolTime;
			m_fCastSpeed = _fCastTime;
			m_strSkillTarget = _strSkillTarget;
			m_nMaxTargetNumber = _nMaxTargetNumber;
			m_strAttackPriority = _strAttackPriority;
			m_fDuration = _fDuration;
			m_strExplanation = _strExplanation;
			m_bIsActive = _bIsActive;
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
    public int nPhsyicMagnification;   //물리 속성 공격
    public int nMagicMagnification;    //마법 속성 공격
    public string strSkillTarget;      //대상
    public int nMaxTargetNumber;       //최대 공격 개수
    public int nAttackNumber;          //공격 횟수
    public string strAttackPriority;   //공격 우선순위
    public string strExplanation;      //스킬 설명

    public BasicSkill(int _nindex,int _nCharacterIndex,string _strSkillName,string _strSkillType,int _nTier, int _nSkillClass,
                        string _strJob,int _nAttribute, int _nAttackType, int _nPhsyicMagnification, int _nMagicMagnification,string _strSkillTarget,
                        int _nMaxTargetNumber, int _nAttackNumber,string _strAttackPriority,string _strExplain)
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
                                nPhsyicMagnification = _nPhsyicMagnification;
                                nMagicMagnification = _nMagicMagnification;
                                strSkillTarget = _strSkillTarget;
                                nMaxTargetNumber = _nMaxTargetNumber;
                                nAttackNumber = _nAttackNumber;
                                strAttackPriority = _strAttackPriority;
                                strExplanation = _strExplain;

                        }
	}


	[System.Serializable]
	public class PassiveSkill {

    public int nIndex;             //스킬에 대한 인덱스
    public int nCharacterIndex;    //소유가능한 캐릭터 인덱스
    public string strSkillName;    //스킬 이름
    public string strSkillType;    //스킬 타입 0 =basic attack,1= formation, 2 = active attack, 3 =  buff, 4 = debuff,
    public int nTier;              //캐릭터 전직 단계
    public int nSkillClass;        //스킬 분류
    public string strJob;          //소유 가능한 직업
    public int nAttribute;         //속성 물리,마법인지,두개다
    public int nAttackType;        //공격 타입 (근접, 원거리, 두개 =다)
	public List<int> Option_List;		//옵션 인덱스 
    public string strExplanation;      //스킬 설명

    public PassiveSkill(int _nindex,int _nCharacterIndex,string _strSkillName,string _strSkillType,int _nTier, int _nSkillClass,
                        string _strJob,int _nAttribute, int _nAttackType,List<int> _option_list, string _strExplain)
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
								Option_List = _option_list;
                                strExplanation = _strExplain;

                        }
	}


	#endregion
}
