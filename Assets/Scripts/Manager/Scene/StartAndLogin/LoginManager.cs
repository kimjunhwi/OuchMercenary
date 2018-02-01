using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using ReadOnlys;

using Facebook.Unity;

using Amazon;
using Amazon.CognitoSync;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using Amazon.CognitoSync.SyncManager;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class LoginManager : MonoBehaviour 
{
	public Transform canvas;

	AsyncOperation ao;

	public Dataset playerInfo;  			//aws Cognito playerInfo

	public string sEmail;
	public string sNick;					//Auth or nonAuth Set Nick
	public string sGoogleID;	
	public string sFaceBookID;
	public E_LOGIN_PORVIDER_INDEX eLoginProviderIndex;

	private int nCurPlayerCount = 0;

	public bool bIsSuccessed = false;

	public Button GoogleLogin_Button;
	public Button FaceBookLogin_Button;
	public Button GuestLogin_Button;
	public Button nickConfirm_Button;

	public GameObject nickInputObj;

	public InputField nickInputField;


	public GameObject LoginCategory_Panel;


	private Player m_Player;

	//Loading Progress bar
	private const int ntotalBasicDataCount = 20;	//전체 데이터 갯수 
	public Slider curSlider;						//현재 진행도
	public Slider totalSlider;						//전체 진행도
	public Text progress_Text;						//전체 진행도 텍스트
	public int nCurProgressValue = 0;				//현재 진행도 값
    public GameObject progressBar_Panel;            //로딩바 패널

    //Aws에서 받는 리스트
    //PlayerDatas
    public DBPlayersCharacter_Personal_ForGet DBPlayersInfo_ForGet = new DBPlayersCharacter_Personal_ForGet();
    public bool bIsPlayerDataLoaded = false;
    public bool bIsPlayerDeleteLoaded = false;
    //Datas
	public List<DBBaiscCharacter_ForGet> lDBBasicCheacter_GetList = new List<DBBaiscCharacter_ForGet> ();
	public List<DBActiveSkill_ForGet> lDBActiveSkill_GetList = new List<DBActiveSkill_ForGet> ();
	public List<DBActiveSkillType_ForGet> lDBActiveSkillType_GetList = new List<DBActiveSkillType_ForGet> ();
	public List<DBPassiveSkill_ForGet> lDBPassiveSkill_GetList = new List<DBPassiveSkill_ForGet> ();
	public List<DBPassiveSkillOptionIndex_ForGet> lDBPassisveSkillOptionIndex_GetList = new List<DBPassiveSkillOptionIndex_ForGet> ();
	public List<DBbasicSkill_ForGet> lDBBasicSkill_GetList = new List<DBbasicSkill_ForGet> ();
	public List<DBEquipment_Weapon_ForGet> lDBEquipmentWeapon_GetList = new List<DBEquipment_Weapon_ForGet> ();
	public List<DBEquipment_Armor_ForGet> lDBEquipmentArmor_GetList = new List<DBEquipment_Armor_ForGet> ();
	public List<DBEquipment_Glove_ForGet> lDBEquipmentGlove_GetList = new List<DBEquipment_Glove_ForGet> ();
	public List<DBEquipment_Accessory_ForGet> lDBEquipmentAccessory_GetList = new List<DBEquipment_Accessory_ForGet> ();
	public List<DBEquipment_RandomOption_ForGet> lDBEquipmentRandomOption_GetList = new List<DBEquipment_RandomOption_ForGet> ();
	public List<DBStage_ForGet> lDBStageData_GetList = new List<DBStage_ForGet> ();
	public List<DBCraftMaterial_ForGet> lDBCraftMaterial_GetList = new List<DBCraftMaterial_ForGet> ();
	public List<DBBreakMaterial_ForGet> lDBBreakMaterial_GetList = new List<DBBreakMaterial_ForGet> ();
	public List<DBFormationSkill_ForGet> lDBFormationSkill_GetList = new List<DBFormationSkill_ForGet> ();
    public List<DBMaterialData_ForGet> lDBMaterialData_GetList = new List<DBMaterialData_ForGet>();
    public List<DBCalendarData_ForGet> lDBCalendar_GetList = new List<DBCalendarData_ForGet>();
    public List<DBCharacterTicketData_ForGet> lDBCharacterTicket_GetList = new List<DBCharacterTicketData_ForGet>();
    public List<DBWeaponTicketData_ForGet> lDBWeaponTicket_GetList = new List<DBWeaponTicketData_ForGet>();
    public List<DBEmployGachaData_ForGet> lDBEmployGacha_GetList = new List<DBEmployGachaData_ForGet>();




	//DB정보들의 각각의 개수
	private const int nCharacterCount = 59;
	private const int nActiveSkillCount = 144;
	private const int nActiveSkillTypeCount = 64;
	private const int nPassiveSkillCount = 164;
	private const int nPassiveSkillOptionIndexCount = 240;
	private const int nBasicSkillCount = 41;
	private const int nEquipmentWeaponCount = 108;
	private const int nEquipmentArmorCount = 108;
	private const int nEquipmentGloveCount = 108;
	private const int nEquipmentAccessoryCount = 12;
	private const int nEquipmentRandomOptionCount = 17;
	private const int nStageDataCount = 1;
	private const int nCraftMaterialCount = 3;
	private const int nBreakMaterialCount = 3;
	private const int nFormationSkillCount = 5;
    private const int nMaterialDataCount = 7;
    private const int nCalendarCount = 28;
    private const int nCharacterTicketCount = 5;
    private const int nWeaponTicketCount = 4;
    private const int nEmployGachaCount = 5;

    //DB정보 경로
    private const string sDBBasicCharacterInfoPath = "/BasicCharacter.data";
	private const string sDBActiveSkillPath = "/ActiveSkill.data";
	private const string sDBActiveSkilLTypePath = "/ActiveSkilLType.data";
	private const string sDBPassiveSkillPath = "/PassiveSkill.data";
	private const string sDBPassiveSkillOptionIndexPath = "/PassiveSkillOptionIndex.data";
	private const string sDBBasicSkillPath = "/BasicSkill.data";

	private const string sDBEquipmentWeaponPath = "/EquipmentWeapon.data";
	private const string sDBEquipmentArmorPath = "/EquipmentArmor.data";
	private const string sDBEquipmentGlovePath = "/EquipmentGlove.data";
	private const string sDBEquipmentAccessoryPath = "/EquipmentAccessory.data";
	private const string sDBEquipmentRandomOptionPath = "/EquipmentRandomOption.data";

	private const string sDBStageDataPath = "/Stage.data";
	private const string sDBCraftMaterialPath = "/CraftMaterial.data";
	private const string sDBBreakMaterialPath = "/BreakMaterial.data";
	private const string sDBFormationSkillPath = "/FormationSkill.data";
    private const string sDBMaterialDataPath = "/MaterialData.data";

    private const string sDBCalendarPath = "/CalendarData.data"; 
    private const string sDBCharacterTickectPath = "/CharacterTicket.data";
    private const string sDBWeaponTicketPath = "/WeaponTicket.data";
    private const string sDBEmployGachaPath = "/EmployGacha.data";



    //모든 데이터가 로드 됬는지 않됬는지
    bool bIsFinishLoadDate = false;

	//정보 불러올때 띄우는 텍스트
	public Text loginState_Text;
	//playerpref 필요 처음 앱을 시작했는지 아닌지


	//Google
	private string AccessTokken_GP;
	//Identity ID pool 
	private string IdentityPoolId = "ap-northeast-2:7dd3a4a2-9eb4-40e9-839c-bf362be95280";

	//지역 설정 변수
	private string Region = RegionEndpoint.APNortheast2.SystemName;
    private bool bIsNickInput;


	private RegionEndpoint _Region
	{
		get { return RegionEndpoint.GetBySystemName(Region); }
	}

	private CognitoAWSCredentials _credentials;

	private CognitoAWSCredentials Credentials
	{
		get
		{
			if (_credentials == null)
				_credentials = new CognitoAWSCredentials(IdentityPoolId, _Region);
			return _credentials;
		}
	}


	private static IAmazonDynamoDB _ddbClient;	

	private IAmazonDynamoDB Client
	{
		get
		{
			if (_ddbClient == null)
			{
				_ddbClient = new AmazonDynamoDBClient(Credentials, _Region);
			}

			return _ddbClient;
		}
	}

	private DynamoDBContext _context;

	private DynamoDBContext Context
	{
		get
		{
			if(_context == null)
				_context = new DynamoDBContext(_ddbClient);

			return _context;
		}
	}



	private CognitoSyncManager _syncManager;

	private CognitoSyncManager SyncManager
	{
		get
		{
			if (_syncManager == null)
			{
				_syncManager = new CognitoSyncManager(Credentials, new AmazonCognitoSyncConfig { RegionEndpoint = _Region });
			}
			return _syncManager;
		}
	}



	// Use this for initialization
	void Awake () 
	{
		PlayerPrefs.DeleteKey ("FirstAppActive");
		//Login(data sync) -> Version Check(static data download) -> GameStart
		LoginManager_Init();
		//Init SetUpBar
		GameManager.Instance.InitUpbar ();
		//Init 용병관리 Instance
		//GameManager.Instance.InitMercenaryManage();

		totalSlider.maxValue = ntotalBasicDataCount;
		progress_Text.text = nCurProgressValue + " / " + ntotalBasicDataCount;

		#if UNITY_EDITOR
		//처음 실행과 아닐때의 분기
		if (PlayerPrefs.HasKey ("FirstAppActive"))
		{
			//db에서 데이터 읽어오기
			//다운되어있는  prefab에서 가져오기
			Debug.Log ("LoginSequence");
		}
		else 
		{
			PlayerPrefs.SetString ("FirstAppActive", "True");
            //CharacterDBLoadAndPutOperationTest ();

            //StartCoroutine(GameManager.Instance.LoadSceneFromAssetBundle("Assets/AssetBundles/scenes"));

            //StartCoroutine(DBLoadCharacter ());
            //LoadCharacter(0);
            StartCoroutine(StartLoadBasicDataSequence());
            StartCoroutine(CheckBasicDataLoadEnd());

        }
    #elif UNITY_ANDROID

		if (PlayerPrefs.HasKey ("FirstAppActive"))
		{
		//db에서 데이터 읽어오기
		//다운되어있는  prefab에서 가져오기
		Debug.Log ("LoginSequence");
		}
		else 
		{
		PlayerPrefs.SetString ("FirstAppActive", "True");
		//CharacterDBLoadAndPutOperationTest ();

		StartCoroutine(StartLoadBasicDataSequence());
		StartCoroutine(CheckBasicDataLoadEnd());

		}
      #endif

    }

    // Update is called once per frame
    void Update () 
	{
		if(bIsSuccessed == true)
		{
			Debug.Log("Successed");
			//StartLoadScene();
			bIsSuccessed = false;
		}
	}


	//Google, FaceBook 등등 로그인시 초기화 할것들
	void LoginManager_Init()
	{
        //Google로그인 버튼 초기화
		GoogleLogin_Button.onClick.AddListener (GoogleLogin);

        //닉확정 버튼 초기화
        nickConfirm_Button.onClick.AddListener(StartDataSetSave);
        //FaceBookLogin_Button.onClick.AddListener (FaceBookLogin);

        //GuestLogin_Button.onClick.AddListener();

        UnityInitializer.AttachToGameObject(this.gameObject);
		AWSConfigs.LoggingConfig.LogTo = LoggingOptions.UnityLogger;
		//WWW Error 해결코드 2017ver.
		Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;


		//Debug.Log (Credentials);
		//Editor
		//FaceBook 초기화
		FB.Init ();		
		//Google
		//EnableGameSave
		// enables saving game progress.
		PlayGamesClientConfiguration configEditor = new PlayGamesClientConfiguration.Builder().RequestEmail().RequestServerAuthCode(false).RequestIdToken().Build();
		PlayGamesPlatform.InitializeInstance(configEditor);
		//GoogleLogin Active
		PlayGamesPlatform.Activate();
		 
		Debug.Log (Credentials.IdentityPoolId.ToString ());


		_ddbClient = Client;


		_context = Context;

        

        //playerInfo.Put ("Nick", "Smaet");
        //playerInfo.Put ("Provider", "Google");
        //playerInfo.Put ("Email", "dkan56@naver.com");

        //playerInfo.SynchronizeAsync ();



    }

	IEnumerator LoadScene()
    {
		yield return new WaitForSeconds (0.3f);

		ao = SceneManager.LoadSceneAsync ((int)E_SCENE_INDEX.E_MENU);
		ao.allowSceneActivation = false;

		while (!ao.isDone) 
		{
			if (ao.progress == 0.9f) {
				loginState_Text.text = "Press Button";

				if (Input.GetMouseButtonDown (0)) {
					yield return new WaitForSeconds (1.0f);
					ao.allowSceneActivation = true;
				}
			}
			yield return null;
		}
	}


    //Playerdata 업데이트
    public void StartUpdatePlayerData()
    {
      

        StartCoroutine(UpdatePlayerData());
    }
   

    IEnumerator UpdatePlayerData()
    {

        DBPlayersCharacter_Personal_ForGet playerdata = new DBPlayersCharacter_Personal_ForGet();
        playerdata.UserNick     = GameManager.Instance.GetPlayer().UserNick;
        playerdata.UserEamil    = GameManager.Instance.GetPlayer().UserEmail;
        playerdata.Characters   = GameManager.Instance.GetPlayer().Characters;
        playerdata.mail         = GameManager.Instance.GetPlayer().mail;


        bool bIsComplete = false;

        yield return null;


        Context.SaveAsync(playerdata, (res) =>
        {
            if (res.Exception == null)
            {
                Debug.Log("Saved Player Mail");
                bIsComplete = true;
            }
            else
                Debug.Log(res.Exception.ToString());

        });
    }

    IEnumerator LoadDeletePlayerMail()
    {
        yield return null;

        DBPlayersCharacter_Personal_ForGet player = null;
        

        //Load Table Info
        Context.LoadAsync<DBPlayersCharacter_Personal_ForGet>("김스맷_0", (result) =>
        {
            if (result.Exception == null)
            {
                player = result.Result as DBPlayersCharacter_Personal_ForGet;
                // Update few properties.

                Debug.Log("Nick : " + player.UserNick + "\n");

                //player.mail.character_List.Clear();
               // player.mail.equipmnet_List.Clear();

                // Update To Save
                Context.SaveAsync(player, (res) =>
                {
                    if (res.Exception == null)
                    {
                        //Debug.Log("Player Saved! ->  Mail contents : " + player.mail.nMoney);
                        //bIsComplete = true;
                    }

                    else
                        Debug.Log(res.Exception.ToString());

                });
                //StartCoroutine(SaveDeletePlayerMail(player));
            }

        });

    }



    public void StartGetPlayersData()
    {
        StartCoroutine(GetPlayerData());
    }

    
    public IEnumerator GetPlayerData()
    {
        yield return null;

        DBPlayersCharacter_Personal_ForGet player = null;

            //Load Table Info
            Context.LoadAsync<DBPlayersCharacter_Personal_ForGet>("김스맷_0", (result) => {
            if (result.Exception == null)
            {
                player = result.Result as DBPlayersCharacter_Personal_ForGet;
                // Update few properties.

                Debug.Log("Nick : " + player.UserNick + "\n");
                //GetCharacter= character;
                DBPlayersInfo_ForGet = player;
                bIsPlayerDataLoaded = true;
                //Index++;
                player = null;

            }
        });
    }

    //player데이터 받기를 완료 했으면 다시 GameManager에 있는 player클래스에 넣어준다.
    public IEnumerator CheckIsPlayerDataLoaded()
    {
        yield return new WaitForSeconds(0.1f);

        InitPlayerData();
        while (true)
        {
            if (bIsPlayerDataLoaded == true)
            {
                GameManager.Instance.GetPlayer().UserNick  = DBPlayersInfo_ForGet.UserNick;
                GameManager.Instance.GetPlayer().UserEmail = DBPlayersInfo_ForGet.UserEamil;
                GameManager.Instance.GetPlayer().Characters = DBPlayersInfo_ForGet.Characters;
                GameManager.Instance.GetPlayer().mail = DBPlayersInfo_ForGet.mail;

                //만약 메일에서 시간이 날짜를 초과한 것이 있으면 삭제한다.
                int nMailCount = GameManager.Instance.GetPlayer().mail.Count;

                for (int nLoopCount=0 , nMailIndex = 0; nLoopCount < nMailCount; nLoopCount++)
                {
                    DateTime mailTime = GameManager.Instance.GetPlayer().mail[nMailIndex].dateTime;
                    DateTime curTime = System.DateTime.Now;

                    TimeSpan resultTime = curTime - mailTime;

                    //지정한 기간 이상 지났으면 삭제가 되게 (삭제하는 건 다시 나중에 손보기로 함)
                    if (resultTime.TotalDays > GameManager.Instance.GetPlayer().mail[nMailIndex].nDays)
                    {
                        GameManager.Instance.GetPlayer().mail.Remove(GameManager.Instance.GetPlayer().mail[nMailIndex]);
                    }
                    else
                    {
                        nMailIndex++;
                    }

                }


                StartCoroutine(GameManager.Instance.DataLoad());
           

                break;
            }
            yield return null;
        }
    }

    public void InitPlayerData()
    {
        //Test
        //GameManager.Instance.playerData = new DBPlayersCharacter();
        ////init playerData
        //GameManager.Instance.playerData.UserNick = "empty";
        //GameManager.Instance.playerData.UserEmail = "empty";
        //GameManager.Instance.playerData.Characters = new List<DBBasicCharacter>();
        //GameManager.Instance.playerData.mail = new List<Mail>();

        Player player = new Player();
        GameManager.Instance.SetPlayer(player);
        
    }

    public void StartDataSetSave()
    {
        StartCoroutine(DataSetSaveInCognito_FirstLogin());
    }
    //처음 로그인후 닉 과 해당 이메일을 확인조건으로 저장한다
    IEnumerator DataSetSaveInCognito_FirstLogin()
	{
        //playerInfo = SyncManager.OpenOrCreateDataset("PlayerInfo");
        sNick = nickInputField.text;

		playerInfo.Put ("Nick", sNick);
		//playerInfo.Put ("Provider", eLoginProviderIndex.ToString ());
		playerInfo.Put ("Email", sEmail);

		//해당 플레이어의 캐릭터들 정보를 가져온다
		//DynamoDBCheck(sEmail, sNick, true);

        //플레이어 정보 동기화
		playerInfo.SynchronizeAsync ();

        DBPlayersCharacter players = new DBPlayersCharacter();

        players.Characters = new List<DBBasicCharacter>();
        //임시 캐릭터를 집어 넣는다.
        List<DBBasicCharacter> characters = new List<DBBasicCharacter>();
        DBBasicCharacter character = new DBBasicCharacter();
        character = GameManager.Instance.lDbBasicCharacter[0];

        DBActiveSkill activeSkill = new DBActiveSkill();
        activeSkill = GameManager.Instance.lDbActiveSkill[0];
        character.activeSkills.Add(activeSkill);
 
        DBBasicSkill basicSkill = new DBBasicSkill();
        basicSkill = GameManager.Instance.lDbBasickill[0];
        character.basicSkill.Add(basicSkill);
     
        DBPassiveSkill passiveSkill = new DBPassiveSkill();
        passiveSkill = GameManager.Instance.lDbPassiveSkill[0];
        character.passiveSkills.Add(passiveSkill);
      
        characters.Add(character);

        yield return null;
        /*
        players.Index = 0;
        players.UserEmail = sEmail;
        players.UserNick = sNick;
        players.Characters.Add(character);

        Debug.Log("Index : " + players.Index + "players Email : " + sEmail + "players Nick : " + sNick + "players Character : " +
            players.Characters[0].Jobs);

        yield return new WaitForSeconds(0.1f);

        // Save the Character.
        Context.SaveAsync(players, (result) =>
        {
            if (result.Exception == null)
            {
                Debug.Log("playerInfoSaved");
                bIsNickInput = true;
            }
              
            else
                Debug.Log(result.Exception.ToString());
        });


        //string myValue = dataset.Get("myKey");
        //dataset.Put("myKey", "newValue");
        //playerInfo.Get("");
        //playerInfo.Put("","");

        //dataset.Remove("myKey");

        //CharacterDBLoadAndPutOperation ();

        nickInputObj.SetActive (false);
        */
        
	}

	//연동이 되어있으면 이메일과 닉을 체크 해서 연동을 한다
	public void DynamoDBCheck(string _email, string _nick , bool isFirstLogin)
	{
		//First or Not Check
		//처음 로그인시 기본 캐릭셋을 db에 만든다
		if (isFirstLogin) {

		}

		else 
		{
			
		}
	}
	//이메일과 닉을 체크 하여 DB에서 로드  
	IEnumerator CheckEmailAndNick_LoadDB(string _email, string _nick)
	{
		yield return new WaitForSeconds (0.2f);
		while (true) 
		{
			
			yield return null;
		}
	}
	//클래스를 집에서 봐서 바꿔야함
	IEnumerator GetPlayerCountFromDB()
	{
		yield return new WaitForSeconds (0.1f);

		while (true) 
		{
			if (nCurPlayerCount != 0)
				Debug.Log ("GetPlayerCountComplete Count :" + nCurPlayerCount);
			

			nCurPlayerCount = _syncManager.ListDatasets ().Count;
			yield return null;
		}

	}

	#region LoadFromAwsDB
	IEnumerator StartLoadBasicDataSequence()
	{
		yield return new WaitForSeconds (0.1f);
		LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA);
	}

	void LoadBasicDataSequence(E_LOAD_STATE _state)
	{
		curSlider.value = 0;
		progress_Text.text = totalSlider.value + " / " + ntotalBasicDataCount;

		switch (_state) 
		{
		case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:
			curSlider.maxValue = nCharacterCount;

			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBBasicCharacterInfoPath)) {
				//다음꺼에 해당되는 걸넘어간다
				SaveAndLoadBinaryFile (sDBBasicCharacterInfoPath, E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nCharacterCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;
		case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA:
			curSlider.maxValue = nActiveSkillCount;

			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBActiveSkillPath)) {
				//다음꺼에 해당되는 걸넘어간다
				SaveAndLoadBinaryFile(sDBActiveSkillPath, E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nActiveSkillCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;
		case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA:
			curSlider.maxValue = nActiveSkillTypeCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBActiveSkilLTypePath)) {
				//해당 데이터 로컬로 로드
				SaveAndLoadBinaryFile(sDBActiveSkilLTypePath, E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA);
				//다음꺼에 해당되는 걸넘어간다
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA);
				return;
			}

			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nActiveSkillTypeCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;
		case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA:
			curSlider.maxValue = nPassiveSkillCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBPassiveSkillPath)) {
				//다음꺼에 해당되는 걸넘어간다
				SaveAndLoadBinaryFile(sDBPassiveSkillPath, E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA);

				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nPassiveSkillCount; i++)
				DBLoadAndPutOperation (_state, i);


			break;
		case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA:
			curSlider.maxValue = nPassiveSkillOptionIndexCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBPassiveSkillOptionIndexPath)) {
				SaveAndLoadBinaryFile(sDBPassiveSkillOptionIndexPath, E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nPassiveSkillOptionIndexCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;
		case E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA:
			curSlider.maxValue = nBasicSkillCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBBasicSkillPath)) {
				SaveAndLoadBinaryFile(sDBBasicSkillPath, E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA);
				return;
			}

			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nBasicSkillCount; i++)
				DBLoadAndPutOperation (_state, i);


			break;
		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA:
			curSlider.maxValue = nEquipmentWeaponCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBEquipmentWeaponPath)) {
				SaveAndLoadBinaryFile(sDBEquipmentWeaponPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA);
				return;
			}

			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nEquipmentWeaponCount; i++)
				DBLoadAndPutOperation (_state, i);


			break;
		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA:
			curSlider.maxValue = nEquipmentArmorCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBEquipmentArmorPath)) {
				SaveAndLoadBinaryFile(sDBEquipmentArmorPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA);
				return;
			}


			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nEquipmentArmorCount; i++)
				DBLoadAndPutOperation (_state, i);


			break;
		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA:
			curSlider.maxValue = nEquipmentGloveCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBEquipmentGlovePath)) {
				SaveAndLoadBinaryFile(sDBEquipmentGlovePath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nEquipmentGloveCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;
		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA:
			curSlider.maxValue = nEquipmentAccessoryCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBEquipmentAccessoryPath)) {
				SaveAndLoadBinaryFile(sDBEquipmentAccessoryPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nEquipmentAccessoryCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;
		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA:
			curSlider.maxValue = nEquipmentRandomOptionCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBEquipmentRandomOptionPath)) {
				SaveAndLoadBinaryFile(sDBEquipmentRandomOptionPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_STAGEDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nEquipmentRandomOptionCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;

		case E_LOAD_STATE.E_LOAD_GET_STAGEDATA:
			curSlider.maxValue = nStageDataCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBStageDataPath)) {
				SaveAndLoadBinaryFile(sDBStageDataPath, E_LOAD_STATE.E_LOAD_GET_STAGEDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nStageDataCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;

		case E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA:
			curSlider.maxValue = nCraftMaterialCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBCraftMaterialPath)) {
				SaveAndLoadBinaryFile(sDBCraftMaterialPath, E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nCraftMaterialCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;

		case E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA:
			curSlider.maxValue = nBreakMaterialCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBBreakMaterialPath)) {
				SaveAndLoadBinaryFile (sDBBreakMaterialPath, E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA);
				LoadBasicDataSequence (E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA);
				return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nBreakMaterialCount; i++)
				DBLoadAndPutOperation (_state, i);
			break;

		case E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA:
			curSlider.maxValue = nFormationSkillCount;
			//이미 저장된 정보가 있을시 체크 함수로 넘어간다
			if (File.Exists (Application.persistentDataPath + sDBFormationSkillPath))
			{
				SaveAndLoadBinaryFile(sDBFormationSkillPath, E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA);
                LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_MATERIALDATA);
                return;
			}
			StartCoroutine (isFinishLoadData (_state));
			for (int i = 0; i < nFormationSkillCount; i++)
				DBLoadAndPutOperation (_state, i);

			break;

            case E_LOAD_STATE.E_LOAD_GET_MATERIALDATA:
                curSlider.maxValue = nMaterialDataCount;
                //이미 저장된 정보가 있을시 체크 함수로 넘어간다
                if (File.Exists(Application.persistentDataPath + sDBMaterialDataPath))
                {
                    SaveAndLoadBinaryFile(sDBMaterialDataPath, E_LOAD_STATE.E_LOAD_GET_MATERIALDATA);
                    LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_CALENDARDATA);
                    return;
                }

                StartCoroutine(isFinishLoadData(_state));
                for (int i = 0; i < nMaterialDataCount; i++)
                    DBLoadAndPutOperation(_state, i);

                break;

            case E_LOAD_STATE.E_LOAD_GET_CALENDARDATA:
                curSlider.maxValue = nCalendarCount;
                //이미 저장된 정보가 있을시 체크 함수로 넘어간다
                if (File.Exists(Application.persistentDataPath + sDBCalendarPath))
                {
                    SaveAndLoadBinaryFile(sDBCalendarPath, E_LOAD_STATE.E_LOAD_GET_CALENDARDATA);
                    LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA);
                    return;
                }

                StartCoroutine(isFinishLoadData(_state));
                for (int i = 0; i < nCalendarCount; i++)
                    DBLoadAndPutOperation(_state, i);

                break;

            case E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA:
                curSlider.maxValue = nCharacterTicketCount;
                //이미 저장된 정보가 있을시 체크 함수로 넘어간다
                if (File.Exists(Application.persistentDataPath + sDBCharacterTickectPath))
                {
                    SaveAndLoadBinaryFile(sDBCharacterTickectPath, E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA);
                    LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA);
                    return;
                }

                StartCoroutine(isFinishLoadData(_state));
                for (int i = 0; i < nCharacterTicketCount; i++)
                    DBLoadAndPutOperation(_state, i);

                break;

            case E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA:
                curSlider.maxValue = nWeaponTicketCount;
                //이미 저장된 정보가 있을시 체크 함수로 넘어간다
                if (File.Exists(Application.persistentDataPath + sDBWeaponTicketPath))
                {
                    SaveAndLoadBinaryFile(sDBWeaponTicketPath, E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA);
                    LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA);

                    return;
                }

                StartCoroutine(isFinishLoadData(_state));
                for (int i = 0; i < nWeaponTicketCount; i++)
                    DBLoadAndPutOperation(_state, i);

                break;

            case E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA:
                curSlider.maxValue = nEmployGachaCount;
                //이미 저장된 정보가 있을시 체크 함수로 넘어간다
                if (File.Exists(Application.persistentDataPath + sDBEmployGachaPath))
                {
                    SaveAndLoadBinaryFile(sDBEmployGachaPath, E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA);
                    return;
                }

                StartCoroutine(isFinishLoadData(_state));
                for (int i = 0; i < nEmployGachaCount; i++)
                    DBLoadAndPutOperation(_state, i);

                break;


            default:
			break;

		}

	}


	private void DBLoadAndPutOperation(E_LOAD_STATE _state , int _index)
	{

		if (_state == E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA) {
			DBBaiscCharacter_ForGet character = null;
			//Load Table Info
			Context.LoadAsync<DBBaiscCharacter_ForGet> (_index, (result) => {
				if (result.Exception == null) 
				{
					character = result.Result as DBBaiscCharacter_ForGet;
					// Update few properties.

					Debug.Log ("CharacterName : " + character.C_JobNames);
					//TableInfo_Text.text = character.C_JobNames + "\n";
					//GetCharacter= character;
					lDBBasicCheacter_GetList.Add (character);
					//Index++;
					character = null;

					curSlider.value ++;
				}
			});
		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA) {
			DBActiveSkill_ForGet DBActiveSkill = null;
			//Load Table Info
			Context.LoadAsync<DBActiveSkill_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBActiveSkill = result.Result as DBActiveSkill_ForGet;
					// Update few properties.

					Debug.Log ("ActiveSkillName : " + DBActiveSkill.Skill_Name + "\n");
					//GetCharacter= character;
					lDBActiveSkill_GetList.Add (DBActiveSkill);

					//Index++;
					DBActiveSkill = null;

					curSlider.value ++;
				}

			});
		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA) {

			DBActiveSkillType_ForGet DBActiveSkillType = null;

			//Load Table Info
			Context.LoadAsync<DBActiveSkillType_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBActiveSkillType = result.Result as DBActiveSkillType_ForGet;
					// Update few properties.

					Debug.Log ("ActiveSkillTypeIndex : " + DBActiveSkillType.Index + "\n");
					//GetCharacter= character;
					lDBActiveSkillType_GetList.Add (DBActiveSkillType);
					//null
					DBActiveSkillType = null;

					curSlider.value ++;
				}

			});
		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA) {
			DBPassiveSkill_ForGet DBPassiveSkill = null;

			//Load Table Info
			Context.LoadAsync<DBPassiveSkill_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBPassiveSkill = result.Result as DBPassiveSkill_ForGet;
					// Update few properties.

					Debug.Log ("PassiveSkillName : " + DBPassiveSkill.PassiveSkill_Name + "\n");
					//GetCharacter= character;
					lDBPassiveSkill_GetList.Add (DBPassiveSkill);

					//Index++;
					DBPassiveSkill = null;

					curSlider.value ++;
				}

			});

		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA) {

			DBPassiveSkillOptionIndex_ForGet DBPassiveSkillOptionIndex = null;
			//Load Table Info
			Context.LoadAsync<DBPassiveSkillOptionIndex_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBPassiveSkillOptionIndex = result.Result as DBPassiveSkillOptionIndex_ForGet;
					// Update few properties.

					Debug.Log ("PassiveSkillOptionIndex : " + DBPassiveSkillOptionIndex.Index + "\n");
					//GetCharacter= character;
					lDBPassisveSkillOptionIndex_GetList.Add (DBPassiveSkillOptionIndex);
					//null
					DBPassiveSkillOptionIndex = null;

					curSlider.value ++;
				}

			});
		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA) {
			DBbasicSkill_ForGet DBBasicSkill = null;

			//Load Table Info
			Context.LoadAsync<DBbasicSkill_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBBasicSkill = result.Result as DBbasicSkill_ForGet;
					// Update few properties.

					Debug.Log ("BasicSkillName : " + DBBasicSkill.BasicSkill_Name + "\n");
					//GetCharacter= character;
					lDBBasicSkill_GetList.Add (DBBasicSkill);

					//Index++;
					DBBasicSkill = null;

					curSlider.value ++;
				}
			});

		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA) {
			DBEquipment_Weapon_ForGet DBEquipWeapon = null;

			//Load Table Info
			Context.LoadAsync<DBEquipment_Weapon_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBEquipWeapon = result.Result as DBEquipment_Weapon_ForGet;
					// Update few properties.

					Debug.Log ("EquipmentWeaponName : " + DBEquipWeapon.EquipWeapon_Name + "\n");
					//GetCharacter= character;
					lDBEquipmentWeapon_GetList.Add (DBEquipWeapon);

					//Index++;
					DBEquipWeapon = null;

					curSlider.value ++;
				}

			});

		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA) {
			DBEquipment_Armor_ForGet DBEquipArmor = null;

			//Load Table Info
			Context.LoadAsync<DBEquipment_Armor_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBEquipArmor = result.Result as DBEquipment_Armor_ForGet;
					// Update few properties.

					Debug.Log ("EquipmentArmorName : " + DBEquipArmor.EquipArmor_Name + "\n");
					//GetCharacter= character;
					lDBEquipmentArmor_GetList.Add (DBEquipArmor);

					//Index++;
					DBEquipArmor = null;

					curSlider.value ++;
				}

			});
		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA) {
			DBEquipment_Glove_ForGet DBEquipGlove = null;

			//Load Table Info
			Context.LoadAsync<DBEquipment_Glove_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBEquipGlove = result.Result as DBEquipment_Glove_ForGet;
					// Update few properties.

					Debug.Log ("EquipmentGloveName : " + DBEquipGlove.EquipGlove_Name + "\n");
					//GetCharacter= character;
					lDBEquipmentGlove_GetList.Add (DBEquipGlove);

					//Index++;
					DBEquipGlove = null;

					curSlider.value ++;
				}
			});


		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA) {
			DBEquipment_Accessory_ForGet DBEquipAccessory = null;

			//Load Table Info
			Context.LoadAsync<DBEquipment_Accessory_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBEquipAccessory = result.Result as DBEquipment_Accessory_ForGet;
					// Update few properties.

					Debug.Log ("EquipmentGloveName : " + DBEquipAccessory.EquipAccessory_Name + "\n");
					//GetCharacter= character;
					lDBEquipmentAccessory_GetList.Add (DBEquipAccessory);

					//Index++;
					DBEquipAccessory = null;

					curSlider.value ++;
				}
			});
		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA) {

			DBEquipment_RandomOption_ForGet DBEquipRandomOption = null;

			//Load Table Info
			Context.LoadAsync<DBEquipment_RandomOption_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBEquipRandomOption = result.Result as DBEquipment_RandomOption_ForGet;
					// Update few properties.

					Debug.Log ("EquipmentRandomOptionIndex : " + DBEquipRandomOption.Index + "\n");
					//GetCharacter= character;
					lDBEquipmentRandomOption_GetList.Add (DBEquipRandomOption);

					//Index++;
					DBEquipRandomOption = null;

					curSlider.value ++;
				}
			});

		}
		if (_state == E_LOAD_STATE.E_LOAD_GET_STAGEDATA) {

			DBStage_ForGet DBstageData = null;

			//Load Table Info
			Context.LoadAsync<DBStage_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBstageData = result.Result as DBStage_ForGet;
					// Update few properties.

					Debug.Log ("DBStageIndex : " + DBstageData.Index + "\n");
					//GetCharacter= character;
					lDBStageData_GetList.Add (DBstageData);

					//Index++;
					DBstageData = null;

					curSlider.value ++;
				}
			});

		}

		if (_state == E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA) {

			DBCraftMaterial_ForGet craftMaterial = null;

			//Load Table Info
			Context.LoadAsync<DBCraftMaterial_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					craftMaterial = result.Result as DBCraftMaterial_ForGet;
					// Update few properties.

					Debug.Log ("DBCraftMaterial : " + craftMaterial.Index + "\n");
					//GetCharacter= character;
					lDBCraftMaterial_GetList.Add (craftMaterial);

					//Index++;
					craftMaterial = null;

					curSlider.value ++;
				}
			});

		}

		if (_state == E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA) {

			DBBreakMaterial_ForGet DBbreakMaterial = null;

			//Load Table Info
			Context.LoadAsync<DBBreakMaterial_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBbreakMaterial = result.Result as DBBreakMaterial_ForGet;
					// Update few properties.

					Debug.Log ("DBbreakMaterial : " + DBbreakMaterial.Index + "\n");
					//GetCharacter= character;
					lDBBreakMaterial_GetList.Add (DBbreakMaterial);

					//Index++;
					DBbreakMaterial = null;

					curSlider.value ++;
				}
			});
		}

		if (_state == E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA) 
		{
			DBFormationSkill_ForGet DBformationSkill = null;

			//Load Table Info
			Context.LoadAsync<DBFormationSkill_ForGet> (_index, (result) => {
				if (result.Exception == null) {
					DBformationSkill = result.Result as DBFormationSkill_ForGet;
					// Update few properties.

					Debug.Log ("DBFormationSkill : " + DBformationSkill.Index + "\n");
					//GetCharacter= character;
					lDBFormationSkill_GetList.Add (DBformationSkill);

					//Index++;
					DBformationSkill = null;

					curSlider.value ++;
				}
			});

		}

        if (_state == E_LOAD_STATE.E_LOAD_GET_MATERIALDATA)
        {
            DBMaterialData_ForGet DBMaterialData = null;

            //Load Table Info
            Context.LoadAsync<DBMaterialData_ForGet>(_index, (result) => {
                if (result.Exception == null)
                {
                    DBMaterialData = result.Result as DBMaterialData_ForGet;
                    // Update few properties.

                    Debug.Log("DBMaterialData : " + DBMaterialData.Index + "\n");
                    //GetCharacter= character;
                    lDBMaterialData_GetList.Add(DBMaterialData);

                    //Index++;
                    DBMaterialData = null;

                    curSlider.value++;
                }
            });

        }

        if (_state == E_LOAD_STATE.E_LOAD_GET_CALENDARDATA)
        {
            DBCalendarData_ForGet DBCalendarData = null;

            //Load Table Info
            Context.LoadAsync<DBCalendarData_ForGet>(_index, (result) => {
                if (result.Exception == null)
                {
                    DBCalendarData = result.Result as DBCalendarData_ForGet;
                    // Update few properties.

                    Debug.Log("DBCalendarData : " + DBCalendarData.Index + "\n");
                    //GetCharacter= character;
                    lDBCalendar_GetList.Add(DBCalendarData);

                    //Index++;
                    DBCalendarData = null;

                    curSlider.value++;
                }
            });

        }

        if (_state == E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA)
        {
            DBCharacterTicketData_ForGet DBCharacterTicketData = null;

            //Load Table Info
            Context.LoadAsync<DBCharacterTicketData_ForGet>(_index, (result) => {
                if (result.Exception == null)
                {
                    DBCharacterTicketData = result.Result as DBCharacterTicketData_ForGet;
                    // Update few properties.

                    Debug.Log("DBCharacterTicketData : " + DBCharacterTicketData.Index + "\n");
                    //GetCharacter= character;
                    lDBCharacterTicket_GetList.Add(DBCharacterTicketData);

                    //Index++;
                    DBCharacterTicketData = null;

                    curSlider.value++;
                }
            });

        }


        if (_state == E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA)
        {
            DBWeaponTicketData_ForGet DBWeaponTicketData = null;

            //Load Table Info
            Context.LoadAsync<DBWeaponTicketData_ForGet>(_index, (result) => {
                if (result.Exception == null)
                {
                    DBWeaponTicketData = result.Result as DBWeaponTicketData_ForGet;
                    // Update few properties.

                    Debug.Log("DBWeaponTicketData : " + DBWeaponTicketData.Index + "\n");
                    //GetCharacter= character;
                    lDBWeaponTicket_GetList.Add(DBWeaponTicketData);

                    //Index++;
                    DBWeaponTicketData = null;

                    curSlider.value++;
                }
            });

        }

        if (_state == E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA)
        {
            DBEmployGachaData_ForGet DBEmployGachaData = null;

            //Load Table Info
            Context.LoadAsync<DBEmployGachaData_ForGet>(_index, (result) => {
                if (result.Exception == null)
                {
                    DBEmployGachaData = result.Result as DBEmployGachaData_ForGet;
                    // Update few properties.

                    Debug.Log("DBEmployGachaData : " + DBEmployGachaData.Index + "\n");
                    //GetCharacter= character;
                    lDBEmployGacha_GetList.Add(DBEmployGachaData);

                    //Index++;
                    DBEmployGachaData = null;

                    curSlider.value++;
                }
            });

        }

    }


