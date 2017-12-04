using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreBattleSceneManager : MonoBehaviour {

	private const string sInfoText = "전투준비";
	public Transform canvas;

	public Button StartBattle_Button;

	void Start()
	{
		//GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_STAGE_PREBATTLE, canvas, sInfoText);
		//StartBattle_Button.onClick.AddListener(() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_BATTLE, ReadOnlys.E_SCENE_INDEX.E_STAGE_PREBATTLE, canvas));
	}

}
