using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Player : MonoBehaviour 
{
    //New Adds For Test
    //해당플레이어의 캐릭터가 맞는지 체크 하는 2개의 변수
    public string UserNick { get; set; }            // Hash key.
    public string UserEmail { get; set; }

    public List<DBEquipment> Equipments { get; set; }
    public List<DBBasicCharacter> Characters { get; set; }
    public List<Mail> mail { get; set; }

	public List<Equipment> LIST_ITEM = new List<Equipment>();

	public List<CharacterStats> TEST_MY_HERO = new List<CharacterStats>();

	public List<DBBasicCharacter> LIST_CHARACTER = new List<DBBasicCharacter> ();

	int nGold;

	int nLastStageIndex;

	public bool bIsStageLoop = false;

	public int nDefenceChapterOne = 0;
	public int nDefenceChapterTwo = 0;

	public int m_nChapterType = (int)E_STAGE_INDEX.E_STAGE_INDEX_ATTACK;

	public void Init()
	{
		TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1000)); //basic assassin
		TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1002)); //basic assassin
		TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1003)); //basic wizard
		//TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1013)); //thunder mage 
		//TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1018)); //high assassin
		//TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1030)); //hero assassin
		//TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1025)); //high thundermage
		//TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1037)); //hero thunder mage
		TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1032)); //hero warrior
		TEST_MY_HERO.Add (GameManager.Instance.SummonCharacter (1006)); //assassin
		TEST_MY_HERO.Add (GameManager.Instance.SummonCharacter (1040)); //hero priest


		for (int i = 0; i < 53; i++) {
			LIST_CHARACTER.Add ( GameManager.Instance.lDbBasicCharacter [i]);
		}
	}

	public Equipment AddItem(E_EQUIMENT_TYPE _type, int _nIndex)
	{
		Equipment addEquipment =new Equipment();

		switch (_type) 
		{
		case E_EQUIMENT_TYPE.E_WEAPON:
			{
				DBWeapon weaponDB = GameManager.Instance.lDbWeapon [_nIndex];

				addEquipment.nIndex = weaponDB.nIndex;
				addEquipment.strName = weaponDB.sName;
				addEquipment.nTier = weaponDB.nTier;
				addEquipment.nQulity = weaponDB.nQulity;
				addEquipment.strPossibleJob = weaponDB.sJob;
				addEquipment.nEnhance = weaponDB.nEnhanced;
				addEquipment.strEquimnetType = weaponDB.sEquipType;
				addEquipment.fPhysical_Attack_Rating = weaponDB.fPhysical_AttackRating;
				addEquipment.fMagic_Attack_Rating = weaponDB.fMagic_AttackRating;

				addEquipment.nSellCost = weaponDB.nSellCost;
				addEquipment.nMakeMaterialIndex = weaponDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex = weaponDB.nBreakMaterial;
			}
			break;
		case E_EQUIMENT_TYPE.E_ARMOR:
			{
				DBArmor weaponDB = GameManager.Instance.lDBArmor [_nIndex];

				addEquipment.nIndex = weaponDB.nIndex;
				addEquipment.strName = weaponDB.sName;
				addEquipment.nTier = weaponDB.nTier;
				addEquipment.nQulity = weaponDB.nQulity;
				addEquipment.strPossibleJob = weaponDB.sJob;
				addEquipment.nEnhance = weaponDB.nEnhanced;
				addEquipment.strEquimnetType = weaponDB.sEquipType;
				addEquipment.fPhysical_Defense = weaponDB.fPhysical_Defense;
				addEquipment.fMagic_Defense = weaponDB.fMagic_Defense;

				addEquipment.nSellCost = weaponDB.nSellCost;
				addEquipment.nMakeMaterialIndex = weaponDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex = weaponDB.nBreakMaterial;
			}
			break;
		case E_EQUIMENT_TYPE.E_GLOVE:
			{
				DBGlove weaponDB = GameManager.Instance.lDBGlove [_nIndex];

				addEquipment.nIndex = weaponDB.nIndex;
				addEquipment.strName = weaponDB.sName;
				addEquipment.nTier = weaponDB.nTier;
				addEquipment.nQulity = weaponDB.nQulity;
				addEquipment.strPossibleJob = weaponDB.sJob;
				addEquipment.nEnhance = weaponDB.nEnhanced;
				addEquipment.strEquimnetType = weaponDB.sEquipType;
				addEquipment.fPhysical_Defense = weaponDB.fPhysical_Defense;
				addEquipment.fMagic_Defense = weaponDB.fMagic_Defense;

				addEquipment.nSellCost = weaponDB.nSellCost;
				addEquipment.nMakeMaterialIndex = weaponDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex = weaponDB.nBreakMaterial;
			}
			break;
		case E_EQUIMENT_TYPE.E_ACCESSORY:
			{
				DBAccessory weaponDB = GameManager.Instance.lDBAccessory [_nIndex];

				addEquipment.nIndex = weaponDB.nIndex;
				addEquipment.strName = weaponDB.sName;
				addEquipment.nTier = weaponDB.nTier;
				addEquipment.nQulity = weaponDB.nQulity;
				addEquipment.strPossibleJob = weaponDB.sJob;
				addEquipment.nEnhance = weaponDB.nEnhanced;
				addEquipment.strEquimnetType = weaponDB.sEquipType;

				addEquipment.nSellCost = weaponDB.nSellCost;
				addEquipment.nMakeMaterialIndex = weaponDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex = weaponDB.nBreakMaterial;
			}
			break;
		}

		RandomAddOption (addEquipment, addEquipment.strEquimnetType);

		return addEquipment;
	}

	public void RandomAddOption(Equipment _ITEM,string _strRandomOption)
	{
		string[] strRandomOptions = _strRandomOption.Split (',');

		int nRandomIndex = int.Parse(strRandomOptions [Random.Range (0, strRandomOptions.Length)]);

		DBEquipment_RandomOption randomDB = GameManager.Instance.lDBEquipmentRandomOption [nRandomIndex];

		int nIndex = 0;

		while (nIndex < _ITEM.nQulity) 
		{
			switch (randomDB.nOptionIndex) {
			case (int)E_RANDOM_OPTION.E_HP:
				{
					if (_ITEM.fHp != 0)
						continue;

					_ITEM.fHp = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ACCURACY:
				{
					if (_ITEM.fAccuracy != 0)
						continue;

					_ITEM.fAccuracy = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ALL_ATTACK_RATING:
				{
					if (_ITEM.fAll_Attack_RatingPlus != 0)
						continue;

					_ITEM.fAll_Attack_RatingPlus = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_PHSYICAL_ATTACK_RATING:
				{
					if (_ITEM.fPhysical_Attack_Rating != 0)
						continue;

					_ITEM.fPhysical_Attack_Rating = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_MAGIC_ATTACK_RATING:
				{
					if (_ITEM.fMagic_Attack_Rating != 0)
						continue;

					_ITEM.fMagic_Attack_Rating = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ALL_PENETRATE:
				{
					if (_ITEM.fAll_Penetrate != 0)
						continue;

					_ITEM.fAll_Penetrate = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_PHSYICAL_PENETRATE:
				{
					if (_ITEM.fPhysical_Penetrate != 0)
						continue;

					_ITEM.fPhysical_Penetrate = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_MAGIC_PENETRATE:
				{
					if (_ITEM.fMagic_Penetrate != 0)
						continue;

					_ITEM.fMagic_Penetrate = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ALL_DEFENSE:
				{
					if (_ITEM.fAll_Defense != 0)
						continue;

					_ITEM.fAll_Defense = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_PHSYICAL_DEFENSE:
				{
					if (_ITEM.fPhysical_Defense != 0)
						continue;

					_ITEM.fPhysical_Defense = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_MAGIC_DEFENSE:
				{
					if (_ITEM.fMagic_Defense != 0)
						continue;

					_ITEM.fMagic_Defense = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_DODGE:
				{
					if (_ITEM.fDodge != 0)
						continue;

					_ITEM.fDodge = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_CRITICAL_RATING:
				{
					if (_ITEM.fCritical_Rating != 0)
						continue;

					_ITEM.fCritical_Rating = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_CRITICAL_DAMAGE:
				{
					if (_ITEM.fCritical_Damage != 0)
						continue;

					_ITEM.fCritical_Damage = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ATTACK_SPEED:
				{
					if (_ITEM.fAttack_Speed != 0)
						continue;

					_ITEM.fAttack_Speed = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_COOLTIME:
				{
					if (_ITEM.fCoolTime != 0)
						continue;

					_ITEM.fCoolTime = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_EXP_BOOST:
				{
					if (_ITEM.fExpBoost != 0)
						continue;

					_ITEM.fExpBoost = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			}

			nIndex++;
		}
	}


	public void AddCharacter(int _nIndex)
	{
		//Character _charic = GameManager.instance.GetCharicData(_nIndex);

		//if(_charic != null){
		//	LIST_HERO.Add(Character);
		//}
	}

	public int GetGold() { return nGold; }
}
