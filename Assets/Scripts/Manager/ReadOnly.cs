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
		E_ATTACK,
	}


	#region Class 


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

		public int m_nTargetNumber;			//대상 개수
		public int m_nAttack_Priority;		//공격 우선 순위

		public float m_fAttackSpeed;		//공격 속도
		public float m_fMoveSpeed;			//이동 속도

		public float m_fPhysical_Defence;	//물리 방어력
		public float m_fMasic_Defence;		//마법 방어력 

		public float m_fDodge;				//회피율
		public float m_fCritical_Rating;	//크리 확률
		public float m_fCritical_Damage;	//크리 데미지

		public CharacterStats(CharacterStats _charic)
		{
			m_nIndex = _charic.m_nIndex;
			m_strName = _charic.m_strName;
			m_strCharicName = _charic.m_strCharicName;
			m_nEnhace = _charic.m_nEnhace;
			m_strJob = _charic.m_strJob;
			m_nLevel = _charic.m_nLevel;
			m_nTier = _charic.m_nTier;
			m_nAttribute = _charic.m_nAttribute;
			m_nAttackType = _charic.m_nAttackType;
			m_nTribe = _charic.m_nTribe;
			m_fSite = _charic.m_fSite;
			m_fHealth = _charic.m_fHealth;
			m_fAccuracy = _charic.m_fAccuracy;
			m_fPhyiscal_Rating = _charic.m_fPhyiscal_Rating;
			m_fMagic_Rating = _charic.m_fMagic_Rating;
			m_fAttack_Range = _charic.m_fAttack_Range;
			m_fAttack_Area = _charic.m_fAttack_Area;
			m_nTargetNumber = _charic.m_nTargetNumber;
			m_nAttack_Priority = _charic.m_nAttack_Priority;
			m_fAttackSpeed = _charic.m_fAttackSpeed;
			m_fMoveSpeed = _charic.m_fMoveSpeed;
			m_fPhysical_Defence = _charic.m_fPhysical_Defence;
			m_fMasic_Defence = _charic.m_fMasic_Defence;
			m_fDodge = _charic.m_fDodge;
			m_fCritical_Rating = _charic.m_fCritical_Rating;
			m_fCritical_Damage = _charic.m_fCritical_Damage;
		}

		public CharacterStats()
		{

		}
	}

	#endregion
}
