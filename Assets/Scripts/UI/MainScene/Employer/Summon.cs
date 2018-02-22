using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;
using UnityEngine.EventSystems;

public class Summon : MonoBehaviour {

	//하급 올1티어

	//하급,중급,고급
	//70,25,5


	//중급,고급,특수
	//70,25,5

	public int nSummonTier;
	public int nSummonIndex;

	public int nOneTierPercent;
	public int nTwoTierPercent;
	public int nThreeTierPercent;
	public int nSpecialTierPercent;

	public int nCostGold;
	public int nCostJam;

	public E_CHARACTER_TYPE nType;

	public Button BuyButton;


	void Awake()
	{
		BuyButton = transform.GetChild (1).GetComponent<Button> ();

		BuyButton.onClick.AddListener (SummonCharacter);
	}

	public void SummonCharacter()
	{
		if (GameManager.Instance.GetPlayer ().GetGold () < nCostGold) 
		{
			GameManager.Instance.Window_notice ("골드가 부족합니다",null);
			return;
		}

		GameManager.Instance.Window_yesno ("정말 구매하시겠습니까", rt => {
			if (rt == "0") {
				if (GameManager.Instance.GetPlayer ().TEST_MY_HERO.Count >= 300) {
					GameManager.Instance.Window_notice ("용병 슬롯이 가득찼습니다.", null);
					return;
				}

				CharacterStats newCharacter = CreateCharacter(nSummonIndex);

				GameManager.Instance.GetPlayer().TEST_MY_HERO.Add(newCharacter);
			}
		});
			
	}


	CharacterStats CreateCharacter(int _nSummonIndex)
	{
		CharacterStats newCharacter;

		//임시(테스트를 위함

		newCharacter = GameManager.Instance.SummonCharacter (_nSummonIndex);



		return newCharacter;


		//주
//		if (Random.Range (0, 100) < nOneTierPercent) 			nSummonTier = (int)E_CHARACTER_TIER.E_ONE;
//		else if (Random.Range (0, 100) < nTwoTierPercent) 		nSummonTier = (int)E_CHARACTER_TIER.E_TWO;
//		else if (Random.Range (0, 100) < nThreeTierPercent) 	nSummonTier = (int)E_CHARACTER_TIER.E_THREE;
//		else if (Random.Range (0, 100) < nSpecialTierPercent) 	nSummonTier = (int)E_CHARACTER_TIER.E_FOUR;
//
//		List<DBBasicCharacter> charic_List = GameManager.Instance.GetJobList ((int)nType).FindAll (x => x.Tier <= nSummonTier);
//
//		if (charic_List.Count == 1) 
//		{
//			newCharacter = new CharacterStats(charic_List [0]);
//		} 
//		else 
//		{
//			int nIndex = Random.Range (0, charic_List.Count);
//
//			newCharacter = new CharacterStats (charic_List [nIndex]);
//		}
//
//		DBBasicSkill basicSkill = GameManager.Instance.lDbBasickill.Find (x => x.nCharacterIndex == newCharacter.m_nIndex);
//
//		newCharacter.basicSkill.Add (new BasicSkill (basicSkill));
//
//		for (int nIndex = newCharacter.m_nTier; nIndex > 0; nIndex--) 
//		{
//			int nRandomIndex = 0;
//			
//			List<DBActiveSkill> active_List = GameManager.Instance.lDbActiveSkill.FindAll (x => x.m_nTier == nIndex);
//
//			nRandomIndex = Random.Range (0, active_List.Count);
//
//			newCharacter.activeSkill.Add (new ActiveSkill (active_List [nRandomIndex]));
//		
//			List<DBPassiveSkill> passive_List = GameManager.Instance.lDbPassiveSkill.FindAll (x => x.nTier == nIndex);
//
//			nRandomIndex = Random.Range (0, passive_List.Count);
//		}
//
//		return newCharacter;
	}



}
