﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using ReadOnlys;


//리스트에 대한 관리 역할
public class Inventory_TotalItem : GenericListRectScrolling
{
   
    //플레이어가 가지고있는 아이템의 정보들을 초기화 한다(42개)
    public override void Init()
    {
        Debug.Log("전체 아이템 탭 초기화");
        //Const가 아닌 변수들은 초기화를 다시 해준다.
        nPlayerTotalItemCount = 0;
        fInitRectYValue = 0f;
        nLowItemCount = 0;
        nHighItemCount = nInitItemSlotCount;
        bIsNeedScrolling = false;
        fLastValue = 0;
        fPrevHeight = 0;
        fCurHeight = 0;
        nNextIndex = 0;
        bIsLastLine = false;
        bIsInit = true;

        //할당
        nPlayerTotalItemCount = GameManager.Instance.GetPlayer().LIST_ITEM.Count;
        scrollRect = this.gameObject.transform.parent.GetComponent<ScrollRect>();
        rectTransform = this.gameObject.transform.GetComponent<RectTransform>();
        inventoryslots = new InventorySlot[nInitItemSlotCount];

        ItemSortInLevel = GameManager.Instance.GetPlayer().LIST_ITEM;

        int nEmptySlotStartIndex = 0;
     
        for (int i = 0; i < nInitItemSlotCount; i++)
        {
            //end 조건
            if (i >= GameManager.Instance.GetPlayer().LIST_ITEM.Count)
            {
                nHighItemCount = i - 1;
                nEmptySlotStartIndex = nHighItemCount + 1;
                break;
            }
            inventoryslots[i] = this.gameObject.transform.GetChild(i).gameObject.AddComponent<InventorySlot>();
            inventoryslots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventorySlot>();
            inventoryslots[i].Init();
            inventoryslots[i].inventoryItemInfo = inventoryItemInfo;
            inventoryslots[i].SetItemInfo_ALLItem(GameManager.Instance.GetPlayer().LIST_ITEM[i]);
        }
        //기본 슬롯보다 적은경우 빈칸으로 처리.(SetActive(false))
        
        //비어있는 칸들에 대한 처리
        for(int i= nHighItemCount + 1; i < nInitItemSlotCount; i++)
        {
            inventoryslots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventorySlot>();
            inventoryslots[i].Init();
            inventoryslots[i].gameObject.SetActive(false);
        }

        //OnValueChanged로 할때
        scrollRect.onValueChanged.AddListener(StartItemScrollingTest);
        InitList();


        SetFirstItemInfoTo_ItemInfoPanel(inventoryslots[0]);
    }

    protected override void SetFirstItemInfoTo_ItemInfoPanel(InventorySlot _invenSlot)
    {
        _invenSlot.ActiveImage.gameObject.SetActive(true);
        inventoryItemInfo.SetInfo(_invenSlot);
    }

    protected override void StartItemScrollingTest(Vector2 value)
    {
        //Debug.Log("vertieclPosition : " + scrollRect.verticalNormalizedPosition);
        //올릴때
        if (fLastValue > scrollRect.verticalNormalizedPosition)
        {
            //위치값 갱신

            //현재 위치가 다음 갱신될 위치보다 작아지면 
            if ((fInitRectYValue + (fSlotSizeX * (1 + nNextIndex)) + 105f) <= fCurHeight && nHighItemCount != GameManager.Instance.GetPlayer().LIST_ITEM.Count)
            {
                //RePosition Slots
                if (bIsNeedScrolling == true)
                {
                    fPrevHeight = rectTransform.anchoredPosition.y;
                    nNextIndex++;

                    //마지막줄 체크
                    if (nHighItemCount >= nPlayerTotalItemCount - 6)
                    {
                        Debug.Log("마지막줄!");
                        nLowItemCount += nRowCount;
                        nHighItemCount += nPlayerTotalItemCount % 6;
                        bIsLastLine = true;
                    }
                    else
                    {
                        nLowItemCount += nRowCount;
                        nHighItemCount += nRowCount;
                    }

                    Debug.Log("올릴때 위치 재조정");
                    //RePosition Slots
                    if (bIsNeedScrolling == true)
                        RePositionSlots(true, nHighItemCount);
                }
            }
        }
        //멈췄을때
        else if (fLastValue == scrollRect.verticalNormalizedPosition)
        {
            Debug.Log("멈췄을때!!");
        }
        //내릴때
        else
        {
            //현재 위치가 다음 갱신될 위치보다 작아지면 
            if (fCurHeight -105f < fPrevHeight && nLowItemCount != 0)
            {
                if (bIsNeedScrolling == true)
                {
                    fPrevHeight -= (fSlotSizeX + fSpencingValueY);
                    nNextIndex--;

                    Debug.Log("내릴때마다 위치 재조정");
                    //RePosition Slots

                    RePositionSlots(false, nLowItemCount);

                    if(bIsLastLine == true)
                    {
                        bIsLastLine = false;
                        nLowItemCount -= nRowCount;
                        nHighItemCount -= nPlayerTotalItemCount % 6;
                    }
                    else
                    {
                        nLowItemCount -= nRowCount;
                        nHighItemCount -= nRowCount;
                    }
                }
                 

            }
        }

        fLastValue = scrollRect.verticalNormalizedPosition;
        fCurHeight = rectTransform.anchoredPosition.y;
      
    }

