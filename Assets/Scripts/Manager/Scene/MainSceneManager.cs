using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ReadOnlys;

public class MainSceneManager : MonoBehaviour 
{
	public Button Stage_Button;

	void Start () 
	{
		Stage_Button.onClick.AddListener (LoadScene);
	}
	
	public void LoadScene()
	{
		SceneManager.LoadScene ((int)E_SCENE_INDEX.E_STAGE);
	}
}
