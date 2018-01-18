using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
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
	public E_SCENE_INDEX prevSceneIndex;		//이전 씬 인덱스
	public E_SCENE_INDEX nextSceneIndex;		//다음 씬 인덱스
	public Transform curSceneCanvas;			//현재씬의 캔버스
	public bool isPrevLoad;						//이전씬을 로드 할건지 아닌지

	Player m_Player;

	public LoginManager loginManager;

	public bool bIsLoad = false;

	public List<DBBasicCharacter> assasinList = new List<DBBasicCharacter>();
	public List<DBBasicCharacter> warriorList = new List<DBBasicCharacter>();
	public List<DBBasicCharacter> archerList = new List<DBBasicCharacter>();
	public List<DBBasicCharacter> knightList = new List<DBBasicCharacter>();
	public List<DBBasicCharacter> priestList = new List<DBBasicCharacter>();
	public List<DBBasicCharacter> wizzardList = new List<DBBasicCharacter> ();
	public List<DBBasicCharacter> commandList = new List<DBBasicCharacter>();

	//DB에서 게임내에 저장할 리스트 (기본 데이터들)
	public List<DBBasicCharacter> lDbBasicCharacter = new List<DBBasicCharacter>();
	public List<DBActiveSkill> lDbActiveSkill = new List<DBActiveSkill> ();
	public List<DBActiveSkillType> lDbActiveSkillType = new List<DBActiveSkillType> ();
	public List<DBPassiveSkill> lDbPassiveSkill = new List <DBPassiveSkill>();
	public List<DBPassiveSkillOptionIndex> lDbPassiveSkillOptionIndex = new List<DBPassiveSkillOptionIndex> ();
	public List<DBBasicSkill> lDbBasickill = new List<DBBasicSkill> ();

	public List<DBWeapon> lDbWeapon = new List<DBWeapon> ();
	public List<DBArmor> lDBArmor = new List<DBArmor> ();
	public List<DBGlove> lDBGlove = new List<DBGlove> ();
	public List<DBAccessory> lDBAccessory = new List<DBAccessory> ();
	public List<DBEquipment_RandomOption> lDBEquipmentRandomOption = new List<DBEquipment_RandomOption> ();
	public List<DBCraftMaterial> lDBCraftMaterial = new List<DBCraftMaterial> ();
	public List<DBBreakMaterial> lDBBreakMaterial = new List<DBBreakMaterial> ();
	//Stage
	public List<DBStageData> lDBStageData = new List<DBStageData> ();
	//FormationSkill
	public List<DBFormationSkill> lDBFomationSkill = new List<DBFormationSkill>();


    public List<string> lSceneIndex = new List<string>();

	public List<Sprite> CharacterBoxImage_List = new List<Sprite> ();
	//Scene 마다 있는 UpBar
	public Upbar upBar;
	public GameObject upBarHold_obj;			//UpBarHoldrer
	//용병관리창 hold 하는 obj
	public MercenaryManagePanel mercenaryManagePanel;
	public GameObject mercenaryManageHold_Obj;	//MercenaryManageHolder

	//PassiveSkill의 대한 데이터를 파싱
	public AllPassiveSkillData[] cAllPassiveSkill = null;
	public AllPassiveSkillOptionData[] cAllPassiveOption = null;
	public AllActiveSkillType[] cAllActiveType = null;

    //load한 애셋번들을 가지고 있는다.
    public List<AssetBundle> loadedAssetBundle = new List<AssetBundle>();
    public List<bool> loadAssetIsDone = new List<bool>();
    

	public List<Sprite> getSpriteArray = new List<Sprite>();
	public bool isSpriteDown;

    public GameObject prefabHold_Obj;

	public IEnumerator DataLoad()
    {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();
        prefabHold_Obj = GameObject.Find("PrefabHold");
        DontDestroyOnLoad(prefabHold_Obj);
        // Unicode Parsing ---------------------------------------------------------

        Load_TableInfo_AllActiveType();


		//패시브 스킬에 관한 정보들을 파싱
		Load_TableInfo_AllPassive();

		//패시브 스킬등의 옵션등을 위한 파싱
		Load_TableInfo_AllPassiveOption();

		//CharacterBox Image 


		// -------------------------------------------------------------------------

		#if UNITY_EDITOR

		loginManager.bIsSuccessed = true;


#elif UNITY_ANDROID



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

		SortJobIndex ();


        yield break;
    }

	public Player GetPlayer() { return m_Player; }

	void SortJobIndex()
	{
		for (int nIndex = 0; nIndex < lDbBasicCharacter.Count; nIndex++) 
		{
			switch (lDbBasicCharacter [nIndex].C_JobIndex) 
			{
			case (int)E_CHARACTER_TYPE.E_ASSASIN: 	assasinList.Add (lDbBasicCharacter[nIndex]); 	break;
			case (int)E_CHARACTER_TYPE.E_WARRIOR: 	warriorList.Add (lDbBasicCharacter [nIndex]); 	break;
			case (int)E_CHARACTER_TYPE.E_ARCHER: 	archerList.Add (lDbBasicCharacter [nIndex]);	break;
			case (int)E_CHARACTER_TYPE.E_WIZZARD: 	wizzardList.Add (lDbBasicCharacter [nIndex]); 	break;
			case (int)E_CHARACTER_TYPE.E_KNIGHT: 	knightList.Add (lDbBasicCharacter [nIndex]); 	break;
			case (int)E_CHARACTER_TYPE.E_PRIEST: 	priestList.Add (lDbBasicCharacter [nIndex]); 	break;
			case (int)E_CHARACTER_TYPE.E_COMMAND: 	commandList.Add (lDbBasicCharacter [nIndex]); 	break;
			}
		}
	}

	public List<DBBasicCharacter> GetJobList(int _E_TYPE)
	{
		switch (_E_TYPE) 
		{
		case (int)E_CHARACTER_TYPE.E_ASSASIN: 	return assasinList; 
		case (int)E_CHARACTER_TYPE.E_WARRIOR: 	return warriorList; 
		case (int)E_CHARACTER_TYPE.E_ARCHER: 	return archerList;	
		case (int)E_CHARACTER_TYPE.E_WIZZARD: 	return wizzardList; 
		case (int)E_CHARACTER_TYPE.E_KNIGHT: 	return knightList; 	
		case (int)E_CHARACTER_TYPE.E_PRIEST: 	return priestList; 	
		case (int)E_CHARACTER_TYPE.E_COMMAND: 	return commandList;	
		case (int)E_CHARACTER_TYPE.E_ALL:		return lDbBasicCharacter;
		}

		return null;
	}

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
			target = hit.collider.gameObject;  //히트 된 게임 오브젝트를 타겟으로 지정.
		}
		return target;
	}



	#region StaticGameObject
	public void CharacterBoxImageLoad(string _strPath)
	{
		Sprite image = (Sprite)Instantiate (Resources.Load (_strPath, typeof(Sprite)));
		CharacterBoxImage_List.Add (image);
	}

	//용병관리창 init
	public void InitMercenaryManage()
	{
		if (mercenaryManagePanel == null) {
			mercenaryManageHold_Obj = GameObject.Find ("MercenaryManageHold");
			DontDestroyOnLoad (mercenaryManageHold_Obj);

			GameObject go = (GameObject)Instantiate (Resources.Load ("Prefabs/UI/MercenaryManage/MercenaryManagePanel", typeof(GameObject)));

			mercenaryManagePanel = go.GetComponent<MercenaryManagePanel> ();

			go.transform.SetParent (mercenaryManageHold_Obj.transform);

			go.SetActive (false);
			DontDestroyOnLoad (go);
		} 
		else 
		{
			mercenaryManagePanel.gameObject.transform.position = new Vector3 (0f, 0f, 0f);
			mercenaryManagePanel.gameObject.transform.SetParent(mercenaryManageHold_Obj.transform);
			mercenaryManagePanel.gameObject.SetActive (false);
		}
	}

	public void SetUpMercenaryManage(Transform _trans)
	{
		mercenaryManagePanel.gameObject.transform.SetParent (_trans, false);
		mercenaryManagePanel.gameObject.transform.SetAsLastSibling ();
		mercenaryManagePanel.gameObject.SetActive (true);

		SetUpbar (mercenaryManagePanel.gameObject.transform);


	}

    #endregion

    #region Upbar
    public void InitUpbar()
	{
		if (upBar == null) 
		{	
			string assetBundleDirectory = "Assets/AssetBundles";
		
			upBarHold_obj = GameObject.Find ("UpBarHold");
			DontDestroyOnLoad (upBarHold_obj);
			if (System.IO.File.Exists ("Assets/AssetBundles/bundle/upbar"))
			{

				StartCoroutine (LoadFromMemoryAsync ("Assets/AssetBundles/bundle/upbar"));
				//StartCoroutine (LoadSpriteFromAssetBundel ("Assets/AssetBundles/mainsceneicon"));

                //StartCoroutine(LoadAnimationDataFromAssetBundel("Assets/AssetBundles/characteranim/archer"));
                //StartCoroutine(LoadAnimationDataFromAssetBundel("Assets/AssetBundles/characteranim/assassin"));

              

                Debug.Log ("경로에 해당 파일이 있음.");

				/*
				assetBundle = AssetBundle.LoadFromFile ("Assets/AssetBundles/ui_upbar");	



				GameObject go = assetBundle.LoadAsset<GameObject>("ui_upbar");

				//GameObject go = (GameObject)Instantiate (Resources.Load ("Prefabs/UpBar", typeof(GameObject)));

				upBar = go.GetComponent<Upbar> ();
				go.transform.SetParent (upBarHold_obj.transform);
				//홈버튼 추가
				upBar.Home_Button.onClick.AddListener (upBar.SetUpHomeButton);

				go.SetActive (false);
				DontDestroyOnLoad (go);

		*/
			} 

			else {
				Debug.Log ("경로에 해당 파일이 없음.");
			}
		}
		else 
		{
			upBar.gameObject.transform.position = new Vector3 (0f, 490f, 0f);
			upBar.gameObject.transform.SetParent(upBarHold_obj.transform);
			upBar.gameObject.SetActive (false);
			//StartCoroutine (StartInitUpbar ());	
		}
	}

    //해당 씬의 정보와 캔버스에 Upbar를 붙힌.
    public void SetUpbar(E_SCENE_INDEX _sIndex, Transform _trans, string _str, MainSceneManager _mainScene)
    {
        upBar.mainSceneManager = _mainScene;
        upBar.gameObject.transform.SetParent(_trans, false);

        //뒤에 더미도 같이 만든다 (로딩시 비는 것을 막기 위해)
        //GameObject upbar_Dummy = (GameObject)Instantiate (Resources.Load ("Prefabs/UpBar_Dummy", typeof(GameObject)));
        //upbar_Dummy.gameObject.transform.SetParent (_trans, false);
        //upbar_Dummy.GetComponent<Upbar> ().stageInfo_Text.text = _str;

        upBar.gameObject.transform.SetSiblingIndex(_trans.childCount - 2);
        upBar.UpbarChangeInfo(_sIndex, _str);
    }

    public void SetUpbar(Transform _trans)
    {
        upBar.gameObject.transform.SetParent(_trans, false);

        upBar.gameObject.transform.SetAsLastSibling();

        upBar.UpbarChangeInfo(E_SCENE_INDEX.E_MERMANAGE, "용병관리");
    }

    //해당 경로에 있는 에셋 번들 불러오기 (UpBar)
    IEnumerator LoadFromMemoryAsync(string _path)
    {

        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_path));

        yield return createRequest;

        AssetBundle bundle = createRequest.assetBundle;

        Sprite[] prefab = bundle.LoadAllAssets<Sprite>();

        for (int i = 0; i < prefab.Length; i++)
            Debug.Log(prefab[i].name);

        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/UpBar", typeof(GameObject)));

        upBar = go.GetComponent<Upbar>();

        upBar.upbarSprites = new Sprite[prefab.Length];



        for (int i = 0; i < prefab.Length; i++)
        {
            upBar.upbarSprites[i] = prefab[i];
        }

        upBar.SetSprite();

        go.transform.SetParent(upBarHold_obj.transform);
        //홈버튼 추가 및 함수 할당
        upBar.Home_Button.onClick.AddListener(upBar.SetUpHomeButton);

        go.SetActive(false);
        DontDestroyOnLoad(go);

        /*
		upBar = prefab.GetComponent<Upbar> ();
		prefab.transform.SetParent (upBarHold_obj.transform);
		//홈버튼 추가
		upBar.Home_Button.onClick.AddListener (upBar.SetUpHomeButton);

		prefab.SetActive (false);
		DontDestroyOnLoad (prefab);
			

		
		AssetBundleCreateRequest createRequest = AssetBundle.LoadFromFileAsync(_path);

		var myLoadedAssetBundle = createRequest.assetBundle;

		if (myLoadedAssetBundle == null)
		{
			Debug.Log("Failed to load AssetBundle!");
			yield break;
		}
		else
			Debug.Log("Successed to load AssetBundle!");

		var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Upbar");
		//Instantiate(prefab, Vector3.zero, Quaternion.identity);

		upBar = prefab.GetComponent<Upbar> ();
		prefab.transform.SetParent (upBarHold_obj.transform);
		//홈버튼 추가
		upBar.Home_Button.onClick.AddListener (upBar.SetUpHomeButton);

		prefab.SetActive (false);
		DontDestroyOnLoad (prefab);
		*/
    }

    #endregion

    #region LoadAssetBundle
    public IEnumerator LoadAssetBundle(string _path, E_CHECK_ASSETDATA _assetData)
    { 
        AssetBundle bundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(_path));

        var bundles = bundle.LoadAllAssets();

        //해당 번들을 리스트에 저장(해제시 사용)
        loadedAssetBundle.Add(bundle);

        switch (_assetData)
        {
            case E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABDATA:
                Debug.Log("MainScenePrefabData Load Complete");
                break;

            case E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABS:

                GameObject mainbackground = Instantiate(bundle.LoadAsset<GameObject>("MainBackground"));
                mainbackground.transform.SetParent(prefabHold_Obj.transform);
                mainbackground.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                mainbackground.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                GameObject activeButton = Instantiate(bundle.LoadAsset<GameObject>("ActiveButton"));
                activeButton.transform.SetParent(prefabHold_Obj.transform);
                activeButton.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                activeButton.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                GameObject mercenaryHealPanel = Instantiate(bundle.LoadAsset<GameObject>("MercenaryHealPanel"));
                mercenaryHealPanel.transform.SetParent(prefabHold_Obj.transform);
                mercenaryHealPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                mercenaryHealPanel.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                GameObject TrainningPanel = Instantiate(bundle.LoadAsset<GameObject>("TrainningPanel"));
                TrainningPanel.transform.SetParent(prefabHold_Obj.transform);
                TrainningPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                TrainningPanel.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                GameObject infoUI = Instantiate(bundle.LoadAsset<GameObject>("InfoUI"));
                infoUI.transform.SetParent(prefabHold_Obj.transform);
                infoUI.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                infoUI.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                GameObject fadePanel = Instantiate(bundle.LoadAsset<GameObject>("FadePanel"));
                fadePanel.transform.SetParent(prefabHold_Obj.transform);
                fadePanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                fadePanel.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                fadePanel.AddComponent<FadeInOut>();
                FadeInOut fadeInOut = fadePanel.GetComponent<FadeInOut>();
                fadeInOut.panel_Image = fadePanel.GetComponent<Image>();
                fadeInOut.fMultipleValue = 7f;

           

                Debug.Log("MainScenePrefabs Load Complete");
                break;


            default:
                break;


        }


        //로드가 다 됬는지 체크
        while (true)
        {
            switch (_assetData)
            {
                //메인씬 프리팹 데이터 로드
                case E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABDATA:
          
                    if (bundle != null)
                    {
                        loadAssetIsDone.Insert((int)E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABDATA, true);
                        Debug.Log("MainScenePrefabData Load Complete");
                        break;
                    }
                    break;
                //메인씬 프리팹 오브젝트 로드
                case E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABS:
                    if (bundle != null)
                    {
                        loadAssetIsDone.Insert((int)E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABS, true);
                        Debug.Log("MainScenePrefabs Load Complete");
                        break;
                    }
                    Debug.Log("MainScenePrefabs Load Complete");
                    break;
            }

            
            yield return null;
        }

    }

    #endregion


    #region LoadFromAssetBundle_Character
    public IEnumerator LoadSpriteFromAssetBundel(string _path)
    {
        isSpriteDown = false;
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_path));

        yield return createRequest;

        AssetBundle bundle = createRequest.assetBundle;

        Sprite[] prefab = bundle.LoadAllAssets<Sprite>();

        for (int i = 0; i < prefab.Length; i++)
        {
            //Debug.Log (prefab [i].name);
            getSpriteArray.Add(prefab[i]);

            if (i == prefab.Length - 1)
                isSpriteDown = true;
        }
        //"."을 기준으로 다시 정렬
        getSpriteArray.Sort(delegate (Sprite A, Sprite B)
        {
            if (int.Parse(A.name.Substring(0, A.name.IndexOf("."))) > int.Parse(B.name.Substring(0, B.name.IndexOf(".")))) return 1;
            else if (int.Parse(A.name.Substring(0, A.name.IndexOf("."))) < int.Parse(B.name.Substring(0, B.name.IndexOf(".")))) return -1;
            return 0;
        });

        for (int i = 0; i < getSpriteArray.Count; i++)
        {
            Debug.Log(getSpriteArray[i]);
        }
    }
    public IEnumerator LoadAnimationDataFromAssetBundel(string _path)
    {
        isSpriteDown = false;
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_path));

        yield return createRequest;

        AssetBundle bundle = createRequest.assetBundle;


        var bundles = bundle.LoadAllAssets();
    }



    public IEnumerator LoadAnimationFromAssetBundel(string _path)
    {
        isSpriteDown = false;
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_path));

        yield return createRequest;


        AssetBundle bundle = createRequest.assetBundle;

        

        GameObject go = Instantiate(bundle.LoadAsset<GameObject>("Archer_Basic"));

        go.transform.position = Vector3.zero;

        GameObject go2 = Instantiate(bundle.LoadAsset<GameObject>("Assassin_Basic"));

        go2.transform.position = Vector3.zero;

    }
    #endregion
    //메인씬의 프리팹에 대한 데이터 로드
    public IEnumerator LoadMainScenePrefabsDataFromAssetBundel(string _path)
    {
        AssetBundle bundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(_path));
        loadedAssetBundle.Add(bundle);

        var bundles = bundle.LoadAllAssets();
      
        //로드가 다 됬는지 체크
        while (true)
        {
            if (bundle != null)
            {
                loadAssetIsDone.Insert((int)E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABDATA, true);
                break;
            }
            yield return null;
        }


    }

    public IEnumerator LoadMainScenePrefabsFromAssetBundel(string _path)
    {
        isSpriteDown = false;
        //AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_path));

        //yield return createRequest;



        //AssetBundle bundle = createRequest.assetBundle;

        AssetBundle bundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(_path));

        loadedAssetBundle.Add(bundle);

        var bundles = bundle.LoadAllAssets();
        

        GameObject mainbackground = Instantiate(bundle.LoadAsset<GameObject>("MainBackground"));
        mainbackground.transform.SetParent(prefabHold_Obj.transform);
        mainbackground.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        mainbackground.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        GameObject activeButton = Instantiate(bundle.LoadAsset<GameObject>("ActiveButton"));
        activeButton.transform.SetParent(prefabHold_Obj.transform);
        activeButton.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        activeButton.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        GameObject mercenaryHealPanel = Instantiate(bundle.LoadAsset<GameObject>("MercenaryHealPanel"));
        mercenaryHealPanel.transform.SetParent(prefabHold_Obj.transform);
        mercenaryHealPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        mercenaryHealPanel.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        GameObject TrainningPanel = Instantiate(bundle.LoadAsset<GameObject>("TrainningPanel"));
        TrainningPanel.transform.SetParent(prefabHold_Obj.transform);
        TrainningPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        TrainningPanel.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        GameObject infoUI = Instantiate(bundle.LoadAsset<GameObject>("InfoUI"));
        infoUI.transform.SetParent(prefabHold_Obj.transform);
        infoUI.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        infoUI.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        GameObject fadePanel = Instantiate(bundle.LoadAsset<GameObject>("FadePanel"));
        fadePanel.transform.SetParent(prefabHold_Obj.transform);
        fadePanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        fadePanel.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        fadePanel.AddComponent<FadeInOut>();
        FadeInOut fadeInOut =  fadePanel.GetComponent<FadeInOut>();
        fadeInOut.panel_Image = fadePanel.GetComponent<Image>();
        fadeInOut.fMultipleValue = 7f;
        
        //로드가 다 됬는지 체크
        while (true)
        {
            if (bundle != null)
            {
                loadAssetIsDone.Insert((int)E_CHECK_ASSETDATA.E_CHECK_ASSETDATA_MAINSCENE_PREFABS, true);
                break;
            }
            yield return null;
        }
    }

    #region MainScenePrefabs

    #endregion

    public void InitLoadedAssetBundle()
    {
        AssetBundle bundle;
        for (int i = 0; i < loadedAssetBundle.Count; i++)
        {
            bundle = loadedAssetBundle[i];
            bundle.Unload(true);
            loadedAssetBundle.Remove(bundle) ;
        }
    }

    //현재 씬과 이전 씬을 설정하여 로딩 후 씬을 불러온다
    public void LoadScene(E_SCENE_INDEX _sceneIndex, E_SCENE_INDEX _prevSceneIndex, bool _isNoTip)
	{
       

        prevSceneIndex = _prevSceneIndex;
		nextSceneIndex = _sceneIndex;

		//메인 씬이 아니면 Upbar초기화 (싱글턴 오브젝트로 빼놓는다)
		if (_sceneIndex != E_SCENE_INDEX.E_MENU)
			GameManager.Instance.InitUpbar ();
		
		if (_isNoTip == true)
			SceneManager.LoadScene ((int)E_SCENE_INDEX.E_LOADING_SHORT);
		else
			SceneManager.LoadScene ((int)E_SCENE_INDEX.E_LOADING);
		
	}


    #region LoadTableInfo

    void Load_TableInfo_AllPassive()
	{
		if (cAllPassiveSkill != null) return;

		string txtFilePath = "AllPassive";
		TextAsset ta = LoadTextAsset(txtFilePath);
		List<string> line = LineSplit(ta.text);

		AllPassiveSkillData[] kInfo = new AllPassiveSkillData[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new AllPassiveSkillData();
			kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo[i - 1].nCharacterIndex = int.Parse(Cells[1]);
			kInfo[i - 1].nSkillClass = int.Parse(Cells[2]);
			kInfo[i - 1].nTier = int.Parse(Cells[3]);
			kInfo[i - 1].strJob = Cells[4];
			kInfo[i - 1].nAttribute = int.Parse(Cells[5]);
			kInfo[i - 1].nAttackType = int.Parse(Cells[6]);
			kInfo[i - 1].strOption_List = Cells[7];
		}
		cAllPassiveSkill = kInfo;
	}

	void Load_TableInfo_AllPassiveOption()
	{
		if (cAllPassiveOption != null) return;

		string txtFilePath = "AllPassiveOption";
		TextAsset ta = LoadTextAsset(txtFilePath);
		List<string> line = LineSplit(ta.text);

		AllPassiveSkillOptionData[] kInfo = new AllPassiveSkillOptionData[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new AllPassiveSkillOptionData();
			kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo[i - 1].nOptionIndex = int.Parse(Cells[1]);
			kInfo[i - 1].fValue = float.Parse(Cells[2]);
			kInfo[i - 1].fPlus = float.Parse(Cells[3]);
			kInfo[i - 1].nCalculate = int.Parse(Cells[4]);
			kInfo[i - 1].strExplain = Cells[5];
		}
		cAllPassiveOption = kInfo;
	}

	void Load_TableInfo_AllActiveType()
	{
		if (cAllActiveType != null) return;

		string txtFilePath = "ActiveSkillType";
		TextAsset ta = LoadTextAsset(txtFilePath);
		List<string> line = LineSplit(ta.text);

		AllActiveSkillType[] kInfo = new AllActiveSkillType[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new AllActiveSkillType();
			kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo[i - 1].nActiveType = int.Parse(Cells[1]);
			kInfo[i - 1].nTargetIndex = int.Parse(Cells[2]);
		}
		cAllActiveType = kInfo;
	}


	#endregion

	#region SplitText

    TextAsset LoadTextAsset(string _txtFile)
    {
        TextAsset ta;
        ta = Resources.Load("Unicode/" + _txtFile) as TextAsset;
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
		GameObject Root_ui = GameObject.Find("Canvas"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("prefabs/Window_POPUP/Window_notice"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowNotice w = go.GetComponent<CWindowNotice>();
		w.Show(_msg, _callback);
	}

	public void Window_yesno(string strTitle,  System.Action<string> _callback)
	{
		GameObject Root_ui = GameObject.Find("Canvas"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_POPUP/Window_yesno"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowYesNo w = go.GetComponent<CWindowYesNo>();
		w.Show(strTitle, _callback);
	}

	public void Window_Check(string strValue,System.Action<string> _callback)
	{
		GameObject Root_ui = GameObject.Find("Canvas"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_POPUP/Window_Check"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowCheck w = go.GetComponent<CWindowCheck>();
		w.Show(strValue, _callback);
	}

	public void Window_Goblin_yesno(string strTitle, string strValue,Sprite _spriteGoods, System.Action<string> _callback)
	{
		GameObject Root_ui = GameObject.Find("Canvas"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_POPUP/Window_Goblin_Buff_Yes_No"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
	}
    #endregion


	#region Character Summon

	public CharacterStats SummonCharacter(int _nIndex)
	{
		CharacterStats _summonCharacter;

		_summonCharacter = new CharacterStats (lDbBasicCharacter.Find (x => x.C_Index == _nIndex));

		DBBasicSkill basicSkill = GameManager.Instance.lDbBasickill.Find (x => x.nCharacterIndex == _summonCharacter.m_nIndex);

		_summonCharacter.basicSkill.Add (new BasicSkill (basicSkill));

		for (int nIndex = _summonCharacter.m_nTier; nIndex > 0; nIndex--) 
		{
			int nRandomIndex = 0;
			
			List<DBActiveSkill> active_List = GameManager.Instance.lDbActiveSkill.FindAll (x => x.m_nTier == nIndex);

			nRandomIndex = Random.Range (0, active_List.Count);

			_summonCharacter.activeSkill.Add (new ActiveSkill (active_List [nRandomIndex]));


			List<DBPassiveSkill> passive_List = GameManager.Instance.lDbPassiveSkill.FindAll (x => x.nCharacterIndex == -1 && nIndex == x.nTier);

			passive_List.AddRange (GameManager.Instance.lDbPassiveSkill.FindAll (x => x.nCharacterIndex == _summonCharacter.m_nIndex));

			nRandomIndex = Random.Range (0, passive_List.Count);

			PassiveSkill newPassiveSkill = new PassiveSkill (GameManager.Instance.lDbPassiveSkill [nRandomIndex]);

			//newPassiveSkill.optionData = new DBPassiveSkillOptionIndex(GameManager.Instance.lDbPassiveSkillOptionIndex[newPassiveSkill.nOptionIndex]);

			_summonCharacter.passiveSkill.Add (newPassiveSkill);
		}

		_summonCharacter.m_nBatchIndex = 5;

		return _summonCharacter;
	}
	#endregion

}