    #region ScrollingToCoroutine
    public void StartItemListScrolling()
    {
        InitList();
        if(bIsNeedScrolling ==  true)
            StartCoroutine(ItemScrolling());
    }

    IEnumerator ItemScrolling()
    {
        
        while(true)
        {
            //Debug.Log("Vector2 : " + value);
            //올릴때
            if (fLastValue > scrollRect.verticalNormalizedPosition)
            {
                //위치값 갱신

                //현재 위치가 다음 갱신될 위치보다 작아지면 
                if ((fInitRectYValue + (fSlotSizeX * (1 + nNextIndex)) + 105f) <= fCurHeight && nHighItemCount != GameManager.Instance.GetPlayer().LIST_ITEM.Count)
                {
                    //RePosition Slots
                    if (bIsNeedScrolling == true)
                    {
                        fPrevHeight = rectTransform.anchoredPosition.y;
                        nNextIndex++;

                        //마지막줄 체크
                        if (nHighItemCount >= nPlayerTotalItemCount - 6)
                        {
                            Debug.Log("마지막줄!");
                            nLowItemCount += nRowCount;
                            nHighItemCount += nPlayerTotalItemCount % 6;
                            bIsLastLine = true;
                        }
                        else
                        {
                            nLowItemCount += nRowCount;
                            nHighItemCount += nRowCount;
                        }

                        Debug.Log("올릴때 위치 재조정");
                        //RePosition Slots
                        if (bIsNeedScrolling == true)
                            RePositionSlots(true, nHighItemCount);
                    }
                }
            }
            //멈췄을때
            else if (fLastValue == scrollRect.verticalNormalizedPosition)
            {
                Debug.Log("멈췄을때!!");
              
            }
            //내릴때
            else
            {
                
                //현재 위치가 다음 갱신될 위치보다 작아지면 
                if (fCurHeight - 105f < fPrevHeight && nLowItemCount != 0)
                {
                    if (bIsNeedScrolling == true)
                    {
                        fPrevHeight -= (fSlotSizeX + fSpencingValueY);
                        nNextIndex--;

                        Debug.Log("내릴때마다 위치 재조정");
                        //RePosition Slots

                        RePositionSlots(false, nLowItemCount);

                        if (bIsLastLine == true)
                        {
                            bIsLastLine = false;
                            nLowItemCount -= nRowCount;
                            nHighItemCount -= nPlayerTotalItemCount % 6;
                        }
                        else
                        {
                            nLowItemCount -= nRowCount;
                            nHighItemCount -= nRowCount;
                        }
                    }


                }

            }

            fLastValue = scrollRect.verticalNormalizedPosition;
            fCurHeight = rectTransform.anchoredPosition.y;

            yield return null;
        }
        
    }
    #endregion


    protected override void InitList()
    {
        //현재 아이템 120
        //120 / 6 = 10;
        //기본이 7줄이 있으니 -7

        int nRepeatCount = GameManager.Instance.GetPlayer().LIST_ITEM.Count;
        int nRemainder = 0;
        
        nRepeatCount /= nRowCount;                                                      //몫                 20
        nRemainder = GameManager.Instance.GetPlayer().LIST_ITEM.Count % nRowCount;      //나머지             0

        //기본줄(7줄) 이상이면 몫에서 현재 줄 수를 빼서 나온 반복횟수만큼 늘린다.
        if (nRepeatCount >= nColCount)
        {
            nRepeatCount -= nColCount;

            for (int i = 0; i < nRepeatCount; i++)
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + fSlotSizeX + fSpencingValueY);

            if(nRemainder != 0)
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + fSlotSizeX + fSpencingValueY);

