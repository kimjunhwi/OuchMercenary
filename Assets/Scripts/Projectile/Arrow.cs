using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile {

	public Vector3 gravity = new Vector3(0, -1000, 0);

	//Vector3 m_vecPower;
    //float m_fGround;

	private void OnDisable()
	{
		//m_vecPower = Vector3.zero;
		//m_fGround = 0.0f;
	}

	  public override IEnumerator Shoot(SimpleObjectPool _simpleObjectPool, Vector3 _StartPosition, Vector3 _endPosition, float _fSpeed)
	  {
//		  gameObject.transform.position = _StartPosition;
//
//		  var dir = _endPosition - transform.position;
//          var h = dir.y;
//          dir.y = 0;
//          var dist = dir.magnitude;
//          dir.y = dist;
//          dist += h;
//          var vel = Mathf.Sqrt(dist * gravity.magnitude);
//
//          m_vecPower = vel * dir.normalized;
//          m_fGround = _endPosition.y;
//
//
//			while(true)
//			{
//			if (Vector3.Distance (_StartPosition, _endPosition) < 0.5f)
//				break;
//
//				transform.Translate(m_vecPower * Time.deltaTime);
//
//				transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (_endPosition - transform.position), 10);
//
//				m_vecPower += gravity * Time.deltaTime;
//
//			yield return new WaitForSeconds(2.0f);
//			}
//
//			_simpleObjectPool.ReturnObject(gameObject);
//
			yield break;
	  }
}
