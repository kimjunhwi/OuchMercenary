using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	protected SpriteRenderer spriteRenderer;
	protected Vector2 m_vecStartPosition;

	protected Vector2 m_vecEndPosition;

	protected readonly float m_fSpeed = 0.04f;

	protected virtual void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	// Use this for initialization
	
	//오브젝트 풀에 되돌리기 위한 오브젝트 풀과, 투사체가 발사될 시작지점과 도착지점, 속도 
	public virtual IEnumerator Shoot(SimpleObjectPool _simpleObjectPool,SkillManager _skillManager, Character _AttackCharacter, Character _TargetCharacter, bool _bIsCritical){yield return null;}
}
