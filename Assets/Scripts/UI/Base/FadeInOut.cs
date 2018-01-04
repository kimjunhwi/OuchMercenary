using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

//MainScene에서의 UI에 대한 FaedInOut
public class FadeInOut : MonoBehaviour
{
	public Image panel_Image;
	public float fMultipleValue;
	public bool bCompleteAlpha = false;
	public bool bNonAlpha = false;
	public bool isBack = false;
	public float fImageAlpha = 0f;

	public MainSceneManager mainSceneManager;

	public E_ACTIVEBUTTON eCurActiveButton;

	public IEnumerator FadeInOutOnce(E_ACTIVEBUTTON _button, bool _isBack)
	{
		fImageAlpha = 0f;
		eCurActiveButton = _button;

		//check 1
		bCompleteAlpha = false;
		//check 2
		bNonAlpha = false;
	
		isBack = _isBack;
		panel_Image.color = new Color (panel_Image.color.r, panel_Image.color.g, panel_Image.color.b, 0);

		this.gameObject.SetActive (true);
		fImageAlpha = panel_Image.color.a;

		StartCoroutine (ActivePanel ());

		while (true)
		{
			if (bCompleteAlpha == true && bNonAlpha == true) {
				this.gameObject.SetActive (false);
				Debug.Log ("FadeInOut 종료");
				break;
			}

			if (bCompleteAlpha == false && bNonAlpha == false && panel_Image.color.a != 255f) 
			{
				Debug.Log ("차는중" + fImageAlpha);
				if(panel_Image.color.a >= 1.0f)
					bCompleteAlpha = true;
				
				fImageAlpha += Time.deltaTime * fMultipleValue;
				panel_Image.color = new Color (panel_Image.color.r, panel_Image.color.g, panel_Image.color.b, fImageAlpha);
			}

			if (bCompleteAlpha == true && bNonAlpha == false && panel_Image.color.a != 0) 
			{
				Debug.Log ("빠지는중" + fImageAlpha);
				if (panel_Image.color.a <= 0f)
					bNonAlpha = true;
				
				fImageAlpha -= Time.deltaTime * fMultipleValue;
				panel_Image.color = new Color (panel_Image.color.r, panel_Image.color.g, panel_Image.color.b, fImageAlpha);
			}
			yield return null;
		}


	}

	public IEnumerator ActivePanel()
	{
		//한번만 체크 되게 하기 위한 bool 
		bool onceCheck = false;
		while (true) 
		{
			if (bNonAlpha == true)
				break;

			if (bCompleteAlpha == true) 
			{
				switch (eCurActiveButton) 
				{

				case E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING:


					if (isBack == false) 
					{
						if (onceCheck == false) {
							Debug.Log ("치료소 활성화!!");
							GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_STAGE_HEALING, mainSceneManager.canvas, "치료소", mainSceneManager);
							mainSceneManager.activeButtonPanel [(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING].SetActive (true);
							onceCheck = true;
						}
					} 
					else 
					{
						if(onceCheck == false)
						{
							Debug.Log ("치료소 비활성화!!");
							GameManager.Instance.InitUpbar ();
							mainSceneManager.activeButtonPanel [(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_HEALING].SetActive (false);
							onceCheck = true;
						}
					}

					break;
				case E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING:

					if (isBack == false) 
					{
						if (onceCheck == false) {
							Debug.Log ("훈련소 활성화!!");
							GameManager.Instance.SetUpbar (ReadOnlys.E_SCENE_INDEX.E_STAGE_TRAINNIG, mainSceneManager.canvas, "훈련소", mainSceneManager);
							mainSceneManager.activeButtonPanel [(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING].SetActive (true);
							onceCheck = true;	
						} 
					} 
					else 
					{
						if (onceCheck == false)	
						{
							Debug.Log ("훈련소 비활성화!!");
							GameManager.Instance.InitUpbar ();
							mainSceneManager.activeButtonPanel [(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_TRAINNING].SetActive (false);
							onceCheck = true;
						}
					}

				
					break;

				default:
					break;

				}
			}

			yield return null;
		}
	}
}
