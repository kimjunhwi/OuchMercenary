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

    public E_ACTIVEBUTTON eCurActivePanel;

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
            this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "Button_no")
        {
            Debug.Log("Clicked");
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

    }
    #endregion 
}
