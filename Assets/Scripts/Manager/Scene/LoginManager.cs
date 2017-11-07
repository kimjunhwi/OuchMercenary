﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ReadOnlys;

public class LoginManager : MonoBehaviour {

	AsyncOperation ao;

	public Text loadingText;

	public bool bIsSuccessed = false;

	// Use this for initialization
	void Awake () {
		StartCoroutine( GameManager.Instance.DataLoad());
	}

	// Update is called once per frame
	void Update () 
	{
		if(bIsSuccessed == true)
		{
			Debug.Log("Successed");
			StartLoadScene();
			bIsSuccessed = false;
		}
	}
	private void StartLoadScene()
	{
		StartCoroutine (this.LoadScene());
	}

	IEnumerator LoadScene(){
		yield return new WaitForSeconds (0.3f);

		ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MENU);
		ao.allowSceneActivation = false;

		while (!ao.isDone) {
			if (ao.progress == 0.9f) {
				loadingText.text = "Press Button";

				if (Input.GetMouseButtonDown (0)) {
					yield return new WaitForSeconds (1.0f);
					ao.allowSceneActivation = true;
				}
			}
			yield return null;
		}
	}
}