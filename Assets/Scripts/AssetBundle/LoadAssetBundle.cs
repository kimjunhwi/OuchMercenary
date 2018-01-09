using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class LoadAssetBundle : MonoBehaviour 
{
	//AssetBundleRequest request = loadedAssetBundleObject.LoadAssetAsync<GameObject>(assetName);

	void Start()
	{
		StartCoroutine (InstantiateObject());
	}

	IEnumerator DownLoadAssetBundle()
	{
		yield return null;
	}

	IEnumerator InstantiateObject()
	{

		string assetBundleDirectory_Editor = "Assets/AssetBundles";
		string assetBundleDirectory_Android = Application.streamingAssetsPath;


		//string uri = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;
		string uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/PreBattle/AssetBundles";

		//UnityEngine.Networking.UnityWebRequest request;
		/*
		UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri);
	
	
		//UnityEngine.Networking.UnityWebRequest.GetAssetBundle

		yield return request.Send();

		Debug.Log( request.error );

		AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

		GameObject cube = bundle.LoadAsset<GameObject>("Cube");
		//GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");

		Instantiate(cube);
		//Instantiate(sprite);
		*/

		UnityWebRequest request = UnityWebRequest.Get(uri); 
		yield return request.Send();

		// 에셋 번들을 저장할 경로의 폴더가 존재하지 않는다면 생성시킨다.
		if (!Directory.Exists(assetBundleDirectory_Android))
		{
			Directory.CreateDirectory(assetBundleDirectory_Android);
		}

		// 파일 입출력을 통해 받아온 에셋을 저장하는 과정
		//request.
		FileStream fs = new FileStream(assetBundleDirectory_Android + "/" + "AssetBundles", System.IO.FileMode.Create);
		fs.Write(request.downloadHandler.data, 0, (int)request.downloadedBytes);
		fs.Close();

		while (!File.Exists(assetBundleDirectory_Android + "/" + "assetBundles")) {
			Debug.Log ("다운중...");
			yield return null;
		}

		Debug.Log ("다운완료!");

		//File.WriteAllBytes(assetBundleDirectory + "/" + "character.unity3d", request.downloadHandler.data);
		//AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
		//GameObject cube = bundle.LoadAsset<GameObject>("Cube");

		/*
		 



		// 파일 저장 방법 1
		FileStream fs = new FileStream(assetBundleDirectory + "/" + "character.unity3d", System.IO.FileMode.Create);
		fs.Write(request.downloadHandler.data, 0, (int)request.downloadedBytes);
		fs.Close();

		// 파일 저장 방법 2
		File.WriteAllBytes(assetBundleDirectory + "/" + "character.unity3d", request.downloadHandler.data);



		// 파일 저장 방법 3
		for (ulong i = 0; i < request.downloadedBytes; i++)
		{
			fs.WriteByte(request.downloadHandler.data[i]);
			// 저장 진척도 표시


		}
		*/

	}
}
