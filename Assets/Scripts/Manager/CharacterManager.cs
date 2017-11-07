using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class CharacterManager : MonoBehaviour {

	public ArrayList ARRAY_CHARIC = new ArrayList();

	public void Charic_add(Character _character)
	{
		ARRAY_CHARIC.Add(_character);
	}

	//remove
	public void Charic_remove(Character _charic)
	{        
		//Destroy(_charic.kGo);
		ARRAY_CHARIC.Remove(_charic);
	}    
	public void Charic_remove_all()
	{
		ARRAY_CHARIC.Clear();
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

	// 필요한 캐릭터를 반환 ------------------------------------------ 20170413
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
}
