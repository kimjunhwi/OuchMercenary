using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Player : MonoBehaviour 
{
	//임시 캐릭터 박스 
	private const string characterBoxStr01 = "Character/CharacterBox/1Tier_Ancher_130";
	private const string characterBoxStr02 = "Character/CharacterBox/1Tier_Asassin_130";
	private const string characterBoxStr03 = "Character/CharacterBox/1Tier_Commander_130";
	private const string characterBoxStr04 = "Character/CharacterBox/1Tier_Knight_130";
	private const string characterBoxStr05 = "Character/CharacterBox/1Tier_Magics_130";
	private const string characterBoxStr06 = "Character/CharacterBox/1Tier_Priest_130";
	private const string characterBoxStr07 = "Character/CharacterBox/1Tier_Warrior_130";

	public List<ActiveSkill> LIST_activeSkill = new List<ActiveSkill>();

	public List<CharacterStats> LIST_HERO = new List<CharacterStats>();
	public List<DBBasicCharacter> LIST_CHARACTER = new List<DBBasicCharacter> ();

	public void Init()
	{
		ActiveSkill tempActiveSkill = null;

		CharacterStats _charic = new CharacterStats ();

		_charic.m_nIndex = 0;
		_charic.m_strJobName = "basicWarrior";
		_charic.m_strJob = "Warrior";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_MELEE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 5;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 5;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 1;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 1;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;
		_charic.m_nBatchIndex = 12;

		//임시 스킬을 부여함 --------------------------------------------------- 나중에 스킬이 확정 됐을 경우 싸악 수정(Unicode파싱 해서 처리)
		_charic.basicSkill.Add(new BasicSkill(1,1001,"a","attack",0,1,"warrior",1,1,100,0,1.0f,"enemy",1,1,"close","p_attack rating의 100%로 공격"));

		//Active
		tempActiveSkill = new ActiveSkill(0,"double Attack",1000,"0,10",2,1,"assassin",1,1,0,20,0,0,0,0,0,0,75,2,1,1,1,"enemy",1,"close",5,"pAttack ratring의 75%로 2회 공격",false);
			
		LIST_activeSkill.Add(tempActiveSkill);

		tempActiveSkill = new ActiveSkill(1,"Power Attack",1001,"1,8",2,2,"warrior",1,1,0,30,0,0,0,0,0,0,120,1,1,1,1,"enemy",1,"close",5,"pAttack ratring의 120%로 2회 공격",false);

		LIST_activeSkill.Add(tempActiveSkill);

		//Passive
		_charic.passiveSkill.Add(new PassiveSkill(0,GameManager.Instance.cAllPassiveOption[0]));
		_charic.passiveSkill.Add(new PassiveSkill(1,GameManager.Instance.cAllPassiveOption[1]));

		//높은것을 정렬retur
		LIST_activeSkill.Sort(delegate(ActiveSkill A, ActiveSkill B)
		{
			if (A.m_nTier < B.m_nTier) return 1;

			else return 0;
		});

		_charic.activeSkill = LIST_activeSkill;

		LIST_HERO.Add (_charic);

		_charic = new CharacterStats ();

		_charic.m_nIndex = 1;
		_charic.m_strJobName = "basicArcher";
		_charic.m_strJob = "Archer";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_RANGE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 10;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 10;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 5;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 1;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;
		_charic.m_nBatchIndex = 2;

		_charic.basicSkill.Add(new BasicSkill(1,1001,"a","attack",0,1,"archer",1,1,100,0,1.0f,"enemy",1,1,"close","p_attack rating의 100%로 공격"));

		LIST_HERO.Add (_charic);

		_charic = new CharacterStats ();

		_charic.m_nIndex = 2;
		_charic.m_strJobName = "basicCommander";
		_charic.m_strJob = "Commander";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_MELEE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 10;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 10;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 5;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 1;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;
		_charic.m_nBatchIndex = 7;

		LIST_HERO.Add (_charic);

		_charic = new CharacterStats ();

		_charic.m_nIndex = 1;
		_charic.m_strJobName = "basicPriest";
		_charic.m_strJob = "Priest";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_RANGE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 10;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 10;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 100f;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 1;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;
		_charic.m_nBatchIndex = 3;

		_charic.basicSkill.Add(new BasicSkill(5,1005,"a","attack",0,1,"priest",1,1,100,100,1.0f,"allay",1,1,"close",""));

		LIST_HERO.Add (_charic);

		_charic.m_nIndex = 1;
		_charic.m_strJobName = "basicPriest";
		_charic.m_strJob = "Priest";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_RANGE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 10;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 10;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 100f;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 1;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;
		_charic.m_nBatchIndex = 3;

		_charic.basicSkill.Add(new BasicSkill(5,1005,"a","attack",0,1,"priest",1,1,100,100,1.0f,"allay",1,1,"close",""));


		LIST_HERO.Add (_charic);


		_charic.m_nIndex =2;
		_charic.m_strJobName = "basicDilldar";
		_charic.m_strJob = "Priest";
		_charic.m_nLevel = 1;
		_charic.m_nTier = 1;
		_charic.m_nAttribute = (int)E_ATTRIBUTE.E_PHYSICAL;
		_charic.m_nAttackType = (int)E_ATTACK_TYPE.E_RANGE;
		_charic.m_nTribe = (int)E_TRIBE.E_HUMAN;
		_charic.m_fSite = 10;
		_charic.m_fHealth = 500;
		_charic.m_fAccuracy = 10;
		_charic.m_fPhyiscal_Rating = 5;
		_charic.m_fMagic_Rating = 5;
		_charic.m_fAttack_Range = 100f;
		_charic.m_fAttackSpeed = 1.0f;
		_charic.m_fMoveSpeed = 1;
		_charic.m_fPhysical_Defence = 5;
		_charic.m_fMasic_Defence = 5;
		_charic.m_fDodge = 1;
		_charic.m_fCritical_Rating = 1;
		_charic.m_fCritical_Damage = 1.5f;
		_charic.m_nBatchIndex = 3;

		_charic.basicSkill.Add(new BasicSkill(5,1005,"a","attack",0,1,"priest",1,1,100,100,1.0f,"allay",1,1,"close",""));


		LIST_HERO.Add (_charic);

		for (int i = 0; i < 53; i++) {
			LIST_CHARACTER.Add (GameManager.Instance.lDbBasicCharacter [i]);
		}


		GameManager.Instance.CharacterBoxImageLoad (characterBoxStr01);
		GameManager.Instance.CharacterBoxImageLoad (characterBoxStr02);
		GameManager.Instance.CharacterBoxImageLoad (characterBoxStr03);
		GameManager.Instance.CharacterBoxImageLoad (characterBoxStr04);
		GameManager.Instance.CharacterBoxImageLoad (characterBoxStr05);
		GameManager.Instance.CharacterBoxImageLoad (characterBoxStr06);
		GameManager.Instance.CharacterBoxImageLoad (characterBoxStr07);


	}

	public void AddCharacter(int _nIndex)
	{
		//Character _charic = GameManager.instance.GetCharicData(_nIndex);

		//if(_charic != null){
		//	LIST_HERO.Add(Character);
		//}
	}
}
