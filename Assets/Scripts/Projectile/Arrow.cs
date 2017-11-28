﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile {

	public Vector3 m_vecMaxY;


	public Vector3 m_vecEndPosition;
	public Vector3 m_vecStartPosition;
	

	public float fTime = 0.0f;

	public readonly float m_fMaxSpeed = 5.0f;

	private void OnDisable()
	{
		m_vecMaxY = Vector3.zero;
		fTime = 0.0f;
	}

	  public override IEnumerator Shoot(SimpleObjectPool _simpleObjectPool,SkillManager _skillManager, Character _AttackCharacter, Character _TargetCharacter, bool _bIsCritical)
	  {
			m_vecEndPosition = _TargetCharacter.transform.position;
			m_vecStartPosition = _AttackCharacter.transform.position;

		  //벡터의 중앙 부분을 구함
		  Vector3 _vecMiddle = (m_vecStartPosition + m_vecEndPosition) * 0.5f;

		//중앙 지점에서 y값을 출발지점과 도착지점의 거리에 40%만큼 위로 더해줌
		  _vecMiddle.y += Vector3.Distance(m_vecStartPosition,m_vecEndPosition) * 0.4f;

			//처음 화살을 쐈을때 방향을 구한다.
			Vector3 vecDirection =  _vecMiddle - m_vecStartPosition;

			vecDirection.Normalize();

			float rot_Z = Mathf.Atan2(vecDirection.y, vecDirection.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.Euler(0f,0f,rot_Z);

			//최고 지점에 도달 했을때 목표 지점으로의 방향을 구함
			vecDirection = _TargetCharacter.transform.position - _vecMiddle;

			vecDirection.Normalize();

			//Mathf.Atan2 (float y,float x) 두 float 값의 탄젠트 값을 리턴
			//Mathf.Rad2Deg 라디안을 각도로 변환해주는 함수
			rot_Z = Mathf.Atan2(vecDirection.y, vecDirection.x) * Mathf.Rad2Deg;

			Quaternion rot = Quaternion.Euler(0f,0f,rot_Z);
		
		  while(true)
		  {
			fTime = Mathf.Min(1, fTime + Time.deltaTime + m_fSpeed);

			  transform.position = CalculateQuadraticBezierPoint(fTime,m_vecStartPosition,_vecMiddle,m_vecEndPosition);

			  //transform.rotation
			transform.rotation = Quaternion.Lerp(transform.rotation,rot,Time.deltaTime + 0.06f);
			  
			  if(Vector3.Distance(transform.position,m_vecEndPosition) < 0.1f)
			  		break;

			  yield return null;
		  }

			_skillManager.BasicAttack(_AttackCharacter,_TargetCharacter,_bIsCritical);

			_simpleObjectPool.ReturnObject (gameObject);
	
		  yield break;
	  }

	  public Vector3 CalculateQuadraticBezierPoint(float _fTime, Vector3 _vecStart,Vector3 _vecMiddle,Vector3 _vecEnd)
	  {
		 var omt = 1 - _fTime;

		  return _vecStart * omt * omt + 2f * _vecMiddle * omt * _fTime + _vecEnd * _fTime * _fTime;
	  }
}
