using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

public class EmployFinishPanel : EmployPanel
{
    public Text job_Text;
    public Text name_Text;
    public GameObject employCharacterHold_Obj;
    public Image confirmButton_Image;
    public Image oneMoreTimeButton_Image;

    public MainSceneManager mainSceneManager;
    public int nCharacterIndex = 0;

    public void Init()
    { 
        job_Text = this.gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        name_Text = this.gameObject.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        employCharacterHold_Obj = this.gameObject.transform.GetChild(1).transform.GetChild(2).gameObject;
        confirmButton_Image = this.gameObject.transform.GetChild(2).GetComponent<Image>();
        oneMoreTimeButton_Image = this.gameObject.transform.GetChild(3).GetComponent<Image>();
    }

    public void SetUpResult(DBBasicCharacter _character)
    {
        nCharacterIndex = _character.Index;
        //InfoUI 숨김
        mainSceneManager.InfoUI_Obj.SetActive(false);
        //Upbar 숨김
        GameManager.Instance.upBar.gameObject.SetActive(false);
        //정보 표시
        job_Text.text = _character.C_JobNames;
        name_Text.text = _character.C_Name;

        if(nCharacterIndex <= 50)
            employCharacterHold_Obj.transform.GetChild(nCharacterIndex).gameObject.SetActive(true);

        this.gameObject.SetActive(true);
    }

    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
        if (eventData.pointerCurrentRaycast.gameObject.name == "EmployFinishButton_Image")
        {
            Debug.Log("용병고용 확인 버튼 클릭!");
            mainSceneManager.InfoUI_Obj.SetActive(true);
            GameManager.Instance.upBar.gameObject.SetActive(true);
            if(nCharacterIndex <= 50)
                employCharacterHold_Obj.transform.GetChild(nCharacterIndex).gameObject.SetActive(false);

            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "EmployOneMoreTimeButton_Image")
        {
            Debug.Log("용병고용 한번더 버튼 클릭!");
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
