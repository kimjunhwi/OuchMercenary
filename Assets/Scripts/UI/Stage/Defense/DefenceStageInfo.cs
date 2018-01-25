using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class DefenceStageInfo : MonoBehaviour {

	public int nStageIndex { get; set; }

	public Text BossNameText;
	public Image BossIconImage;
	public Text BossTribeText;
	public Text GoldText;
	public Text ExpText;
	public Button StartButton;

	void Awake()
	{
		StartButton.onClick.AddListener (BattleButton);
	}

	public void SetUpStage(int _nStageIndex)
	{
		nStageIndex = _nStageIndex;

		DBStageData stageData = GameManager.Instance.lDBStageData [nStageIndex];

		GoldText.text = stageData.nGold.ToString();
		ExpText.text = stageData.fExp.ToString ("F1");

		string[] strWaveEnemies = stageData.strEnemySpawnIndexs.Split ('/');
		string[] strEnemies = strWaveEnemies [strWaveEnemies.Length - 1].Split (',');

		int nBossCharacter = int.Parse (strEnemies [strEnemies.Length - 1]);

		DBBasicCharacter character = GameManager.Instance.lDbBasicCharacter [nBossCharacter];

		BossNameText.text = character.C_Name;
		BossTribeText.text = GameManager.Instance.GetConvertString(character.Tribe);
	}

	public void BattleButton()
	{

	}
}
