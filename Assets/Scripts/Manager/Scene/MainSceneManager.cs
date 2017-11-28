using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ReadOnlys;

public class MainSceneManager : MonoBehaviour 
{
	public Button Stage_Button;

	public Transform canvas;

	void Start () 
	{
		Stage_Button.onClick.AddListener(() => GameManager.Instance.LoadScene (ReadOnlys.E_SCENE_INDEX.E_STAGE , E_SCENE_INDEX.E_MENU , canvas));

		Debug.Log (GameManager.Instance.lDbBasicCharacter [0].Index.ToString () + " : " + GameManager.Instance.lDbBasicCharacter [0].C_Name);

	}
	

}
