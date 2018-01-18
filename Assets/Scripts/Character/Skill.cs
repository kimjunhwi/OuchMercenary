using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Skill : MonoBehaviour {

	float m_fTime = 0.0f;

	Vector3 m_vecPosition;

	Animator animator;

	Character skiller;
	ActiveSkill activeSkill;
	SimpleObjectPool objectPool;

	SkillManager skillManager;
	CharacterManager characterManager;

	E_Type Skiller_Type;

	ArrayList TargetList;

	void Awake()
	{
		animator = GetComponent<Animator> ();
	}

	public void SetUp(SimpleObjectPool _objectPool,Character _skiller, ActiveSkill _activeSkill, CharacterManager _characterManager,SkillManager _skillManager, float _fTime,Vector3 _vecPosition,E_Type _Type)
	{
		objectPool 							= _objectPool;

		skiller 							= _skiller;

		activeSkill 						= _activeSkill;

		skillManager 						= _skillManager;

		characterManager 					= _characterManager;

		m_fTime 							= _fTime;

		transform.position 					= _vecPosition;

		Skiller_Type 						= _Type;

		animator.runtimeAnimatorController	= ObjectCashing.Instance.LoadAnimationController("Animation/" + activeSkill.m_strEffectName);
	}
	
	public void PlaySkill()
	{
		if (m_fTime == 0f)
			StartCoroutine (NoneDelaySkill ());

		else
			StartCoroutine (DelaySkill ());
	}

	public IEnumerator NoneDelaySkill()
	{
		yield return new WaitForSeconds (0.1f);

		if (activeSkill.m_strAttackPriority == "enemy")
			TargetList = characterManager.FindEnemyDistanceArea (transform.position, activeSkill.m_fAttackRange, Skiller_Type);

		else
			TargetList = characterManager.FindFriendDistanceArea (transform.position, activeSkill.m_fAttackRange, Skiller_Type);

		string[] strActiveTypes = activeSkill.m_strSkillType.Split(',');

		for (int nIndex = 0; nIndex < strActiveTypes.Length; nIndex++) 
		{
			AllActiveSkillType skillData = GameManager.Instance.cAllActiveType[int.Parse(strActiveTypes[nIndex])];

			skillManager.ActiveSkill(skillData.nActiveType,activeSkill,skiller,TargetList,false);
		}
	}

	public IEnumerator DelaySkill()
	{
		yield return new WaitForSeconds (0.1f);

		string[] strActiveTypes = activeSkill.m_strSkillType.Split(',');



		

		while (m_fTime > 0) 
		{
			yield return new WaitForSeconds (1.0f);

			if (activeSkill.m_strAttackPriority == "enemy")
				TargetList = characterManager.FindEnemyDistanceArea (transform.position, activeSkill.m_fAttackRange, Skiller_Type);

			else
				TargetList = characterManager.FindFriendDistanceArea (transform.position, activeSkill.m_fAttackRange, Skiller_Type);

			m_fTime -= 1.0f;

			for (int nIndex = 0; nIndex < strActiveTypes.Length; nIndex++) 
			{
				AllActiveSkillType skillData = GameManager.Instance.cAllActiveType [int.Parse(strActiveTypes [nIndex])];

				skillManager.ActiveSkill(skillData.nActiveType,activeSkill,skiller,TargetList,false);

			}
		}
	}
}
