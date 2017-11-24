﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using ReadOnlys;

using Facebook.Unity;

using Amazon;
using Amazon.CognitoSync;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using Amazon.CognitoSync.SyncManager;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
//using UnityEngine.SocialPlatforms.GameCenter;


//모든 데이터 및 로드, 세이브를 관리하는 클래스 
//어디서든 사용해야 하기 때문에 제네릭싱글톤을 통해 구현
public class GameManager : GenericMonoSingleton<GameManager>
{
	AsyncOperation ao;

	Player m_Player;

	public GameObject Root_ui;

	LoginManager loginManager;

	public bool bIsLoad = false;

	public List<DBBasicCharacter> lDbBasicCharacter = new List<DBBasicCharacter> ();

	//Scene 마다 있는 UpBar
	public Upbar upBar;

	public GameObject upBarHold_obj;


	public IEnumerator DataLoad()
    {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();

		#if UNITY_EDITOR

		loginManager.bIsSuccessed = true;



		#elif UNITY_IOS

		GameCenterPlatform.ShowDefaultAchievementCompletionBanner (true);

		Social.localUser.Authenticate ((bool success) => 
		{
		if(success)
		{
		loginManager.bIsSuccessed = true;
		}
		else
		{

		}
		});

		#endif

		m_Player = new Player ();

		m_Player.Init ();

        yield break;
    }

	public Player GetPlayer() { return m_Player; }

	//------------------------------------------------------------------------------------------------
	// 리소스 이미지 로드.
	public Texture2D GetResourceImage(string _imagename)
	{
		string imageName = _imagename; // "path/" + _imagename;
		Texture2D texture = (Texture2D)Resources.Load(imageName);
		return texture;
	}

	// GameObject 텍스처 변경.
	public void GameObject_set_texture(GameObject go, Texture2D _tx)
	{
		go.GetComponent<Renderer>().material.mainTexture = _tx;
		//go.GetComponent<Renderer>().material.color = new Color(1,1,1,1.0f);
	}

	// GameObject에 prefab을 로드
	public GameObject GameObject_from_prefab(string _prefab_name)
	{
		GameObject go = (GameObject)Instantiate(Resources.Load(_prefab_name, typeof(GameObject)));
		return go;
	}
	// GameObject에 prefab을 로드하여 어태치하기
	public GameObject GameObject_from_prefab(string _prefab_name, GameObject _parent)
	{
		GameObject go = (GameObject)Instantiate(Resources.Load(_prefab_name, typeof(GameObject)));
		if (_parent != null) go.transform.SetParent(_parent.transform);
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
		return go;
	}
	// GameObject의 UI Image 의 sprite 변경
	public void GameObject_set_image(GameObject go, string _path) //"image/test"
	{
		//GameObject go = GameObject.FindGameObjectWithTag("userTag1");
		Image myImage = go.GetComponent<Image>();
		myImage.sprite = Resources.Load<Sprite>(_path) as Sprite;
	}  

	// 객체의 이름을 통하여 자식 요소를 찾아서 리턴하는 함수 
	//UILabel _label = CGame.Instance.GameObject_get_child(obj, "_label").GetComponent<UILabel>();
	public GameObject GameObject_get_child(GameObject source, string strName)
	{
		Transform[] AllData = source.GetComponentsInChildren<Transform>(true); //비활성포함.

		GameObject target = null;

		foreach (Transform Obj in AllData)
		{
			if (Obj.name == strName)
			{
				target = Obj.gameObject;
				break;
			}
		}
		return target;
	}

	//객체에 붙은 Child를 제거
	public void GameObject_del_child(GameObject source)
	{
		Transform[] AllData = source.GetComponentsInChildren<Transform>(true); //비활성포함.
		foreach (Transform Obj in AllData)
		{
			if (Obj.gameObject != source) //자신 제외. 
			{
				Destroy(Obj.gameObject);
			}
		}
	}

	//------------------------------------------------------------------------------------------------
	//스크린 좌표
	public Vector3 GetScreenPosition()
	{
		Camera camera = Camera.main;
		Vector3 p = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
		return p;
	}

