using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ReadOnlys;

public class PreBattleSceneManager : MonoBehaviour {

	private const string sInfoText = "전투준비";
	public Transform canvas;

	public Button StartBattle_Button;

	public GameObject [] characterTapList_Objs;



	void Start()
	{
		//GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_STAGE_PREBATTLE, canvas, sInfoText);
		//StartBattle_Button.onClick.AddListener(() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_BATTLE, ReadOnlys.E_SCENE_INDEX.E_STAGE_PREBATTLE, canvas));
	}

	void SetCharacterSlot(E_PREPAREBATTLE_CHARCTERTYPE _index)
	{
		//처음에는 무조건 전체 탭에 있는 캐릭터들

	}
}
