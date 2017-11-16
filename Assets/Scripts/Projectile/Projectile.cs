using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	protected Vector2 m_vecStartPosition;

	protected Vector2 m_vecEndPosition;

	protected float m_fSpeed;
	// Use this for initialization
	
	//오브젝트 풀에 되돌리기 위한 오브젝트 풀과, 투사체가 발사될 시작지점과 도착지점, 속도 
	public virtual IEnumerator Shoot(SimpleObjectPool _simpleObjectPool, Vector3 _StartPosition,Vector3 _endPosition,float _fSpeed){yield return null;}
}