	//마우스 포인트에 타겟 피킹
	public GameObject GetRaycastObject()
	{
		RaycastHit hit;
		GameObject target = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다.
		//마우스 근처에 오브젝트가 있는지 확인
		if (true == (Physics.Raycast(ray.origin, ray.direction * 1000, out hit)))   
		{			
			target = hit.collider.gameObject; //있으면 오브젝트를 저장한다.
		}
		return target;
	}
	public Vector3 GetRaycastObjectPoint()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (true == (Physics.Raycast(ray.origin, ray.direction * 1000, out hit)))   
		{			
			return hit.point;
		}
		return Vector3.zero;
	}

	// 2D 유닛 히트처리 부분.  레이를 쏴서 처리합니다. 
	public GameObject GetRaycastObject2D()
	{
		GameObject target = null;

		Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0);
		if (hit.collider != null)
		{
			//Debug.Log (hit.collider.name);  //이 부분을 활성화 하면, 선택된 오브젝트의 이름이 찍혀 나옵니다. 
			target = hit.collider.gameObject;  //히트 된 게임 오브젝트를 타겟으로 지정
		}
		return target;
	}

	#region StaticGameObject
	public void InitUpbar()
	{
		if (upBar == null) 
		{	
			upBarHold_obj = GameObject.Find ("UpBarHold");
			DontDestroyOnLoad (upBarHold_obj);

			GameObject go = (GameObject)Instantiate (Resources.Load ("Prefabs/UpBar", typeof(GameObject)));
			upBar = go.GetComponent<Upbar> ();
			go.transform.SetParent (upBarHold_obj.transform);
			//홈버튼 추가
			upBar.Home_Button.onClick.AddListener (upBar.SetUpHomeButton);

			go.SetActive (false);
			DontDestroyOnLoad (go);
		}
		else 
		{
			upBar.gameObject.transform.position = new Vector3 (0f, 490f, 0f);
			upBar.gameObject.transform.SetParent(upBarHold_obj.transform);
			upBar.gameObject.SetActive (false);
		
		}
	}

	public void SetUpbar(E_SCENE_INDEX _sIndex ,Transform _trans, string _str)
	{
		upBar.gameObject.transform.SetParent (_trans, false);
		upBar.UpbarChangeInfo (_sIndex, _str);
	}

	public void LoadScene(E_SCENE_INDEX _sceneINdex)
	{
		GameManager.Instance.InitUpbar ();

		SceneManager.LoadScene ((int)_sceneINdex);
	}

	IEnumerator LoadingScene(){
		yield return new WaitForSeconds (0.3f);

		ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MENU);
		ao.allowSceneActivation = false;

		while (!ao.isDone) 
		{
			if (ao.progress == 0.9f) {
				//loginState_Text.text = "Press Button";

				if (Input.GetMouseButtonDown (0)) {
					yield return new WaitForSeconds (1.0f);
					ao.allowSceneActivation = true;
				}
			}
			yield return null;
		}
	}


	#endregion


	#region LoadTableInfo
	/*
    void Load_TableInfo_Weapon()
    {
        if (cWeaponInfo.Length != 0) return;

        string txtFilePath = "Weapon";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        CGameWeaponInfo[] kInfo = new CGameWeaponInfo[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameWeaponInfo();
            kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo[i - 1].strPath = Cells[1];
			kInfo[i - 1].strName = Cells[2];
			kInfo [i - 1].dMaxComplate = double.Parse (Cells [3]);
			kInfo [i - 1].dMinusRepair= double.Parse(Cells[4]);
			kInfo [i - 1].fMinusChargingWater = float.Parse (Cells [5]);
			kInfo [i - 1].dMinusCriticalDamage = double.Parse (Cells [6]);
			kInfo [i - 1].fMinusUseWater= float.Parse(Cells[7]);
			kInfo [i - 1].fMinusCriticalChance = float.Parse (Cells [8]);
			kInfo [i - 1].fMinusAccuracy= float.Parse(Cells[9]);
			kInfo[i - 1].dGold = double.Parse(Cells[10]);
			kInfo[i - 1].dHonor = double.Parse(Cells[11]);
			kInfo[i - 1].fLimitedTime = float.Parse(Cells[12]);
			kInfo[i - 1].nGrade = int.Parse(Cells[13]);
            kInfo[i - 1].WeaponSprite = ObjectCashing.Instance.LoadSpriteFromCache(kInfo[i - 1].strPath);
        }
        cWeaponInfo = kInfo;
    }
	*/
	#endregion

	#region SplitText

    TextAsset LoadTextAsset(string _txtFile)
    {
        TextAsset ta;
        ta = Resources.Load("Data/" + _txtFile) as TextAsset;
        return ta;
    }

    public List<string> LineSplit(string text)
    {
        //Console.WriteLine("LineSplit " + text.Length);

        char[] text_buff = text.ToCharArray();

        List<string> lines = new List<string>();

        int linenum = 0;
        bool makecell = false;

        StringBuilder sb = new StringBuilder("");

        for (int i = 0; i < text.Length; i++)
        {
            char c = text_buff[i];
            //int value = Convert.ToInt32(c); Console.WriteLine(String.Format("{0:x4}", value) + " " + c.ToString());

            if (c == '"')
            {
                char nc = text_buff[i + 1];
                if (nc == '"') { i++; } //next char
                else
                {
                    if (makecell == false) { makecell = true; c = nc; i++; } //next char
                    else { makecell = false; c = nc; i++; } //next char
                }
            }

            //0x0a : LF ( Line Feed : 다음줄로 캐럿을 이동 '\n')
            //0x0d : CR ( Carrage Return : 캐럿을 제일 처음으로 복귀 )			    
            if (c == '\n' && makecell == false)
            {
                char pc = text_buff[i - 1];
                if (pc != '\n')	//file end
                {
                    lines.Add(sb.ToString()); sb.Remove(0, sb.Length);
                    linenum++;
                }
            }
            else if (c == '\r' && makecell == false)
            {
            }
            else
            {
                sb.Append(c.ToString());
            }
        }

        return lines;
    }
	#endregion


    #region window popup

    // 윈도우 팝업 ---------------------------------------------------------------------------------------
	//CGame.Instance.Window_notice("213123 213123 ", rt => { if (rt == "0") print("notice");  });
	public void Window_notice(string _msg, System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("prefabs/Window_notice"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowNotice w = go.GetComponent<CWindowNotice>();
		w.Show(_msg, _callback);
	}

	public void Window_yesno(string strTitle, string strValue,Sprite _sprite,  System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_yesno"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowYesNo w = go.GetComponent<CWindowYesNo>();
		w.Show(strTitle,strValue,_sprite, _callback);
	}

	public void Window_Check(string strValue,System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_Check"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowCheck w = go.GetComponent<CWindowCheck>();
		w.Show(strValue, _callback);
	}

	public void Window_Goblin_yesno(string strTitle, string strValue,Sprite _spriteGoods, System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_Goblin_Buff_Yes_No"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
	}
    #endregion


}
