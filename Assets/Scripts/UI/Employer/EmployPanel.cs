using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class EmployPanel : ButtonUIBase
{
    public E_ACTIVEBUTTON m_eCurActivePanel;

    

    void Awake()
    {
        
    }

    private void Init()
    {
        m_eCurActivePanel = E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT;
    }


    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
        if (eventData.pointerCurrentRaycast.gameObject.name == "1,2TierEmploy_Slot")
        {
            Debug.Log("1~2티어 용병고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1,3TierEmploy_Slot")
        {
            Debug.Log("1~3티어 용병고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "2,3TierEmploy_Slot")
        {
            Debug.Log("2~3티어 용병고용");
            this.gameObject.SetActive(false);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (eventData.pointerCurrentRaycast.gameObject.name == "1.3TierAssasinEmploy_Slot")
        {
            Debug.Log("1~3티어 어쌔신 고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1.3TierWarriorEmploy_Slot")
        {
            Debug.Log("1~3티어 워리어 고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1.3TierPristEmploy_Slot")
        {
            Debug.Log("1~3티어 프리스트 고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "2,3TierEmploy_Slot")
        {
            Debug.Log("2,3티어 용병고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1.3TierWizardEmploy_Slot")
        {
            Debug.Log("1~3티어 위자드 고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1.3TierArcherEmploy_Slot")
        {
            Debug.Log("1~3티어 궁수 고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1.3TierKnight_Slot")
        {
            Debug.Log("1~3티어 나이트 고용");
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "1.3TierCommender_Slot")
        {
            Debug.Log("1~3티어 커맨더 고용");
            this.gameObject.SetActive(false);
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
