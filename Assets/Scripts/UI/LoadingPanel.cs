using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ReadOnlys;

public class LoadingPanel : MonoBehaviour 
{
	AsyncOperation ao;

	public Text tip_Text;
	public Text loading_Text;

	void Start () 
	{
		StartCoroutine (LoadingScene ());
	}

	public IEnumerator LoadingScene()
	{
		//Update랑 안겹치기 위해 (for Safe)
		yield return new WaitForSeconds (0.01f);

		if (GameManager.Instance.isPrevLoad == false) {

			switch (GameManager.Instance.nextSceneIndex) {
			case E_SCENE_INDEX.E_MENU:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MENU);
				break;
			case E_SCENE_INDEX.E_STAGE:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_STAGE);
				break;

			case E_SCENE_INDEX.E_BATTLE:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_BATTLE);
				break;
			case E_SCENE_INDEX.E_MERMANAGE:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MERMANAGE);
				break;
			case E_SCENE_INDEX.E_EMPLOYER:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_EMPLOYER);
				break;
		
			default:
				break;
			}
		} else {

			switch (GameManager.Instance.prevSceneIndex) {
			case E_SCENE_INDEX.E_MENU:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MENU);
				break;
			case E_SCENE_INDEX.E_STAGE:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_STAGE);
				break;

			case E_SCENE_INDEX.E_BATTLE:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_BATTLE);
				break;
			case E_SCENE_INDEX.E_MERMANAGE:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MERMANAGE);
				break;
			case E_SCENE_INDEX.E_EMPLOYER:
				ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_EMPLOYER);
				break;

			default:
				break;
			}
		}
		ao.allowSceneActivation = false;

		while (!ao.isDone) 
		{
			if (ao.progress == 0.9f) 
			{
				yield return new WaitForSeconds (0.05f);
				GameManager.Instance.isPrevLoad = false;
				ao.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
