using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MercenarySceneManager : MonoBehaviour
{
	private string sInfoText = "용병관리";
	public Transform canvas;

	void Start () 
	{
		//GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_MERMANAGE, canvas, sInfoText,null);
	}

}
