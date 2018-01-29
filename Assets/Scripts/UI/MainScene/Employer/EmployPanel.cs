using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;


enum E_EMPLOY_GACHA
{
    E_EMPLOY_GACHA_1_2Tier = 0,
    E_EMPLOY_GACHA_2_4TIER_MELEE,
    E_EMPLOY_GACHA_2_4TIER_RANGE,
    E_EMPLOY_GACHA_2_4TIER_ASSISTANT,
    E_EMPLOY_GACHA_2_4TIER_COMMANDER,
}


public class EmployPanel : ButtonUIBase
{
    public E_ACTIVEBUTTON m_eCurActivePanel;
    public MainSceneManager mainSceneManager;
    public EmployFinishPanel employFinishPanel;
   

    private void Init()
    {
        m_eCurActivePanel = E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT;
    }

 
    //캐릭터 고용(임시 1~2성 캐릭터 소환)
    public void EmployCharacter(E_EMPLOY _employCharacter)
    {
        //Job 
        int randomMin = 0;
        int randomMax = 0;
        int nJobIndex = 0;
        //Percentage
        List<float> percentageList = null;
        //Character
        DBBasicCharacter character = new DBBasicCharacter();

        switch (_employCharacter)
        {
            case E_EMPLOY.E_EMPLOY_1_2TIER:
                //Jobs
                string job = GameManager.Instance.lDBEmployGacha[(int)E_EMPLOY.E_EMPLOY_1_2TIER].sJob;
                //맨처음 인덱스 
                randomMin = int.Parse(job.Substring(0, job.IndexOf(",")));
                //마지막 인덱스
                job = job.Remove(0, job.Length - 1);
                randomMax = int.Parse(job);
                //직업 인덱스
                nJobIndex = Random.RandomRange(randomMin, randomMax);
                //Percentage
                percentageList = GetPercentage(_employCharacter);
                //확률 계산 해야함
                float resultPercentage = Random.RandomRange(0, 100f);
                Debug.Log("확률 수치 : " + resultPercentage);
                float percentageCheck = 0f;
                //현재 오름 차순 (낮은 것 부터 체크)
                for (int i = 0; i < percentageList.Count; i++)
                {
                    percentageCheck += percentageList[i];
                    if (resultPercentage <= percentageCheck)
                    {
                        character = GetCharacterCategory(_employCharacter, i, nJobIndex);
                        employFinishPanel.SetUpResult(character);
                        break;
                    }
                }
                break;

            case E_EMPLOY.E_EMPLOY_2_4TIER_MELEE:

                break;

            default:
                break;
        }
    }


    public DBBasicCharacter GetCharacterCategory(E_EMPLOY _employCharacter, int _tier ,  int _jobIndex)
    {
        DBBasicCharacter character = new DBBasicCharacter();

        switch (_employCharacter)
        {
            case E_EMPLOY.E_EMPLOY_1_2TIER:
                int random = 0;
                //티어 체크는 높은 순 대로 체크
                if (_tier == 0)
                {
                    Debug.Log("2성 소환");
                    random = Random.RandomRange(1, 2);
                }
                
                else if(_tier == 1)
                {
                    Debug.Log("1성 소환");
                    random = 0;
                }

                switch (_jobIndex)
                {
                    case (int)E_CHARACTER_TYPE.E_ASSASIN:
                        //각 직업 리스트의 첫번째 껄 참조
                        character = GetCharacter(GameManager.Instance.assasinList[random]);
                        break;
                    case (int)E_CHARACTER_TYPE.E_WARRIOR:
                        character = GetCharacter(GameManager.Instance.warriorList[random]);
                        break;
                    case (int)E_CHARACTER_TYPE.E_ARCHER:
                        character = GetCharacter(GameManager.Instance.archerList[random]);
                        break;
                    case (int)E_CHARACTER_TYPE.E_WIZZARD:
                        character = GetCharacter(GameManager.Instance.wizzardList[random]);
                        break;
                    case (int)E_CHARACTER_TYPE.E_KNIGHT:
                        character = GetCharacter(GameManager.Instance.knightList[random]); 
                        break;
                    case (int)E_CHARACTER_TYPE.E_PRIEST:
                        character = GetCharacter(GameManager.Instance.priestList[random]);
                        break;
                    case (int)E_CHARACTER_TYPE.E_COMMAND:
                        character = GetCharacter(GameManager.Instance.commandList[random]);

                        break;
                }
                break;
            default:
                break;
        }


       

        return character;
    }

