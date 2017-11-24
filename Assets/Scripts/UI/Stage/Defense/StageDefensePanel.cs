using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ReadOnlys;

public enum StageChapterInfo 
{
	Chapter01 = 0,
	Chapter02 ,
}

public class StageDefensePanel : MonoBehaviour 
{
	public Toggle chapter1Toggle;
	public Toggle chapter2Toggle;

	public GameObject chapter1Panel;
	public GameObject chapter2Panel;

	void Start()
	{
		
		
		chapter1Panel.SetActive (true);
		chapter2Panel.SetActive (false);

		chapter1Toggle.onValueChanged.AddListener((x)=>ActivePanel(StageChapterInfo.Chapter01));
		chapter2Toggle.onValueChanged.AddListener((x)=>ActivePanel(StageChapterInfo.Chapter02));
	}

	public void ActivePanel(StageChapterInfo _chapterIndex)
	{
		
		switch (_chapterIndex) 
		{
		case StageChapterInfo.Chapter01:
			Debug.Log ("Active Chapter01 Panel!!");

			chapter2Panel.SetActive (false);
			chapter1Panel.SetActive (true);

			break;
		case StageChapterInfo.Chapter02:
			Debug.Log ("Active Chapter01 Panel!!");

			chapter2Panel.SetActive (true);
			chapter1Panel.SetActive (false);

			break;

		default:
			break;
		}
	
	}



}
