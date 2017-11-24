using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDefSceneManager : MonoBehaviour 
{
	private const string sInfoText = "스테이지 씬";
	public Transform canvas;


	void Start()	
	{
		GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_STAGE, canvas, sInfoText);



	}
}
