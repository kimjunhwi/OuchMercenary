using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;
using UnityEngine.SceneManagement;

//메뉴 씬을 관리 하는 매니저
public class MenuManager : MonoBehaviour {

	public Button BattleScneButton;

	void Awake()
	{
		BattleScneButton.onClick.AddListener (MoveBattleScene );
	}

	void MoveBattleScene()
	{
		SceneManager.LoadScene ((int)E_SCENE_INDEX.E_BATTLE);
	}
}
