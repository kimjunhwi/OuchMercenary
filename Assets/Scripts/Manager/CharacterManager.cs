﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class CharacterManager : MonoBehaviour {

	IComparer compare = new SortunityLayerCompare();
	public ArrayList ARRAY_CHARIC = new ArrayList();

	int nSortingIndex = 0;

	public void Add(Character _character)
	{
		ARRAY_CHARIC.Add(_character);
	}

	//remove
	public void Remove(Character _charic)
	{        
		//Destroy(_charic.kGo);
		ARRAY_CHARIC.Remove(_charic);
	}    
	public void Remove_all()
	{
		ARRAY_CHARIC.Clear();
	}

	public int SearchTypeCount(E_Type _type)
	{
		int nCount = 0;

		foreach (Character charic in ARRAY_CHARIC) 
		{
			if (charic.IsDead () == true) 
				continue;

			if (charic.E_CHARIC_TYPE == _type) 
			{
				nCount++;	
			}
		}

		return nCount;
	}

	public void Actions()
	{
		foreach (Character obj in ARRAY_CHARIC) 
		{
			obj.ActionUpdate();
		}
	}

	public class SortunitClass
	{
		public float m_value1 { get; set; }
		public float m_value2 { get; set; }
		public Character m_charic { get; set; }
	}

	public class SortunitClassCompare : IComparer
	{
		public int Compare(object x, object y)
		{
			return Compare((SortunitClass)x, (SortunitClass)y);
		}
		public int Compare(SortunitClass x, SortunitClass y)
		{
			//return x.m_value.CompareTo( y.m_value ); // 작은 순서대로 정렬.
			//return y.m_value.CompareTo( x.m_value ); // 큰 순서대로 정렬.

			int v = x.m_value1.CompareTo(y.m_value1);   // 작은 순서
			if (v == 0)
				v = x.m_value2.CompareTo(y.m_value2);   //m_value 이 같을땐 	m_value2.
			return v;
		}
	}

	public class SortunityLayerCompare : IComparer
	{
		public int Compare(object x, object y)
		{
			return Compare((Character)x, (Character)y);
		}
		public int Compare(Character x, Character y)
		{
			//return x.m_value.CompareTo( y.m_value ); // 작은 순서대로 정렬.
			//return y.m_value.CompareTo( x.m_value ); // 큰 순서대로 정렬.

			return x.transform.position.y.CompareTo (y.transform.position.y);
		}
	}

	// 필요한 캐릭터를 반환 ------------------------------------------
	// 거리에 따른 캐릭터들중 반대편 캐릭들을 전부 반환한다.
	public ArrayList FindTarget(Character _charic, float _fDistance)
	{
		ArrayList TargetArray = new ArrayList();
		ArrayList SortArray = new ArrayList(); //조건에 맞추어 정렬.

		foreach (Character kCharic in ARRAY_CHARIC)
		{
			if (kCharic.IsDead() == true) continue;
			if (kCharic == _charic) continue; //자신제외
			if (kCharic.E_CHARIC_TYPE == _charic.E_CHARIC_TYPE) continue; //아군 제외.

			float fDistance = Vector3.Distance(kCharic.gameObject.transform.position, _charic.gameObject.transform.position);

			if(fDistance < _fDistance)
				SortArray.Add(new SortunitClass() { m_value1 = fDistance, m_charic = kCharic });
		}

		if (SortArray.Count > 0)
		{
			// 작은 순서대로 정렬.
			SortArray.Sort(new SortunitClassCompare());

			foreach (SortunitClass sort in SortArray)
			{
				TargetArray.Add(sort.m_charic);
			}
		}

		return TargetArray;
	}

	//공격 타겟 캐릭터를 기준으로 범위안에 있는 캐릭터리스트를 반환한다.
	public ArrayList FindTargetArea(Character _Attackcharic, Character _TargetCharic, float _fDistance)
	{
		ArrayList TargetArray = new ArrayList();
		ArrayList SortArray = new ArrayList(); //조건에 맞추어 정렬.

		foreach (Character kCharic in ARRAY_CHARIC)
		{
			if (kCharic.IsDead() == true) continue;
			if (kCharic == _Attackcharic) continue; //공격 캐릭 제외
			if (kCharic.E_CHARIC_TYPE == _Attackcharic.E_CHARIC_TYPE) continue; //아군 제외.

			float fDistance = Vector3.Distance(kCharic.gameObject.transform.position, _TargetCharic.gameObject.transform.position);

			if(fDistance <= _fDistance)
				SortArray.Add(new SortunitClass() { m_value1 = fDistance, m_charic = kCharic });
		}

		if (SortArray.Count > 0)
		{
			// 작은 순서대로 정렬.
			SortArray.Sort(new SortunitClassCompare());

			foreach (SortunitClass sort in SortArray)
			{
				TargetArray.Add(sort.m_charic);
			}
		}

		return TargetArray;
	}

	public ArrayList FindMyMinHealthTarget(Character _AttackCharacter,float _fDistance)
	{
		ArrayList TargetArray = new ArrayList();
		ArrayList SortArray = new ArrayList(); //조건에 맞추어 정렬.

		foreach (Character kCharic in ARRAY_CHARIC)
		{
			if (kCharic.IsDead() == true) continue;
			if (kCharic == _AttackCharacter) continue; //공격 캐릭 제외
			if (kCharic.E_CHARIC_TYPE != _AttackCharacter.E_CHARIC_TYPE) continue; //아군이 아닐 경우
			if(kCharic.GetStats().m_fHealth == kCharic.GetCurrentHealth()) continue; //체력이 최대치일 경우 리턴 

			SortArray.Add(new SortunitClass() { m_value1 = kCharic.GetCurrentHealth(), m_charic = kCharic });
		}

		if (SortArray.Count > 0)
		{
			// 작은 순서대로 정렬.
			SortArray.Sort(new SortunitClassCompare());

			foreach (SortunitClass sort in SortArray)
			{
				TargetArray.Add(sort.m_charic);
			}
		}

		return TargetArray;
	}

	public ArrayList FindMyCharacterArea(Character _StartCharic, Character _TargetCharic, float _fDistance)
	{
		ArrayList TargetArray = new ArrayList();
		ArrayList SortArray = new ArrayList(); //조건에 맞추어 정렬.

		foreach (Character kCharic in ARRAY_CHARIC)
		{
			if (kCharic.IsDead() == true) continue;
			if (kCharic == _StartCharic) continue; //공격 캐릭 제외
			if (kCharic.E_CHARIC_TYPE != _StartCharic.E_CHARIC_TYPE) continue; //아군이 아닐 경우

			float fDistance = Vector3.Distance(kCharic.gameObject.transform.position, _TargetCharic.gameObject.transform.position);

			if(fDistance < _fDistance)
				SortArray.Add(new SortunitClass() { m_value1 = fDistance, m_charic = kCharic });
		}

		if (SortArray.Count > 0)
		{
			// 작은 순서대로 정렬.
			SortArray.Sort(new SortunitClassCompare());

			foreach (SortunitClass sort in SortArray)
			{
				TargetArray.Add(sort.m_charic);
			}
		}

		return TargetArray;
	}


	//범위 스킬을 위함, _vecTargetPosition에 생성 되며 범위 안의 적 캐릭터를 반환
	public ArrayList FindEnemyDistanceArea(Vector3 _vecTargetPosition, float _fDistance, E_Type _skillerType)
	{
		ArrayList TargetArray = new ArrayList();
		ArrayList SortArray = new ArrayList(); //조건에 맞추어 정렬.

		foreach (Character kCharic in ARRAY_CHARIC)
		{
			if (kCharic.IsDead() == true) continue;
			if (kCharic.E_CHARIC_TYPE == _skillerType) continue; //아군이 아닐 경우

			float fDistance = Vector3.Distance(kCharic.gameObject.transform.position, _vecTargetPosition);

			if(fDistance < _fDistance)
				SortArray.Add(new SortunitClass() { m_value1 = fDistance, m_charic = kCharic });
		}

		ArraySort (SortArray, TargetArray);

		return TargetArray;
	}

	//범위 스킬을 위함, _vecTargetPosition에 생성 되며 범위 안의 아군 캐릭터를 반환
	public ArrayList FindFriendDistanceArea(Vector3 _vecTargetPosition, float _fDistance, E_Type _skillerType)
	{
		ArrayList TargetArray = new ArrayList();
		ArrayList SortArray = new ArrayList(); //조건에 맞추어 정렬.

		foreach (Character kCharic in ARRAY_CHARIC)
		{
			if (kCharic.IsDead() == true) continue;
			if (kCharic.E_CHARIC_TYPE != _skillerType) continue; //아군이 아닐 경우

			float fDistance = Vector3.Distance(kCharic.gameObject.transform.position, _vecTargetPosition);

			if(fDistance < _fDistance)
				SortArray.Add(new SortunitClass() { m_value1 = fDistance, m_charic = kCharic });
		}

		ArraySort (SortArray, TargetArray);

		return TargetArray;
	}
		

	public void ArraySort(ArrayList _list,ArrayList _addList)
	{
		if (_list.Count > 0)
		{
			// 작은 순서대로 정렬.
			_list.Sort(new SortunitClassCompare());

			foreach (SortunitClass sort in _list)
			{
				_addList.Add(sort.m_charic);
			}
		}
	}


	// 플레이어 캐릭터들의 (공격 or 수비) 모드를 바꾼다. ------------------------------------------ 
	public void PlayerCharacterChangeMode()
	{
		foreach (Character kCharic in ARRAY_CHARIC)
		{
			if (kCharic.E_CHARIC_TYPE == E_Type.E_Enemy) continue; //적 제외.

			//현재 활성화 중인 상태의 반대로 넣어줌
			kCharic.bIsMode = (kCharic.bIsMode) ? false : true; 

			kCharic.CheckCharacterState (E_CHARACTER_STATE.E_TARGET_MOVE);
		}
	}

	// 캐릭터의 레이어를 정렬한다 y축이 낮을수록 앞으로 ------------------------------------------ 
	public void SortingCharacterLayer()
	{
		if (ARRAY_CHARIC.Count != 0) 
		{
			//높은것을 정렬retur
			ARRAY_CHARIC.Sort(compare);

			nSortingIndex = ARRAY_CHARIC.Count;

			foreach (Character kCharic in ARRAY_CHARIC) 
			{
				kCharic.SortingLayer (nSortingIndex--);
			}
		}
	}
}
