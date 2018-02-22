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
	BattleManager battleManager;
	CharacterManager characterManager;



	E_Type Skiller_Type;

	ArrayList TargetList;

	void Awake()
	{
		animator = GetComponent<Animator> ();

	}

	void Start()
	{
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();

		skillManager = battleManager.skillManager;
		characterManager = battleManager.characterManager;
		objectPool = battleManager.skillObjectPool;
	}

	public void SetUp(Character _skiller, ActiveSkill _activeSkill,Vector3 _vecPosition)
	{
		skiller 							= _skiller;

		activeSkill 						= _activeSkill;

		m_fTime 							= activeSkill.m_fDuration;

		transform.position 					= _vecPosition;

		animator.runtimeAnimatorController	= ObjectCashing.Instance.LoadAnimationController("Animation/Effect/" + activeSkill.m_strEffectName);

		PlaySkill ();
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

		yield return new WaitForSeconds (1.0f);

		objectPool.ReturnObject (gameObject);
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

		objectPool.ReturnObject (gameObject);
	}
}
