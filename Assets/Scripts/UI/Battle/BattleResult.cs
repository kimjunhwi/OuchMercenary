using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;



public class BattleResult : MonoBehaviour {

	GameObject CharacterPanel;
	GameObject TakeItemPanel;

	Button HomeButton;
	Button ReStartButton;
	Button StageSelectButton;
	Button ExitButtonl;

	Transform takeParent;

	Text RubyText;
	Text GoldText;
	Text ResultText;

	List<ResultCharacterData> resultCharacterList = new List<ResultCharacterData>();

	BattleManager battleManager;

	void Awake()
	{
		

		//ExitButtonl.onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_BATTLE, true)); 
	}

	public void Init(BattleManager _battleManger)
	{
		battleManager = _battleManger;

		CharacterPanel = transform.GetChild (1).gameObject;
		TakeItemPanel = transform.GetChild (2).gameObject;

		takeParent = TakeItemPanel.transform.GetChild (1).transform;

		RubyText = TakeItemPanel.transform.GetChild (2).GetChild(1).GetComponent<Text> ();
		GoldText = TakeItemPanel.transform.GetChild (3).GetChild(1).GetComponent<Text> ();

		HomeButton = transform.GetChild (3).GetComponent<Button> ();
		ReStartButton = transform.GetChild (4).GetComponent<Button> ();
		StageSelectButton = transform.GetChild (5).GetComponent<Button> ();
		ExitButtonl = transform.GetChild (6).GetComponent<Button> ();

		ResultText = transform.GetChild (7).GetComponent<Text> ();

		HomeButton.onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_BATTLE, true)); 
		ReStartButton.onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_BATTLE, E_SCENE_INDEX.E_BATTLE, false)); 
		StageSelectButton.onClick.AddListener(() => GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_STAGE, E_SCENE_INDEX.E_BATTLE, true));
	}

	public void SetUp(bool _bIsWin)
	{
		resultCharacterList.Clear ();

		for (int nIndex = 0; nIndex < battleManager.playCharacter_List.Count; nIndex++) 
		{
			GameObject obj = Instantiate (battleManager.ResultCharacterUIPool.GetObject());
			obj.transform.SetParent (CharacterPanel.transform, false);

			ResultCharacterData resultData = obj.AddComponent<ResultCharacterData> ();
			resultData.SetUp(battleManager.playCharacter_List[nIndex].GetBasicStats());
		}

		gameObject.SetActive (true);
		CharacterPanel.SetActive (true);

		if (_bIsWin) 
		{
			TakeItemPanel.SetActive (true);

			GoldText.text = battleManager.stageData.nGold.ToString();

			ResultText.text = "Clear!!";

			for (int nIndex = 0; nIndex < CharacterPanel.transform.childCount; nIndex++) 
			{
				ResultCharacterData resultData = CharacterPanel.transform.GetChild (nIndex).GetComponent<ResultCharacterData> ();

				StartCoroutine(resultData.ExpChargeSlider(50));
			}

			string[] strEquipmentRates = battleManager.stageData.strEquipmentRates.Split(',');
			string[] strCharacterRates = battleManager.stageData.strCharacterDropRates.Split (',');
			string[] strMaterialRates = battleManager.stageData.strMaterialDropRates.Split (',');

//			for (int nIndex = 0; nIndex < strEquipmentRates.Length; nIndex++) 
//			{
//				int nRate = int.Parse (strEquipmentRates [nIndex]);
//
//				if (Random.Range (0, 100) < nRate) 
//				{
//					GameObject obj = Instantiate (battleManager.TakeItemPool.GetObject ());
//					obj.transform.SetParent (takeParent, false);
//
//					GameManager.Instance.
//				}
//			}



			for (int nIndex = 0; nIndex < strCharacterRates.Length; nIndex++) 
			{
				int nRate = int.Parse (strCharacterRates [nIndex]);

				nRate = 100;

				if (Random.Range (0, 100) < nRate) 
				{
					GameObject obj = Instantiate (battleManager.TakeItemPool.GetObject ());
					obj.transform.SetParent (takeParent, false);

					CharacterStats charicData = GameManager.Instance.SummonCharacter (1000);

					obj.transform.GetChild(0).GetComponent<Image>().sprite = ObjectCashing.Instance.LoadSpriteFromCache("UI/BoxImages/Character/" + charicData.m_sImage);

					battleManager.player.TEST_MY_HERO.Add (charicData);
				}
			}

			for (int nIndex = 0; nIndex < strMaterialRates.Length; nIndex++) 
			{
				int nRate = int.Parse (strMaterialRates [nIndex]);

				if (Random.Range (0, 100) < nRate) 
				{
					GameObject obj = Instantiate (battleManager.TakeItemPool.GetObject ());
					obj.transform.SetParent (takeParent, false);

					DBMaterialData materialData = GameManager.Instance.lDBMaterialData [int.Parse (battleManager.stageData.strMaterialDropIndexs.Split(',')[nIndex])];

					obj.transform.GetChild (0).GetComponent<Image> ().sprite = ObjectCashing.Instance.LoadSpriteFromCache(materialData.sImagePath);
				}
			}

		} 
		else 
		{
			ResultText.text = "Faield!!";

			for (int nIndex = 0; nIndex < CharacterPanel.transform.childCount; nIndex++) 
			{
				ResultCharacterData resultData = CharacterPanel.transform.GetChild (nIndex).GetComponent<ResultCharacterData> ();

				StartCoroutine(resultData.StaminaDwonSlider(20));
			}
		}

	}


}
