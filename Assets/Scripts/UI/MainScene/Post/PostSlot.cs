using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ReadOnlys;

enum E_SLOT_CHILDINDEX
{
    E_SLOT_CHILDINDEX_IMAGE = 0,
    E_SLOT_CHILDINDEX_TITLE,
    E_SLOT_CHILDINDEX_CONTENT,
    E_SLOT_CHILDINDEX_LEFTDAYS,
    E_SLOT_CHILDINDEX_GETBUTTON,
}



public class PostSlot : ButtonUIBase
{
    public Image post_Image;
    public Text title_Text;
    public Text Contents_Text;
    public Text LeftDays_Text;
    public Image GetButton_Image;

    public SimpleObjectPool PostSimpleObject;
    public PostPanel postPanel;
    public Mail postSlotMailInfo;
    public PostGetPanel postGetPanel;

    public void InitSlot()
    {
        post_Image = this.gameObject.transform.GetChild((int)E_SLOT_CHILDINDEX.E_SLOT_CHILDINDEX_IMAGE).GetComponent<Image>();
        title_Text = this.gameObject.transform.GetChild((int)E_SLOT_CHILDINDEX.E_SLOT_CHILDINDEX_TITLE).GetComponent<Text>();
        Contents_Text = this.gameObject.transform.GetChild((int)E_SLOT_CHILDINDEX.E_SLOT_CHILDINDEX_CONTENT).GetComponent<Text>();
        LeftDays_Text = this.gameObject.transform.GetChild((int)E_SLOT_CHILDINDEX.E_SLOT_CHILDINDEX_LEFTDAYS).GetComponent<Text>();
        GetButton_Image = this.gameObject.transform.GetChild((int)E_SLOT_CHILDINDEX.E_SLOT_CHILDINDEX_IMAGE).GetComponent<Image>();
    }

    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "Get_Button")
        {
            //postPanel사이즈 와 위치 조정
            postPanel.postPanelListTrans.anchoredPosition = new Vector2(postPanel.postPanelListTrans.anchoredPosition.x, postPanel.postPanelListTrans.anchoredPosition.y - postPanel.fInitPostListPosY_Value);
            postPanel.postPanelListTrans.sizeDelta = new Vector2(postPanel.postPanelListTrans.sizeDelta.x, postPanel.postPanelListTrans.sizeDelta.y - postPanel.fInitPostListHeight_Value);
            //player의 메일 데이터 지우기 
            // GameManager.Instance.playerData.GetPlayer().Remove(postSlotMailInfo);
            postSlotMailInfo = null;
            //수정된 플레이어 정보 서버에 업데이트
            //GameManager.Instance.loginManager.StartUpdatePlayerData();

            //해당 메일에 대한 결과창을 보여준다.
            postGetPanel.SetUpPostGetResult_Single();

            //postSlot pool로 되돌림
            PostSimpleObject.ReturnObject(this.gameObject);
            Debug.Log("Get_Button : " + title_Text.text);


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
