using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PostGetPanel : ButtonUIBase
{
    public SimpleObjectPool m_PostGetSimpleObject;

    public RectTransform postGetPanelListTrans;

    public float fInitPostListPosY_Value = -60f;
    public float fInitPostListHeight_Value = 250f;
    public int nListValue = 1;
    public PostPanel postPanel;
    

    //AssetLoad 할때 초기화를 함.
    public void InitPostGetPanel()
    {
        postGetPanelListTrans = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<RectTransform>();
    }
    
    //결과창 셋업(하나씩 받을때)
    public void SetUpPostGetResult_Single()
    {
        this.gameObject.SetActive(true);

        GameObject postGetSlot_Obj = m_PostGetSimpleObject.GetObject();
        PostGetSlot postGetSlot = postGetSlot_Obj.GetComponent<PostGetSlot>();
        RectTransform postGetSlotRectTransform = postGetSlot.gameObject.GetComponent<RectTransform>();

        //postGetSlot.image = postGetPanel;
        //RectTransform postSlotRectTransform = postSlot.gameObject.GetComponent<RectTransform>();

        postGetSlot_Obj.transform.SetParent(postGetPanelListTrans);

        postGetSlotRectTransform.anchoredPosition3D = new Vector3(postGetSlotRectTransform.anchoredPosition3D.x, postGetSlotRectTransform.anchoredPosition3D.y, 1f);
        postGetSlotRectTransform.localScale = new Vector3(1f, 1f, 1f);

        
    }

    //결과창 셋업(모두 받기로 받을때)
    public void SetUpPostGetResult_All(int _mailCount)
    {
        nListValue = 1;
        //갯수가 11개 이상일때와 아닐때
        //11개 이상이 아닐때는 위치조정 할 필요 x 그래도 위치를 y를 0으로 조정
        //11개 이상일때는 위치조정까지 해야함

        //결과창 켜기
        this.gameObject.SetActive(true);
        //3줄 이상 넘어갈시 위치조정와 사이즈 루프 카운트 계산
        int nLoopCount = (int)(_mailCount / 5);

        //2줄 이하이면 그냥 원래 위치에 사이즈만 늘린다.
        if (_mailCount <= 10)
        {
            //위치
            postGetPanelListTrans.anchoredPosition = new Vector2(postGetPanelListTrans.anchoredPosition.x, 0);
            //사이즈
            //postGetPanelListTrans.sizeDelta = new Vector2(postGetPanelListTrans.sizeDelta.x, postGetPanelListTrans.sizeDelta.y + fInitPostListHeight_Value);

        }
        //3줄 이상 부터는 루프를 돌면서 사이즈와 위치를 조정한다.
        else
        {
            nLoopCount -= 2;

            for (int i=0; i< nLoopCount; i++)
            {
                //위치
                postGetPanelListTrans.anchoredPosition = new Vector2(postGetPanelListTrans.anchoredPosition.x, fInitPostListPosY_Value * nListValue);
                //사이즈
                postGetPanelListTrans.sizeDelta = new Vector2(postGetPanelListTrans.sizeDelta.x, postGetPanelListTrans.sizeDelta.y + fInitPostListHeight_Value);

                nListValue += 2;
            }
          
        }
        //갯수 만큼 보여줄 슬롯을 만든다.
      
        for (int i=0; i< _mailCount; i++)
        {
            GameObject postGetSlot_Obj = m_PostGetSimpleObject.GetObject();
            PostGetSlot postGetSlot = postGetSlot_Obj.GetComponent<PostGetSlot>();
            RectTransform postGetSlotRectTransform = postGetSlot.gameObject.GetComponent<RectTransform>();

            postGetSlot_Obj.transform.SetParent(postGetPanelListTrans);

            postGetSlotRectTransform.anchoredPosition3D = new Vector3(postGetSlotRectTransform.anchoredPosition3D.x, postGetSlotRectTransform.anchoredPosition3D.y, 1f);
            postGetSlotRectTransform.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        //이미지가 비어있으면 해당슬롯에 선택되어있는 캐릭터이미지를 넣는다
        if (eventData.pointerCurrentRaycast.gameObject.name == "PostGetConfirm_Button")
        {
            //해당 슬롯의 이미지 초기화 해야함(확인 버튼을 누르면 꺼지기 때문에 postPanel로 빼냄)
            postPanel.ResetPostGetSlots();
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
