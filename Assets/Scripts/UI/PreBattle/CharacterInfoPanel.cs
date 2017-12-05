﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ReadOnlys;

public class CharacterInfoPanel : ToggleUIBase 
{

	int nToggleCount = 0;
	void Start()
	{
		//캐릭터 정보 패널 초기화
		InitToggle();
	
	}

	public void InitToggle()
	{
		//1.전체, 2. 지휘관, 3. 근거리, 4. 원거리, 5. 즐겨찾기
		ActivePanel (E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL);

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

		Debug.Log("init Toggle!!");
	}

	public override void ActivePanel<T> (T _chapterIndex) 
	{
		if (nToggleCount == 1) {
			nToggleCount = 0;
			return;
		}
		var eType = Enum.Parse(typeof( ReadOnlys.E_PREPAREBATTLE_CHARCTERTYPE), _chapterIndex.ToString());
		nToggleCount++;
	
	
		switch ((E_PREPAREBATTLE_CHARCTERTYPE)eType) {
		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL:
			Debug.Log ("Active CharacterTotal Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);

			break;

		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER:
			Debug.Log ("Active Commander Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);

			break;
		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE:
			Debug.Log ("Active CharacterMelee Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);

			break;

		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE:
			Debug.Log ("Active CharacterRange Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (true);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (false);

			break;
		case E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE:
			Debug.Log ("Active CharacterFavorite Panel!!");
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE].SetActive (false);
			togglePanel [(int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE].SetActive (true);

			break;

		default:
			break;
		}

	}
}