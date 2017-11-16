using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

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
