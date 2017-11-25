using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour 
{
	public Transform canvas;

	public Button Ready_Button;

	void Start()
	{
		Ready_Button.onClick.AddListener(() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_STAGE_PREBATTLE, 
			ReadOnlys.E_SCENE_INDEX.E_STAGE_DEFENSE, canvas));
	}


}
