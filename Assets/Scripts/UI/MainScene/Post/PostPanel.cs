using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PostPanel : ButtonUIBase
{
    public SimpleObjectPool m_PostSimpleObject;

    public  float fInitPostListPosY_Value = -110f;
    public  float fInitPostListHeight_Value = 220f;

    public float fInitPostListPos;
    public float fInitPostListHeight;

    public int nMailCount = 0;

    //fInitPostListPos = -110 씩 계속
    //fInitPostListHeight = 220씩 추가

    public RectTransform postPanelListTrans;
    public PostGetPanel postGetPanel;

    public void InitPostPanel()
    {
        postPanelListTrans = this.gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<RectTransform>();
       
    }

    //플레이어 정보를 체크해서 우편 슬롯을 배치한다.
    public void CheckDataAndDispatchPostSlot()
    {
        Player player = new Player();
        player = GameManager.Instance.GetPlayer();
        nMailCount = GameManager.Instance.GetPlayer().mail.Count;
        
        //if (postPanelListTrans == null)
        //   InitPostPanel();

        //플레이어 정보에 따라 슬롯을 추가한다.
        //추가 할때 마다 rectList의 height도 늘린다.
        //asset번들로 빼놓은 sprite에서 받아온다 이미지는 
        //나머지는 플레이어 정보에서 슬롯에 집어넣는다.
        postPanelListTrans.anchoredPosition = new Vector2(postPanelListTrans.anchoredPosition.x, 0);
        postPanelListTrans.sizeDelta = new Vector2(postPanelListTrans.sizeDelta.x, 0);


        //만약 안받고 남아 있으면 일단 한번 다 지운후에 다시 만든다. (시간 처리 나중에 해야함)
        if (postPanelListTrans.childCount != 0)
        {
            int nLeftCount = postPanelListTrans.childCount;
            for (int i=0; i < nLeftCount; i++)
            {
                 m_PostSimpleObject.ReturnObject(postPanelListTrans.GetChild(0).gameObject);
            }
        }

        //메일 리스트의 개수 만큼 돈다.
        for (int i = 0; i < nMailCount; i++)
        {
            GameObject postSlot_Obj = m_PostSimpleObject.GetObject();
            PostSlot postSlot = postSlot_Obj.GetComponent<PostSlot>();
            postSlot.postGetPanel = postGetPanel;
            RectTransform postSlotRectTransform = postSlot.gameObject.GetComponent<RectTransform>();
        

            postSlot_Obj.SetActive(true);

            //PostSlot init
            postSlot.title_Text.text = "";
            postSlot.Contents_Text.text = "";
            postSlot.LeftDays_Text.text = "";

            //postSlot.post_Image
            postSlot.title_Text.text = GameManager.Instance.GetPlayer().mail[i].sTitle;
            postSlot.Contents_Text.text = GameManager.Instance.GetPlayer().mail[i].sContents;

            //Time(시간)
            DateTime mailTime = GameManager.Instance.GetPlayer().mail[i].dateTime;
            DateTime curTime = System.DateTime.Now;
            TimeSpan resultTime = curTime - mailTime;

            //날짜
            double dValue_Days = 60 * 60 * 24;
            //시간
            double dValue_Hours = 60 * 60;
            //분
            double dValue_Minute = 60;

            double leftDayTime = GameManager.Instance.GetPlayer().mail[i].nDays * dValue_Days;
            double totalSecond = leftDayTime -  resultTime.TotalSeconds;

            //초로 환산하여 계산
            //만약 하루 이상 일경우에는 일수로 표시
            if(totalSecond >= dValue_Days)
            {
                float resultDays =  (float)totalSecond / (float)dValue_Days;
                resultDays = Mathf.Floor(resultDays);
                postSlot.LeftDays_Text.text = resultDays.ToString() + "일";
            }
            else if(totalSecond >= dValue_Hours)
            {
                float resultHours = (float)totalSecond / (float)dValue_Hours;
                resultHours = Mathf.Floor(resultHours);
                postSlot.LeftDays_Text.text = resultHours.ToString() + "시간";
            }
            else
            {
                float resultMinute = (float)totalSecond / (float)dValue_Minute;
                resultMinute = Mathf.Floor(resultMinute);
                postSlot.LeftDays_Text.text = resultMinute.ToString() + "분";
            }
            

            postSlot.PostSimpleObject = m_PostSimpleObject;
            postSlot.postPanel = this;
            postSlot.postSlotMailInfo = GameManager.Instance.GetPlayer().mail[i];
            //postSlot SetParent
            postSlot_Obj.transform.SetParent(this.gameObject.transform.GetChild(3).gameObject.transform.GetChild(0));

            postSlotRectTransform.anchoredPosition3D = new Vector3(postSlotRectTransform.anchoredPosition3D.x, postSlotRectTransform.anchoredPosition3D.y, 1f);
            postSlotRectTransform.localScale = new Vector3(1f, 1f, 1f);
            //위치
            postPanelListTrans.anchoredPosition = new Vector2(postPanelListTrans.anchoredPosition.x, postPanelListTrans.anchoredPosition.y + fInitPostListPosY_Value);
            //사이즈
            postPanelListTrans.sizeDelta = new Vector2(postPanelListTrans.sizeDelta.x, postPanelListTrans.sizeDelta.y + fInitPostListHeight_Value);
        }
        
    }

    public void ResetPostGetSlots()
    {
        //위치 사이즈도 다시 초기화
        postGetPanel.postGetPanelListTrans.anchoredPosition = new Vector2(postGetPanel.postGetPanelListTrans.anchoredPosition.x, 0);
        //사이즈
        postGetPanel.postGetPanelListTrans.sizeDelta = new Vector2(postGetPanel.postGetPanelListTrans.sizeDelta.x, postGetPanel.fInitPostListHeight_Value);

        //오브젝트 풀로 다시 돌린다.
        for (int i = 0; i < nMailCount; i++)
            postGetPanel.m_PostGetSimpleObject.ReturnObject(postGetPanel.postGetPanelListTrans.GetChild(0).gameObject);
    }

    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
        if (eventData.pointerCurrentRaycast.gameObject.name == "Close_Button")
        {
           this.gameObject.SetActive(false);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "Refresh_Button")
        {
            //플레이어의 정보를 다시 가져와 메일함에 있는지 없는지 확인한다.
            GameManager.Instance.loginManager.StartGetPlayersData();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "GetAll_Button")
        {
            //postGetPanel.SetUpPostGetResult_All(GameManager.Instance.GetPlayer().mail.Count);
            //Test용
            postGetPanel.SetUpPostGetResult_All(50);
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