IEnumerator isFinishLoadData(E_LOAD_STATE _state)
{

	yield return new WaitForSeconds(0.1f);

	int nPotCount = 0;
	string sInputText = null;
	switch (_state) 
	{
	        case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:
	        	sInputText = "기본 캐릭터 받아오는중 ";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA:
	        	sInputText = "기본 액티브스킬 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA:
	        	sInputText = "기본 액티브스킬 타입 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA:
	        	sInputText = "기본 패시브스킬 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA:
	        	sInputText = "기본 패시브스킬 옵션인덱스 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA:
	        	sInputText = "기본 베이직스킬 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA:
	        	sInputText = "기본 무기 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA:
	        	sInputText = "기본 아머 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA:
	        	sInputText = "기본 장갑 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA:
	        	sInputText = "기본 악세사리 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA:
	        	sInputText = "기본 장비 랜덤옵션 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_STAGEDATA:
	        	sInputText = "스테이지 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA:
	        	sInputText = "제작 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA:
	        	sInputText = "분해 정보 받아오는중";
	        	break;
	        case E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA:
	        	sInputText = "포메이션 스킬 정보 받아오는중";
	        	break;
            case E_LOAD_STATE.E_LOAD_GET_MATERIALDATA:
                sInputText = "재료 정보 받아오는중";
                break;
            case E_LOAD_STATE.E_LOAD_GET_CALENDARDATA:
                sInputText = "출석보상 정보 받아오는중";
                break;
            case E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA:
                sInputText = "캐릭터 티켓 정보 받아오는중";
                break;
            case E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA:
                sInputText = "장비 티켓 정보 받아오는중";
                break;
            case E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA:
                sInputText = "용병고용 정보 받아오는중";
                break;
            default:
		break;

	}

		while (true) {
			//break 조건
			if (lDBBasicCheacter_GetList.Count == nCharacterCount && _state == E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA) {
				totalSlider.value++;
				loginState_Text.text = "기본 캐릭터 불러오기 완료";
				#if UNITY_EDITOR

				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBActiveSkill_GetList.Count == nActiveSkillCount && _state == E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA) {
				totalSlider.value++;

				loginState_Text.text = "ActiveSkill 불러오기 완료";
				#if UNITY_EDITOR

				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBActiveSkillType_GetList.Count == nActiveSkillTypeCount && _state == E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA) {
				totalSlider.value++;
				loginState_Text.text = "ActiveSkillType 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBPassiveSkill_GetList.Count == nPassiveSkillCount && _state == E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA) {
				totalSlider.value++;
				loginState_Text.text = "PassiveSkill 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}


			if (lDBPassisveSkillOptionIndex_GetList.Count == nPassiveSkillOptionIndexCount && _state == E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA) {
				totalSlider.value++;
				loginState_Text.text = "PassiveSkillOptionIndex 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBBasicSkill_GetList.Count == nBasicSkillCount && _state == E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA) {
				totalSlider.value++;
				loginState_Text.text = "BasicSkill 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentWeapon_GetList.Count == nEquipmentWeaponCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA) {
				totalSlider.value++;
				loginState_Text.text = "EquipmentWeapon 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentArmor_GetList.Count == nEquipmentArmorCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA) {
				totalSlider.value++;
				loginState_Text.text = "EquipmentArmor 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentGlove_GetList.Count == nEquipmentGloveCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA) {
				totalSlider.value++;
				loginState_Text.text = "EquipmentGlove 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentAccessory_GetList.Count == nEquipmentAccessoryCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA) {
				totalSlider.value++;
				loginState_Text.text = "EquipmentAccessory 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentRandomOption_GetList.Count == nEquipmentRandomOptionCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA) {
				totalSlider.value++;
				loginState_Text.text = "EquipmentRandomOption 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBStageData_GetList.Count == nStageDataCount && _state == E_LOAD_STATE.E_LOAD_GET_STAGEDATA) {
				totalSlider.value++;
				loginState_Text.text = "StageData 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBCraftMaterial_GetList.Count == nCraftMaterialCount && _state == E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA) {
				totalSlider.value++;
				loginState_Text.text = "CraftMaterial 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}
			if (lDBBreakMaterial_GetList.Count == nBreakMaterialCount && _state == E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA) {
				totalSlider.value++;
				loginState_Text.text = "BreakMaterial 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBFormationSkill_GetList.Count == nFormationSkillCount && _state == E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA) {
				totalSlider.value++;
				loginState_Text.text = " FormationSkill 불러오기 완료";
				#if UNITY_EDITOR
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager (_state);


				#elif UNITY_ANDROID
			//LoginCategory_Panel.SetActive (true);
			InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

            if (lDBMaterialData_GetList.Count == nMaterialDataCount && _state == E_LOAD_STATE.E_LOAD_GET_MATERIALDATA)
            {
                totalSlider.value++;
                loginState_Text.text = " MaterialData 불러오기 완료";
                #if UNITY_EDITOR
                LoginCategory_Panel.SetActive(false);
                InsertInfoToUsingListInGameManager(_state);


                #elif UNITY_ANDROID
			    //LoginCategory_Panel.SetActive (true);
			    InsertInfoToUsingListInGameManager(_state);
                #endif
                break;
            }

            if (lDBCalendar_GetList.Count == nCalendarCount && _state == E_LOAD_STATE.E_LOAD_GET_CALENDARDATA)
            {
                totalSlider.value++;
                loginState_Text.text = " Calendar 불러오기 완료";
                #if UNITY_EDITOR
                LoginCategory_Panel.SetActive(false);
                InsertInfoToUsingListInGameManager(_state);


                #elif UNITY_ANDROID
			    //LoginCategory_Panel.SetActive (true);
			    InsertInfoToUsingListInGameManager(_state);
                #endif
                break;
            }

            if (lDBCharacterTicket_GetList.Count == nCharacterTicketCount && _state == E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA)
            {
                totalSlider.value++;
                loginState_Text.text = " CharacterTicket 불러오기 완료";
                #if UNITY_EDITOR
                LoginCategory_Panel.SetActive(false);
                InsertInfoToUsingListInGameManager(_state);


                #elif UNITY_ANDROID
			    //LoginCategory_Panel.SetActive (true);
			    InsertInfoToUsingListInGameManager(_state);
                #endif
                break;
            }

            if (lDBWeaponTicket_GetList.Count == nWeaponTicketCount && _state == E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA)
            {
                totalSlider.value++;
                loginState_Text.text = " WeaponTicket 불러오기 완료";
                #if UNITY_EDITOR
                LoginCategory_Panel.SetActive(false);
                InsertInfoToUsingListInGameManager(_state);


                #elif UNITY_ANDROID
			    //LoginCategory_Panel.SetActive (true);
			    InsertInfoToUsingListInGameManager(_state);
                #endif
                break;
            }

            if (lDBEmployGacha_GetList.Count == nEmployGachaCount && _state == E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA)
            {
                totalSlider.value++;
                loginState_Text.text = " EmployGacha 불러오기 완료";
                #if UNITY_EDITOR
                LoginCategory_Panel.SetActive(false);
                InsertInfoToUsingListInGameManager(_state);


                #elif UNITY_ANDROID
			    //LoginCategory_Panel.SetActive (true);
			    InsertInfoToUsingListInGameManager(_state);
                #endif
                break;
            }


            yield return new WaitForSeconds (0.2f);

			if (nPotCount == 0) {
				loginState_Text.text = sInputText + ".";
				nPotCount++;
			} else if (nPotCount == 1) {
				loginState_Text.text = sInputText + "..";
				nPotCount++;
			} else {
				loginState_Text.text = sInputText + "...";
				nPotCount = 0;
			}
		}
}


public void InsertInfoToUsingListInGameManager(E_LOAD_STATE _state)
{
	switch(_state)
	{

	case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:
		DBBasicCharacter dbBaseCharacters = new DBBasicCharacter();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDbBasicCharacter.Capacity = nCharacterCount;
		for (int i = 0; i < nCharacterCount; i++)
		{
			dbBaseCharacters = new DBBasicCharacter ();

			dbBaseCharacters.C_JobIndex 			= lDBBasicCheacter_GetList [i].C_JobIndex;
			dbBaseCharacters.Accurancy 		        = lDBBasicCheacter_GetList [i].Accurancy;
			dbBaseCharacters.activeSkills 	        = lDBBasicCheacter_GetList [i].activeSkills;
			dbBaseCharacters.AttackRange 	        = lDBBasicCheacter_GetList [i].AttackRange;
			dbBaseCharacters.AttackSpeed 	        = lDBBasicCheacter_GetList [i].AttackSpeed;
			dbBaseCharacters.Attribute 		        = lDBBasicCheacter_GetList [i].Attribute;
			dbBaseCharacters.basicSkill 	        = lDBBasicCheacter_GetList [i].basicSkill;
			dbBaseCharacters.passiveSkills 			= lDBBasicCheacter_GetList [i].passiveSkills;
			dbBaseCharacters.Betch_Index 	        = lDBBasicCheacter_GetList [i].Betch_Index;
			dbBaseCharacters.CC_Registance 	        = lDBBasicCheacter_GetList [i].CC_Registance;
			dbBaseCharacters.Crit_Dmg 		        = lDBBasicCheacter_GetList [i].Crit_Dmg;
			dbBaseCharacters.Crit_Rating 	        = lDBBasicCheacter_GetList [i].Crit_Rating;
			dbBaseCharacters.C_Enhance 		        = lDBBasicCheacter_GetList [i].C_Enhance;
			dbBaseCharacters.C_Index 		        = lDBBasicCheacter_GetList [i].C_Index;
			dbBaseCharacters.C_JobNames 	        = lDBBasicCheacter_GetList [i].C_JobNames;
			dbBaseCharacters.C_Name 		        = lDBBasicCheacter_GetList [i].C_Name;
			dbBaseCharacters.Dodge 			        = lDBBasicCheacter_GetList [i].Dodge;
			dbBaseCharacters.Exp 			        = lDBBasicCheacter_GetList [i].Exp;
			dbBaseCharacters.ExpMax 		        = lDBBasicCheacter_GetList [i].ExpMax;
			dbBaseCharacters.Health 		        = lDBBasicCheacter_GetList [i].Health;
			dbBaseCharacters.Index 			        = lDBBasicCheacter_GetList [i].Index;
			dbBaseCharacters.Jobs 			        = lDBBasicCheacter_GetList [i].Jobs;
			dbBaseCharacters.Levels 		        = lDBBasicCheacter_GetList [i].Levels;
			dbBaseCharacters.Magic_AttackRating 	= lDBBasicCheacter_GetList [i].Magic_AttackRating;
			dbBaseCharacters.Magic_Defense 			= lDBBasicCheacter_GetList [i].Magic_Defense;
			dbBaseCharacters.Magic_Penetrate 		= lDBBasicCheacter_GetList [i].Magic_Penetrate;
			dbBaseCharacters.MoveSpeed 				= lDBBasicCheacter_GetList [i].MoveSpeed;
			dbBaseCharacters.Physic_AttackRating 	= lDBBasicCheacter_GetList [i].Physic_AttackRating;
			dbBaseCharacters.Physic_Defense 		= lDBBasicCheacter_GetList [i].Physic_AttackRating;
			dbBaseCharacters.Physic_Penetrate 		= lDBBasicCheacter_GetList [i].Physic_Penetrate;
			dbBaseCharacters.Site 					= lDBBasicCheacter_GetList [i].Site;
			dbBaseCharacters.Tier 					= lDBBasicCheacter_GetList [i].Tier;
			dbBaseCharacters.Tribe 					= lDBBasicCheacter_GetList [i].Tribe;
            dbBaseCharacters.m_nStamina             = lDBBasicCheacter_GetList[i].Stamina;
            dbBaseCharacters.m_sImage               = lDBBasicCheacter_GetList[i].Image;


            GameManager.Instance.lDbBasicCharacter.Add(dbBaseCharacters);
			dbBaseCharacters = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBBasicCharacterInfoPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA);

		break;
	case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA:

		DBActiveSkill activeSkill = null;
		//용량 설정 (ActiveSkill 개수 만큼) 
		GameManager.Instance.lDbActiveSkill.Capacity = nActiveSkillCount;
		for (int i = 0; i < nActiveSkillCount; i++)
		{
			activeSkill = new DBActiveSkill() ;
			activeSkill.m_nIndex = lDBActiveSkill_GetList [i].Index;
			activeSkill.m_nCharacterIndex = lDBActiveSkill_GetList [i].C_Index;
			activeSkill.m_strName = lDBActiveSkill_GetList [i].Skill_Name;
			activeSkill.m_strSkillType = lDBActiveSkill_GetList [i].Skill_Type;
			activeSkill.m_nSkillClass = lDBActiveSkill_GetList [i].Skill_Class;
			activeSkill.m_nTier = lDBActiveSkill_GetList [i].Tier;
			activeSkill.m_strJob = lDBActiveSkill_GetList [i].Jobs;
			activeSkill.m_nLevels = lDBActiveSkill_GetList [i].Levels;
			activeSkill.m_nAttribute = lDBActiveSkill_GetList [i].Skill_Attribute;
			activeSkill.m_nAttackType = lDBActiveSkill_GetList [i].Skill_AttackType;
			activeSkill.m_fAttack_ActvieRating = lDBActiveSkill_GetList [i].Skill_AttackActiveRating;
			activeSkill.m_fCriticalAttack_ActiveRating = lDBActiveSkill_GetList [i].Skill_CriAttackActiveRating;
			activeSkill.m_nAttackCount_ActiveRating = lDBActiveSkill_GetList [i].Skill_AttackCountActiveRating;
			activeSkill.m_fMiss_ActiveRating = lDBActiveSkill_GetList [i].Skill_MissActiveRating;
			activeSkill.m_fDodgy_ActiveRating = lDBActiveSkill_GetList [i].Skill_DodgeActiveRating;
			activeSkill.m_fHit_ActiveRating = lDBActiveSkill_GetList [i].Skill_HitActiveRating;
			activeSkill.m_fCoolTime = lDBActiveSkill_GetList [i].Skill_CoolTime;
			activeSkill.m_fCastTime = lDBActiveSkill_GetList [i].Skill_CastTime;
			activeSkill.m_fPhysicalMagnification = lDBActiveSkill_GetList [i].Skill_PhyMagnification;
			activeSkill.m_fMagicMagnification = lDBActiveSkill_GetList [i].Skill_MagicMagnification;
			activeSkill.m_nAttackNumber = lDBActiveSkill_GetList [i].Skill_AttackNumber;
			activeSkill.m_fAttackRange = lDBActiveSkill_GetList [i].Skill_AttackRange;
			activeSkill.m_fAttackArea = lDBActiveSkill_GetList [i].Skill_AttackArea;
			activeSkill.m_nMaxTargetNumber = lDBActiveSkill_GetList [i].Skill_MaxTargetNumber;
			activeSkill.m_strAttackPriority = lDBActiveSkill_GetList [i].Skill_AttackPriority;
			activeSkill.m_fKnockback_Power = lDBActiveSkill_GetList [i].Skill_KnockBackPower;
			activeSkill.m_fDuration = lDBActiveSkill_GetList [i].Skill_Duration;
			activeSkill.m_strEffectName = lDBActiveSkill_GetList [i].Skill_EffectName;
			activeSkill.m_strAnimationClip = lDBActiveSkill_GetList [i].Skill_AnimationClip;
			activeSkill.m_strExplanation = lDBActiveSkill_GetList [i].Skill_Explanation;
			activeSkill.m_bIsCooltime = lDBActiveSkill_GetList [i].Skill_IsCoolTime;

			GameManager.Instance.lDbActiveSkill.Add(activeSkill);
			activeSkill = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBActiveSkillPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA);

		break;
	case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA:

		DBActiveSkillType activeSkillType = new DBActiveSkillType();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDbActiveSkillType.Capacity = nActiveSkillTypeCount;
		for (int i = 0; i < nActiveSkillTypeCount; i++)
		{
			activeSkillType = new DBActiveSkillType ();
			activeSkillType.nIndex = lDBActiveSkillType_GetList [i].Index;
			activeSkillType.nActiveType = lDBActiveSkillType_GetList [i].ActiveSkillType_Index;
			activeSkillType.nTargetIndex = lDBActiveSkillType_GetList [i].ActiveSkillType_TargetNumber;

			GameManager.Instance.lDbActiveSkillType.Add(activeSkillType);
			activeSkillType = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBActiveSkilLTypePath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA);
		break;

	case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA:

		DBPassiveSkill passiveSkill = new DBPassiveSkill();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDbPassiveSkill.Capacity = nPassiveSkillCount;
		for (int i = 0; i < nPassiveSkillCount; i++)
		{
			passiveSkill = new DBPassiveSkill ();

			passiveSkill.nIndex = lDBPassiveSkill_GetList [i].Index;
			passiveSkill.nCharacterIndex = lDBPassiveSkill_GetList [i].C_Index;
			passiveSkill.strSkillName = lDBPassiveSkill_GetList [i].PassiveSkill_Name;
			passiveSkill.strSkillType = lDBPassiveSkill_GetList [i].PassiveSkill_SkillType;
			passiveSkill.nSkillClass = lDBPassiveSkill_GetList [i].PassiveSkill_Class;
			passiveSkill.nTier = lDBPassiveSkill_GetList [i].PassiveSkill_Tier;
			passiveSkill.strJob = lDBPassiveSkill_GetList [i].PassiveSkill_Job;
			passiveSkill.nAttribute = lDBPassiveSkill_GetList [i].PassiveSkill_Attribute;
			passiveSkill.nAttackType = lDBPassiveSkill_GetList [i].PassiveSkill_AttackType;
			passiveSkill.strOption_List = lDBPassiveSkill_GetList [i].PassiveSkill_OptionIndex;
			passiveSkill.strExplanation = lDBPassiveSkill_GetList [i].PassiveSkill_Explanation;

			GameManager.Instance.lDbPassiveSkill.Add(passiveSkill);
			passiveSkill = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBPassiveSkillPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA);

		break;
	case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA:

		DBPassiveSkillOptionIndex passiveSkillOptionIndex = new DBPassiveSkillOptionIndex();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDbPassiveSkillOptionIndex.Capacity = nPassiveSkillOptionIndexCount;
		for (int i = 0; i < nPassiveSkillOptionIndexCount; i++)
		{
			passiveSkillOptionIndex = new DBPassiveSkillOptionIndex ();

			passiveSkillOptionIndex.nIndex = lDBPassisveSkillOptionIndex_GetList [i].Index;
			passiveSkillOptionIndex.nOptionIndex = lDBPassisveSkillOptionIndex_GetList [i].PassiveSkillOptionIndex_Option;
			passiveSkillOptionIndex.fValue = lDBPassisveSkillOptionIndex_GetList [i].PassiveSkillOptionIndex_Value;
			passiveSkillOptionIndex.fPlus = lDBPassisveSkillOptionIndex_GetList [i].PassiveSkillOptionIndex_Plus;
			passiveSkillOptionIndex.nCalculate = lDBPassisveSkillOptionIndex_GetList [i].PassiveSkillOptionIndex_Calculate;

			GameManager.Instance.lDbPassiveSkillOptionIndex.Add(passiveSkillOptionIndex);
			passiveSkillOptionIndex = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBPassiveSkillOptionIndexPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA);


		break;
	case E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA:

		DBBasicSkill basicSkill = new DBBasicSkill();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDbBasickill.Capacity = nBasicSkillCount;
		for (int i = 0; i < nBasicSkillCount; i++)
		{
			basicSkill = new DBBasicSkill ();

			basicSkill.nIndex = lDBBasicSkill_GetList [i].Index;
			basicSkill.nCharacterIndex = lDBBasicSkill_GetList [i].C_Index;
			basicSkill.strSkillName = lDBBasicSkill_GetList [i].BasicSkill_Name;
			basicSkill.strSkillType = lDBBasicSkill_GetList [i].BasicSkill_Type;
			basicSkill.nSkillClass = lDBBasicSkill_GetList [i].BasicSkill_Class;
			basicSkill.nTier = lDBBasicSkill_GetList [i].BasicSkill_Tier;
			basicSkill.strJob = lDBBasicSkill_GetList [i].BasicSkill_Job;
			basicSkill.nAttribute = lDBBasicSkill_GetList [i].BasicSkill_Attribute;
			basicSkill.fPhsyicMagnification = lDBBasicSkill_GetList [i].BasicSkill_PhysicMagnification;
			basicSkill.fMagicMagnification = lDBBasicSkill_GetList [i].BasicSkill_MagicMagnification;
			basicSkill.fAttackArea = lDBBasicSkill_GetList [i].BasicSkill_AttackArea;
			basicSkill.strSkillTarget = lDBBasicSkill_GetList [i].BasicSkill_SkillTarget;
			basicSkill.nMaxTargetNumber = lDBBasicSkill_GetList [i].BasicSkill_MaxTargetNumber;
			basicSkill. nAttackNumber= lDBBasicSkill_GetList [i].BasicSkill_AttackNumber;
			basicSkill.strAttackPriority = lDBBasicSkill_GetList [i].BasicSkill_AttackPriority;
			basicSkill.strExplanation = lDBBasicSkill_GetList [i].BasicSkill_Explanation;
			GameManager.Instance.lDbBasickill.Add(basicSkill);
			basicSkill = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBBasicSkillPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA);
		break;

	case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA:

		DBWeapon weapon = new DBWeapon();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDbWeapon.Capacity = nEquipmentWeaponCount;
		for (int i = 0; i < nEquipmentWeaponCount; i++)
		{
			weapon = new DBWeapon ();

			weapon.nIndex = lDBEquipmentWeapon_GetList [i].Index;
			weapon.sName = lDBEquipmentWeapon_GetList [i].EquipWeapon_Name;
			weapon.nTier = lDBEquipmentWeapon_GetList [i].EquipWeapon_Tier;
			weapon.nQulity = lDBEquipmentWeapon_GetList [i].EquipWeapon_Qulity;
			weapon.sJob = lDBEquipmentWeapon_GetList [i].EquipWeapon_Job;
			weapon.nEnhanced = lDBEquipmentWeapon_GetList [i].EquipWeapon_Enhance;
			weapon.sEquipType = lDBEquipmentWeapon_GetList [i].EquipWeapon_EquipType;
			weapon.fPhysical_AttackRating = lDBEquipmentWeapon_GetList [i].EquipWeapon_PhysicalAttackRating;
			weapon.fMagic_AttackRating = lDBEquipmentWeapon_GetList [i].EquipWeapon_MagicAttackRating;
			weapon.nRandomOption = lDBEquipmentWeapon_GetList [i].EquipWeapon_RandomOption;
			weapon.nSellCost = lDBEquipmentWeapon_GetList [i].EquipWeapon_SellCost;
			weapon.nMakeMaterial = lDBEquipmentWeapon_GetList [i].EquipWeapon_MakeMaterial;
			weapon.nBreakMaterial = lDBEquipmentWeapon_GetList [i].EquipWeapon_BreakMaterial;
			GameManager.Instance.lDbWeapon.Add (weapon);

			weapon = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBEquipmentWeaponPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA);
		break;
	case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA:
		DBArmor armor = new DBArmor();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBArmor.Capacity = nEquipmentArmorCount;
		for (int i = 0; i < nEquipmentArmorCount; i++)
		{
			armor = new DBArmor ();
			armor.nIndex 					= lDBEquipmentArmor_GetList [i].Index;
			armor.sName 					= lDBEquipmentArmor_GetList [i].EquipArmor_Name;
			armor.nTier 					= lDBEquipmentArmor_GetList [i].EquipArmor_Tier;
			armor.nQulity 					= lDBEquipmentArmor_GetList [i].EquipArmor_Qulity;
			armor.sJob 						= lDBEquipmentArmor_GetList [i].EquipArmor_Job;
			armor.nEnhanced 				= lDBEquipmentArmor_GetList [i].EquipArmor_Enhance;
			armor.sEquipType 				= lDBEquipmentArmor_GetList [i].EquipArmor_EquipType;
			armor.fPhysical_Defense 		= lDBEquipmentArmor_GetList [i].EquipArmor_PhysicalDefense;
			armor.fMagic_Defense 			= lDBEquipmentArmor_GetList [i].EquipArmor_MagicDefense;
			armor.nHp 						= lDBEquipmentArmor_GetList [i].EquipArmor_Hp;
			armor.nRandomOption 			= lDBEquipmentArmor_GetList [i].EquipArmor_RandomOption;
			armor.nSellCost 				= lDBEquipmentArmor_GetList [i].EquipArmor_SellCost;
			armor.nMakeMaterial 			= lDBEquipmentArmor_GetList [i].EquipArmor_MakeMaterial;
			armor.nBreakMaterial 			= lDBEquipmentArmor_GetList [i].EquipArmor_BreakMaterial;

			GameManager.Instance.lDBArmor.Add (armor);

			armor = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBEquipmentArmorPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA);
		break;
	case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA:
		DBGlove glove = new DBGlove();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBGlove.Capacity = nEquipmentGloveCount;
		for (int i = 0; i < nEquipmentGloveCount; i++)
		{
			glove = new DBGlove ();
			glove.nIndex 					= lDBEquipmentGlove_GetList [i].Index;
			glove.sName 					= lDBEquipmentGlove_GetList [i].EquipGlove_Name;
			glove.nTier 					= lDBEquipmentGlove_GetList [i].EquipGlove_Tier;
			glove.nQulity 					= lDBEquipmentGlove_GetList [i].EquipGlove_Qulity;
			glove.sJob 						= lDBEquipmentGlove_GetList [i].EquipGlove_Job;
			glove.nEnhanced 				= lDBEquipmentGlove_GetList [i].EquipGlove_Enhance;
			glove.sEquipType 				= lDBEquipmentGlove_GetList [i].EquipGlove_EquipType;
			glove.fPhysical_Defense 		= lDBEquipmentGlove_GetList [i].EquipGlove_PhysicalDefense;
			glove.fMagic_Defense 			= lDBEquipmentGlove_GetList [i].EquipGlove_MagicDefense;
			glove.nRandomOption 			= lDBEquipmentGlove_GetList [i].EquipGlove_RandomOption;
			glove.nSellCost 				= lDBEquipmentGlove_GetList [i].EquipGlove_SellCost;
			glove.nMakeMaterial 			= lDBEquipmentGlove_GetList [i].EquipGlove_MakeMaterial;
			glove.nBreakMaterial 			= lDBEquipmentGlove_GetList [i].EquipGlove_BreakMaterial;
			GameManager.Instance.lDBGlove.Add (glove);

			glove = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBEquipmentGlovePath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA);
		break;
	case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA:
		DBAccessory accessory = new DBAccessory();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBAccessory.Capacity = nEquipmentAccessoryCount;
		for (int i = 0; i < nEquipmentAccessoryCount; i++)
		{
			accessory = new DBAccessory ();
			accessory.nIndex 					= lDBEquipmentAccessory_GetList [i].Index;
			accessory.sName 					= lDBEquipmentAccessory_GetList [i].EquipAccessory_Name;
			accessory.nTier 					= lDBEquipmentAccessory_GetList [i].EquipAccessory_Tier;
			accessory.nQulity 					= lDBEquipmentAccessory_GetList [i].EquipAccessory_Qulity;
			accessory.sJob 						= lDBEquipmentAccessory_GetList [i].EquipAccessory_Job;
			accessory.nEnhanced 				= lDBEquipmentAccessory_GetList [i].EquipAccessory_Enhance;
			accessory.sEquipType 				= lDBEquipmentAccessory_GetList [i].EquipAccessory_EquipType;
			accessory.nRandomOption 			= lDBEquipmentAccessory_GetList [i].EquipAccessory_RandomOption;
			accessory.nSellCost 				= lDBEquipmentAccessory_GetList [i].EquipAccessory_SellCost;
			accessory.nMakeMaterial 			= lDBEquipmentAccessory_GetList [i].EquipAccessory_MakeMaterial;
			accessory.nBreakMaterial 			= lDBEquipmentAccessory_GetList [i].EquipAccessory_BreakMaterial;
			GameManager.Instance.lDBAccessory.Add (accessory);

			accessory = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBEquipmentAccessoryPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA);
		break;
	case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA:

		DBEquipment_RandomOption equipmentRandomOption = new DBEquipment_RandomOption();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBEquipmentRandomOption.Capacity = nEquipmentRandomOptionCount;
		for (int i = 0; i < nEquipmentAccessoryCount; i++)
		{
			equipmentRandomOption = new DBEquipment_RandomOption ();
			equipmentRandomOption.nIndex 					= lDBEquipmentRandomOption_GetList [i].Index;
			equipmentRandomOption.nOptionIndex 				= lDBEquipmentRandomOption_GetList [i].OptionIndex;
			equipmentRandomOption.nStartValue 				= lDBEquipmentRandomOption_GetList [i].nStartValue;
			equipmentRandomOption.nEndValue 				= lDBEquipmentRandomOption_GetList [i].nEndValue;
			GameManager.Instance.lDBEquipmentRandomOption.Add (equipmentRandomOption);

			equipmentRandomOption = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBEquipmentRandomOptionPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_STAGEDATA);

		break;

	case E_LOAD_STATE.E_LOAD_GET_STAGEDATA:

		DBStageData DBStage = new DBStageData();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBStageData.Capacity = nStageDataCount;
		for (int i = 0; i < nStageDataCount; i++)
		{
			DBStage = new DBStageData ();
			DBStage.nIndex 						= lDBStageData_GetList [i].Index;
			DBStage.strStageNumber 				= lDBStageData_GetList [i].StageNumber;
			DBStage.strStageName 				= lDBStageData_GetList [i].StageName;
			DBStage.strWaveTimes 				= lDBStageData_GetList [i].Wave;
			DBStage.strEnemySpawnIndexs 		= lDBStageData_GetList [i].EnemySpawn;
			DBStage.strCreateTimes 				= lDBStageData_GetList [i].CreateTime;
			DBStage.strYPositions 				= lDBStageData_GetList [i].YPosition;
			DBStage.nGold 						= lDBStageData_GetList [i].Gold;
			DBStage.fExp 						= lDBStageData_GetList [i].Exp;
			DBStage.strEquimnetIndexs 			= lDBStageData_GetList [i].Drop_Equipment;
			DBStage.strCharacterDropIndexs 		= lDBStageData_GetList [i].Drop_Character;
			DBStage.strMaterialDropIndexs 		= lDBStageData_GetList [i].Drop_Material;
			DBStage.strEquipmentRates 			= lDBStageData_GetList [i].Drop_EquipmentRates;
			DBStage.strCharacterDropRates 		= lDBStageData_GetList [i].Drop_CharacterRates;
			DBStage.strMaterialDropRates 		= lDBStageData_GetList [i].Drop_MaterialRates;
			DBStage.strBackground 				= lDBStageData_GetList [i].Background;

			GameManager.Instance.lDBStageData.Add (DBStage);

			DBStage = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBStageDataPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA);
		break;

	case E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA:

		DBCraftMaterial DBCraftMaterial = new DBCraftMaterial();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBCraftMaterial.Capacity = nCraftMaterialCount;
		for (int i = 0; i < nCraftMaterialCount; i++)
		{
			DBCraftMaterial = new DBCraftMaterial ();
			DBCraftMaterial.nIndex 					= lDBCraftMaterial_GetList [i].Index;
			DBCraftMaterial.nIron 					= lDBCraftMaterial_GetList [i].Iron;
			DBCraftMaterial.nFabric 				= lDBCraftMaterial_GetList [i].Fabric;
			DBCraftMaterial.nWood 					= lDBCraftMaterial_GetList [i].Wood;
			DBCraftMaterial.nWeaponStone 			= lDBCraftMaterial_GetList [i].WeaponStone;
			DBCraftMaterial.nArmorStone 			= lDBCraftMaterial_GetList [i].ArmorStone;
			DBCraftMaterial.nAccessoryStone 		= lDBCraftMaterial_GetList [i].AccessoryStone;
			DBCraftMaterial.nEpicStone 				= lDBCraftMaterial_GetList [i].EpicStone;
			DBCraftMaterial.nGoldCost 				= lDBCraftMaterial_GetList [i].GoldCost;
			DBCraftMaterial.nTier 					= lDBCraftMaterial_GetList [i].Tier;
			DBCraftMaterial.Qulity 					= lDBCraftMaterial_GetList [i].Qulity;
			DBCraftMaterial.strEquipType 			= lDBCraftMaterial_GetList [i].EquipType;

			GameManager.Instance.lDBCraftMaterial.Add (DBCraftMaterial);

			DBCraftMaterial = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBCraftMaterialPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA);
		break;

	case E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA:

		DBBreakMaterial DBBreakMaterial = new DBBreakMaterial();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBBreakMaterial.Capacity = nBreakMaterialCount;
		for (int i = 0; i < nBreakMaterialCount; i++)
		{
			DBBreakMaterial = new DBBreakMaterial ();
			DBBreakMaterial.nIndex 					= lDBBreakMaterial_GetList [i].Index;
			DBBreakMaterial.nIron 						= lDBBreakMaterial_GetList [i].Iron;
			DBBreakMaterial.nFabric 					= lDBBreakMaterial_GetList [i].Fabric;
			DBBreakMaterial.nWood 						= lDBBreakMaterial_GetList [i].Wood;
			DBBreakMaterial.nWeaponStone 				= lDBBreakMaterial_GetList [i].WeaponStone;
			DBBreakMaterial.nArmorStone 				= lDBBreakMaterial_GetList [i].ArmorStone;
			DBBreakMaterial.nAccessoryStone 			= lDBBreakMaterial_GetList [i].AccessoryStone;
			DBBreakMaterial.nEpicStone 				= lDBBreakMaterial_GetList [i].EpicStone;

			GameManager.Instance.lDBBreakMaterial.Add (DBBreakMaterial);

			DBBreakMaterial = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBBreakMaterialPath, _state);
		//다음 데이터 불러오기
		LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA);
		break;


	case E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA:

		DBFormationSkill DBFormationSkills = new DBFormationSkill();
		//용량 설정 (캐릭터의 개수 만큼) 
		GameManager.Instance.lDBFomationSkill.Capacity = nFormationSkillCount;
		for (int i = 0; i < nBreakMaterialCount; i++)
		{
			DBFormationSkills = new DBFormationSkill ();
			DBFormationSkills.nIndex 					= lDBFormationSkill_GetList [i].Index;
			DBFormationSkills.nCharacterIndex 			= lDBFormationSkill_GetList [i].C_Index;
			DBFormationSkills.strName 					= lDBFormationSkill_GetList [i].FormationSkill_Name;
			DBFormationSkills.strSkillType 				= lDBFormationSkill_GetList [i].FormationSkill_Type;
			DBFormationSkills.nSkillClass 				= lDBFormationSkill_GetList [i].FormationSkill_Class;
			DBFormationSkills.nTier 					= lDBFormationSkill_GetList [i].FormationSkill_Tier;
			DBFormationSkills.strFomationTarget 		= lDBFormationSkill_GetList [i].FormationSkill_Target;
			DBFormationSkills.nOptionIndex 				= lDBFormationSkill_GetList [i].FormationSkill_OptionIndex;
			DBFormationSkills.strExplanation 			= lDBFormationSkill_GetList [i].FormationSkill_Explanation;


			GameManager.Instance.lDBFomationSkill.Add (DBFormationSkills);

			DBFormationSkills = null;
		}
		//정렬
		ListAdjustSort (_state);
		//로컬 저장
		SaveAndLoadBinaryFile (sDBFormationSkillPath, _state);
        //다음 데이터 불러오기
        LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_MATERIALDATA);
        break;


        case E_LOAD_STATE.E_LOAD_GET_MATERIALDATA:

                DBMaterialData DBMaterialData = new DBMaterialData();
                //용량 설정 (캐릭터의 개수 만큼) 
                GameManager.Instance.lDBMaterialData.Capacity = nMaterialDataCount;
                for (int i = 0; i < nBreakMaterialCount; i++)
                {
                    DBMaterialData = new DBMaterialData();
                    DBMaterialData.nIndex = lDBMaterialData_GetList[i].Index;
                    DBMaterialData.sMaterialName = lDBMaterialData_GetList[i].MaterialName;
                    DBMaterialData.sImagePath = lDBMaterialData_GetList[i].MaterialImagePath;
                    DBMaterialData.sExplanation = lDBMaterialData_GetList[i].MaterialExplanation;
                    GameManager.Instance.lDBMaterialData.Add(DBMaterialData);

                    DBFormationSkills = null;
                }
                //정렬
                ListAdjustSort(_state);
                //로컬 저장
                SaveAndLoadBinaryFile(sDBMaterialDataPath, _state);
                //다음 데이터 불러오기
                LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_CALENDARDATA);

                break;

            case E_LOAD_STATE.E_LOAD_GET_CALENDARDATA:

                DBCalendar DBCalendar = new DBCalendar();
                //용량 설정 (캐릭터의 개수 만큼) 
                GameManager.Instance.lDBCalendar.Capacity = nCalendarCount;
                for (int i = 0; i < nCalendarCount; i++)
                {
                    DBCalendar = new DBCalendar();
                    DBCalendar.nIndex = lDBCalendar_GetList[i].Index;
                    DBCalendar.nGold = lDBCalendar_GetList[i].Gold;
                    DBCalendar.nGem = lDBCalendar_GetList[i].Gem;
                    DBCalendar.nWeaponTicket = lDBCalendar_GetList[i].WeaponTicket;
                    DBCalendar.nCharacterTicket = lDBCalendar_GetList[i].CharacterTicket;
                    DBCalendar.nWeapon = lDBCalendar_GetList[i].Weapon;
                    DBCalendar.nCharacter = lDBCalendar_GetList[i].Character;
                    DBCalendar.nIron = lDBCalendar_GetList[i].Iron;
                    DBCalendar.nFabric = lDBCalendar_GetList[i].Fabric;
                    DBCalendar.nWood = lDBCalendar_GetList[i].Wood;
                    DBCalendar.nWeaponStone = lDBCalendar_GetList[i].WeaponStone;
                    DBCalendar.ArmorStone = lDBCalendar_GetList[i].ArmorStone;
                    DBCalendar.AccessoryStone = lDBCalendar_GetList[i].AccessoryStone;

                    GameManager.Instance.lDBCalendar.Add(DBCalendar);

                    DBCalendar = null;
                }
                //정렬
                ListAdjustSort(_state);
                //로컬 저장
                SaveAndLoadBinaryFile(sDBCalendarPath, _state);
                //다음 데이터 불러오기
                LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA);
                break;

            case E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA:

                DBCharacterTicket DBCharacterTicket = new DBCharacterTicket();
                //용량 설정 (캐릭터의 개수 만큼) 
                GameManager.Instance.lDBCharacterTicket.Capacity = nCharacterTicketCount;
                for (int i = 0; i < nCharacterTicketCount; i++)
                {
                    DBCharacterTicket = new DBCharacterTicket();
                    DBCharacterTicket.nIndex = lDBCharacterTicket_GetList[i].Index;
                    DBCharacterTicket.sName = lDBCharacterTicket_GetList[i].Name;
                    DBCharacterTicket.sJob = lDBCharacterTicket_GetList[i].Job;
                    DBCharacterTicket.sPercentage = lDBCharacterTicket_GetList[i].Percentage;
                    DBCharacterTicket.sTier = lDBCharacterTicket_GetList[i].Tier;
                    DBCharacterTicket.sTribe = lDBCharacterTicket_GetList[i].Tribe;
                    DBCharacterTicket.sImage = lDBCharacterTicket_GetList[i].Image;
                    DBCharacterTicket.sExplanation = lDBCharacterTicket_GetList[i].Explanation;
                    GameManager.Instance.lDBCharacterTicket.Add(DBCharacterTicket);

                    DBCharacterTicket = null;
                }
                //정렬
                ListAdjustSort(_state);
                //로컬 저장
                SaveAndLoadBinaryFile(sDBCharacterTickectPath, _state);
                //다음 데이터 불러오기
                LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA);
                break;

            case E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA:

                DBWeaponTicket DBWeaponTicket = new DBWeaponTicket();
                //용량 설정 (캐릭터의 개수 만큼) 
                GameManager.Instance.lDBWeaponTicket.Capacity = nWeaponTicketCount;
                for (int i = 0; i < nWeaponTicketCount; i++)
                {
                    DBWeaponTicket = new DBWeaponTicket();
                    DBWeaponTicket.nIndex       = lDBWeaponTicket_GetList[i].Index;
                    DBWeaponTicket.sName        = lDBWeaponTicket_GetList[i].Name;
                    DBWeaponTicket.sEquipType   = lDBWeaponTicket_GetList[i].EquipType;
                    DBWeaponTicket.sPercentage  = lDBWeaponTicket_GetList[i].Percentage;
                    DBWeaponTicket.sTier        = lDBWeaponTicket_GetList[i].Tier;
                    DBWeaponTicket.sQulity      = lDBWeaponTicket_GetList[i].Qulity;
                    DBWeaponTicket.sImage       = lDBWeaponTicket_GetList[i].Image;
                    DBWeaponTicket.sExplanation = lDBWeaponTicket_GetList[i].Explanation;
                    GameManager.Instance.lDBWeaponTicket.Add(DBWeaponTicket);

                    DBWeaponTicket = null;
                }
                //정렬
                ListAdjustSort(_state);
                //로컬 저장
                SaveAndLoadBinaryFile(sDBWeaponTicketPath, _state);
                //다음 데이터 불러오기
                LoadBasicDataSequence(E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA);
                break;

            case E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA:

                DBEmployGacha DBEmployGacha = new DBEmployGacha();
                //용량 설정 (캐릭터의 개수 만큼) 
                GameManager.Instance.lDBEmployGacha.Capacity = nEmployGachaCount;
                for (int i = 0; i < nEmployGachaCount; i++)
                {
                    DBEmployGacha = new DBEmployGacha();
                    DBEmployGacha.nIndex = lDBEmployGacha_GetList[i].Index;
                    DBEmployGacha.sName = lDBEmployGacha_GetList[i].Name;
                    DBEmployGacha.sJob = lDBEmployGacha_GetList[i].Job;
                    DBEmployGacha.sPercentage = lDBEmployGacha_GetList[i].Percentage;
                    DBEmployGacha.sTier = lDBEmployGacha_GetList[i].Tier;
                    DBEmployGacha.sTribe = lDBEmployGacha_GetList[i].Tribe;
                    DBEmployGacha.nCost_Gold = lDBEmployGacha_GetList[i].Cost_Gold;
                    DBEmployGacha.nCost_Gem = lDBEmployGacha_GetList[i].Cost_Gem;
                    DBEmployGacha.sImage = lDBEmployGacha_GetList[i].Image;
                    DBEmployGacha.sExplanation = lDBEmployGacha_GetList[i].Explanation;

                    GameManager.Instance.lDBEmployGacha.Add(DBEmployGacha);

                    DBEmployGacha = null;
                }
                //정렬
                ListAdjustSort(_state);
                //로컬 저장
                SaveAndLoadBinaryFile(sDBEmployGachaPath, _state);
              
                break;
            default:
		        break;
	}


}

