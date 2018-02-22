using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class InventoryItemAbilityInfoPanel : MonoBehaviour
{
    private const int abilitySlotCount = 6;
    public InventoryAbilitySlot[] slots;
    int nRandomIndex = 0;

    public void Init()
    {
        slots = new InventoryAbilitySlot[abilitySlotCount];
        for(int i=0; i<abilitySlotCount; i++)
        {
            slots[i] = this.gameObject.transform.GetChild(i).gameObject.AddComponent<InventoryAbilitySlot>();
            slots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventoryAbilitySlot>();
            slots[i].Init();
        }
    }

    public void InitSlotPosition()
    {
        for(int i=0; i<abilitySlotCount;i++)
        {
            RectTransform rectTransform = slots[i].GetComponent<RectTransform>();
            if (i == 0)
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -50f);
            else if (i ==1)
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -150f);
            else if (i == 2)
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -250f);
            else if (i == 3)
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -350f);
            else if (i == 4)
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -450f);
            else
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -550f);
        }
    }

    //현재 옵션의 개수 만큼 슬롯을 보여준다.
    public void SetAbilitySlot(int _optionCount, InventorySlot _slot)
    {
        nRandomIndex = 0;
        //슬롯 위치 초기화
        InitSlotPosition();

        RectTransform rectTransform = null;

        for (int i = 0; i < abilitySlotCount; i++)
        {
            if (i < _optionCount)
            {
                if(_slot.sItemType != "10")
                {
                    slots[i].gameObject.SetActive(true);
                    rectTransform = slots[i].GetComponent<RectTransform>();
                    SetAbilityInfo(_slot.equipment, slots[i]);

                }
                else
                {
                    slots[i].gameObject.SetActive(true);
                    rectTransform = slots[i].GetComponent<RectTransform>();
                    SetIngredientInfo(_slot.ingredient.sExplanation, slots[i]);
                }
               
              
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
       
    }

    public void SetIngredientInfo(string _str, InventoryAbilitySlot _slot)
    {
        _slot.AbilityExplanation_Text.text = "";
        _slot.AbilityValue_Text.text = _str;
    }

    public void SetAbilityInfo(Equipment _equipment, InventoryAbilitySlot _slot)
    {
        bool bIsExit = false;

        while (true)
        {
            if (bIsExit == true)
                break;

            switch (nRandomIndex)
            {
                case (int)E_RANDOM_OPTION.E_HP:
                    {
                        if (_equipment.fHp != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "체력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fHp);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_ACCURACY:
                    {
                        if (_equipment.fAccuracy != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "명중률";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fAccuracy);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_ALL_ATTACK_RATING:
                    {
                        if (_equipment.fAll_Attack_RatingPlus != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "전체 공격력+";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fAll_Attack_RatingPlus);
                            nRandomIndex++;

                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_PHSYICAL_ATTACK_RATING:
                    {
                        if (_equipment.fPhysical_Attack_Rating != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "물리 공격력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fPhysical_Attack_Rating);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;
                        }

                    }
                    break;
                case (int)E_RANDOM_OPTION.E_MAGIC_ATTACK_RATING:
                    {
                        if (_equipment.fMagic_Attack_Rating != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "마법 공격력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fMagic_Attack_Rating);
                            nRandomIndex++;

                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_ALL_PENETRATE:
                    {
                        if (_equipment.fAll_Penetrate != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "전체 관통력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fAll_Penetrate);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;
                        }

                    }
                    break;
                case (int)E_RANDOM_OPTION.E_PHSYICAL_PENETRATE:
                    {
                        if (_equipment.fPhysical_Penetrate != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "물리 관통력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fPhysical_Penetrate);
                            nRandomIndex++;

                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_MAGIC_PENETRATE:
                    {
                        if (_equipment.fMagic_Penetrate != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "마법 관통력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fMagic_Penetrate);
                            nRandomIndex++;

                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_ALL_DEFENSE:
                    {
                        if (_equipment.fAll_Defense != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "전체 방어력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fAll_Defense);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_PHSYICAL_DEFENSE:
                    {
                        if (_equipment.fPhysical_Defense != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "물리 방어력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fPhysical_Defense);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;

                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_MAGIC_DEFENSE:
                    {
                        if (_equipment.fMagic_Defense != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "마법 방어력";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fMagic_Defense);
                            nRandomIndex++;

                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_DODGE:
                    {
                        if (_equipment.fDodge != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "회피";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fDodge);
                            nRandomIndex++;

                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_CRITICAL_RATING:
                    {
                        if (_equipment.fCritical_Rating != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "치명타 확률";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fCritical_Rating);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;

                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_CRITICAL_DAMAGE:
                    {
                        if (_equipment.fCritical_Damage != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "치명타 피해량";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fCritical_Damage);
                            nRandomIndex++;

                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_ATTACK_SPEED:
                    {
                        if (_equipment.fAttack_Speed != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "공격속도";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fAttack_Speed);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;

                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_COOLTIME:
                    {
                        if (_equipment.fCoolTime != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "쿨타임";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fCoolTime);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;

                        }
                    }
                    break;
                case (int)E_RANDOM_OPTION.E_EXP_BOOST:
                    {
                        if (_equipment.fExpBoost != 0)
                        {
                            _slot.AbilityExplanation_Text.text = "경험치 추가량";
                            _slot.AbilityValue_Text.text = string.Format("{0}", _equipment.fExpBoost);
                            nRandomIndex++;
                            bIsExit = true;
                            continue;
                        }
                    }
                    break;
                default:
                    break;

        
            }
            nRandomIndex++;
        }
        
    }
}
