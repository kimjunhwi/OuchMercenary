using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ReadOnlys;

public class LogoScene : MonoBehaviour {

	public void NextScene()
	{
		SceneManager.LoadScene ((int)E_SCENE_INDEX.E_START);
	}
}
		