public void SaveAndLoadBinaryFile(string _path, E_LOAD_STATE _loadState)
{

	//저장된 캐릭에 대한 정보가 없다면 바이너리화 하여 저장
	if (!File.Exists (Application.persistentDataPath +_path))
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = new FileStream (Application.persistentDataPath + _path, FileMode.Create);

		switch (_loadState)
		{
		        case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:

		        	bf.Serialize (fileStream, GameManager.Instance.lDbBasicCharacter);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDbActiveSkill);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDbActiveSkillType);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDbPassiveSkill);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDbPassiveSkillOptionIndex);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDbBasickill);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDbWeapon);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBArmor);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBGlove);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBAccessory);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBEquipmentRandomOption);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_STAGEDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBStageData);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBCraftMaterial);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBBreakMaterial);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA:
		        	bf.Serialize (fileStream, GameManager.Instance.lDBFomationSkill);
		        	break;
                case E_LOAD_STATE.E_LOAD_GET_MATERIALDATA:
                    bf.Serialize(fileStream, GameManager.Instance.lDBMaterialData);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_CALENDARDATA:
                    bf.Serialize(fileStream, GameManager.Instance.lDBCalendar);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA:
                    bf.Serialize(fileStream, GameManager.Instance.lDBCharacterTicket);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA:
                    bf.Serialize(fileStream, GameManager.Instance.lDBWeaponTicket);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA:
                    bf.Serialize(fileStream, GameManager.Instance.lDBEmployGacha);
                    bIsFinishLoadDate = true;
                    break;
                default:
			break;
		}

		fileStream.Close ();

		Debug.Log ("Saved In Binary Data");
	}
	//있으면 해당 경로에서 로드
	else
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = new FileStream (Application.persistentDataPath + _path, FileMode.Open);

		switch (_loadState)
		{
		        case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:

		        	GameManager.Instance.lDbBasicCharacter = (List<DBBasicCharacter>)bf.Deserialize (fileStream);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA:
		        	GameManager.Instance.lDbActiveSkill = (List<DBActiveSkill>)bf.Deserialize(fileStream);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA:
		        	GameManager.Instance.lDbActiveSkillType = (List<DBActiveSkillType>)bf.Deserialize(fileStream);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA:
		        	GameManager.Instance.lDbPassiveSkill = (List<DBPassiveSkill>)bf.Deserialize(fileStream);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA:
		        	GameManager.Instance.lDbPassiveSkillOptionIndex = (List<DBPassiveSkillOptionIndex>)bf.Deserialize(fileStream);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA:
		        	GameManager.Instance.lDbBasickill = (List<DBBasicSkill>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA:
		        	GameManager.Instance.lDbWeapon = (List<DBWeapon>)bf.Deserialize (fileStream);

		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA:
		        	GameManager.Instance.lDBArmor = (List<DBArmor>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA:
		        	GameManager.Instance.lDBGlove = (List<DBGlove>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA:
		        	GameManager.Instance.lDBAccessory = (List<DBAccessory>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA:
		        	GameManager.Instance.lDBEquipmentRandomOption = (List<DBEquipment_RandomOption>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_STAGEDATA:
		        	GameManager.Instance.lDBStageData = (List<DBStageData>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA:
		        	GameManager.Instance.lDBCraftMaterial = (List<DBCraftMaterial>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA:
		        	GameManager.Instance.lDBBreakMaterial = (List<DBBreakMaterial>)bf.Deserialize (fileStream);
		        	break;
		        case E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA:
		        	GameManager.Instance.lDBFomationSkill = (List<DBFormationSkill>)bf.Deserialize (fileStream);
		        	break;
                case E_LOAD_STATE.E_LOAD_GET_MATERIALDATA:
                    GameManager.Instance.lDBMaterialData = (List<DBMaterialData>)bf.Deserialize(fileStream);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_CALENDARDATA:
                    GameManager.Instance.lDBCalendar = (List<DBCalendar>)bf.Deserialize(fileStream);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA:
                    GameManager.Instance.lDBCharacterTicket = (List<DBCharacterTicket>)bf.Deserialize(fileStream);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA:
                    GameManager.Instance.lDBWeaponTicket = (List<DBWeaponTicket>)bf.Deserialize(fileStream);
                    break;
                case E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA:
                    GameManager.Instance.lDBEmployGacha = (List<DBEmployGacha>)bf.Deserialize(fileStream);
                    bIsFinishLoadDate = true;
                    break;

                default:
			break;
		}

		fileStream.Close ();

		Debug.Log (_loadState.ToString() +  "Load In Binary Data");
		totalSlider.value++;

	}


}
//모든 데이터 로드가 끝났는지 아닌지 계속해서 체크
IEnumerator CheckBasicDataLoadEnd()
{
	yield return new WaitForSeconds (0.1f);

	while (true) 
	{
		if(bIsFinishLoadDate== true)
		{
                //Test();
                //로그인 관련 패널 온(구글, 페이스북, Guest)
                //LoginCategory_Panel.SetActive(true);
                ////로딩바 false
                //TestUserData (김스맷_0)
                StartGetPlayersData();
                StartCoroutine(CheckIsPlayerDataLoaded());
                //GetTestPlayerData();
                progressBar_Panel.SetActive(false);
			    break;
		}
		else
			yield return null;
	}

	yield return null;

}

//List 정렬 (Index 기준)
private void ListAdjustSort(E_LOAD_STATE _loadState)
{
		switch (_loadState) {
		    case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:

		    	GameManager.Instance.lDbBasicCharacter.Sort (delegate(DBBasicCharacter A, DBBasicCharacter B) {
		    		if (A.Index > B.Index)
		    			return 1;
		    		else if (A.Index < B.Index)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("BasicCharacter Sort Confirm!!");
		    	break;
		    case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA:
		    	GameManager.Instance.lDbActiveSkill.Sort (delegate(DBActiveSkill A, DBActiveSkill B) {
		    		if (A.m_nIndex > B.m_nIndex)
		    			return 1;
		    		else if (A.m_nIndex < B.m_nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("ActiveSkill Sort Confirm!!");

		    	break;
		    case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA:

		    	GameManager.Instance.lDbActiveSkillType.Sort (delegate(DBActiveSkillType A, DBActiveSkillType B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("ActiveSkillType Sort Confirm!!");
		    	break;
		    case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA:
		    	GameManager.Instance.lDbPassiveSkill.Sort (delegate(DBPassiveSkill A, DBPassiveSkill B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("PassiveSkill Sort Confirm!!");
		    	break;
		    case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA:
		    	GameManager.Instance.lDbPassiveSkillOptionIndex.Sort (delegate(DBPassiveSkillOptionIndex A, DBPassiveSkillOptionIndex B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("PassiveSkillOptionIndex Sort Confirm!!");
		    	break;
		    case E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA:
		    	GameManager.Instance.lDbBasickill.Sort (delegate(DBBasicSkill A, DBBasicSkill B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("BasicSkill Sort Confirm!!");
		    	break;

		    case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA:
		    	GameManager.Instance.lDbWeapon.Sort (delegate(DBWeapon A, DBWeapon B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("DBweapon Sort Confirm!!");
		    	break;


		    case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA:
		    	GameManager.Instance.lDBArmor.Sort (delegate(DBArmor A, DBArmor B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("DBArmor Sort Confirm!!");
		    	break;


		    case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA:
		    	GameManager.Instance.lDBGlove.Sort (delegate(DBGlove A, DBGlove B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("DBGlove Sort Confirm!!");
		    	break;


		    case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA:
		    	GameManager.Instance.lDBAccessory.Sort (delegate(DBAccessory A, DBAccessory B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("DBAccessory Sort Confirm!!");
		    	break;


		    case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA:
		    	GameManager.Instance.lDBEquipmentRandomOption.Sort (delegate(DBEquipment_RandomOption A, DBEquipment_RandomOption B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("DBEquipmet_RandomOption Sort Confirm!!");
		    	break;



		    case E_LOAD_STATE.E_LOAD_GET_STAGEDATA:
		    	GameManager.Instance.lDBStageData.Sort (delegate(DBStageData A, DBStageData B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("StageData Sort Confirm!!");
		    	break;


		    case E_LOAD_STATE.E_LOAD_GET_CRAFTMATERIALDATA:
		    	GameManager.Instance.lDBCraftMaterial.Sort (delegate(DBCraftMaterial A, DBCraftMaterial B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("CraftMaterial Sort Confirm!!");
		    	break;
		    case E_LOAD_STATE.E_LOAD_GET_BREAKMATERIALDATA:
		    	GameManager.Instance.lDBBreakMaterial.Sort (delegate(DBBreakMaterial A, DBBreakMaterial B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("BreakMaterial Sort Confirm!!");
		    	break;

		    case E_LOAD_STATE.E_LOAD_GET_FORMATIONSKILLDATA:
		    	GameManager.Instance.lDBFomationSkill.Sort (delegate(DBFormationSkill A, DBFormationSkill B) {
		    		if (A.nIndex > B.nIndex)
		    			return 1;
		    		else if (A.nIndex < B.nIndex)
		    			return -1;
		    		return 0;
		    	});
		    	Debug.Log ("FormationSkill Sort Confirm!!");
		    	break;
            case E_LOAD_STATE.E_LOAD_GET_MATERIALDATA:
                GameManager.Instance.lDBMaterialData.Sort(delegate (DBMaterialData A, DBMaterialData B) {
                    if (A.nIndex > B.nIndex)
                        return 1;
                    else if (A.nIndex < B.nIndex)
                        return -1;
                    return 0;
                });
                Debug.Log("MaterialData Sort Confirm!!");
                break;
            case E_LOAD_STATE.E_LOAD_GET_CALENDARDATA:
                GameManager.Instance.lDBCalendar.Sort(delegate (DBCalendar A, DBCalendar B) {
                    if (A.nIndex > B.nIndex)
                        return 1;
                    else if (A.nIndex < B.nIndex)
                        return -1;
                    return 0;
                });
                Debug.Log("Calendar Sort Confirm!!");
                break;
            case E_LOAD_STATE.E_LOAD_GET_CHARACTERTICKETDATA:
                GameManager.Instance.lDBCharacterTicket.Sort(delegate (DBCharacterTicket A, DBCharacterTicket B) {
                    if (A.nIndex > B.nIndex)
                        return 1;
                    else if (A.nIndex < B.nIndex)
                        return -1;
                    return 0;
                });
                Debug.Log("CharacterTicket Sort Confirm!!");
                break;
            case E_LOAD_STATE.E_LOAD_GET_WEAPONTICKETDATA:
                GameManager.Instance.lDBWeaponTicket.Sort(delegate (DBWeaponTicket A, DBWeaponTicket B) {
                    if (A.nIndex > B.nIndex)
                        return 1;
                    else if (A.nIndex < B.nIndex)
                        return -1;
                    return 0;
                });
                Debug.Log("WeaponTicket Sort Confirm!!");
                break;
            case E_LOAD_STATE.E_LOAD_GET_EMPLOYGACHADATA:
                GameManager.Instance.lDBEmployGacha.Sort(delegate (DBEmployGacha A, DBEmployGacha B) {
                    if (A.nIndex > B.nIndex)
                        return 1;
                    else if (A.nIndex < B.nIndex)
                        return -1;
                    return 0;
                });
                Debug.Log("EmployGacha Sort Confirm!!");
                break;
            default:
			break;
		}
}
#endregion

		
	#region GoogleLogin

	public void GoogleLogin()
	{

		Debug.Log ("trying to login Google");
		Social.localUser.Authenticate ((bool success) => {
			if (success) 
			{
				Debug.Log("Success To Login Google");
				//AccessTokken_GP = ((PlayGamesLocalUser)Social.localUser).GetIdToken ();
				StartCoroutine(GetGoogleToken());
			} 
			else {
				Debug.Log("Login failed for some reason");
				return;
			}
		});

	}


	IEnumerator GetGoogleToken()
	{
		Debug.Log ("Start Getting google Token....");
		while (true) 
		{
			if (string.IsNullOrEmpty (((PlayGamesLocalUser)Social.localUser).GetIdToken ())) 
			{
				yield return null;
			}
			else
			{
				Debug.Log("Email : " + ((PlayGamesLocalUser)Social.localUser).Email);
				Debug.Log("UserName : " + ((PlayGamesLocalUser)Social.localUser).userName);
				Debug.Log ("구글 토큰 : " + ((PlayGamesLocalUser)Social.localUser).GetIdToken ());

				//여기서 처음 접속한 플레이어와 연동이 된 플레이어가 갈린다.
				AccessTokken_GP = PlayGamesPlatform.Instance.GetIdToken();
				//Cognito Check 해당 유저가 이전에 연동을 했었는지

				//DynamoDBCheck

				//데이터셋을 만들 곳

				eLoginProviderIndex = E_LOGIN_PORVIDER_INDEX.E_GOOGLE;

				//awsCurrentProvider_text.text = Credentials.
				Credentials.AddLogin ("accounts.google.com", AccessTokken_GP);

				playerInfo = SyncManager.OpenOrCreateDataset("PlayerInfo");
                

				playerInfo.OnSyncSuccess += this.HandleSyncSuccess; 		// OnSyncSucess uses events/delegates pattern
				playerInfo.OnSyncFailure += this.HandleSyncFailure; 		// OnSyncFailure uses events/delegates pattern
				playerInfo.OnSyncConflict = this.HandleSyncConflict;
				playerInfo.OnDatasetMerged = this.HandleDatasetMerged;
				playerInfo.OnDatasetDeleted = this.HandleDatasetDeleted;

				sEmail = ((PlayGamesLocalUser)Social.localUser).Email;
				sNick = ((PlayGamesLocalUser)Social.localUser).userName;

                //GetPlayerCount
                //StartCoroutine (GetPlayerCountFromDB ());

                nickInputObj.SetActive(true);
                //이메일 비교하여 접속한적이 있는지 없는지 비교
                if (sEmail == playerInfo.Get ("Email")) 
				{
					//player DataLoad

				}
				//접속한 적이 있으면 Data Load -> playerInfo
				else 
				{
                    //없다면 닉을 치는 창을 띄움
                   
                }

				break;
			}
		}
	}
	#endregion

	#region FaceBook
	public void FaceBookLogin()
	{
		FB.LogInWithReadPermissions (new List<string>{ "email" }, LoginCallBack);
	}
	public void FaceBookLogOut()
	{
		FB.LogOut ();
		//FBName_Text.text = "Facebook_LogOut!";
	}

	void LoginCallBack(ILoginResult result)
	{
		Debug.Log ("-----------------LoginCallBack FaceBookLogin--------------------");
		Debug.Log (result.AccessToken.UserId);

		LoginFacebook (result.AccessToken.TokenString);

		if (string.IsNullOrEmpty (result.Error))
		{

		}
		else 
		{
			Debug.Log ("Error during login : " + result.Error);
			//statusShow_text.text = "Error during login : " + result.Error;
		}
	}

	public void LoginFacebook(string token)
	{
		Debug.Log ("-----------------LoginTo FaceBookLogin--------------------");
		//statusShow_text.text = "Login....";

		//Credentials.AddLogin ("graph.facebook.com", token);
		//FB.API ("me/picture?width=100&height=100", HttpMethod.GET, PictureCallBack);
		FB.API ("me?fields=first_name", HttpMethod.GET, NameCallBack);
		//isLoggedIn = true;

		if (FB.IsLoggedIn)
		{
			//Credentials.AddLogin ("graph.facebook.com", );
			//statusShow_text.text = "AccessToFaceBook...";
			//FBHasLogin (AccessTokken, AccessId);
			//Credentials.AddLogin ("graph.facebook.com", token);
			//FB.API ("me/picture?width=100&height=100", HttpMethod.GET, PictureCallBack);
			//FB.API ("me?fields=first_name", HttpMethod.GET, NameCallBack);
			//isLoggedIn = true;
			//awsCurrentProvider_text.text = Credentials.CurrentLoginProviders;
		} 
		else 
		{
			//NotLoggedInUI.SetActive (true);
			//LoggedInUI.SetActive (false);
			//isLoggedIn = false;
		}
	}


	void NameCallBack(IGraphResult result)
	{
		IDictionary<string,object>  profil = result.ResultDictionary;
		//FBName_Text.text = "Hello " + profil["first_name"].ToString();
	}
	#endregion

	#region Sync events
	private bool HandleDatasetDeleted(Dataset dataset)
	{

		//statusMessage = dataset.Metadata.DatasetName + "Dataset has been deleted.";
		Debug.Log(dataset.Metadata.DatasetName + " Dataset has been deleted");

		// Clean up if necessary 

		// returning true informs the corresponding dataset can be purged in the local storage and return false retains the local dataset
		return true;
	}

	public bool HandleDatasetMerged(Dataset dataset, List<string> datasetNames)
	{
		//statusMessage = "Merging datasets between different identities.";
		Debug.Log(dataset + " Dataset needs merge");
		// returning true allows the Synchronize to resume and false cancels it
		return true;
	}

	private bool HandleSyncConflict(Amazon.CognitoSync.SyncManager.Dataset dataset, List<SyncConflict> conflicts)
	{

		//statusMessage = "Handling Sync Conflicts.";
		Debug.Log("OnSyncConflict");
		List<Amazon.CognitoSync.SyncManager.Record> resolvedRecords = new List<Amazon.CognitoSync.SyncManager.Record>();

		foreach (SyncConflict conflictRecord in conflicts)
		{
			// This example resolves all the conflicts using ResolveWithRemoteRecord 
			// SyncManager provides the following default conflict resolution methods:
			//      ResolveWithRemoteRecord - overwrites the local with remote records
			//      ResolveWithLocalRecord - overwrites the remote with local records
			//      ResolveWithValue - for developer logic  
			resolvedRecords.Add(conflictRecord.ResolveWithRemoteRecord());
		}

		// resolves the conflicts in local storage
		dataset.Resolve(resolvedRecords);

		// on return true the synchronize operation continues where it left,
		//      returning false cancels the synchronize operation
		return true;
	}

	private void HandleSyncSuccess(object sender, SyncSuccessEventArgs e)
	{

		var dataset = sender as Dataset;

		if (dataset.Metadata != null) {
			Debug.Log("Successfully synced for dataset: " + dataset.Metadata);
		} else {
			Debug.Log("Successfully synced for dataset");
		}

		//Get CharacterList
		//CharacterDBLoadAndPutOperation ();


		/*
		if (dataset == playerInfo)
		{
			//alias = string.IsNullOrEmpty(playerInfo.Get("alias")) ? "Enter your alias" : dataset.Get("alias");
			//playerName = string.IsNullOrEmpty(playerInfo.Get("playerName")) ? "Enter your name" : dataset.Get("playerName");
		}
		*/
		Debug.Log ("Syncing to CognitoSync Cloud succeeded");
	}

	private void HandleSyncFailure(object sender, SyncFailureEventArgs e)
	{
		var dataset = sender as Dataset;
		Debug.Log("Sync failed for dataset : " + dataset.Metadata.DatasetName);
		Debug.LogException(e.Exception);

		//statusMessage = "Syncing to CognitoSync Cloud failed";
	}
    #endregion


    #region Public Authentication Providers
#if USE_FACEBOOK_LOGIN
	private void FacebookLoginCallback(FBResult result)
	{
	Debug.Log("FB.Login completed");

	if (result.Error != null || !FB.IsLoggedIn)
	{
	Debug.LogError(result.Error);
	statusMessage = result.Error;
	}
	else
	{
	Debug.Log(result.Text);
	Credentials.AddLogin ("graph.facebook.com", FB.AccessToken);
	}
	}

#endif
    #endregion



    //특정플레이어를 검색하기 위한 테이블
    [DynamoDBTable("PlayersInfoTable_Personal")]
    public class DBPlayersCharacter_Personal_ForGet
    {
        [DynamoDBHashKey]
        public string UserNick { get; set; }
        [DynamoDBProperty("UserEmail")]
        public string UserEamil { get; set; }               // Hash key.
        [DynamoDBProperty("CharactersInfo")]
        public List<DBBasicCharacter> Characters { get; set; }
        [DynamoDBProperty("UserMailInfo")]
        public List<Mail> mail { get; set; }
    }


    [DynamoDBTable("CharacterBasicInfoTable")]
	public class DBBaiscCharacter_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 						// Hash key.


		[DynamoDBProperty("C_Index")]
		public int C_Index { get; set; }					// CharacterIndex  

		[DynamoDBProperty("C_JobIndex")]
		public int C_JobIndex { get; set;}				// Character 직업 이름

		[DynamoDBProperty("C_JobName")]
		public string C_JobNames { get; set;}				// Character 직업 이름

		[DynamoDBProperty("C_Names")]
		public string C_Name { get; set;}					// Character Name

		[DynamoDBProperty("C_Enhances")]
		public int C_Enhance { get; set; }					// Character 강화 단계

		[DynamoDBProperty("Jobs")]
		public string Jobs {get;set;}							// 직업 이름

		[DynamoDBProperty("Level")]
		public int Levels { get; set;}						// Character Level

		[DynamoDBProperty("Tier")]
		public int Tier { get; set;}						// Chracter Tier

		[DynamoDBProperty("Attribute")]
		public int Attribute { get; set; }					// Character 특성 (물리, 마법, None)

		[DynamoDBProperty("AttackType")]
		public int AttackType { get; set;}					// 근거리, 원거리 타입

		[DynamoDBProperty("Tribe")]
		public int Tribe { get; set;}						// Character 종족

		[DynamoDBProperty("Site")]
		public float Site { get; set; }						// Character 인지범위

		[DynamoDBProperty("Health")]
		public float Health { get; set;}					// Character 체력

		[DynamoDBProperty("Accurancy")]
		public float Accurancy { get; set;}					// Character 정확도

		[DynamoDBProperty("AttackRange")]
		public float AttackRange { get; set;}				// Character 공격 사거리

		[DynamoDBProperty("Physic_AttackRating")]
		public float Physic_AttackRating { get; set;}		// Character 물리 공격력

		[DynamoDBProperty("Magic_AttackRating")]
		public float Magic_AttackRating { get; set;}		// Character 마법 공격력

		[DynamoDBProperty("AttackSpped")]
		public float AttackSpeed { get; set;}				// Character 공격 속도

		[DynamoDBProperty("MoveSpeed")]
		public float MoveSpeed { get; set;}					// Character 이동 속도

		[DynamoDBProperty("Physic_Defense")]
		public float Physic_Defense { get; set;}			// Character 물리 방어력

		[DynamoDBProperty("Magic_Defense")]
		public float Magic_Defense { get; set; }			// Character 마법 방어력

		[DynamoDBProperty("Dodge")]
		public float Dodge { get; set;}						// Character 회피력

		[DynamoDBProperty("Crit_Rating")]
		public float Crit_Rating { get; set; }				// Character 크리 확률

		[DynamoDBProperty("Crit_Dmg")]
		public float Crit_Dmg { get; set; }					// Character 크리 데미지

		[DynamoDBProperty("Physic_Penetrate")]				
		public float Physic_Penetrate {get; set;}			// Character 물리 관통

		[DynamoDBProperty("Magic_Penetrate")]			
		public float Magic_Penetrate {get; set;}			// Character 마법 관통

		[DynamoDBProperty("CC_Registance")]
		public float CC_Registance {	get; set; }			// Character 상태이상 저항

		[DynamoDBProperty("Exp")]
		public float Exp {	get; set; }						// Character 현재 경험치

		[DynamoDBProperty("ExpMax")]
		public float ExpMax {	get; set; }					// Character 최대 경험치

		[DynamoDBProperty("Betch_Index")]
		public int Betch_Index {	get; set; }             // Character 배치위치 

        [DynamoDBProperty("Stamina")]
        public int Stamina { get; set; }                    // Character 스태미나

        [DynamoDBProperty("Image")]
        public string Image { get; set; }                    // Character 이미지
        
        [DynamoDBProperty("Favorite")]
        public int Favorite { get; set; }                    // Favorite 이미지

        [DynamoDBProperty("BasicSkill")]				
		public List<DBBasicSkill> basicSkill {get; set;}

		[DynamoDBProperty("ActiveSkill")]				
		public List<DBActiveSkill> activeSkills {get; set;}

		[DynamoDBProperty("PassiveSkill")]
		public List<DBPassiveSkill> passiveSkills {get; set;}

	}
   



    [DynamoDBTable("CharacterActiveSkillList")]
	public class DBActiveSkill_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("C_Index")]
		public int C_Index { get; set; }							// CharacterIndex 

		[DynamoDBProperty("Skill_Name")]
		public string	Skill_Name { get; set;}						// SkillName

		[DynamoDBProperty("Skill_Type")]
		public string Skill_Type { get; set;}						// SkillType

		[DynamoDBProperty("Skill_Class")]
		public int Skill_Class { get; set; }						// SkillClass

		[DynamoDBProperty("Tier")]
		public int Tier { get; set;}								// Skill Tier

		[DynamoDBProperty("Jobs")]
		public string Jobs {get;set;}								// Skill 사용 가능한 직업

		[DynamoDBProperty("Level")]
		public int Levels { get; set;}								// Skill Level

		[DynamoDBProperty("Skill_Attribute")]
		public int Skill_Attribute { get; set; }					// Skill 특성 (물리, 마법, None)

		[DynamoDBProperty("Skill_AttackType")]
		public int Skill_AttackType { get; set;}					// 근거리, 원거리 타입

		[DynamoDBProperty("Skill_Attack_ActiveRating")]
		public float Skill_AttackActiveRating { get; set;}			// Skill 공격시 발동 확률

		[DynamoDBProperty("Skill_CriAttack_ActiveRating")]
		public float Skill_CriAttackActiveRating { get; set;}		// Skill 공격시 크리티컬 발동 확률

		[DynamoDBProperty("Skill_AttackCount_ActiveRating")]
		public int Skill_AttackCountActiveRating { get; set;}		// Skill n번 공격시  발동 확률

		[DynamoDBProperty("Skill_Miss_ActiveRating")]
		public float Skill_MissActiveRating { get; set;}			// Skill 빗나갈시  발동 확률

		[DynamoDBProperty("Skill_Dodge_ActiveRating")]
		public float Skill_DodgeActiveRating { get; set;}			// Skill 회피시 발동 확률

		[DynamoDBProperty("Skill_Hit_ActiveRating")]
		public float Skill_HitActiveRating { get; set;}				// Skill 맞을시 발동 확률

		[DynamoDBProperty("Skill_CoolTime")]
		public float Skill_CoolTime { get; set; }					// Skill CoolTime

		[DynamoDBProperty("Skill_CastTime")]
		public float Skill_CastTime { get; set;}					// Skill CastingTime

		[DynamoDBProperty("Skill_Physical_Magnification")]
		public float Skill_PhyMagnification { get; set;}			// Skill 물리공격 배율

		[DynamoDBProperty("Skill_Magical_Magnification")]
		public float Skill_MagicMagnification { get; set;}			// Skill 마법공격 배율

		[DynamoDBProperty("Skill_AttackNumber")]
		public int Skill_AttackNumber { get; set;}					// Skill 공격 횟수

		[DynamoDBProperty("Skill_AttackRange")]
		public float Skill_AttackRange { get; set;}					// Skill 공격 범위

		[DynamoDBProperty("Skill_AttackArea")]
		public float Skill_AttackArea { get; set;}					// Skill 공격 스킬 범위

		[DynamoDBProperty("Skill_MaxTargetNumber")]
		public int Skill_MaxTargetNumber { get; set; }			// Skill 공격 타겟 횟수

		[DynamoDBProperty("Skill_AttackPriority")]
		public string Skill_AttackPriority { get; set;}				// Skill 공격 우선순위(근거리, 원거리) 

		[DynamoDBProperty("Skill_KnocBackPower")]
		public float Skill_KnockBackPower { get; set;}				// Skill 넉백 쌔기.

		[DynamoDBProperty("Skill_Duration")]
		public float Skill_Duration { get; set;}					// Skill 지속시간

		[DynamoDBProperty("Skill_EffectName")]
		public string Skill_EffectName { get; set;}					// Skill 이펙트 이름

		[DynamoDBProperty("Skill_AnimatonClip")]
		public string Skill_AnimationClip { get; set;}				// Skill 넉백 쌔기.

		[DynamoDBProperty("Skill_Explanation")]
		public string Skill_Explanation { get; set;}				// Skill 설명

		[DynamoDBProperty("Skill_IsCoolTime")]
		public int Skill_IsCoolTime { get; set;}					// Skill 쿨 타임인지 아닌지
	}


	//ActiveSkillType DB용
	[DynamoDBTable("CharacterActiveSkillTypeList")]
	public class DBActiveSkillType_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 	// Hash key.

		[DynamoDBProperty("ActiveSkillType_Index")]
		public int ActiveSkillType_Index { get; set;}						// SkillName

		[DynamoDBProperty("ActiveSkillType_TargetNumber")]
		public int ActiveSkillType_TargetNumber { get; set;}						// SkillName

	}

	//PassiveSkill DB용
	[DynamoDBTable("CharacterPassiveSkillList")]
	public class DBPassiveSkill_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 			// Hash key.

		[DynamoDBProperty("C_Index")]
		public int C_Index { get; set; }		// CharacterIndex 

		[DynamoDBProperty("PassiveSkill_Name")]
		public string PassiveSkill_Name { get; set;}			// PassiveSkill_Name

		[DynamoDBProperty("PassiveSkill_SkillType")]
		public string PassiveSkill_SkillType { get; set;}		// PassivsSkill_Type

		[DynamoDBProperty("PassiveSkill_Class")]
		public int PassiveSkill_Class { get; set;}			// PassivsSkill_Class

		[DynamoDBProperty("PassiveSkill_Tier")]
		public int PassiveSkill_Tier { get; set;}		// PassivsSkill_Tier

		[DynamoDBProperty("PassiveSkill_Job")]
		public string PassiveSkill_Job { get; set;}		// PassivsSkill_Job


		[DynamoDBProperty("PassiveSkill_Attribute")]
		public int PassiveSkill_Attribute { get; set;}		// PassivsSkill_속성

		[DynamoDBProperty("PassiveSkill_AtttackType")]
		public int PassiveSkill_AttackType { get; set;}		// PassivsSkill_AttackType

		[DynamoDBProperty("PassiveSkill_OptionIndex")]
		public string PassiveSkill_OptionIndex { get; set;}		// PassivsSkill_Type

		[DynamoDBProperty("PassiveSkill_Explanation")]
		public string PassiveSkill_Explanation { get; set;}		// PassivsSkill_설명
	}

	//ActiveSkillType DB용
	[DynamoDBTable("PassiveSkillOptionIndexList")]
	public class DBPassiveSkillOptionIndex_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 	// Hash key.

		[DynamoDBProperty("PassiveSkillOptionIndex_Option")]
		public int PassiveSkillOptionIndex_Option { get; set;}						// SkillName

		[DynamoDBProperty("PassiveSkillOptionIndex_Value")]
		public float PassiveSkillOptionIndex_Value { get; set;}						// SkillName

		[DynamoDBProperty("PassiveSkillOptionIndex_Plus")]
		public float PassiveSkillOptionIndex_Plus { get; set;}						// SkillName

		[DynamoDBProperty("PassiveSkillOptionIndex_Calculate")]
		public int PassiveSkillOptionIndex_Calculate { get; set;}						// SkillName

	}



	//BasicSkill DB용
	[DynamoDBTable("CharacterBasicSkillList")]
	public class DBbasicSkill_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					        // Hash key.

		[DynamoDBProperty("C_Index")]
		public int C_Index { get; set; }				        // CharacterIndex 

		[DynamoDBProperty("BasicSkill_Name")]
		public string BasicSkill_Name { get; set;}		        // BasicSkill_Name

		[DynamoDBProperty("BasicSkill_Type")]
		public string BasicSkill_Type { get; set;}		        // BasicSkill_Type

		[DynamoDBProperty("BasicSkill_Class")]
		public int BasicSkill_Class{ get; set;}			        // BasicSkill_Class

		[DynamoDBProperty("BasicSkill_Tier")]
		public int BasicSkill_Tier { get; set;}			        // BasicSkill_Name

		[DynamoDBProperty("BasicSkill_Job")]
		public string BasicSkill_Job { get; set;}			    // BasicSkill_Job

		[DynamoDBProperty("BasicSkill_Attribute")]
		public int BasicSkill_Attribute { get; set;}	        // BasicSkill_Attribute

		[DynamoDBProperty("BasicSkill_AttackType")]
		public int BasicSkill_AttackType { get; set;}	        // BasicSkill_AttackType

		[DynamoDBProperty("BasicSkill_PhysicMagnification")]
		public int BasicSkill_PhysicMagnification { get; set;}	// BasicSkill_PhysicMagnification

		[DynamoDBProperty("BasicSkill_MagicMagnification")]
		public int BasicSkill_MagicMagnification { get; set;}	// BasicSkill_MagicMagnification

		[DynamoDBProperty("BasicSkill_AttackArea")]
		public float BasicSkill_AttackArea { get; set;}	        // BasicSkill_공격범위

		[DynamoDBProperty("BasicSkill_SkillTarget")]
		public string BasicSkill_SkillTarget { get; set;}	    // BasicSkill_SkillTarget

		[DynamoDBProperty("BasicSkill_MaxTargetNumber")]
		public int BasicSkill_MaxTargetNumber { get; set;}	    // BasicSkill_MaxTargetNumber

		[DynamoDBProperty("BasicSkill_AttackNumber")]
		public int BasicSkill_AttackNumber { get; set;}		    // BasicSkill_AttackNumber

		[DynamoDBProperty("BasicSkill_AttackPriority")]
		public string BasicSkill_AttackPriority { get; set;}    // BasicSkill_AtttackPriority

        [DynamoDBProperty("BasicSkill_RangeSprite")]
        public string BasicSkill_RangeSprite { get; set; }      // BasicSkill_RangeSprite

        [DynamoDBProperty("BasicSkill_Explanation")]
		public string BasicSkill_Explanation { get; set;}	    // BasicSkill_Explanation

	}

	//Equipment_Weapon DB용
	[DynamoDBTable("EquipmentWeaponList")]
	public class DBEquipment_Weapon_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("EquipWeapon_Name")]
		public string EquipWeapon_Name { get; set;} 	

		[DynamoDBProperty("EquipWeapon_Tier")]
		public int EquipWeapon_Tier { get; set;} 

		[DynamoDBProperty("EquipWeapon_Qulity")]
		public int EquipWeapon_Qulity { get; set;} 

		[DynamoDBProperty("EquipWeapon_Job")]
		public string EquipWeapon_Job { get; set;} 

		[DynamoDBProperty("EquipWeapon_Enhance")]
		public int EquipWeapon_Enhance { get; set;} 

		[DynamoDBProperty("EquipWeapon_EquipType")]
		public string EquipWeapon_EquipType { get; set;} 

		[DynamoDBProperty("EquipWeapon_PhysicalAttackRating")]
		public float EquipWeapon_PhysicalAttackRating { get; set;} 

		[DynamoDBProperty("EquipWeapon_MagicAttackRating")]
		public float EquipWeapon_MagicAttackRating { get; set;} 

		[DynamoDBProperty("EquipWeapon_RandomOption")]
		public string EquipWeapon_RandomOption { get; set;} 

		[DynamoDBProperty("EquipWeapon_SellCost")]
		public int EquipWeapon_SellCost { get; set;} 

		[DynamoDBProperty("EquipWeapon_MakeMaterial")]
		public int EquipWeapon_MakeMaterial { get; set;} 

		[DynamoDBProperty("EquipWeapon_BreakMaterial")]
		public int EquipWeapon_BreakMaterial { get; set;} 
	}

	//Equipment_Armor DB용
	[DynamoDBTable("EquipmentArmorList")]
	public class DBEquipment_Armor_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("EquipArmor_Name")]
		public string EquipArmor_Name { get; set;} 	

		[DynamoDBProperty("EquipArmor_Tier")]
		public int EquipArmor_Tier { get; set;} 

		[DynamoDBProperty("EquipArmor_Qulity")]
		public int EquipArmor_Qulity { get; set;} 

		[DynamoDBProperty("EquipArmor_Job")]
		public string EquipArmor_Job { get; set;} 

		[DynamoDBProperty("EquipArmor_Enhance")]
		public int EquipArmor_Enhance { get; set;} 

		[DynamoDBProperty("EquipArmor_EquipType")]
		public string EquipArmor_EquipType { get; set;} 

		[DynamoDBProperty("EquipArmor_PhysicalDefense")]
		public int EquipArmor_PhysicalDefense{ get; set;} 

		[DynamoDBProperty("EquipArmor_MagicDefense")]
		public int EquipArmor_MagicDefense { get; set;} 

		[DynamoDBProperty("EquipArmor_Hp")]
		public int EquipArmor_Hp { get; set;} 

		[DynamoDBProperty("EquipArmor_RandomOption")]
		public string EquipArmor_RandomOption { get; set;} 

		[DynamoDBProperty("EquipArmor_SellCost")]
		public int EquipArmor_SellCost { get; set;} 

		[DynamoDBProperty("EquipArmor_MakeMaterial")]
		public int EquipArmor_MakeMaterial { get; set;} 

		[DynamoDBProperty("EquipArmor_BreakMaterial")]
		public int EquipArmor_BreakMaterial { get; set;} 
	}


	//Equipment_Glove DB용
	[DynamoDBTable("EquipmentGloveList")]
	public class DBEquipment_Glove_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("EquipGlove_Name")]
		public string EquipGlove_Name { get; set;} 	

		[DynamoDBProperty("EquipGlove_Tier")]
		public int EquipGlove_Tier { get; set;} 

		[DynamoDBProperty("EquipGlove_Qulity")]
		public int EquipGlove_Qulity { get; set;} 

		[DynamoDBProperty("EquipGlove_Job")]
		public string EquipGlove_Job { get; set;} 

		[DynamoDBProperty("EquipGlove_Enhance")]
		public int EquipGlove_Enhance { get; set;} 

		[DynamoDBProperty("EquipGlove_EquipType")]
		public string EquipGlove_EquipType { get; set;} 

		[DynamoDBProperty("EquipGlove_PhysicalDefense")]
		public int EquipGlove_PhysicalDefense{ get; set;} 

		[DynamoDBProperty("EquipGlove_MagicDefense")]
		public int EquipGlove_MagicDefense { get; set;} 

		[DynamoDBProperty("EquipGlove_RandomOption")]
		public string EquipGlove_RandomOption { get; set;} 

		[DynamoDBProperty("EquipGlove_SellCost")]
		public int EquipGlove_SellCost { get; set;} 

		[DynamoDBProperty("EquipGlove_MakeMaterial")]
		public int EquipGlove_MakeMaterial { get; set;} 

		[DynamoDBProperty("EquipGlove_BreakMaterial")]
		public int EquipGlove_BreakMaterial { get; set;} 
	}

	//Equipment_Accessory DB용
	[DynamoDBTable("EquipmentAccessoryList")]
	public class DBEquipment_Accessory_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("EquipAccessory_Name")]
		public string EquipAccessory_Name { get; set;} 	

		[DynamoDBProperty("EquipAccessory_Tier")]
		public int EquipAccessory_Tier { get; set;} 

		[DynamoDBProperty("EquipAccessory_Qulity")]
		public int EquipAccessory_Qulity { get; set;} 

		[DynamoDBProperty("EquipAccessory_Job")]
		public string EquipAccessory_Job { get; set;} 

		[DynamoDBProperty("EquipAccessory_Enhance")]
		public int EquipAccessory_Enhance { get; set;} 

		[DynamoDBProperty("EquipAccessory_EquipType")]
		public string EquipAccessory_EquipType { get; set;} 

		[DynamoDBProperty("EquipAccessory_RandomOption")]
		public string EquipAccessory_RandomOption { get; set;} 

		[DynamoDBProperty("EquipAccessory_SellCost")]
		public int EquipAccessory_SellCost { get; set;} 

		[DynamoDBProperty("EquipAccessory_MakeMaterial")]
		public int EquipAccessory_MakeMaterial { get; set;} 

		[DynamoDBProperty("EquipAccessory_BreakMaterial")]
		public int EquipAccessory_BreakMaterial { get; set;} 
	}

	//Equipment_RandomOption DB용
	[DynamoDBTable("EquipmentRandomOptionList")]
	public class DBEquipment_RandomOption_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("EquipRandomOption_Index")]
		public int OptionIndex { get; set;} 	

		[DynamoDBProperty("EquipRandomOption_StartValue")]
		public int nStartValue { get; set;} 

		[DynamoDBProperty("EquipRandomOption_EndValue")]
		public int nEndValue { get; set;} 
	}
	[DynamoDBTable("DBStage")]
	public class DBStage_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 						// Hash key.

		[DynamoDBProperty("StageNumber")]
		public string StageNumber { get; set; }						// Stage

		[DynamoDBProperty("StageName")]
		public string StageName { get; set; }						// Stage

		[DynamoDBProperty("Wave")]
		public string Wave { get; set; }					// Wave 

		[DynamoDBProperty("EnemySpawn")]
		public string EnemySpawn { get; set; }				// EnemySpawn 

		[DynamoDBProperty("CreateTime")]
		public string CreateTime { get; set; }				// CreateTime 

		[DynamoDBProperty("YPosition")]
		public string YPosition { get; set; }				// YPosition 

		[DynamoDBProperty("Gold")]
		public int Gold { get; set; }						// Gold 

		[DynamoDBProperty("Exp")]
		public float Exp { get; set; }						// Exp 

		[DynamoDBProperty("Drop_Equipment")]
		public string Drop_Equipment { get; set; }				// Drop_Equipment

		[DynamoDBProperty("Drop_Character")]
		public string Drop_Character { get; set; }				// Drop_Character 

		[DynamoDBProperty("Drop_Material")]
		public string Drop_Material { get; set; }				// Drop_Material

		[DynamoDBProperty("Drop_EquipmentRates")]
		public string Drop_EquipmentRates { get; set; }				// Drop_Equipment

		[DynamoDBProperty("Drop_CharacterRates")]
		public string Drop_CharacterRates { get; set; }				// Drop_Equipment

		[DynamoDBProperty("Drop_MaterialRates")]
		public string Drop_MaterialRates { get; set; }				// Drop_Equipment

		[DynamoDBProperty("Background")]
		public string Background { get; set; }				// Drop_Equipment


	}

	[DynamoDBTable("DBCraftMaterial")]
	public class DBCraftMaterial_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("Iron")]
		public int Iron { get; set; }					// Iron

		[DynamoDBProperty("Fabric")]
		public int Fabric { get; set; }					// Fabric 

		[DynamoDBProperty("Wood")]
		public int Wood { get; set; }					// Wood 

		[DynamoDBProperty("WeaponStone")]
		public int WeaponStone { get; set; }			// WeaponStone 

		[DynamoDBProperty("ArmorStone")]
		public int ArmorStone { get; set; }				// ArmorStone 

		[DynamoDBProperty("AccessoryStone")]
		public int AccessoryStone { get; set; }			// AccessoryStone 

		[DynamoDBProperty("EpicStone")]
		public int EpicStone { get; set; }				// EpicStone 

		[DynamoDBProperty("GoldCost")]
		public int GoldCost { get; set; }				// GoldCost

		[DynamoDBProperty("Tier")]
		public int Tier { get; set; }					// Tier 

		[DynamoDBProperty("Qulity")]
		public int Qulity { get; set; }					// Qulity

		[DynamoDBProperty("EquipType")]
		public string EquipType { get; set; }			// EquipType

	}

	[DynamoDBTable("DBBreakMaterial")]
	public class DBBreakMaterial_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("Iron")]
		public int Iron { get; set; }					// Iron

		[DynamoDBProperty("Fabric")]
		public int Fabric { get; set; }					// Fabric 

		[DynamoDBProperty("Wood")]
		public int Wood { get; set; }					// Wood 

		[DynamoDBProperty("WeaponStone")]
		public int WeaponStone { get; set; }			// WeaponStone 

		[DynamoDBProperty("ArmorStone")]
		public int ArmorStone { get; set; }				// ArmorStone 

		[DynamoDBProperty("AccessoryStone")]
		public int AccessoryStone { get; set; }			// AccessoryStone 

		[DynamoDBProperty("EpicStone")]
		public int EpicStone { get; set; }				// EpicStone 
	}

	[DynamoDBTable("DBFormationSkill")]
	public class DBFormationSkill_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 								// Hash key.

		[DynamoDBProperty("C_Index")]
		public int C_Index { get; set; }							// Iron

		[DynamoDBProperty("FormationSkill_Name")]
		public string FormationSkill_Name { get; set; }				// Fabric 

		[DynamoDBProperty("FormationSkill_Type")]
		public string FormationSkill_Type { get; set; }				// Wood 

		[DynamoDBProperty("FormationSkill_Class")]
		public int FormationSkill_Class { get; set; }				// WeaponStone 

		[DynamoDBProperty("FormationSkill_Tier")]
		public int FormationSkill_Tier { get; set; }				// ArmorStone 

		[DynamoDBProperty("FormationSkill_Target")]
		public string FormationSkill_Target { get; set; }			// AccessoryStone 

		[DynamoDBProperty("FormationSkill_OptionIndex")]
		public int FormationSkill_OptionIndex { get; set; }			// EpicStone 

		[DynamoDBProperty("FormationSkill_Explanation")]
		public string FormationSkill_Explanation { get; set; }		// EpicStone 

	}

    [DynamoDBTable("DBMaterialData")]
    public class DBMaterialData_ForGet
    {
        [DynamoDBHashKey]
        public int Index { get; set; }                              // Hash key.

        [DynamoDBProperty("MaterialName")]
        public string MaterialName { get; set; }

        [DynamoDBProperty("MaterialImagePath")]
        public string MaterialImagePath { get; set; }

        [DynamoDBProperty("MaterialExplanation")]
        public string MaterialExplanation { get; set; }

    }


    [DynamoDBTable("DBCalendarData")]
    public class DBCalendarData_ForGet
    {
        [DynamoDBHashKey]
        public int Index { get; set; }                              // Hash key.

        [DynamoDBProperty("Gold")]
        public int Gold { get; set; }

        [DynamoDBProperty("Gem")]
        public int Gem { get; set; }

        [DynamoDBProperty("WeaponTicket")]
        public int WeaponTicket { get; set; }

        [DynamoDBProperty("CharacterTicket")]
        public int CharacterTicket { get; set; }

        [DynamoDBProperty("Weapon")]
        public int Weapon { get; set; }

        [DynamoDBProperty("Character")]
        public int Character { get; set; }

        [DynamoDBProperty("Iron")]
        public int Iron { get; set; }

        [DynamoDBProperty("Fabric")]
        public int Fabric { get; set; }

        [DynamoDBProperty("Wood")]
        public int Wood { get; set; }

        [DynamoDBProperty("WeaponStone")]
        public int WeaponStone { get; set; }

        [DynamoDBProperty("ArmorStone")]
        public int ArmorStone { get; set; }

        [DynamoDBProperty("AccessoryStone")]
        public int AccessoryStone { get; set; }
    }

    [DynamoDBTable("DBCharacterTicketData")]
    public class DBCharacterTicketData_ForGet
    {
        [DynamoDBHashKey]
        public int Index { get; set; }                              // Hash key.

        [DynamoDBProperty("Name")]
        public string Name { get; set; }

        [DynamoDBProperty("Job")]
        public string Job { get; set; }

        [DynamoDBProperty("Percentage")]
        public string Percentage { get; set; }

        [DynamoDBProperty("Tier")]
        public string Tier { get; set; }

        [DynamoDBProperty("Tribe")]
        public string Tribe { get; set; }

        [DynamoDBProperty("Image")]
        public string Image { get; set; }

        [DynamoDBProperty("Explanation")]
        public string Explanation { get; set; }
    }

    [DynamoDBTable("DBWeaponTicketData")]
    public class DBWeaponTicketData_ForGet
    {
        [DynamoDBHashKey]
        public int Index { get; set; }                              // Hash key.

        [DynamoDBProperty("Name")]
        public string Name { get; set; }

        [DynamoDBProperty("EquipType")]
        public string EquipType { get; set; }

        [DynamoDBProperty("Percentage")]
        public string Percentage { get; set; }

        [DynamoDBProperty("Tier")]
        public string Tier { get; set; }

        [DynamoDBProperty("Qulity")]
        public string Qulity { get; set; }

        [DynamoDBProperty("Image")]
        public string Image { get; set; }

        [DynamoDBProperty("Explanation")]
        public string Explanation { get; set; }
    }

    [DynamoDBTable("DBEmployGachaData")]
    public class DBEmployGachaData_ForGet
    {
        [DynamoDBHashKey]
        public int Index { get; set; }                              // Hash key.

        [DynamoDBProperty("Name")]
        public string Name { get; set; }

        [DynamoDBProperty("Job")]
        public string Job { get; set; }

        [DynamoDBProperty("Percentage")]
        public string Percentage { get; set; }

        [DynamoDBProperty("Tier")]
        public string Tier { get; set; }

        [DynamoDBProperty("Tribe")]
        public string Tribe { get; set; }

        [DynamoDBProperty("Cost_Gold")]
        public int Cost_Gold { get; set; }

        [DynamoDBProperty("Cost_Gem")]
        public int Cost_Gem { get; set; }

        [DynamoDBProperty("Image")]
        public string Image { get; set; }

        [DynamoDBProperty("Explanation")]
        public string Explanation { get; set; }
    }
}