    //확률 파싱해서 뽑아오기
    public List<float> GetPercentage(E_EMPLOY _emplayCharacter)
    {
        string sPercenstage = GameManager.Instance.lDBEmployGacha[(int)_emplayCharacter].sPercentage;
        List<float> percentageList = new List<float>();
        for (int i = 0; i < sPercenstage.Length; i++)
        {
            //마지막
            if (sPercenstage.Contains(",") == false)
                percentageList.Add(float.Parse(sPercenstage.Substring(0, sPercenstage.Length)));
            else
            {
                percentageList.Add(float.Parse(sPercenstage.Substring(0, sPercenstage.IndexOf(","))));
                sPercenstage = sPercenstage.Remove(0, sPercenstage.IndexOf(",") + 1);

            }
        }
        /*
        //오름차순 정렬(엑셀에서 값 수정으로 없앨수 있음)
        percentageList.Sort(delegate (float A, float B)
        {
            if (A > B)
                return 1;
            else if (A < B)
                return -1;
            return 0;
        });
        */

        return percentageList;

    }
    //캐릭터 뽑기(임시)
    public DBBasicCharacter GetCharacter(DBBasicCharacter _character)
    {
        //스킬 개수
        //중복 x
        //Basic     ->  1개 (단 티어에 따라 다름)
        //passive   ->  1Tier 1개 2Tier 2개 3Tier 3개 4Tier 3개 
        //Active    ->  최대 3개 1Tier 1개 2Tier 2개 3Tier 3개 4Tier 3개 (단 2,3,4Tier)
        //tribe     
        

        DBBasicCharacter character = new DBBasicCharacter();
        character = _character;

        //스킬 할당을 다시 해야함.
        DBActiveSkill activeSkill = new DBActiveSkill();
        DBPassiveSkill  passiveSkill = new DBPassiveSkill();
        //basicCharacter Tier가 올라갈때 마다 티어에 맞는 스킬 할당
        DBBasicSkill basicSkill = GameManager.Instance.lDbBasickill.Find(x => x.nCharacterIndex == character.C_Index);

        List<DBActiveSkill> active_List = GameManager.Instance.lDbActiveSkill.FindAll(x => x.m_nTier == character.Tier);
        int nRandomIndex = Random.Range(0, active_List.Count);
        activeSkill = active_List[nRandomIndex];

        List<DBPassiveSkill> passive_List = GameManager.Instance.lDbPassiveSkill.FindAll(x => x.nTier == character.Tier);
        nRandomIndex = Random.Range (0, passive_List.Count);
        passiveSkill = passive_List[nRandomIndex];

        character.basicSkill.Add(basicSkill);
        character.activeSkills.Add(activeSkill);
        character.passiveSkills.Add(passiveSkill);

        //체크용
        if (_character.Tier == 1 || _character.Tier == 2)
        {
            Debug.Log(_character.C_JobNames.ToString() + " Tier : " + _character.Tier.ToString());
        }
        return character;
    }


    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
        //소환 함수 넣어야함.
        if (eventData.pointerCurrentRaycast.gameObject.name == "1,2TierEmploySlot_Button")
        {
            //Debug.Log("1~2성 캐릭터가 등장합니다");
            mainSceneManager.SetCustomWindow(E_CUSTOMWINDOW.E_CUSTOMWINDOW_EMPLOY_GACHA_1_2TIER, GameManager.Instance.lDBEmployGacha[(int)E_EMPLOY.E_EMPLOY_1_2TIER].sName + "을 하시겠습니까?");
            //EmployCharacter();

        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1,3TierEmploySlot_Button")
        {
            Debug.Log("1~3티어 용병고용");
         
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "2,3TierEmploySlot_Button")
        {
            Debug.Log("2~3티어 용병고용");
           
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (eventData.pointerCurrentRaycast.gameObject.name == "2,4TierMeleeEmploySlot_Button")
        {
            Debug.Log("2~4성 암살자, 전사 캐릭터가 등장합니다");
           
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "2,4TierRangeEmploySlot_Button")
        {
            Debug.Log("2~4성 궁수, 마법사 캐릭터가 등장합니다");
         
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "2,4TierAssistentEmploySlot_Button")
        {
            Debug.Log("2~4성 , 기사,사제 캐릭터가 등장합니다");
         
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "2,4TierCommenderEmploySlot_Button")
        {
            Debug.Log("2~4성 지휘관 캐릭터가 등장합니다");
        
        }
    
        else
        {
            Debug.Log("Clicked");
        }






    }

    public override void OnPointerDown(PointerEventData eventData)
    {

    }

    #endregion

}
