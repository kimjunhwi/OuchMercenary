using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class MercenaryManageCharacterInfo : ToggleUIBase
{
	public GameObject CharacterTotalObj;

	public SimpleObjectPool MerManageHeroSlotPool;
	public MercenaryDispatchPanel mDispatchPanel;
	public E_PREPAREBATTLE_CHARCTERTYPE curCharacterType;

	//캐릭터슬롯 y크기
	private float fCSlotY_Value = 180;
	//캐릭터슬롯 빈공간
	private float fPlusSpacing_Value = 10f;
	//캐릭터 슬롯 y축 포지션
	private float fCSlotY_Pos = -90;
	//중복 방지 카운트
	int nToggleCount = 0;

	public RectTransform [] rectTransformList_Panel;
	public ScrollRect[] scrollRect_List;

	public bool [] isFirstInitCharacterSlot;
	public GameObject blockImage;

	public int childTotalCount =0;
	private int nCount =0;


	float fLastValue = 0f;
	float timer = 0f;

	//Scroll State
	public E_PRECHARACTERSLOT_STATE eSlotState;

	//내릴때 사용되는 변수들 
	public float [] fPrevHeight;
	public float [] fCurHeight;
	//내릴때 다음 인덱스를 저장하는 배열 (각각의 캐릭터 분류 마다)
	public int[] nNextIndex;

	void Start()
	{
		eSlotState = E_PRECHARACTERSLOT_STATE.E_PRECSLOT_STATE_SCROOLSTOP;

		MerManageHeroSlotPool.PreloadPool ();

		//캐릭터 정보 패널 초기화
		InitToggle();

		//StartCoroutine (UpdateCharacterList_Type ());
	}



	//각각의 캐릭터 패널들 초기화
	public void InitToggle()
	{
		//1.전체, 2. 지휘관, 3. 근거리, 4. 원거리, 5. 즐겨찾기
		ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL);
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER);
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE);
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE);
		//ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE);

		toggle[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].onValueChanged.AddListener(
			(x)=>ActivePanel(E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL));

		toggle[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].onValueChanged.AddListener(
			(x)=>ActivePanel(E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER));

		toggle[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].onValueChanged.AddListener(
			(x)=>ActivePanel(E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE));

		toggle[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].onValueChanged.AddListener(
			(x)=>ActivePanel(E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE));

		toggle[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].onValueChanged.AddListener(
			(x)=>ActivePanel(E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE));

		//togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (true);
		//togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
		//togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
		//togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
		//togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);

		Debug.Log("init Toggle!!");

		//CharacterTotalObj.SetActive (false);
	}

	//각각의 패널 활성화
	public override void ActivePanel<T> (T _chapterIndex) 
	{
		//중복 호출 방지
		if (nToggleCount == 1) {
			nToggleCount = 0;
			return;
		}


		var eType = Enum.Parse(typeof( ReadOnlys.E_PREPAREBATTLE_CHARCTERTYPE), _chapterIndex.ToString());
		nToggleCount++;


		switch ((E_PREPAREBATTLE_CHARCTERTYPE)eType) 
		{
		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL:

			curCharacterType = E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL;

			Debug.Log ("Active CharacterTotal Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);

			//캐릭터 슬롯 생성
			if (isFirstInitCharacterSlot [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL] == false) {
				RectTransform listRectTransform_Total = togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild (1)
					.transform.GetChild (0).GetComponent<RectTransform> ();

				CreateCharacterSlot (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL, listRectTransform_Total);
				isFirstInitCharacterSlot [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL] = true;
				//스크롤 되는 패널 
				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL] = listRectTransform_Total;
				//스크롤에 달려있는 스크롤 렉트
				scrollRect_List [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL] = listRectTransform_Total.gameObject.GetComponentInParent<ScrollRect> ();
			}

			break;

		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER:

			curCharacterType = E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER;

			Debug.Log ("Active Commander Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);


			//캐릭터 슬롯 생성
			if (isFirstInitCharacterSlot [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER] == false) 
			{

				RectTransform listRectTransform_Commander = togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].transform.GetChild (1)
					.transform.GetChild (0).GetComponent<RectTransform> ();

				CreateCharacterSlot (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER, listRectTransform_Commander);
				isFirstInitCharacterSlot[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER] = true;

				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER] = listRectTransform_Commander;
			}

			break;
		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE:

			curCharacterType = E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE;

			Debug.Log ("Active CharacterMelee Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);



			//캐릭터 슬롯 생성
			if (isFirstInitCharacterSlot [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE] == false) 
			{
				RectTransform listRectTransform_MELEE = togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].transform.GetChild (1)
					.transform.GetChild (0).GetComponent<RectTransform> ();

				CreateCharacterSlot (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE, listRectTransform_MELEE);
				isFirstInitCharacterSlot[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE] = true;

				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE] = listRectTransform_MELEE;
			}

			break;

		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE:

			curCharacterType = E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE;

			Debug.Log ("Active CharacterRange Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);


			//캐릭터 슬롯 생성
			if (isFirstInitCharacterSlot [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE] == false) 
			{
				RectTransform listRectTransform_Range = togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].transform.GetChild (1)
					.transform.GetChild (0).GetComponent<RectTransform> ();

				CreateCharacterSlot (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE, listRectTransform_Range);
				isFirstInitCharacterSlot[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE] = true;

				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE] = listRectTransform_Range;
			}

			break;
		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE:

			curCharacterType = E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE;

			Debug.Log ("Active CharacterFavorite Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (true);


			//캐릭터 슬롯 생성
			if (isFirstInitCharacterSlot [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE] == false) 
			{
				RectTransform listRectTransform_Favorite = togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].transform.GetChild (1)
					.transform.GetChild (0).GetComponent<RectTransform> ();

				CreateCharacterSlot (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE, listRectTransform_Favorite);
				isFirstInitCharacterSlot[(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE] = true;

				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE] = listRectTransform_Favorite;
			}

			break;
		default:
			break;
		}
	}

	//캐릭터 슬롯 생성 (초기에 4개 생성 이후 부터는 계산을 통한 업데이트)
	public void CreateCharacterSlot(E_PREPAREBATTLE_CHARCTERTYPE _typeIndex,  RectTransform listRectTransform)
	{

		//가지고 있는 캐릭터의 개수에 따라 전체 리스트 크기를 설정
		for (int iList = 0; iList < 7; iList++) {
			//Contents Adjust
			//전체 크기
			listRectTransform.sizeDelta = new Vector2 (listRectTransform.sizeDelta.x, listRectTransform.sizeDelta.y + fCSlotY_Value + fPlusSpacing_Value);
		}
		//Contents의 위치
		listRectTransform.anchoredPosition = new Vector2 (listRectTransform.anchoredPosition.x, -(listRectTransform.sizeDelta.y / 2));

		//50개일때 7개
		//해당 타입에 따라 캐릭터 슬롯을 만든다
		//전체 캐릭에 따라 개수를 다르게 생성 하도록 한다
		for (int i = 0; i < 7; i++) {		
			GameObject cSlot_Obj = MerManageHeroSlotPool.GetObject ();

			cSlot_Obj.name = "CharacterSlot" + i.ToString ();
			MerManageHeroSlot cSlot = cSlot_Obj.GetComponent<MerManageHeroSlot> ();
			//cSlot.cSlotRect = cSlot_Obj.GetComponent<RectTransform> ();


			//CharacterSlot Init
			//cSlot.characterBox_Image.sprite = GameManager.Instance.CharacterBoxImage_List [i];
			//cSlot.mDispatchPanel = mDispatchPanel;
			//cSlot.characterHealth_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [i].Health.ToString ();
			//cSlot.characterLevel_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [i].Levels.ToString ();
			//cSlot.characterName_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [i].C_JobNames;
			//cSlot.characterInfo = GameManager.Instance.GetPlayer ().LIST_CHARACTER [i];
			//cSlot.cInfoPanel = this;

			//전체 크기
			//listRectTransform.sizeDelta = new Vector2 (listRectTransform.sizeDelta.x, listRectTransform.sizeDelta.y + fCSlotY_Value + fPlusSpacing_Value);
			//Contents의 위치
			//listRectTransform.anchoredPosition = new Vector2 (listRectTransform.anchoredPosition.x, -(listRectTransform.sizeDelta.y / 2));


			//CharacterSlot 추가
			cSlot_Obj.transform.SetParent (togglePanel [(int)_typeIndex].transform.GetChild (1)
				.transform.GetChild (0), false);
			RectTransform cSlotObj_Rect = cSlot_Obj.GetComponent<RectTransform> ();

			//캐릭터 각각의 슬롯 위치(Vertical layout  없을 때)

			if (i == 0)
				cSlotObj_Rect.anchoredPosition = new Vector2 (0f, (fCSlotY_Pos * (i + 1)) - 10f);
			else {
				RectTransform nextElementTrans = listRectTransform.transform.GetChild (i - 1).gameObject.GetComponent<RectTransform> ();
				cSlotObj_Rect.anchoredPosition = new Vector2 (0f, ((nextElementTrans.anchoredPosition.y + (fCSlotY_Pos * 2)) - 10f));

			}

		}
		childTotalCount = 7;
	}

	//타입에 따른 캐릭터 리스트 갱신
	IEnumerator UpdateCharacterList_Type()
	{

		fLastValue = 0f;
		nCount = 4;
		float fInitRectYValue =  rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y ;


		yield return new WaitForSeconds (0.1f);

		while (true)
		{

			switch (curCharacterType) {

			case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL:

				if (fLastValue > scrollRect_List [(int)curCharacterType].verticalNormalizedPosition)
				{
					//Debug.Log ("올릴때!!!");
					eSlotState = E_PRECHARACTERSLOT_STATE.E_PRECSLOT_STATE_SCROOLUP;
					blockImage.SetActive (true);
					/*
					float value = (UpRectTransformListY_Value [(int)curCharacterType] + fUpRectChangeSlotCheckValue) - rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y;
				
					if (value < 0 ) {
						

						ResetCharacterSlot (true);
					}

					
					if (UpRectTransformListY_Value [(int)curCharacterType] + (fUpRectChangeSlotCheckValue * 2) < rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y) {
						blockImage.SetActive (true);
						ResetCharacterSlot (true);
					}
					if (UpRectTransformListY_Value [(int)curCharacterType] + (fUpRectChangeSlotCheckValue  * 3 )< rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y) {
						blockImage.SetActive (true);
						ResetCharacterSlot (true);
					}
					*/

					if (fInitRectYValue + (190 * (1 + nNextIndex[(int)curCharacterType] )) <= fCurHeight[(int)curCharacterType])
					{
						//Debug.Log ("SlotGoDown");
						//fcurSlotPos = cInfoPanel.rectTransformList_Panel [(int)cInfoPanel.curCharacterType].anchoredPosition.y;
						nNextIndex [(int)curCharacterType]++;
						fPrevHeight[(int)curCharacterType] = rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y;
						ResetCharacterSlot (true);
					}


				}
				else if(fLastValue == scrollRect_List [(int)curCharacterType].verticalNormalizedPosition)
				{
					blockImage.SetActive (false);
					eSlotState = E_PRECHARACTERSLOT_STATE.E_PRECSLOT_STATE_SCROOLSTOP;
					//Debug.Log("멈췄을 때");


				}
				else 
				{
					//Debug.Log ("내릴때!!!!");
					eSlotState = E_PRECHARACTERSLOT_STATE.E_PRECSLOT_STATE_SCROOLDOWN;
					blockImage.SetActive (true);



					if ( fCurHeight[(int)curCharacterType] < fPrevHeight[(int)curCharacterType])
					{
						//Debug.Log ("SlotGoUp");
						fPrevHeight [(int)curCharacterType] -= 190;
						nNextIndex [(int)curCharacterType]--;
						ResetCharacterSlot (false);
					}

					//맨위에 꺼에 대한 마무리 처리
					if (nNextIndex [(int)curCharacterType] == -1)
					{
						//Debug.Log ("맨 위에 꺼 생성");
						nNextIndex [(int)curCharacterType] = 0;
						ResetCharacterSlot (false);
						fPrevHeight [(int)curCharacterType] = fCurHeight [(int)curCharacterType];
						break;
					}

					/*
					if (DownRectTransformListY_Value [(int)curCharacterType] - 135f > rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y) {
						//Debug.Log ("올릴때 한개 지나감");
						//rectTrasformListY_Value [(int)curCharacterType] = rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y;

						ResetCharacterSlot (false);
					}
					*/
				}
				//yield return null;

				break;
			default:
				Debug.Log ("Error");
				break;
			}
			fLastValue = scrollRect_List[(int)curCharacterType].verticalNormalizedPosition;
			fCurHeight [(int)curCharacterType] = rectTransformList_Panel [(int)curCharacterType].anchoredPosition.y ;
			//Debug.Log (fLastValue);
			yield return null;


		}
	}

	public void ResetCharacterSlot(bool isUp)
	{
		nCount++;

		if (isUp) 
		{
			//전체 크기
			//rectTransformList_Panel [(int)curCharacterType].sizeDelta = new Vector2 (rectTransformList_Panel [(int)curCharacterType].sizeDelta.x,
			//	rectTransformList_Panel [(int)curCharacterType].sizeDelta.y + fCSlotY_Value + fPlusSpacing_Value);
			//Contents의 위치
			//rectTransformList_Panel [(int)curCharacterType].anchoredPosition = new Vector2 (rectTransformList_Panel [(int)curCharacterType].anchoredPosition.x,
			//	- (rectTransformList_Panel [(int)curCharacterType].sizeDelta.y / 2));

			//내려갈때 위에 0번째 차일드 맨 아래로 하고 다음 정보를 불러온다
			//맨 마지막 인덱스의 정보 + 1의 캐릭터 정보를 가져온다.
			CharacterSlot cSlotLast =
				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild (childTotalCount - 1).GetComponent<CharacterSlot> ();

			CharacterSlot cSlotFirst =
				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild (0).GetComponent<CharacterSlot> ();


			//float normalizePosition = rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetSiblingIndex() / rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild ;
			//scroll.verticalNormalizedPosition = 1-normalizePosition;
			//Debug.Log (scrollRect_List [(int)curCharacterType].verticalNormalizedPosition = 1 - normalizePosition);
			if (cSlotFirst.characterInfo.Index >= 46)
				return;

			//cSlot내용 조정
			cSlotFirst.characterHealth_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotLast.characterInfo.Index + 1].Health.ToString ();
			cSlotFirst.characterLevel_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotLast.characterInfo.Index + 1].Levels.ToString ();
			cSlotFirst.characterName_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotLast.characterInfo.Index + 1].C_JobNames;
			cSlotFirst.characterInfo = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotLast.characterInfo.Index + 1];


			cSlotFirst.gameObject.GetComponent<RectTransform> ().anchoredPosition = 
				new Vector2 (0f, ((cSlotLast.cSlotRect.anchoredPosition.y + (fCSlotY_Pos * 2)) - 10));
			cSlotFirst.gameObject.transform.SetAsLastSibling ();
			//Debug.Log ("마지막 인덱스 : " + cSlotLast.characterInfo.Index);

		}
		else
		{
			//내려갈때 위에 0번째 차일드 맨 아래로 하고 다음 정보를 불러온다
			//맨 마지막 인덱스의 정보 + 1의 캐릭터 정보를 가져온다.
			CharacterSlot cSlotLast =
				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild (childTotalCount - 1).GetComponent<CharacterSlot> ();

			CharacterSlot cSlotFirst =
				rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild (0).GetComponent<CharacterSlot> ();


			if (cSlotLast.characterInfo.Index < 7)
				return;



			cSlotLast.characterHealth_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1].Health.ToString ();
			cSlotLast.characterLevel_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1].Levels.ToString ();
			cSlotLast.characterName_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1].C_JobNames;
			cSlotLast.characterInfo = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1];

			cSlotLast.gameObject.GetComponent<RectTransform> ().anchoredPosition = 
				new Vector2 (0f, ((cSlotFirst.cSlotRect.anchoredPosition.y - (fCSlotY_Pos * 2)) + 10));

			cSlotLast.gameObject.transform.SetAsFirstSibling ();

		}
	}

	public void FirstElementReset()
	{
		CharacterSlot cSlotLast =
			rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild (childTotalCount - 1).GetComponent<CharacterSlot> ();

		CharacterSlot cSlotFirst =
			rectTransformList_Panel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].transform.GetChild (0).GetComponent<CharacterSlot> ();


		cSlotLast.characterHealth_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1].Health.ToString ();
		cSlotLast.characterLevel_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1].Levels.ToString ();
		cSlotLast.characterName_Text.text = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1].C_JobNames;
		cSlotLast.characterInfo = GameManager.Instance.GetPlayer ().LIST_CHARACTER [cSlotFirst.characterInfo.Index - 1];

		cSlotLast.gameObject.GetComponent<RectTransform> ().anchoredPosition = 
			new Vector2 (0f, ((cSlotFirst.cSlotRect.anchoredPosition.y - (fCSlotY_Pos * 2)) + 10));

		cSlotLast.gameObject.transform.SetAsFirstSibling ();
	}
}
