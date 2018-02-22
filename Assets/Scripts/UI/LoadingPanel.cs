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
                    ao = SceneManager.LoadSceneAsync((int)E_SCENE_INDEX.E_MENU);
#if UNITY_EDITOR
                    //이미지
                    //메인씬 이미지.
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/images", E_CHECK_ASSETDATA.PREFABIMAGES));
                    //용병고용                                                                            
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Employ/characters", E_CHECK_ASSETDATA.EMPLOY_CHARACTERS));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Employ/images", E_CHECK_ASSETDATA.EMPLOY_IMAGES));
                    //인벤토리(가방)                                                                      
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/weapon", E_CHECK_ASSETDATA.INVENTORY_ITEM_WEAPONIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/armor", E_CHECK_ASSETDATA.INVENTORY_ITEM_ARMORIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/glove", E_CHECK_ASSETDATA.INVENTORY_ITEM_GLOVEIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/accessory", E_CHECK_ASSETDATA.INVENTORY_ITEM_ACCESSORYIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/quality", E_CHECK_ASSETDATA.INVENTORY_ITEM_QUALITYIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/material", E_CHECK_ASSETDATA.INVENTORY_ITEM_MATERIAL));

                    //메인씬 프리팹들.
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/prefabs", E_CHECK_ASSETDATA.PREFABS));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/slot", E_CHECK_ASSETDATA.POST_SLOTS));


#elif UNITY_ANDROID
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/images", E_CHECK_ASSETDATA.PREFABIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/prefabs", E_CHECK_ASSETDATA.PREFABS));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/slot", E_CHECK_ASSETDATA.POST_SLOTS));
                    //용병고용                                                                            
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Employ/characters", E_CHECK_ASSETDATA.EMPLOY_CHARACTERS));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Employ/images", E_CHECK_ASSETDATA.EMPLOY_IMAGES));
                    //인벤토리(가방)                                                                      
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/weapon", E_CHECK_ASSETDATA.INVENTORY_ITEM_WEAPONIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/armor", E_CHECK_ASSETDATA.INVENTORY_ITEM_ARMORIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/glove", E_CHECK_ASSETDATA.INVENTORY_ITEM_GLOVEIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/accessory", E_CHECK_ASSETDATA.INVENTORY_ITEM_ACCESSORYIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/Inventory/item/images/quality", E_CHECK_ASSETDATA.INVENTORY_ITEM_QUALITYIMAGES));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/Inventory/item/images/material", E_CHECK_ASSETDATA.INVENTORY_ITEM_MATERIAL));

                                 //메인씬 프리팹들.
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/prefabs", E_CHECK_ASSETDATA.PREFABS));
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/slot", E_CHECK_ASSETDATA.POST_SLOTS));
#endif
                    break;
			case E_SCENE_INDEX.E_STAGE:
                  
                    ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_STAGE);


				    break;
			case E_SCENE_INDEX.E_BATTLE:
				    ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_BATTLE);
				    break;
			case E_SCENE_INDEX.E_MERMANAGE:
				    ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MERMANAGE);
#if UNITY_EDITOR
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/mercenarymanage/characterbox", E_CHECK_ASSETDATA.MERCENARY_CHARACTERBOX_IMAGES));
#elif UNITY_ANDROID
                    StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/mercenarymanage/characterbox", E_CHECK_ASSETDATA.MERCENARY_CHARACTERBOX_IMAGES));
#endif
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
#if UNITY_EDITOR
                    ////이미지
                    ////메인씬 이미지.
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/images", E_CHECK_ASSETDATA.PREFABIMAGES));
                    ////용병고용                                                                            
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Employ/characters", E_CHECK_ASSETDATA.EMPLOY_CHARACTERS));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Employ/images", E_CHECK_ASSETDATA.EMPLOY_IMAGES));
                    ////인벤토리(가방)                                                                      
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/weapon", E_CHECK_ASSETDATA.INVENTORY_ITEM_WEAPONIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/armor", E_CHECK_ASSETDATA.INVENTORY_ITEM_ARMORIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/glove", E_CHECK_ASSETDATA.INVENTORY_ITEM_GLOVEIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/accessory", E_CHECK_ASSETDATA.INVENTORY_ITEM_ACCESSORYIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/quality", E_CHECK_ASSETDATA.INVENTORY_ITEM_QUALITYIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/Inventory/item/images/material", E_CHECK_ASSETDATA.INVENTORY_ITEM_MATERIAL));

                    ////메인씬 프리팹들.
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/prefabs", E_CHECK_ASSETDATA.PREFABS));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.streamingAssetsPath + "/AssetBundles/MainScene/slot", E_CHECK_ASSETDATA.POST_SLOTS));
#elif UNITY_ANDROID
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/images", E_CHECK_ASSETDATA.PREFABIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/prefabs", E_CHECK_ASSETDATA.PREFABS));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/slot", E_CHECK_ASSETDATA.POST_SLOTS));
                    ////용병고용                                                                            
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Employ/characters", E_CHECK_ASSETDATA.EMPLOY_CHARACTERS));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Employ/images", E_CHECK_ASSETDATA.EMPLOY_IMAGES));
                    ////인벤토리(가방)                                                                      
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/weapon", E_CHECK_ASSETDATA.INVENTORY_ITEM_WEAPONIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/armor", E_CHECK_ASSETDATA.INVENTORY_ITEM_ARMORIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/glove", E_CHECK_ASSETDATA.INVENTORY_ITEM_GLOVEIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundle/MainScene/Inventory/item/images/accessory", E_CHECK_ASSETDATA.INVENTORY_ITEM_ACCESSORYIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/Inventory/item/images/quality", E_CHECK_ASSETDATA.INVENTORY_ITEM_QUALITYIMAGES));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/Inventory/item/images/material", E_CHECK_ASSETDATA.INVENTORY_ITEM_MATERIAL));

                                 //메인씬 프리팹들.
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/prefabs", E_CHECK_ASSETDATA.PREFABS));
                    //StartCoroutine(GameManager.Instance.LoadAssetBundle(Application.dataPath + "!assets/" + "/AssetBundles/MainScene/slot", E_CHECK_ASSETDATA.POST_SLOTS));
#endif
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
			if (ao.progress == 0.9f && GameManager.Instance.loadAssetIsDone[(int)E_CHECK_ASSETDATA.PREFABIMAGES] == true &&
                 GameManager.Instance.loadAssetIsDone[(int)E_CHECK_ASSETDATA.PREFABS] == true)
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
