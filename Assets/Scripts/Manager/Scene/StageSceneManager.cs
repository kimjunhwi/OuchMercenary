using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class StageSceneManager : MonoBehaviour
{
	private const string sInfoText = "스테이지 씬";
	public Transform canvas;

	void Start () 
	{
		GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_STAGE, canvas, sInfoText, null);
	}

}
