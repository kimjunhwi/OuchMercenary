using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

//MainScene에서의 UI에 대한 FaedInOut
public class FadeInOut : MonoBehaviour
{
	public Image panel_Image;                   //실제로 적용되는 이미지
	public float fMultipleValue;                //배속 변수
	public bool bCompleteAlpha = false;         //투명화가 완료 됬는지 체크하는 변수
	public bool bNonAlpha = false;              //다시 검은 화면으로 갔을때 체크하는 변수
	public bool isBack = false;                 //뒤로가기 버튼으로 체크하는 변수
	public float fImageAlpha = 0f;              //이미지의 실제 알파값 임시로 담는 변수

	public MainSceneManager mainSceneManager;   //메인씬 매니져

	public E_ACTIVEBUTTON eCurActiveButton;     //현재 실행하는 버튼의 종류

	public IEnumerator FadeInOutOnce(E_ACTIVEBUTTON _button, bool _isBack)
	{
		fImageAlpha = 0f;
		eCurActiveButton = _button;

        //check 1 (투명화가 완료 됬는지 체크하는 변수)
        bCompleteAlpha = false;
        //check 2 (다시 검은 화면으로 갔을때 체크하는 변수)
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
				//Debug.Log ("FadeInOut 종료");
				break;
			}

			if (bCompleteAlpha == false && bNonAlpha == false && panel_Image.color.a != 255f) 
			{
				//Debug.Log ("차는중" + fImageAlpha);
				if(panel_Image.color.a >= 1.0f)
					bCompleteAlpha = true;
				
				fImageAlpha += Time.deltaTime * fMultipleValue;
				panel_Image.color = new Color (panel_Image.color.r, panel_Image.color.g, panel_Image.color.b, fImageAlpha);
			}

			if (bCompleteAlpha == true && bNonAlpha == false && panel_Image.color.a != 0) 
			{
				//Debug.Log ("빠지는중" + fImageAlpha);
				if (panel_Image.color.a <= 0f)
					bNonAlpha = true;
				
				fImageAlpha -= Time.deltaTime * fMultipleValue;
				panel_Image.color = new Color (panel_Image.color.r, panel_Image.color.g, panel_Image.color.b, fImageAlpha);
			}
			yield return null;
		}


	}
    //메인매뉴에서 특정 패널이 활성화가 될때 
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
                //치료소
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
                //훈련소
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

                    //용병고용소
                    case E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT:

                        if (isBack == false)
                        {
                            if (onceCheck == false)
                            {
                                Debug.Log("용병고용소 활성화!!");
                                GameManager.Instance.SetUpbar(ReadOnlys.E_SCENE_INDEX.E_EMPLOYER, mainSceneManager.canvas, "용병고용", mainSceneManager);
                                mainSceneManager.activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT].SetActive(true);
                                onceCheck = true;
                            }
                        }
                        else
                        {
                            if (onceCheck == false)
                            {
                                Debug.Log("용병고용소 비활성화!!");
                                GameManager.Instance.InitUpbar();
                                mainSceneManager.activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_EMPLOYMENT].SetActive(false);
                                onceCheck = true;
                            }
                        }


                        break;
                    //스테이지
                    case E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE:

                        if (isBack == false)
                        {
                            if (onceCheck == false)
                            {
                                Debug.Log("스테이지 활성화!!");
                                GameManager.Instance.SetUpbar(ReadOnlys.E_SCENE_INDEX.E_EMPLOYER, mainSceneManager.canvas, "스테이지", mainSceneManager);
                                mainSceneManager.activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE].SetActive(true);
                                onceCheck = true;
                            }
                        }
                        else
                        {
                            if (onceCheck == false)
                            {
                                Debug.Log("스테이지 비활성화!!");
                                GameManager.Instance.InitUpbar();
                                mainSceneManager.activeButtonPanel[(int)E_ACTIVEBUTTON.E_ACTIVEBUTTON_STAGE].SetActive(false);
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
