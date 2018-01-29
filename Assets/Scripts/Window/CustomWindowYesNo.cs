using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomWindowYesNo : ButtonUIBase
{
    public Image mainImage;
    public Image yesButton_Image;
    public Image noButton_Image;
    public Image CloseButton_Image;
    public Text contents_Text;

    public EmployPanel employPanel;


    //현재 창이 보여주는 정보의 상태
    public E_CUSTOMWINDOW eCurState;

    public void initWindow()
    {
        mainImage = gameObject.transform.GetChild(1).GetComponent<Image>();
        yesButton_Image = gameObject.transform.GetChild(2).GetComponent<Image>();
        noButton_Image = gameObject.transform.GetChild(3).GetComponent<Image>();
        CloseButton_Image = gameObject.transform.GetChild(4).GetComponent<Image>();
        contents_Text = gameObject.transform.GetChild(5).GetComponent<Text>();
    }
    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "Button_yes")
        {
            switch (eCurState)
            {
                case E_CUSTOMWINDOW.E_CUSTOMWINDOW_EMPLOY_GACHA_1_2TIER:
                    employPanel.EmployCharacter(E_EMPLOY.E_EMPLOY_1_2TIER);
                    break;
                default:
                    break;
            }

            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "Button_no")
        {
            Debug.Log("Clicked");
            this.gameObject.SetActive(false);
        }
        else if(eventData.pointerCurrentRaycast.gameObject.name == "Button_Close")
        {
            Debug.Log("Clicked");
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
