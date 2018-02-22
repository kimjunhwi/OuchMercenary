using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Text;
using UnityEditor.UI;
using ReadOnlys;


public class BuildDataToBinary : MonoBehaviour
{
 
    [MenuItem("Data/BuildDataToBinary")]
    static void BuildAllAssetBundles()
    {
        Load_TableInfo_Character();
        Load_TableInfo_ActiveSkill();
        Load_TableInfo_ActiveSkillType();
        Load_TableInfo_PassiveSkill();
        Load_TableInfo_OptionIndexSkill();
        Load_TableInfo_BasicSkill();
        Load_TableInfo_Equipment_Weapon();
        Load_TableInfo_Equipment_Armor();
        Load_TableInfo_Equipment_Glove();
        Load_TableInfo_Equipment_Accessory();
        Load_TableInfo_Equipment_RandomOption();
        Load_TableInfo_StageData();
        Load_TableInfo_CraftMaterial();
        Load_TableInfo_BreakMaterial();
        Load_TableInfo_FormationSkill();
        Load_TableInfo_MaterialData();
        Load_TableInfo_CalendarData();
        Load_TableInfo_CharacterTicketData();
        Load_TableInfo_WeaponTicketData();
        Load_TableInfo_GachaData();
    }

    #region LoadTableInfo
    public static void Load_TableInfo_Character()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/BasicCharacter.data";

        if (File.Exists(path))
            File.Delete(path);
       

