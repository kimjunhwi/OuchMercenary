using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Warrior : Player_Character {

	protected override void Awake ()
	{
		base.Awake ();

	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	public override void Setup (CharacterStats _charic,CharacterManager _charicManager, SkillManager _skillManager,BattleManager _BattleManager, E_Type _E_TYPE,Vector3 _vecPosition, int _nBatchIndex= 0)
	{
		base.Setup(_charic,_charicManager,_skillManager,_BattleManager, _E_TYPE,_vecPosition);		
	}


	public override void Attack ()
	{
		nActiveSkillIndex = CheckActiveAttack ();

		if (nActiveSkillIndex == (int)E_SKILL_TYPE.E_NONE) {

			Debug.Log ("BaseAttack");

			for (int nAttackCount = 0; nAttackCount < charicStats.basicSkill[0].nAttackNumber; nAttackCount++) {

				bool bIsCritical = false;

				//만약 공격 하려던 캐릭터가 죽어있다면
				if(targetCharacter.IsDead())
				{ 
					//다시 공격범위 안에 있는 캐릭터를 탐색
					ResetTargetCharacter(charicStats.m_fAttack_Range);

					//공격 범위안에 있는 캐릭이 null일 경우 종료
					if (targetCharacter == null)
						return;
				}

				nAttackCount++;

				//
				if(Random.Range(0,100) < charicStats.m_fCritical_Rating)
				{
					bIsCritical = true;

					for(int nIndex = 0; nIndex < charicStats.activeSkill.Count;nIndex++)
					{
						if(Random.Range(0,100) < charicStats.activeSkill[nIndex].m_fCriticalAttack_ActiveRating)
						{
							nActiveSkillIndex = nIndex;

							break;
						}
					}

					if(nActiveSkillIndex != (int)E_SKILL_TYPE.E_NONE)
					{

						PlayActiveSkill(nActiveSkillIndex,bIsCritical);

						continue;
					}
				}

				//1.사거리안에 들어온 캐릭터가 1개 이상일 경우
				//2.적 캐릭터에서 공격 범위 만큼의 리스트를 구한 후
				//3.그 안에 있는 캐릭터에게 공격 횟수 만큼 데미지를 준다.
				ArrayList targetLists = characterManager.FindTargetArea(this,targetCharacter,charicStats.basicSkill[0].fAttackArea);

				for (int nIndex = 0; nIndex < charicStats.basicSkill[0].nMaxTargetNumber; nIndex++) {
					//만약 타겟수가 공격 해야할 캐릭터 수보다 적을 경우 종료
					if (targetLists.Count <= nIndex) {
						break;
					}

					skillManager.BasicAttack (this, (Character)targetLists [nIndex],bIsCritical);
				}
			}
		} 
		else 
		{
			StartCoroutine(SkillCoolTime());

			if (charicStats.activeSkill [nActiveSkillIndex].m_fCastTime == 0) 
			{
				base.PlayActiveSkill (nActiveSkillIndex, false);

			} 
			else 
			{
				CheckCharacterState (E_CHARACTER_STATE.E_CAST);
			}
		}
	}
}