            bIsNeedScrolling = true;
        }

        //아니라면 계산을 해서 줄인다.
        else
        {
            //나머지가 없으면 딱 떨어진다는 소리이미으로 nRepeatCount 를 -5해서 절대값으로 해서 해당 횟수만큼 계산한다.
            if(nRemainder == 0)
            {
                nRepeatCount -= nColCount;
                nRepeatCount = Mathf.Abs(nRepeatCount);

                for (int i = 0; i < nRepeatCount; i++)
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - (fSlotSizeX + fSpencingValueY));
            }
            //딱떨어지지 않으면 한줄을 더 추가해 준다.
            else
            {
                nRepeatCount = nColCount - nRepeatCount;

                for (int i = 0; i < nRepeatCount; i++)
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - (fSlotSizeX + fSpencingValueY));

                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + fSlotSizeX + fSpencingValueY);
            }

            bIsNeedScrolling = false;
        }
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, - (rectTransform.sizeDelta.y / 2));

        nNextIndex = 0;
        fInitRectYValue = rectTransform.anchoredPosition.y;

    }

    protected override void RePositionSlots(bool _isUp, int _itemIndex)
    {
        //가져온 인덱스에서 7을 뺴서 해당하는 줄의 처음의 인덱스를 가져온다.
        int nItemIndex = _itemIndex;

        //올릴때 
        if (_isUp == true)
        {
            if(bIsLastLine == true)
                nItemIndex -= nPlayerTotalItemCount % 6;
            else
                nItemIndex -= nRowCount;
           
            nItemIndex = Mathf.Abs(nItemIndex);
            //6개의 슬롯을 아래로 옮겨야 한다.
            for (int i=0; i< 6; i++)
            {
                InventorySlot inventorySlot = this.gameObject.transform.GetChild(0).GetComponent<InventorySlot>();
                inventorySlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(inventorySlot.GetComponent<RectTransform>().anchoredPosition.x,
                     inventorySlot.GetComponent<RectTransform>().anchoredPosition.y - (((fSlotSizeX + fSpencingValueY) * nColCount)));
                inventorySlot.gameObject.transform.SetAsLastSibling();
                inventorySlot.gameObject.SetActive(true);
                inventorySlot.ActiveImage.gameObject.SetActive(false);

          

                //빈칸에 대한 처리.
                if (nItemIndex >= GameManager.Instance.GetPlayer().LIST_ITEM.Count)
                    inventorySlot.gameObject.SetActive(false);
                else
                {
                    //해당 슬롯의 정보 업데이트
                    inventorySlot.SetItemInfo_ALLItem(GameManager.Instance.GetPlayer().LIST_ITEM[nItemIndex]);

                    /*
                    if (inventorySlot.equipment.nIndex == inventoryItemInfo.nSelectedIndex)
                        inventorySlot.ActiveImage.gameObject.SetActive(true);
                    else
                        inventorySlot.ActiveImage.gameObject.SetActive(false);
                        */

                    Debug.Log("ItemIndex : " + nItemIndex + "ItemName : " + (GameManager.Instance.GetPlayer().LIST_ITEM[nItemIndex].strName));
                    nItemIndex++;
                }
               
            }
        }
        else
        {
            nItemIndex -= 1;
           // nItemIndex = Mathf.Abs(nItemIndex);
            //7개의 슬롯을 위로 옮겨야 한다.
            for (int i = 0  ; i < 6; i++)
            {
            
                InventorySlot inventorySlot = this.gameObject.transform.GetChild(this.gameObject.transform.GetChildCount() - 1).GetComponent<InventorySlot>();
                inventorySlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(inventorySlot.GetComponent<RectTransform>().anchoredPosition.x,
                     inventorySlot.GetComponent<RectTransform>().anchoredPosition.y + (((fSlotSizeX + fSpencingValueY) * nColCount)));
                inventorySlot.gameObject.transform.SetAsFirstSibling();
                inventorySlot.gameObject.SetActive(true);

                //해당 슬롯의 정보 업데이트
                inventorySlot.SetItemInfo_ALLItem(GameManager.Instance.GetPlayer().LIST_ITEM[nItemIndex]);
                Debug.Log("ItemIndex : " + nItemIndex + "ItemName : " + (GameManager.Instance.GetPlayer().LIST_ITEM[nItemIndex].strName));
                nItemIndex--;
            }
        }
    }

    public override void ContentsListInit()
    {
        Debug.Log("전체 아이템 탭 위치와 슬롯 초기화");
        nLowItemCount = 0;
        nHighItemCount = nInitItemSlotCount;
        fPrevHeight = 0;
        fLastValue = 0;
        fCurHeight = 0;
        nNextIndex = 0;
        bIsLastLine = false;
        bIsNeedScrolling = true;



        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -(rectTransform.sizeDelta.y / 2));

        int nEmptySlotStartIndex = 0;

        //ListIndex 갱신
        for (int i = 0; i < GameManager.Instance.GetPlayer().LIST_ITEM.Count; i++)
            GameManager.Instance.GetPlayer().LIST_ITEM[i].nListIndex = i;

        for (int i = 0; i < nInitItemSlotCount; i++)
        {
            //end 조건
            if (i >= GameManager.Instance.GetPlayer().LIST_ITEM.Count)
            {
                nHighItemCount = i - 1;
                nEmptySlotStartIndex = nHighItemCount + 1;
                break;
            }
            inventoryslots[i].gameObject.SetActive(true);
            inventoryslots[i].SetItemInfo_ALLItem(GameManager.Instance.GetPlayer().LIST_ITEM[i]);
            inventoryslots[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(inventoryslots[i].initSlotPosition.x, inventoryslots[i].initSlotPosition.y);
            inventoryslots[i].transform.SetAsLastSibling();
           
        }
        //기본 슬롯보다 적은경우 빈칸으로 처리.(SetActive(false))

        //비어있는 칸들에 대한 처리
        for (int i = nHighItemCount + 1; i < nInitItemSlotCount; i++)
        {
            inventoryslots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventorySlot>();
            inventoryslots[i].Init();
            inventoryslots[i].gameObject.SetActive(false);
        }



        fInitRectYValue = rectTransform.anchoredPosition.y;
        SetFirstItemInfoTo_ItemInfoPanel(inventoryslots[0]);
    }


    protected override void SortListInLevelOrder()
    {
        Debug.Log("전체 아이템 탭 레벨에 따른 정렬");
        nLowItemCount = 0;
        nHighItemCount = nInitItemSlotCount;
        fPrevHeight = 0;
        fLastValue = 0;
        fCurHeight = 0;
        nNextIndex = 0;
        bIsLastLine = false;
        bIsNeedScrolling = true;

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -(rectTransform.sizeDelta.y / 2));

        EquipmentSortInLevel.Sort(delegate (Equipment A, Equipment B)
        {
            if (A.nEnhance > B.nEnhance) return 1;
            else if (A.nEnhance > B.nEnhance) return -1;
            return 0;
        });

        //레벨에 따른 정렬을 위한 리스트 ;

        int nEmptySlotStartIndex = 0;

        for (int i = 0; i < nInitItemSlotCount; i++)
        {
            //end 조건
            if (i >= EquipmentSortInLevel.Count)
            {
                nHighItemCount = i - 1;
                nEmptySlotStartIndex = nHighItemCount + 1;
                break;
            }
            inventoryslots[i].gameObject.SetActive(true);
            inventoryslots[i].SetItemInfo_ALLItem(EquipmentSortInLevel[i]);
            inventoryslots[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(inventoryslots[i].initSlotPosition.x, inventoryslots[i].initSlotPosition.y);
            inventoryslots[i].transform.SetAsLastSibling();
        }
        //기본 슬롯보다 적은경우 빈칸으로 처리.(SetActive(false))

        //비어있는 칸들에 대한 처리
        for (int i = nHighItemCount + 1; i < nInitItemSlotCount; i++)
        {
            inventoryslots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventorySlot>();
            inventoryslots[i].Init();
            inventoryslots[i].gameObject.SetActive(false);
        }



        fInitRectYValue = rectTransform.anchoredPosition.y;
        SetFirstItemInfoTo_ItemInfoPanel(inventoryslots[0]);
    }
    protected override void SortListInEnhancedOrder()
    {
        Debug.Log("전체 아이템 탭 레벨에 따른 정렬");
        nLowItemCount = 0;
        nHighItemCount = nInitItemSlotCount;
        fPrevHeight = 0;
        fLastValue = 0;
        fCurHeight = 0;
        nNextIndex = 0;
        bIsLastLine = false;
        bIsNeedScrolling = true;

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -(rectTransform.sizeDelta.y / 2));

        EquipmentSortInLevel.Sort(delegate (Equipment A, Equipment B)
        {
            if (A.nEnhance > B.nEnhance) return 1;
            else if (A.nEnhance > B.nEnhance) return -1;
            return 0;
        });

        //레벨에 따른 정렬을 위한 리스트 ;

        int nEmptySlotStartIndex = 0;

        for (int i = 0; i < nInitItemSlotCount; i++)
        {
            //end 조건
            if (i >= EquipmentSortInLevel.Count)
            {
                nHighItemCount = i - 1;
                nEmptySlotStartIndex = nHighItemCount + 1;
                break;
            }
            inventoryslots[i].gameObject.SetActive(true);
            inventoryslots[i].SetItemInfo_ALLItem(EquipmentSortInLevel[i]);
            inventoryslots[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(inventoryslots[i].initSlotPosition.x, inventoryslots[i].initSlotPosition.y);
            inventoryslots[i].transform.SetAsLastSibling();
        }
        //기본 슬롯보다 적은경우 빈칸으로 처리.(SetActive(false))

        //비어있는 칸들에 대한 처리
        for (int i = nHighItemCount + 1; i < nInitItemSlotCount; i++)
        {
            inventoryslots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventorySlot>();
            inventoryslots[i].Init();
            inventoryslots[i].gameObject.SetActive(false);
        }



        fInitRectYValue = rectTransform.anchoredPosition.y;
        SetFirstItemInfoTo_ItemInfoPanel(inventoryslots[0]);
    }
}
