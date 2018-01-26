using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ReadOnlys;

public class PreBattleScenePanel : MonoBehaviour {

	private const string sInfoText = "전투준비";
	public Transform canvas;

	public GameObject InfoGameObject;


	public Button StartBattle_Button;

	void Start()
	{
	}

	public void SetCharacterSlot(int _index)
	{
		//처음에는 무조건 전체 탭에 있는 캐릭터들

	}
}
