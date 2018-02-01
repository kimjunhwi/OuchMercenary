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
        //현재 로드 되어있는 assetBundle을 해제 한다.
        GameManager.Instance.InitLoadedAssetBundle();
        //Update랑 안겹치기 위해 (for Safe)
        yield return new WaitForSeconds(0.01f);

        //이전씬으로 갈것인가 아닌가.
        if (GameManager.Instance.isPrevLoad == false)
        {
			switch (GameManager.Instance.nextSceneIndex)
            {
                //다음 씬으로가기 전에 해당 씬에 대한 에셋을 로드한다.
			case E_SCENE_INDEX.E_MENU:
				    ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MENU);
                    StartCoroutine( GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/images", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABDATA));
                    StartCoroutine( GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/prefabs", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABS));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/slot", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_SLOTS));

                    //용병고용 
                    StartCoroutine(GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/employ/characters", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_EMPLOY_CHARACTERS));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/employ/images", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_EMPLOY_IMAGES));

                    StartCoroutine(GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/employcharacter", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_EMPLOYCHARACTER));


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

        else
        {

			switch (GameManager.Instance.prevSceneIndex)
            {
			case E_SCENE_INDEX.E_MENU:
				    ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MENU);
                    StartCoroutine(GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/images", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABDATA));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle("Assets/AssetBundles/mainscene/prefabs", E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABS));
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


        while (true) 
		{
			if (ao.progress == 0.9f && GameManager.Instance.loadAssetIsDone[(int)E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABDATA] == true &&
                 GameManager.Instance.loadAssetIsDone[(int)E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABS] == true)

            {
				//yield return new WaitForSeconds (0.05f);
				GameManager.Instance.isPrevLoad = false;
				ao.allowSceneActivation = true;
                break;
			}
			yield return null;
		}
	}
}