        string txtFilePath = "Character";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBBasicCharacter> cCharacterInfo_List = new List<DBBasicCharacter>();
        DBBasicCharacter[] cCharacterInfo = new DBBasicCharacter[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "")
                continue;
            cCharacterInfo[i - 1] = new DBBasicCharacter();
            cCharacterInfo[i - 1].Index = int.Parse(Cells[0]);
            cCharacterInfo[i - 1].C_Index = int.Parse(Cells[1]);
            cCharacterInfo[i - 1].C_JobIndex = int.Parse(Cells[2]);
            cCharacterInfo[i - 1].C_JobNames = Cells[3];
            cCharacterInfo[i - 1].C_Name = Cells[4];
            cCharacterInfo[i - 1].C_Enhance = int.Parse(Cells[5]);
            cCharacterInfo[i - 1].Jobs = Cells[6];
            cCharacterInfo[i - 1].Levels = int.Parse(Cells[7]);
            cCharacterInfo[i - 1].Tier = int.Parse(Cells[8]);
            cCharacterInfo[i - 1].Attribute = int.Parse(Cells[9]);
            cCharacterInfo[i - 1].AttackType = int.Parse(Cells[10]);
            cCharacterInfo[i - 1].Tribe = int.Parse(Cells[11]);
            cCharacterInfo[i - 1].Site = float.Parse(Cells[12]);
            cCharacterInfo[i - 1].Health = float.Parse(Cells[13]);
            cCharacterInfo[i - 1].Accurancy = float.Parse(Cells[14]);
            cCharacterInfo[i - 1].AttackRange = float.Parse(Cells[15]);
            cCharacterInfo[i - 1].Physic_AttackRating = float.Parse(Cells[16]);
            cCharacterInfo[i - 1].Magic_AttackRating = float.Parse(Cells[17]);
            cCharacterInfo[i - 1].AttackSpeed = float.Parse(Cells[18]);
            cCharacterInfo[i - 1].MoveSpeed = float.Parse(Cells[19]);
            cCharacterInfo[i - 1].Physic_Defense = float.Parse(Cells[20]);
            cCharacterInfo[i - 1].Magic_Defense = float.Parse(Cells[21]);
            cCharacterInfo[i - 1].Dodge = float.Parse(Cells[22]);
            cCharacterInfo[i - 1].Crit_Rating = float.Parse(Cells[23]);
            cCharacterInfo[i - 1].Crit_Dmg = float.Parse(Cells[24]);
            cCharacterInfo[i - 1].Physic_Penetrate = float.Parse(Cells[25]);
            cCharacterInfo[i - 1].Magic_Penetrate = float.Parse(Cells[26]);
            cCharacterInfo[i - 1].CC_Registance = float.Parse(Cells[27]);
            cCharacterInfo[i - 1].Exp = float.Parse(Cells[28]);
            cCharacterInfo[i - 1].ExpMax = float.Parse(Cells[29]);
            cCharacterInfo[i - 1].Betch_Index = int.Parse(Cells[30]);
            cCharacterInfo[i - 1].m_nStamina = int.Parse(Cells[31]);
            cCharacterInfo[i - 1].m_sImage = Cells[32];
            cCharacterInfo[i - 1].m_nFavorite = int.Parse(Cells[33]);
            cCharacterInfo[i - 1].nListIndex = int.Parse(Cells[34]);
            cCharacterInfo_List.Add(cCharacterInfo[i - 1]);
        }
        
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fileStream = new FileStream(path , FileMode.Create);
        bf.Serialize(fileStream, cCharacterInfo_List);
        fileStream.Close();
    }

    
    public static void Load_TableInfo_ActiveSkill()
    {
        //if (cCharacterState.Length != 0) 
        //	return;

        string path = Application.streamingAssetsPath + "/Data/ActiveSkill.data";
        if (File.Exists(path))
            File.Delete(path);

        string txtFilePath = "ActiveSkill";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        List<DBActiveSkill> cActiveSkill_List = new List<DBActiveSkill>();
        DBActiveSkill[] cActiveSkill = new DBActiveSkill[line.Count - 1];
        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            cActiveSkill[i - 1] = new DBActiveSkill();
            cActiveSkill[i - 1].m_nIndex = int.Parse(Cells[0]);
            cActiveSkill[i - 1].m_nCharacterIndex = int.Parse(Cells[1]);
            cActiveSkill[i - 1].m_strName = Cells[2];
            cActiveSkill[i - 1].m_strSkillType = Cells[3];
            cActiveSkill[i - 1].m_nSkillClass = int.Parse(Cells[4]);
            cActiveSkill[i - 1].m_nTier = int.Parse(Cells[5]);
            cActiveSkill[i - 1].m_strJob = Cells[6];
            cActiveSkill[i - 1].m_nLevels = int.Parse(Cells[7]);

            cActiveSkill[i - 1].m_nAttribute = int.Parse(Cells[8]);
            cActiveSkill[i - 1].m_nAttackType = int.Parse(Cells[9]);
            cActiveSkill[i - 1].m_nActivePriority = int.Parse(Cells[10]);
            cActiveSkill[i - 1].m_fAttack_ActvieRating = float.Parse(Cells[11]);
            cActiveSkill[i - 1].m_fCriticalAttack_ActiveRating = float.Parse(Cells[12]);
            cActiveSkill[i - 1].m_nAttackCount_ActiveRating = int.Parse(Cells[13]);
            cActiveSkill[i - 1].m_fMiss_ActiveRating = float.Parse(Cells[14]);
            cActiveSkill[i - 1].m_fDodgy_ActiveRating = float.Parse(Cells[15]);
            cActiveSkill[i - 1].m_fHit_ActiveRating = float.Parse(Cells[16]);
            cActiveSkill[i - 1].m_fCoolTime = float.Parse(Cells[17]);
            cActiveSkill[i - 1].m_fCastTime = float.Parse(Cells[18]);
            cActiveSkill[i - 1].m_fPhysicalMagnification = float.Parse(Cells[19]);
            cActiveSkill[i - 1].m_fMagicMagnification = float.Parse(Cells[20]);
            cActiveSkill[i - 1].m_nAttackNumber = int.Parse(Cells[21]);
            cActiveSkill[i - 1].m_fAttackRange = float.Parse(Cells[22]);
            cActiveSkill[i - 1].m_fAttackArea = float.Parse(Cells[23]);
            cActiveSkill[i - 1].m_nMaxTargetNumber = int.Parse(Cells[24]);
            cActiveSkill[i - 1].m_strAttackPriority = Cells[25];
            cActiveSkill[i - 1].m_fKnockback_Power = float.Parse(Cells[26]);
            cActiveSkill[i - 1].m_fDuration = float.Parse(Cells[27]);
            cActiveSkill[i - 1].m_strAnimationClip = Cells[28];
            cActiveSkill[i - 1].m_strEffectName = Cells[29];
            cActiveSkill[i - 1].m_strExplanation = Cells[30];
            cActiveSkill[i - 1].m_bIsCooltime = int.Parse(Cells[31]);
            cActiveSkill_List.Add(cActiveSkill[i - 1]);
        }
        

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, cActiveSkill_List);
        fileStream.Close();
    }
    
    public static void Load_TableInfo_ActiveSkillType()
    {
        string path = Application.streamingAssetsPath + "/Data/ActiveSkilLType.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "ActiveSkillType";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBActiveSkillType> cAllActiveSkillType_List = new List<DBActiveSkillType>();
        DBActiveSkillType[] cAllActiveSkillType = new DBActiveSkillType[line.Count - 1];



        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null)
                continue;
            if (i == 0)
                continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "")
                continue;

            cAllActiveSkillType[i - 1] = new DBActiveSkillType();
            cAllActiveSkillType[i - 1].nIndex = int.Parse(Cells[0]);
            cAllActiveSkillType[i - 1].nActiveType = int.Parse(Cells[1]);
            cAllActiveSkillType[i - 1].nTargetIndex = int.Parse(Cells[2]);
            cAllActiveSkillType_List.Add(cAllActiveSkillType[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, cAllActiveSkillType_List);
        fileStream.Close();
    }

    
    public static void Load_TableInfo_PassiveSkill()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/PassiveSkill.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "PassiveSkill";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBPassiveSkill> cPassiveSkillInfo_List = new List<DBPassiveSkill>();
        DBPassiveSkill[] cPassiveSkillInfo = new DBPassiveSkill[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            cPassiveSkillInfo[i - 1] = new DBPassiveSkill();
            cPassiveSkillInfo[i - 1].nIndex = int.Parse(Cells[0]);
            cPassiveSkillInfo[i - 1].nCharacterIndex = int.Parse(Cells[1]);
            cPassiveSkillInfo[i - 1].strSkillName = Cells[2];
            cPassiveSkillInfo[i - 1].strSkillType = Cells[3];
            cPassiveSkillInfo[i - 1].nSkillClass = int.Parse(Cells[4]);
            cPassiveSkillInfo[i - 1].nTier = int.Parse(Cells[5]);
            cPassiveSkillInfo[i - 1].strJob = Cells[6];
            cPassiveSkillInfo[i - 1].nAttribute = int.Parse(Cells[7]);
            cPassiveSkillInfo[i - 1].nAttackType = int.Parse(Cells[8]);
            cPassiveSkillInfo[i - 1].strOption_List = Cells[9];
            cPassiveSkillInfo[i - 1].strExplanation = Cells[10];
            cPassiveSkillInfo_List.Add(cPassiveSkillInfo[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, cPassiveSkillInfo_List);
        fileStream.Close();
    }

    
    public static void Load_TableInfo_OptionIndexSkill()
    {
        //if (cCharacterState.Length != 0) 
        //	return;

        string path = Application.streamingAssetsPath + "/Data/PassiveSkillOptionIndex.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "OptionIndex";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBPassiveSkillOptionIndex> cPassiveSkillOptionIndexInfo_List = new List<DBPassiveSkillOptionIndex>();
        DBPassiveSkillOptionIndex[] cPassiveSkillOptionIndexInfo = new DBPassiveSkillOptionIndex[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            cPassiveSkillOptionIndexInfo[i - 1] = new DBPassiveSkillOptionIndex();
            cPassiveSkillOptionIndexInfo[i - 1].nIndex = int.Parse(Cells[0]);
            cPassiveSkillOptionIndexInfo[i - 1].nOptionIndex = int.Parse(Cells[1]);
            cPassiveSkillOptionIndexInfo[i - 1].fValue = float.Parse(Cells[2]);
            cPassiveSkillOptionIndexInfo[i - 1].fPlus = float.Parse(Cells[3]);
            cPassiveSkillOptionIndexInfo[i - 1].nCalculate = int.Parse(Cells[4]);
            cPassiveSkillOptionIndexInfo_List.Add(cPassiveSkillOptionIndexInfo[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, cPassiveSkillOptionIndexInfo_List);
        fileStream.Close();

    }
    
    public static void Load_TableInfo_BasicSkill()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/BasicSkill.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "BasicSkill";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBBasicSkill> cBasicSkill_List = new List<DBBasicSkill>();
        DBBasicSkill[] cBasicSkill = new DBBasicSkill[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            cBasicSkill[i - 1] = new DBBasicSkill();
            cBasicSkill[i - 1].nIndex = int.Parse(Cells[0]);
            cBasicSkill[i - 1].nCharacterIndex = int.Parse(Cells[1]);
            cBasicSkill[i - 1].strSkillName = Cells[2];
            cBasicSkill[i - 1].strSkillType = Cells[3];
            cBasicSkill[i - 1].nSkillClass = int.Parse(Cells[4]);
            cBasicSkill[i - 1].nTier = int.Parse(Cells[5]);
            cBasicSkill[i - 1].strJob = Cells[6];
            cBasicSkill[i - 1].nAttribute = int.Parse(Cells[7]);
            cBasicSkill[i - 1].nAttackType = int.Parse(Cells[8]);
            cBasicSkill[i - 1].fPhsyicMagnification = float.Parse(Cells[9]);
            cBasicSkill[i - 1].fMagicMagnification = float.Parse(Cells[10]);
            cBasicSkill[i - 1].fAttackArea = float.Parse(Cells[11]);
            cBasicSkill[i - 1].strSkillTarget = Cells[12];
            cBasicSkill[i - 1].nMaxTargetNumber = int.Parse(Cells[13]);
            cBasicSkill[i - 1].nAttackNumber = int.Parse(Cells[14]);
            cBasicSkill[i - 1].strAttackPriority = Cells[15];
            cBasicSkill[i - 1].strRangeSprite = Cells[16];
            cBasicSkill[i - 1].strExplanation = Cells[17];
            cBasicSkill_List.Add(cBasicSkill[i - 1]);

        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, cBasicSkill_List);
        fileStream.Close();
    }

    public static void Load_TableInfo_Equipment_Weapon()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/EquipmentWeapon.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/Weapon";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBWeapon> equipMent_Weapon_List = new List<DBWeapon>();
        DBWeapon []equipMent_Weapon = new DBWeapon[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            equipMent_Weapon[i - 1] = new DBWeapon();
            equipMent_Weapon[i - 1].nIndex = int.Parse(Cells[0]);
            equipMent_Weapon[i - 1].sName = Cells[1];
            equipMent_Weapon[i - 1].nTier = int.Parse(Cells[2]);
            equipMent_Weapon[i - 1].nQulity = int.Parse(Cells[3]);
            equipMent_Weapon[i - 1].sJob = Cells[4];
            equipMent_Weapon[i - 1].nEnhanced = int.Parse(Cells[5]);
            equipMent_Weapon[i - 1].sItemType = Cells[6];
            equipMent_Weapon[i - 1].fPhysical_AttackRating = Cells[7];
            equipMent_Weapon[i - 1].fMagic_AttackRating = Cells[8];
            equipMent_Weapon[i - 1].nRandomOption = Cells[9];
            equipMent_Weapon[i - 1].nSellCost = int.Parse(Cells[10]);
            equipMent_Weapon[i - 1].nMakeMaterial = int.Parse(Cells[11]);
            equipMent_Weapon[i - 1].nBreakMaterial = int.Parse(Cells[12]);
            equipMent_Weapon[i - 1].sImage = Cells[13];
            equipMent_Weapon[i - 1].nSelected = int.Parse(Cells[14]);
            equipMent_Weapon[i - 1].nListIndex = int.Parse(Cells[15]);
            equipMent_Weapon_List.Add(equipMent_Weapon[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, equipMent_Weapon_List);
        fileStream.Close();
    }
    
    public static void Load_TableInfo_Equipment_Armor()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/EquipmentArmor.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/Amor";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBArmor> equipMent_Armor_List = new List<DBArmor>();
        DBArmor[] equipMent_Armor = new DBArmor[line.Count-1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            equipMent_Armor[i - 1] = new DBArmor();
            equipMent_Armor[i - 1].nIndex = int.Parse(Cells[0]);
            equipMent_Armor[i - 1].sName = Cells[1];
            equipMent_Armor[i - 1].nTier = int.Parse(Cells[2]);
            equipMent_Armor[i - 1].nQulity = int.Parse(Cells[3]);
            equipMent_Armor[i - 1].sJob = Cells[4];
            equipMent_Armor[i - 1].nEnhanced = int.Parse(Cells[5]);
            equipMent_Armor[i - 1].sItemType = Cells[6];
            equipMent_Armor[i - 1].fPhysical_Defense = Cells[7];
            equipMent_Armor[i - 1].fMagic_Defense = Cells[8];
            equipMent_Armor[i - 1].sHp = Cells[9];
            equipMent_Armor[i - 1].nRandomOption = Cells[10];
            equipMent_Armor[i - 1].nSellCost = int.Parse(Cells[11]);
            equipMent_Armor[i - 1].nMakeMaterial = int.Parse(Cells[12]);
            equipMent_Armor[i - 1].nBreakMaterial = int.Parse(Cells[13]);
            equipMent_Armor[i - 1].sImage = Cells[14];
            equipMent_Armor[i - 1].nSelected = int.Parse(Cells[15]);
            equipMent_Armor[i - 1].nListIndex = int.Parse(Cells[15]);
            equipMent_Armor_List.Add(equipMent_Armor[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, equipMent_Armor_List);
        fileStream.Close();
    }


    
    public static void Load_TableInfo_Equipment_Glove()
    {
        //if (cCharacterState.Length != 0) 
        //	return;

        string path = Application.streamingAssetsPath + "/Data/EquipmentGlove.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/Glove";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBGlove> equipMent_Glove_List = new List<DBGlove>();
        DBGlove[] equipMent_Glove = new DBGlove[line.Count -1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            equipMent_Glove[i - 1] = new DBGlove();
            equipMent_Glove[i - 1].nIndex = int.Parse(Cells[0]);
            equipMent_Glove[i - 1].sName = Cells[1];
            equipMent_Glove[i - 1].nTier = int.Parse(Cells[2]);
            equipMent_Glove[i - 1].nQulity = int.Parse(Cells[3]);
            equipMent_Glove[i - 1].sJob = Cells[4];
            equipMent_Glove[i - 1].nEnhanced = int.Parse(Cells[5]);
            equipMent_Glove[i - 1].sItemType = Cells[6];
            equipMent_Glove[i - 1].fPhysical_Defense = Cells[7];
            equipMent_Glove[i - 1].fMagic_Defense = Cells[8];
            equipMent_Glove[i - 1].nRandomOption = Cells[9];
            equipMent_Glove[i - 1].nSellCost = int.Parse(Cells[10]);
            equipMent_Glove[i - 1].nMakeMaterial = int.Parse(Cells[11]);
            equipMent_Glove[i - 1].nBreakMaterial = int.Parse(Cells[12]);
            equipMent_Glove[i - 1].sImage = Cells[13];
            equipMent_Glove[i - 1].nSelected = int.Parse(Cells[14]);
            equipMent_Glove[i - 1].nListIndex = int.Parse(Cells[15]);
            equipMent_Glove_List.Add(equipMent_Glove[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, equipMent_Glove_List);
        fileStream.Close();

    }

    
    public static void Load_TableInfo_Equipment_Accessory()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/EquipmentAccessory.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/Accessory";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBAccessory> equipMent_Accessory_List = new List<DBAccessory>();
        DBAccessory []equipMent_Accessory = new DBAccessory[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            equipMent_Accessory[i - 1] = new DBAccessory();
            equipMent_Accessory[i - 1].nIndex = int.Parse(Cells[0]);
            equipMent_Accessory[i - 1].sName = Cells[1];
            equipMent_Accessory[i - 1].nTier = int.Parse(Cells[2]);
            equipMent_Accessory[i - 1].nQulity = int.Parse(Cells[3]);
            equipMent_Accessory[i - 1].sJob = Cells[4];
            equipMent_Accessory[i - 1].sHp = Cells[5];
            equipMent_Accessory[i - 1].nEnhanced = int.Parse(Cells[6]);
            equipMent_Accessory[i - 1].sItemType = Cells[7];
            equipMent_Accessory[i - 1].nRandomOption = Cells[8];
            equipMent_Accessory[i - 1].nSellCost = int.Parse(Cells[9]);
            equipMent_Accessory[i - 1].nMakeMaterial = int.Parse(Cells[10]);
            equipMent_Accessory[i - 1].nBreakMaterial = int.Parse(Cells[11]);
            equipMent_Accessory[i - 1].sImage = Cells[12];
            equipMent_Accessory[i - 1].nSelected = int.Parse(Cells[13]);
            equipMent_Accessory[i - 1].nListIndex = int.Parse(Cells[13]);
            equipMent_Accessory_List.Add(equipMent_Accessory[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, equipMent_Accessory_List);
        fileStream.Close();
    }
    
    public static void Load_TableInfo_Equipment_RandomOption()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/EquipmentRandomOption.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/RandomOption";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBEquipment_RandomOption> equipMent_RandomOption_List = new List<DBEquipment_RandomOption>();
        DBEquipment_RandomOption[] equipMent_RandomOption = new DBEquipment_RandomOption[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            equipMent_RandomOption[i - 1] = new DBEquipment_RandomOption();
            equipMent_RandomOption[i - 1].nIndex = int.Parse(Cells[0]);
            equipMent_RandomOption[i - 1].nOptionIndex = int.Parse(Cells[1]);
            equipMent_RandomOption[i - 1].nStartValue = int.Parse(Cells[2]);
            equipMent_RandomOption[i - 1].nEndValue = int.Parse(Cells[3]);
            equipMent_RandomOption_List.Add(equipMent_RandomOption[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, equipMent_RandomOption_List);
        fileStream.Close();
    }

    
    public static void Load_TableInfo_StageData()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/Stage.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Stage";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBStageData> stageData_List = new List<DBStageData>();
        DBStageData[] stageData = new DBStageData[line.Count -1 ];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            stageData[i - 1] = new DBStageData();
            stageData[i - 1].nIndex = int.Parse(Cells[0]);
            stageData[i - 1].strStageNumber = Cells[1];
            stageData[i - 1].strStageName = Cells[2];
            stageData[i - 1].strWaveTimes = Cells[3];
            stageData[i - 1].strEnemySpawnIndexs = Cells[4];
            stageData[i - 1].strCreateTimes = Cells[5];
            stageData[i - 1].strYPositions = Cells[6];
            stageData[i - 1].nGold = int.Parse(Cells[7]);
            stageData[i - 1].fExp = float.Parse(Cells[8]);
            stageData[i - 1].strEquimnetIndexs = Cells[9];
            stageData[i - 1].strCharacterDropIndexs = Cells[10];
            stageData[i - 1].strMaterialDropIndexs = Cells[11];
            stageData[i - 1].strEquipmentRates = Cells[12];
            stageData[i - 1].strCharacterDropRates = Cells[13];
            stageData[i - 1].strMaterialDropRates = Cells[14];
            stageData[i - 1].strBackground = Cells[15];
            stageData_List.Add(stageData[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, stageData_List);
        fileStream.Close();

    }
    
    public static void Load_TableInfo_CraftMaterial()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/CraftMaterial.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/MakeMaterial";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBCraftMaterial> craftMaterial_List = new List<DBCraftMaterial>();
        DBCraftMaterial[] craftMaterial = new DBCraftMaterial[line.Count -1 ];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            craftMaterial[i - 1] = new DBCraftMaterial();
            craftMaterial[i - 1].nIndex = int.Parse(Cells[0]);
            craftMaterial[i - 1].nIron = int.Parse(Cells[1]);
            craftMaterial[i - 1].nFabric = int.Parse(Cells[2]);
            craftMaterial[i - 1].nWood = int.Parse(Cells[3]);
            craftMaterial[i - 1].nWeaponStone = int.Parse(Cells[4]);
            craftMaterial[i - 1].nArmorStone = int.Parse(Cells[5]);
            craftMaterial[i - 1].nAccessoryStone = int.Parse(Cells[6]);
            craftMaterial[i - 1].nEpicStone = int.Parse(Cells[7]);
            craftMaterial[i - 1].nGoldCost = int.Parse(Cells[8]);
            craftMaterial[i - 1].nTier = int.Parse(Cells[9]);
            craftMaterial[i - 1].Qulity = int.Parse(Cells[10]);

            craftMaterial[i - 1].strEquipType = Cells[11];
            craftMaterial_List.Add(craftMaterial[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, craftMaterial_List);
        fileStream.Close();
    }

    public static void Load_TableInfo_BreakMaterial()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/BreakMaterial.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/BreakMaterial";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBBreakMaterial> breakMaterial_List = new List<DBBreakMaterial>();
        DBBreakMaterial[] breakMaterial = new DBBreakMaterial[line.Count -1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            breakMaterial[i - 1] = new DBBreakMaterial();
            breakMaterial[i - 1].nIndex = int.Parse(Cells[0]);
            breakMaterial[i - 1].nIron = int.Parse(Cells[1]);
            breakMaterial[i - 1].nFabric = int.Parse(Cells[2]);
            breakMaterial[i - 1].nWood = int.Parse(Cells[3]);
            breakMaterial[i - 1].nWeaponStone = int.Parse(Cells[4]);
            breakMaterial[i - 1].nArmorStone = int.Parse(Cells[5]);
            breakMaterial[i - 1].nAccessoryStone = int.Parse(Cells[6]);
            breakMaterial[i - 1].nEpicStone = int.Parse(Cells[7]);
            breakMaterial_List.Add(breakMaterial[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, breakMaterial_List);
        fileStream.Close();
    }
    
    public static void Load_TableInfo_FormationSkill()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/FormationSkill.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "FormationSkill";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBFormationSkill> formationSkill_List = new List<DBFormationSkill>();
        DBFormationSkill[] formationSkill = new DBFormationSkill[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            formationSkill[i - 1] = new DBFormationSkill();
            formationSkill[i - 1].nIndex = int.Parse(Cells[0]);
            formationSkill[i - 1].nCharacterIndex = int.Parse(Cells[1]);
            formationSkill[i - 1].strName = Cells[2];
            formationSkill[i - 1].strSkillType = Cells[3];
            formationSkill[i - 1].nSkillClass = int.Parse(Cells[4]);
            formationSkill[i - 1].nTier = int.Parse(Cells[5]);
            formationSkill[i - 1].strFomationTarget = Cells[6];
            formationSkill[i - 1].nOptionIndex = int.Parse(Cells[7]);
            formationSkill[i - 1].strExplanation = Cells[8];
            formationSkill_List.Add(formationSkill[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, formationSkill_List);
        fileStream.Close();
    }

    
    public static void Load_TableInfo_MaterialData()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/MaterialData.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "Equipment/Material";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBMaterialData> materialData_List = new List<DBMaterialData>();
        DBMaterialData[] materialData = new DBMaterialData[line.Count - 1] ;


        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            materialData[i - 1] = new DBMaterialData();
            materialData[i - 1].nIndex = int.Parse(Cells[0]);
            materialData[i - 1].sMaterialName = Cells[1];
            materialData[i - 1].sImagePath = Cells[2];
            materialData[i - 1].nCount = int.Parse(Cells[3]);
            materialData[i - 1].sExplanation = Cells[4];
            materialData[i - 1].nSelected = int.Parse(Cells[5]);
            materialData[i - 1].nSellCost = int.Parse(Cells[6]);
            materialData[i - 1].nListIndex = int.Parse(Cells[7]);

            materialData[i - 1].sItemType = Cells[8];
            materialData_List.Add(materialData[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, materialData_List);
        fileStream.Close();
    }
    
    public static void Load_TableInfo_CalendarData()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/CalendarData.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "ETC/Calendar";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBCalendar> calendarData_List = new List<DBCalendar>();
        DBCalendar[] calendarData = new DBCalendar[line.Count -1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            calendarData[i - 1] = new DBCalendar();
            calendarData[i - 1].nIndex = int.Parse(Cells[0]);
            calendarData[i - 1].nGold = int.Parse(Cells[1]);
            calendarData[i - 1].nGem = int.Parse(Cells[2]);
            calendarData[i - 1].nWeaponTicket = int.Parse(Cells[3]);
            calendarData[i - 1].nCharacterTicket = int.Parse(Cells[4]);
            calendarData[i - 1].nWeapon = int.Parse(Cells[5]);
            calendarData[i - 1].nCharacter = int.Parse(Cells[6]);
            calendarData[i - 1].nIron = int.Parse(Cells[7]);
            calendarData[i - 1].nFabric = int.Parse(Cells[8]);
            calendarData[i - 1].nWood = int.Parse(Cells[9]);
            calendarData[i - 1].nWeaponStone = int.Parse(Cells[10]);
            calendarData[i - 1].ArmorStone = int.Parse(Cells[11]);
            calendarData[i - 1].AccessoryStone = int.Parse(Cells[12]);
            calendarData_List.Add(calendarData[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, calendarData_List);
        fileStream.Close();
        Debug.Log("Calender Data Save Complete!");
    }


    public static void Load_TableInfo_CharacterTicketData()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/CharacterTicket.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "ETC/CharacterTicket";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBCharacterTicket> characterTicketData_List = new List<DBCharacterTicket>();
        DBCharacterTicket[] characterTicketData = new DBCharacterTicket[line.Count -1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            characterTicketData[i - 1] = new DBCharacterTicket();
            characterTicketData[i - 1].nIndex = int.Parse(Cells[0]);
            characterTicketData[i - 1].sName = Cells[1];
            characterTicketData[i - 1].sJob = Cells[2];
            characterTicketData[i - 1].sPercentage = Cells[3];
            characterTicketData[i - 1].sTier = Cells[4];
            characterTicketData[i - 1].sTribe = Cells[5];
            characterTicketData[i - 1].sImage = Cells[6];
            characterTicketData[i - 1].sExplanation = Cells[7];
            characterTicketData_List.Add(characterTicketData[i - 1]);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, characterTicketData_List);
        fileStream.Close();
    }
    
    public static void Load_TableInfo_WeaponTicketData()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/WeaponTicket.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "ETC/WeaponTicket";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBWeaponTicket> weaponTicketData_List = new List<DBWeaponTicket>();
        DBWeaponTicket[] weaponTicketData = new DBWeaponTicket[line.Count -1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            weaponTicketData[i - 1] = new DBWeaponTicket();
            weaponTicketData[i - 1].nIndex = int.Parse(Cells[0]);
            weaponTicketData[i - 1].sName = Cells[1];
            weaponTicketData[i - 1].sEquipType = Cells[2];
            weaponTicketData[i - 1].sPercentage = Cells[3];
            weaponTicketData[i - 1].sTier = Cells[4];
            weaponTicketData[i - 1].sQulity = Cells[5];
            weaponTicketData[i - 1].sImage = Cells[6];
            weaponTicketData[i - 1].sExplanation = Cells[7];
            weaponTicketData_List.Add(weaponTicketData[i - 1]);

        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, weaponTicketData_List);
        fileStream.Close();
    }
    
    public static void Load_TableInfo_GachaData()
    {
        //if (cCharacterState.Length != 0) 
        //	return;
        string path = Application.streamingAssetsPath + "/Data/EmployGacha.data";
        if (File.Exists(path))
            File.Delete(path);
        string txtFilePath = "ETC/Gacha";
        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        List<DBEmployGacha> employGachaData_List = new List<DBEmployGacha>();
        DBEmployGacha[] employGachaData = new DBEmployGacha[line.Count -1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue;   // Title skip

            string[] Cells = line[i].Split("\t"[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            employGachaData[i - 1] = new DBEmployGacha();
            employGachaData[i - 1].nIndex = int.Parse(Cells[0]);
            employGachaData[i - 1].sName = Cells[1];
            employGachaData[i - 1].sJob = Cells[2];
            employGachaData[i - 1].sPercentage = Cells[3];
            employGachaData[i - 1].sTier = Cells[4];
            employGachaData[i - 1].sTribe = Cells[5];
            employGachaData[i - 1].nCost_Gold = int.Parse(Cells[6]);
            employGachaData[i - 1].nCost_Gem = int.Parse(Cells[7]);
            employGachaData[i - 1].sImage = Cells[8];
            employGachaData[i - 1].sExplanation = Cells[9];
            employGachaData_List.Add(employGachaData[i - 1]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        bf.Serialize(fileStream, employGachaData_List);
        fileStream.Close();
    }
    
    #endregion
    #region SplitText

    public static TextAsset LoadTextAsset(string _txtFile)
    {
        TextAsset ta;
        ta = Resources.Load("ParsingData/" + _txtFile) as TextAsset;
        return ta;
    }

    public static List<string> LineSplit(string text)
    {
        //Console.WriteLine("LineSplit " + text.Length);

        char[] text_buff = text.ToCharArray();

        List<string> lines = new List<string>();

        int linenum = 0;
        bool makecell = false;

        StringBuilder sb = new StringBuilder("");

        for (int i = 0; i < text.Length; i++)
        {
            char c = text_buff[i];
            //int value = Convert.ToInt32(c); Console.WriteLine(String.Format("{0:x4}", value) + " " + c.ToString());

            if (c == '"')
            {
                char nc = text_buff[i + 1];
                if (nc == '"') { i++; } //next char
                else
                {
                    if (makecell == false) { makecell = true; c = nc; i++; } //next char
                    else { makecell = false; c = nc; i++; } //next char
                }
            }

            //0x0a : LF ( Line Feed : 다음줄로 캐럿을 이동 '\n')
            //0x0d : CR ( Carrage Return : 캐럿을 제일 처음으로 복귀 )			    
            if (c == '\n' && makecell == false)
            {
                char pc = text_buff[i - 1];
                if (pc != '\n')	//file end
                {
                    lines.Add(sb.ToString()); sb.Remove(0, sb.Length);
                    linenum++;
                }
            }
            else if (c == '\r' && makecell == false)
            {
            }
            else
            {
                sb.Append(c.ToString());
            }
        }

        return lines;
    }
    #endregion

}
