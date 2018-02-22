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
    
	public List<Item> LIST_ITEM = new List<Item>();
    public List<Equipment> LIST_ITEM_WEAPON = new List<Equipment>();
    public List<Equipment> LIST_TIEM_ARMOR = new List<Equipment>();
    public List<Equipment> LIST_ITEM_GLOVE = new List<Equipment>();
    public List<Equipment> LIST_ITEM_ACCESSORY = new List<Equipment>();
    public List<Ingredient> LIST_ITEM_INGREDIENT = new List<Ingredient>();

    public List<CharacterStats> LIST_HERO = new List<CharacterStats>();
	public List<CharacterStats> TEST_MY_HERO = new List<CharacterStats>();
	public List<DBBasicCharacter> LIST_CHARACTER = new List<DBBasicCharacter> ();

	public int nGold;

	int nLastStageIndex;

	public int nDefenceChapterOne = 0;
	public int nDefenceChapterTwo = 0;

	public int m_nChapterType = (int)E_STAGE_INDEX.E_STAGE_INDEX_ATTACK;

	public void Init()
	{
		TEST_MY_HERO.Add(GameManager.Instance.SummonCharacter(1032));
		TEST_MY_HERO.Add (GameManager.Instance.SummonCharacter (1006));

		for (int i = 0; i < 53; i++) {
			LIST_CHARACTER.Add ( GameManager.Instance.lDbBasicCharacter [i]);
		}
        
        //테스트용
        //전체 아이템 셋업.
        SetAllItemList();
        //무기 아이템 셋업
        SetWeaponItemList();
        //방어구 아이템 셋업
        SetArmorItmeList();
        //장신구 아이템 셋업
        SetAccessorytemList();
        //재료 아이템 셋업
        SetMateiralItemList();
    }

    public void SetWeaponItemList()
    {
        //테스트용
        int itemIndexWeapon = 0;
        int itemIndexGlove = 0;
        int itemIndexArmor = 0;
        int itemIndexAccessory = 0;
        int nValue = 0;

        for (int j = 0; j < 50; j++)
        {
            GameManager.Instance.GetPlayer().LIST_ITEM_WEAPON.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_WEAPON), itemIndexWeapon, j ));
            itemIndexWeapon++;
            if (itemIndexWeapon > 11)
                itemIndexWeapon = 0;
        }

    }


    public void SetArmorItmeList()
    {
        //테스트용
        int itemIndexWeapon = 0;
        int itemIndexGlove = 0;
        int itemIndexArmor = 0;
        int itemIndexAccessory = 0;
        int nValue = 0;


        for (int j = 0; j < 100; j++)
        {
            if (nValue == 0)
            {
                GameManager.Instance.GetPlayer().LIST_TIEM_ARMOR.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_ARMOR), itemIndexArmor, j));
                itemIndexArmor++;
                if (itemIndexArmor > 11)
                    itemIndexArmor = 0;
                nValue++;
            }
            else if (nValue == 1)
            {
                GameManager.Instance.GetPlayer().LIST_TIEM_ARMOR.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_GLOVE), itemIndexGlove, j));
                itemIndexGlove++;
                if (itemIndexGlove > 11)
                    itemIndexGlove = 0;
                nValue = 0;
            }
        }
    }


    public void SetAccessorytemList()
    {
        //테스트용
        int itemIndexWeapon = 0;
        int itemIndexGlove = 0;
        int itemIndexArmor = 0;
        int itemIndexAccessory = 0;
        int nValue = 0;

        for (int j = 0; j < 50; j++)
        {
            GameManager.Instance.GetPlayer().LIST_ITEM_ACCESSORY.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_ACCESSORY), itemIndexAccessory, j));
            itemIndexAccessory++;
            if (itemIndexAccessory > 3)
                itemIndexAccessory = 0;
            nValue = 0;

        }
    }
    
    public void SetAllItemList()
    {
        //테스트용
        int itemIndexWeapon = 0;
        int itemIndexGlove = 0;
        int itemIndexArmor = 0;
        int itemIndexAccessory = 0;
        int itemIndexIngredient = 0;
        int nValue = 0;

        for (int j = 0; j < 63; j++)
        {
            if (nValue == 0)
            {
                GameManager.Instance.GetPlayer().LIST_ITEM.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_WEAPON), itemIndexWeapon, j));
                itemIndexWeapon++;
                if (itemIndexWeapon > 11)
                    itemIndexWeapon = 0;
                nValue++;
            }
            else if (nValue == 1)
            {
                GameManager.Instance.GetPlayer().LIST_ITEM.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_ARMOR), itemIndexArmor, j));
                itemIndexArmor++;
                if (itemIndexArmor > 11)
                    itemIndexArmor = 0;
                nValue++;
            }
            else if (nValue == 2)
            {
                GameManager.Instance.GetPlayer().LIST_ITEM.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_GLOVE), itemIndexGlove, j));
                itemIndexGlove++;
                if (itemIndexGlove > 11)
                    itemIndexGlove = 0;
                nValue++;
            }
            else if(nValue == 3)
            {

                GameManager.Instance.GetPlayer().LIST_ITEM.Add(GameManager.Instance.GetPlayer().AddItem_Equipment((E_ITEM_TYPE.E_ACCESSORY), itemIndexAccessory, j));
                itemIndexAccessory++;
                if (itemIndexAccessory > 3)
                    itemIndexAccessory = 0;
                nValue++;
            }
            else
            {
                GameManager.Instance.GetPlayer().LIST_ITEM.Add(GameManager.Instance.GetPlayer().AddItem_Ingrdient((E_ITEM_TYPE.E_MATERIAL), itemIndexIngredient, j));
                itemIndexIngredient++;
                if (itemIndexIngredient > 5)
                    itemIndexIngredient = 0;

                nValue = 0;
            }
        }
    }

    public void SetMateiralItemList()
    {
        //테스트용
        int itemMaterialGredient = 0;
  
        for (int j = 0; j < 50; j++)
        {
            GameManager.Instance.GetPlayer().LIST_ITEM_INGREDIENT.Add(GameManager.Instance.GetPlayer().AddItem_Ingrdient((E_ITEM_TYPE.E_MATERIAL), itemMaterialGredient, j));
            itemMaterialGredient++;
            if (itemMaterialGredient > 5)
                itemMaterialGredient = 0;
        }
    }

    public Ingredient AddItem_Ingrdient(E_ITEM_TYPE _type, int _nIndex, int _nListIndex)
    {
        Ingredient addIngredient = new Ingredient();

        switch (_type)
        {
            case E_ITEM_TYPE.E_MATERIAL:
                {
                    DBMaterialData materialDB = GameManager.Instance.lDBMaterialData[_nIndex];
                    addIngredient.nIndex = materialDB.nIndex;
                    addIngredient.strName = materialDB.sMaterialName;
                    addIngredient.sImage = materialDB.sImagePath;
                    addIngredient.nCount = materialDB.nCount;
                  
                    addIngredient.sExplanation = materialDB.sExplanation;
                    addIngredient.nSelected = materialDB.nSelected;
                    addIngredient.nSellCost = materialDB.nSellCost;
                    addIngredient.nListIndex = materialDB.nListIndex;

                    addIngredient.strItemType = materialDB.sItemType;
                }
                break;
        }

        addIngredient.nCount = _nListIndex;

        return addIngredient;


    }

    public Equipment AddItem_Equipment(E_ITEM_TYPE _type, int _nIndex, int _nListIndex)
	{
		Equipment addEquipment =new Equipment();
      

		switch (_type) 
		{
		case E_ITEM_TYPE.E_WEAPON:
			{
				DBWeapon weaponDB = GameManager.Instance.lDbWeapon [_nIndex];

				addEquipment.nIndex = weaponDB.nIndex;
				addEquipment.strName = weaponDB.sName;
				addEquipment.nTier = weaponDB.nTier;
			
				addEquipment.strPossibleJob = weaponDB.sJob;
				addEquipment.nEnhance = weaponDB.nEnhanced;
              

                addEquipment.strItemType = weaponDB.sItemType;
                addEquipment.fPhysical_Attack_Rating = RandomAddValue(weaponDB.fPhysical_AttackRating);
                addEquipment.fMagic_Attack_Rating = RandomAddValue(weaponDB.fMagic_AttackRating);

                addEquipment.nQulity = GetQuality_Weapon(addEquipment.fPhysical_Attack_Rating, addEquipment.fMagic_Attack_Rating,
                            weaponDB.fPhysical_AttackRating, weaponDB.fMagic_AttackRating);

                
                addEquipment.nSellCost = weaponDB.nSellCost;
				addEquipment.nMakeMaterialIndex = weaponDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex = weaponDB.nBreakMaterial;
                addEquipment.sImage = weaponDB.sImage;
                if(addEquipment.nQulity > 0)
                      RandomAddOption(addEquipment, weaponDB.nRandomOption);

                addEquipment.nListIndex = _nListIndex;
            }
			break;
		case E_ITEM_TYPE.E_ARMOR:
			{
				DBArmor armorDB = GameManager.Instance.lDBArmor [_nIndex];

				addEquipment.nIndex                 = armorDB.nIndex;
				addEquipment.strName                = armorDB.sName;
				addEquipment.nTier                  = armorDB.nTier;
			
				addEquipment.strPossibleJob         = armorDB.sJob;
				addEquipment.nEnhance               = armorDB.nEnhanced;
              
                addEquipment.strItemType            = armorDB.sItemType;
				addEquipment.fPhysical_Defense      = RandomAddValue(armorDB.fPhysical_Defense);
				addEquipment.fMagic_Defense         = RandomAddValue(armorDB.fMagic_Defense);
                addEquipment.fHp                    = RandomAddValue(armorDB.sHp);
                addEquipment.nQulity = GetQuality_Armor(addEquipment.fPhysical_Defense, addEquipment.fMagic_Defense, addEquipment.fHp,
                        armorDB.fPhysical_Defense, armorDB.fMagic_Defense, armorDB.sHp);

                addEquipment.nSellCost              = armorDB.nSellCost;
				addEquipment.nMakeMaterialIndex     = armorDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex    = armorDB.nBreakMaterial;
                addEquipment.sImage                 = armorDB.sImage;
                if (addEquipment.nQulity > 0)
                    RandomAddOption(addEquipment, armorDB.nRandomOption);
                 addEquipment.nListIndex = _nListIndex;
             }
			break;
		case E_ITEM_TYPE.E_GLOVE:
			{
				DBGlove gloveDB = GameManager.Instance.lDBGlove [_nIndex];

				addEquipment.nIndex = gloveDB.nIndex;
				addEquipment.strName = gloveDB.sName;
				addEquipment.nTier = gloveDB.nTier;
				
				addEquipment.strPossibleJob = gloveDB.sJob;
				addEquipment.nEnhance = gloveDB.nEnhanced;
				addEquipment.strItemType = gloveDB.sItemType;

				addEquipment.fPhysical_Defense = RandomAddValue(gloveDB.fPhysical_Defense);
				addEquipment.fMagic_Defense = RandomAddValue(gloveDB.fMagic_Defense);
                addEquipment.nQulity = GetQuality_Glove(addEquipment.fPhysical_Defense, addEquipment.fMagic_Defense, gloveDB.fPhysical_Defense, gloveDB.fMagic_Defense);

                addEquipment.nSellCost = gloveDB.nSellCost;
				addEquipment.nMakeMaterialIndex = gloveDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex = gloveDB.nBreakMaterial;
                addEquipment.sImage = gloveDB.sImage;
                if (addEquipment.nQulity > 0)
                    RandomAddOption(addEquipment, gloveDB.nRandomOption);
                addEquipment.nListIndex = _nListIndex;
            }
			break;
		case E_ITEM_TYPE.E_ACCESSORY:
			{
				DBAccessory accessoryDB = GameManager.Instance.lDBAccessory [_nIndex];

				addEquipment.nIndex = accessoryDB.nIndex;
				addEquipment.strName = accessoryDB.sName;
				addEquipment.nTier = accessoryDB.nTier;
				
                addEquipment.fHp = RandomAddValue(accessoryDB.sHp);

                addEquipment.nQulity = GetQuality_Accessory(addEquipment.fHp, accessoryDB.sHp);

                addEquipment.strPossibleJob = accessoryDB.sJob;
				addEquipment.nEnhance = accessoryDB.nEnhanced;
				addEquipment.strItemType = accessoryDB.sItemType;

				addEquipment.nSellCost = accessoryDB.nSellCost;
				addEquipment.nMakeMaterialIndex = accessoryDB.nMakeMaterial;
				addEquipment.nBreakMaterialIndex = accessoryDB.nBreakMaterial;
                addEquipment.sImage = accessoryDB.sImage;
                if (addEquipment.nQulity > 0)
                    RandomAddOption(addEquipment, accessoryDB.nRandomOption);
                addEquipment.nListIndex = _nListIndex;
            }
			break;


        }
        //테스트용 (강화)
        if (addEquipment.nTier > 1)
            addEquipment.nEnhance = 5;


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
			switch (randomDB.nOptionIndex)
            {
			case (int)E_RANDOM_OPTION.E_HP:
				{
					if (_ITEM.fHp != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }
					

					_ITEM.fHp = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ACCURACY:
				{
					if (_ITEM.fAccuracy != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }
						

					_ITEM.fAccuracy = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ALL_ATTACK_RATING:
				{
					if (_ITEM.fAll_Attack_RatingPlus != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fAll_Attack_RatingPlus = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_PHSYICAL_ATTACK_RATING:
				{
					if (_ITEM.fPhysical_Attack_Rating != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fPhysical_Attack_Rating = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_MAGIC_ATTACK_RATING:
				{
					if (_ITEM.fMagic_Attack_Rating != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fMagic_Attack_Rating = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ALL_PENETRATE:
				{
					if (_ITEM.fAll_Penetrate != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fAll_Penetrate = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_PHSYICAL_PENETRATE:
				{
					if (_ITEM.fPhysical_Penetrate != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fPhysical_Penetrate = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_MAGIC_PENETRATE:
				{
					if (_ITEM.fMagic_Penetrate != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fMagic_Penetrate = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ALL_DEFENSE:
				{
					if (_ITEM.fAll_Defense != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fAll_Defense = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_PHSYICAL_DEFENSE:
				{
					if (_ITEM.fPhysical_Defense != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fPhysical_Defense = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_MAGIC_DEFENSE:
				{
					if (_ITEM.fMagic_Defense != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fMagic_Defense = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_DODGE:
				{
					if (_ITEM.fDodge != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fDodge = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_CRITICAL_RATING:
				{
					if (_ITEM.fCritical_Rating != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fCritical_Rating = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_CRITICAL_DAMAGE:
				{
					if (_ITEM.fCritical_Damage != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fCritical_Damage = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_ATTACK_SPEED:
				{
					if (_ITEM.fAttack_Speed != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fAttack_Speed = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_COOLTIME:
				{
					if (_ITEM.fCoolTime != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fCoolTime = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			case (int)E_RANDOM_OPTION.E_EXP_BOOST:
				{
					if (_ITEM.fExpBoost != 0)
                        {
                            nRandomIndex = int.Parse(strRandomOptions[Random.Range(0, strRandomOptions.Length)]);
                            randomDB = GameManager.Instance.lDBEquipmentRandomOption[nRandomIndex];
                            continue;
                        }

                        _ITEM.fExpBoost = (int)Random.Range (randomDB.nStartValue, randomDB.nEndValue);
				}
				break;
			}

			nIndex++;
		}
	}

    public float RandomAddValue( string _value)
    {
        string sValue = _value;
        //최소값
        float minValue = float.Parse(sValue.Substring(0, sValue.IndexOf(",")));
        sValue = sValue.Remove(0, sValue.IndexOf(",") + 1);
        //최대값
        float maxValue = float.Parse(sValue);
        float random = Random.RandomRange(minValue, maxValue + 1);
        
        return Mathf.Floor(random); 
    }
    //물리 공격력(결과), 마법 공격력(결과) 물리 공격력(최대), 마법 공격력(최대)
    public int GetQuality_Weapon(float _fPhyAtkRValue, float _fMagAtkRValue, string _PhyAtkMValue, string _MagAtkMValue )
    {
        string nPhyAtkString = _PhyAtkMValue;
        string nMagAtkString = _MagAtkMValue;

        nPhyAtkString = nPhyAtkString.Remove(0,nPhyAtkString.IndexOf(","));
        float fPhyAtkMax = float.Parse(nPhyAtkString);

        nMagAtkString = nMagAtkString.Remove(0, nMagAtkString.IndexOf(","));
        float fMagAtkMax = float.Parse(nMagAtkString);

        float fPhyResult = (_fPhyAtkRValue / fPhyAtkMax) * 100f;
        float fMagResult = (_fMagAtkRValue / fMagAtkMax) * 100f;

        float Result = fPhyResult + fMagResult;

        //Quality C
        if (Result <= 69)
            return 0;
        else if (Result <= 84)
            return 1;
        else
            return 2;
    }

    //물리 방어력(결과), 마법 방어력(결과), 체력(결과),  물리 방어력(최대), 마법 방어력(최대), 체력(최대)
    public int GetQuality_Armor(float _fPhyDefRValue, float _fMagDefRValue, float _fHpRValue, string _PhyDefMValue, string _MagDefMValue, string _HpMValue)
    {
        string PhyDefString = _PhyDefMValue;
        string MagDefString = _MagDefMValue;
        string HpString = _HpMValue;

        PhyDefString = PhyDefString.Remove(0, PhyDefString.IndexOf(","));
        float fPhyDefMax = float.Parse(PhyDefString);

        MagDefString = MagDefString.Remove(0, MagDefString.IndexOf(","));
        float fMagDefMax = float.Parse(MagDefString);

        HpString = HpString.Remove(0, HpString.IndexOf(","));
        float fHp = float.Parse(HpString);


        float fPhyResult = (_fPhyDefRValue / fPhyDefMax) * 100f;
        float fMagResult = (_fMagDefRValue / fMagDefMax) * 100f;
        float fHpResult = (_fHpRValue / fHp) * 100f;

        float Result = fPhyResult + fMagResult + fHpResult;

        //Quality C
        if (Result <= 69)
            return 0;
        else if (Result <= 84)
            return 1;
        else
            return 2;
    }

    //물리 방어력(결과), 마법 방어력(결과),  물리 방어력(최대), 마법 방어력(최대),
    public int GetQuality_Glove(float _fPhyDefRValue, float _fMagDefRValue, string _PhyDefMValue, string _MagDefMValue)
    {
        string PhyDefString = _PhyDefMValue;
        string MagDefString = _MagDefMValue;
      

        PhyDefString = PhyDefString.Remove(0, PhyDefString.IndexOf(","));
        float fPhyDefMax = float.Parse(PhyDefString);

        MagDefString = MagDefString.Remove(0, MagDefString.IndexOf(","));
        float fMagDefMax = float.Parse(MagDefString);


        float fPhyResult = (_fPhyDefRValue / fPhyDefMax) * 100f;
        float fMagResult = (_fMagDefRValue / fMagDefMax) * 100f;
       

        float Result = fPhyResult + fMagResult;

        //Quality C
        if (Result <= 69)
            return 0;
        else if (Result <= 84)
            return 1;
        else
            return 2;
    }

    //물리 방어력(결과), 마법 방어력(결과),  물리 방어력(최대), 마법 방어력(최대),
    public int GetQuality_Accessory(float _fHpRValue, string _HpMValue)
    {
        string HpString = _HpMValue;

        HpString = HpString.Remove(0, HpString.IndexOf(","));
        float fHp = float.Parse(HpString);


        float fHpResult = (_fHpRValue / fHp) * 100f;

        float Result = fHpResult;

        //Quality C
        if (Result <= 69)
            return 0;
        else if (Result <= 84)
            return 1;
        else
            return 2;
    }

    public int GetGold() { return nGold; }
}
