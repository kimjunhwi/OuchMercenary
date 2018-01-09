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
	//Aws에서 받는 리스트
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



	//DB정보들의 각각의 개수
	private const int nCharacterCount = 53;
	private const int nActiveSkillCount = 144;
	private const int nActiveSkillTypeCount = 64;
	private const int nPassiveSkillCount = 10;
	private const int nPassiveSkillOptionIndexCount = 240;
	private const int nBasicSkillCount = 41;
	private const int nEquipmentWeaponCount = 108;
	private const int nEquipmentArmorCount = 108;
	private const int nEquipmentGloveCount = 108;
	private const int nEquipmentAccessoryCount = 12;
	private const int nEquipmentRandomOptionCount = 17;

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


	//모든 데이터가 로드 됬는지 않됬는지
	bool bIsFinishLoadDate = false;

	//정보 불러올때 띄우는 텍스트
	public Text loginState_Text;
	//playerpref 필요 처음 앱을 시작했는지 아닌지


	//Google
	private string AccessTokken_GP;
	//Identity ID pool 
	private string IdentityPoolId = "ap-northeast-2:0c274d71-5a9b-4747-bcd1-6b9d0d161301";

	//지역 설정 변수
	private string Region = RegionEndpoint.APNortheast2.SystemName;	


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

			LoadBasicDataSequence();
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

		LoadBasicDataSequence();
		StartCoroutine(CheckBasicDataLoadEnd());

		}
		#endif



		//CharacterListAdjust ();
		//StartCoroutine( GameManager.Instance.DataLoad());

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

	void LoadBasicDataSequence()
	{
		//만약중간에 없는 데이터가 있으면 서버에서 추가적으로 다운한다.
		//BasicCharacter 
		if (File.Exists (Application.persistentDataPath + sDBBasicCharacterInfoPath))
			SaveAndLoadBinaryFile (sDBBasicCharacterInfoPath, E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA);
		else {
			CharacterDBLoadAndPutOperation ();
			return;
		}
		if (File.Exists (Application.persistentDataPath + sDBActiveSkillPath))
			SaveAndLoadBinaryFile (sDBActiveSkillPath, E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA);
		else {
			ActiveSkillDBLoadAndPutOperation ();
			return;
		}
		if (File.Exists (Application.persistentDataPath + sDBActiveSkilLTypePath))
			SaveAndLoadBinaryFile (sDBActiveSkilLTypePath, E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA);
		else {
			ActiveSkillTypeDBLoadAndPutOperation ();
			return;
		}
		if (File.Exists (Application.persistentDataPath + sDBPassiveSkillPath))
			SaveAndLoadBinaryFile (sDBPassiveSkillPath, E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA);
		else {
			
			PassiveSkillDBLoadAndPutOperation ();
			return;
		}
		if (File.Exists (Application.persistentDataPath + sDBPassiveSkillOptionIndexPath))
			SaveAndLoadBinaryFile (sDBPassiveSkillOptionIndexPath, E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA);
		else {
			PassiveSkillOptionIndexDBLoadAndPutOperation ();
			return;
		}
		if (File.Exists (Application.persistentDataPath + sDBBasicSkillPath))
			SaveAndLoadBinaryFile (sDBBasicSkillPath, E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA);
		else 
		{
			BasicSkillDBLoadAndPutOperation ();
			return;	
		}

		if (File.Exists (Application.persistentDataPath + sDBEquipmentWeaponPath))
			SaveAndLoadBinaryFile (sDBEquipmentWeaponPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA);
		else 
		{
			EquipmentWeaponDBLoadAndPutOperation ();
			return;	
		}



		if (File.Exists (Application.persistentDataPath + sDBEquipmentArmorPath))
			SaveAndLoadBinaryFile (sDBEquipmentArmorPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA);
		else 
		{
			EquipmentArmorDBLoadAndPutOperation ();
			return;	
		}

		if (File.Exists (Application.persistentDataPath + sDBEquipmentGlovePath))
			SaveAndLoadBinaryFile (sDBEquipmentGlovePath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA);
		else 
		{
			EquipmentGloveDBLoadAndPutOperation ();
			return;	
		}

		if (File.Exists (Application.persistentDataPath + sDBEquipmentAccessoryPath))
			SaveAndLoadBinaryFile (sDBEquipmentAccessoryPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA);
		else 
		{
			EquipmentAccessoryDBLoadAndPutOperation ();
			return;	
		}

		if (File.Exists (Application.persistentDataPath + sDBEquipmentRandomOptionPath))
			SaveAndLoadBinaryFile (sDBEquipmentRandomOptionPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA);
		else 
		{
			EquipmentRandomOptionDBLoadAndPutOperation ();
			return;	
		}


	}

	private void StartLoadScene()
	{
		StartCoroutine (this.LoadScene());
	}
	//Google, FaceBook 등등 로그인시 초기화 할것들
	void LoginManager_Init()
	{
		GoogleLogin_Button.onClick.AddListener (GoogleLogin);
		//FaceBookLogin_Button.onClick.AddListener (FaceBookLogin);
		nickConfirm_Button.onClick.AddListener (DataSetSaveInCognito_FirstLogin);

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

		playerInfo = SyncManager.OpenOrCreateDataset("PlayerInfo");

		//playerInfo.Put ("Nick", "Smaet");
		//playerInfo.Put ("Provider", "Google");
		//playerInfo.Put ("Email", "dkan56@naver.com");

		//playerInfo.SynchronizeAsync ();

		Debug.Log ("Init Complete");

	}

	IEnumerator LoadScene(){
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

	//처음 로그인후 닉 과 해당 이메일을 확인조건으로 저장한다
	public void DataSetSaveInCognito_FirstLogin()
	{
		sNick = nickInputField.text;

		playerInfo.Put ("Nick", sNick);
		playerInfo.Put ("Provider", eLoginProviderIndex.ToString ());
		playerInfo.Put ("Email", sEmail);

		//해당 플레이어의 캐릭터들 정보를 가져온다
		//DynamoDBCheck(sEmail, sNick, true);

		playerInfo.SynchronizeAsync ();

		//CharacterDBLoadAndPutOperation ();

		nickInputObj.SetActive (false);
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


	//DB에서 연동하여 데이터를 가져온다 Character
	private void CharacterDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBBasicCharacterInfoPath)) {
			//다음꺼에 해당되는 걸넘어간다
			SaveAndLoadBinaryFile(sDBBasicCharacterInfoPath, E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA);
			ActiveSkillDBLoadAndPutOperation ();
			return;
		}

		int C_Index = 1000;
		//cCharacterState = null;
		DBBaiscCharacter_ForGet character = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA));
		for (int Index = 0; Index < nCharacterCount ; Index++)
		{
			
			//Load Table Info
			Context.LoadAsync<DBBaiscCharacter_ForGet> (Index , C_Index + Index, (result) =>
			{
				if (result.Exception == null) 
				{
					character = result.Result as DBBaiscCharacter_ForGet;
					// Update few properties.

					Debug.Log("CharacterJobName : " + character.C_JobNames + "\n");
					//GetCharacter= character;
					lDBBasicCheacter_GetList.Add(character);
					
					//Index++;
					character = null;
				}
				// Update To Save
				/*
				Context.SaveAsync (character, (res) => {
					if (res.Exception == null)
						//resultText.text += ("\nCharacter updated");
					else
						TableInfo_Text.text = result.Exception.ToString ();
				});
				*/
			});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 ActiveSkill
	private void ActiveSkillDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBActiveSkillPath)) {
			//다음꺼에 해당되는 걸넘어간다
			SaveAndLoadBinaryFile(sDBActiveSkillPath, E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA);
			ActiveSkillTypeDBLoadAndPutOperation ();
			return;
		}


		int C_Index = 1000;
		//cCharacterState = null;
		DBActiveSkill_ForGet DBActiveSkill = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA));
		for (int Index = 0; Index < nActiveSkillCount ; Index++)
		{
			

			//Load Table Info
			Context.LoadAsync<DBActiveSkill_ForGet> (Index, (result) =>
				{
					if (result.Exception == null) 
					{
						DBActiveSkill = result.Result as DBActiveSkill_ForGet;
						// Update few properties.

						Debug.Log("ActiveSkillName : " + DBActiveSkill.Skill_Name + "\n");
						//GetCharacter= character;
						lDBActiveSkill_GetList.Add(DBActiveSkill);
					
						//Index++;
						DBActiveSkill = null;
					}
				
				});
		}
	}
	//DB에서 연동하여 데이터를 가져온다 ActiveSkillType
	private void ActiveSkillTypeDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBActiveSkilLTypePath)) {
			//다음꺼에 해당되는 걸넘어간다
			SaveAndLoadBinaryFile(sDBActiveSkilLTypePath, E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA);
			PassiveSkillDBLoadAndPutOperation ();
			return;
		}


		int C_Index = 1000;
		//cCharacterState = null;
		DBActiveSkillType_ForGet DBActiveSkillType = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA));
		for (int Index = 0; Index < nActiveSkillTypeCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBActiveSkillType_ForGet> (Index , (result) =>
				{
					if (result.Exception == null) 
					{
						DBActiveSkillType = result.Result as DBActiveSkillType_ForGet;
						// Update few properties.

						Debug.Log("ActiveSkillTypeIndex : " + DBActiveSkillType.ActiveSkillType_Index + "\n");
						//GetCharacter= character;
						lDBActiveSkillType_GetList.Add(DBActiveSkillType);
						//null
						DBActiveSkillType = null;
					}
			
				});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 PassiveSkill
	private void PassiveSkillDBLoadAndPutOperation()
	{

		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBPassiveSkillPath)) {
			//다음꺼에 해당되는 걸넘어간다
			SaveAndLoadBinaryFile(sDBPassiveSkillPath, E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA);
			PassiveSkillOptionIndexDBLoadAndPutOperation ();
			return;
		}

		int C_Index = 1000;
		//cCharacterState = null;
		DBPassiveSkill_ForGet DBPassiveSkill = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA));
		for (int Index = 0; Index < nPassiveSkillCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBPassiveSkill_ForGet> (Index , -1, (result) =>
				{
					if (result.Exception == null) 
					{
						DBPassiveSkill = result.Result as DBPassiveSkill_ForGet;
						// Update few properties.

						Debug.Log("PassiveSkillName : " + DBPassiveSkill.PassiveSkill_Name + "\n");
						//GetCharacter= character;
						lDBPassiveSkill_GetList.Add(DBPassiveSkill);

						//Index++;
						DBPassiveSkill = null;
					}

				});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 PassiveSkillOptionIndex
	private void PassiveSkillOptionIndexDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBPassiveSkillOptionIndexPath)) {
			SaveAndLoadBinaryFile(sDBPassiveSkillOptionIndexPath, E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA);
			BasicSkillDBLoadAndPutOperation ();
			return;
		}


		int C_Index = 1000;
		//cCharacterState = null;
		DBPassiveSkillOptionIndex_ForGet DBPassiveSkillOptionIndex = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA));
		for (int Index = 0; Index < nPassiveSkillOptionIndexCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBPassiveSkillOptionIndex_ForGet> (Index , (result) =>
				{
					if (result.Exception == null) 
					{
						DBPassiveSkillOptionIndex = result.Result as DBPassiveSkillOptionIndex_ForGet;
						// Update few properties.

						Debug.Log("PassiveSkillOptionIndex : " + DBPassiveSkillOptionIndex.Index + "\n");
						//GetCharacter= character;
						lDBPassisveSkillOptionIndex_GetList.Add(DBPassiveSkillOptionIndex);
						//null
						DBPassiveSkillOptionIndex = null;
					}

				});
		}
	}


	//DB에서 연동하여 데이터를 가져온다 BasicSkill
	private void BasicSkillDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBBasicSkillPath)) {
			SaveAndLoadBinaryFile(sDBBasicSkillPath, E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA);
			return;
		}

		int C_Index = 1000;
		//cCharacterState = null;
		DBbasicSkill_ForGet DBBasicSkill = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA));
		for (int Index = 0; Index < nBasicSkillCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBbasicSkill_ForGet> (Index , C_Index + Index, (result) =>
				{
					if (result.Exception == null) 
					{
						DBBasicSkill = result.Result as DBbasicSkill_ForGet;
						// Update few properties.

						Debug.Log("BasicSkillName : " + DBBasicSkill.BasicSkill_Name + "\n");
						//GetCharacter= character;
						lDBBasicSkill_GetList.Add(DBBasicSkill);

						//Index++;
						DBBasicSkill = null;
					}

				});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 EquipmentWeapon
	private void EquipmentWeaponDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBEquipmentWeaponPath)) {
			SaveAndLoadBinaryFile(sDBEquipmentWeaponPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA);
			return;
		}
		int C_Index = 1000;
		//cCharacterState = null;
		DBEquipment_Weapon_ForGet DBEquipWeapon = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA));
		for (int Index = 0; Index < nEquipmentWeaponCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBEquipment_Weapon_ForGet> (Index , C_Index + Index, (result) =>
				{
					if (result.Exception == null) 
					{
						DBEquipWeapon = result.Result as DBEquipment_Weapon_ForGet;
						// Update few properties.

						Debug.Log("EquipmentWeaponName : " + DBEquipWeapon.EquipWeapon_Name + "\n");
						//GetCharacter= character;
						lDBEquipmentWeapon_GetList.Add(DBEquipWeapon);

						//Index++;
						DBEquipWeapon = null;
					}

				});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 EquipmentArmor
	private void EquipmentArmorDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBEquipmentArmorPath)) {
			SaveAndLoadBinaryFile(sDBEquipmentArmorPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA);
			return;
		}

		int C_Index = 1000;
		//cCharacterState = null;
		DBEquipment_Armor_ForGet DBEquipArmor = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA));
		for (int Index = 0; Index < nEquipmentArmorCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBEquipment_Armor_ForGet> (Index , C_Index + Index, (result) =>
				{
					if (result.Exception == null) 
					{
						DBEquipArmor = result.Result as DBEquipment_Armor_ForGet;
						// Update few properties.

						Debug.Log("EquipmentArmorName : " + DBEquipArmor.EquipArmor_Name + "\n");
						//GetCharacter= character;
						lDBEquipmentArmor_GetList.Add(DBEquipArmor);

						//Index++;
						DBEquipArmor = null;
					}

				});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 EquipmentGlove
	private void EquipmentGloveDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBEquipmentGlovePath)) {
			SaveAndLoadBinaryFile(sDBEquipmentGlovePath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA);
			return;
		}
		int C_Index = 1000;
		//cCharacterState = null;
		DBEquipment_Glove_ForGet DBEquipGlove = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA));
		for (int Index = 0; Index < nEquipmentGloveCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBEquipment_Glove_ForGet> (Index , C_Index + Index, (result) =>
				{
					if (result.Exception == null) 
					{
						DBEquipGlove = result.Result as DBEquipment_Glove_ForGet;
						// Update few properties.

						Debug.Log("EquipmentGloveName : " + DBEquipGlove.EquipGlove_Name + "\n");
						//GetCharacter= character;
						lDBEquipmentGlove_GetList.Add(DBEquipGlove);

						//Index++;
						DBEquipGlove = null;
					}
				});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 EquipmentAccessory
	private void EquipmentAccessoryDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBEquipmentAccessoryPath)) {
			SaveAndLoadBinaryFile(sDBEquipmentAccessoryPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA);
			return;
		}
		int C_Index = 1000;
		//cCharacterState = null;
		DBEquipment_Accessory_ForGet DBEquipAccessory = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA));
		for (int Index = 0; Index < nEquipmentAccessoryCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBEquipment_Accessory_ForGet> (Index , C_Index + Index, (result) =>
				{
					if (result.Exception == null) 
					{
						DBEquipAccessory = result.Result as DBEquipment_Accessory_ForGet;
						// Update few properties.

						Debug.Log("EquipmentGloveName : " + DBEquipAccessory.EquipAccessory_Name + "\n");
						//GetCharacter= character;
						lDBEquipmentAccessory_GetList.Add(DBEquipAccessory);

						//Index++;
						DBEquipAccessory = null;
					}
				});
		}
	}

	//DB에서 연동하여 데이터를 가져온다 EquipmentRandomOption
	private void EquipmentRandomOptionDBLoadAndPutOperation()
	{
		//이미 저장된 정보가 있을시 체크 함수로 넘어간다
		if (File.Exists (Application.persistentDataPath + sDBEquipmentRandomOptionPath)) {
			SaveAndLoadBinaryFile(sDBEquipmentRandomOptionPath, E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA);
			return;
		}
		int C_Index = 1000;

		//cCharacterState = null;
		DBEquipment_RandomOption_ForGet DBEquipRandomOption = null;
		StartCoroutine(isFinishLoadData(E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA));
		for (int Index = 0; Index < nEquipmentRandomOptionCount ; Index++)
		{
			//Load Table Info
			Context.LoadAsync<DBEquipment_RandomOption_ForGet> (Index ,  C_Index + Index,(result) =>
				{
					if (result.Exception == null) 
					{
						DBEquipRandomOption = result.Result as DBEquipment_RandomOption_ForGet;
						// Update few properties.

						Debug.Log("EquipmentRandomOptionIndex : " + DBEquipRandomOption.Index + "\n");
						//GetCharacter= character;
						lDBEquipmentRandomOption_GetList.Add(DBEquipRandomOption);

						//Index++;
						DBEquipRandomOption = null;
					}
				});
		}
	}



	IEnumerator isFinishLoadData(E_LOAD_STATE _state)
	{

		//yield return new WaitForSeconds(0.2f);

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
		default:
			break;
		
		}

		while (true) 
		{
			//break 조건
			if (lDBBasicCheacter_GetList.Count == nCharacterCount  && _state == E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA) 
			{
				loginState_Text.text = "기본 캐릭터 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);

				
				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBActiveSkill_GetList.Count == nActiveSkillCount && _state == E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA) {
				loginState_Text.text = "ActiveSkill 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBActiveSkillType_GetList.Count == nActiveSkillTypeCount && _state == E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA) {
				loginState_Text.text = "ActiveSkillType 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBPassiveSkill_GetList.Count == nPassiveSkillCount && _state == E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA) {
				loginState_Text.text = "PassiveSkill 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}


			if (lDBPassisveSkillOptionIndex_GetList.Count == nPassiveSkillOptionIndexCount && _state == E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA) {
				loginState_Text.text = "PassiveSkillOptionIndex 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBBasicSkill_GetList.Count == nBasicSkillCount && _state == E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA) {
				loginState_Text.text = "BasicSkill 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentWeapon_GetList.Count == nEquipmentWeaponCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA) {
				loginState_Text.text = "EquipmentWeapon 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentArmor_GetList.Count == nEquipmentArmorCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA) {
				loginState_Text.text = "EquipmentArmor 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentGlove_GetList.Count == nEquipmentGloveCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA) {
				loginState_Text.text = "EquipmentGlove 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentAccessory_GetList.Count == nEquipmentAccessoryCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA) {
				loginState_Text.text = "EquipmentAccessory 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			if (lDBEquipmentRandomOption_GetList.Count == nEquipmentRandomOptionCount && _state == E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA) {
				loginState_Text.text = "EquipmentRandomOption 불러오기 완료";
				#if UNITY_EDITOR
				//StartCoroutine(GameManager.Instance.DataLoad());

				//GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				//StartCoroutine(GetPlayerCountFromDB());
				LoginCategory_Panel.SetActive (false);
				InsertInfoToUsingListInGameManager(_state);


				#elif UNITY_ANDROID
				//LoginCategory_Panel.SetActive (true);
				InsertInfoToUsingListInGameManager(_state);
				#endif
				break;
			}

			yield return new WaitForSeconds(0.2f);

			if (nPotCount == 0) {
				loginState_Text.text = sInputText + ".";
				nPotCount++;
			} else if (nPotCount == 1) {
				loginState_Text.text = sInputText + "..";
				nPotCount++;
			} 
			else {
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
				dbBaseCharacters.Health 		        = lDBBasicCheacter_GetList [i].Exp;
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

				GameManager.Instance.lDbBasicCharacter.Add(dbBaseCharacters);
				dbBaseCharacters = null;
			}
			//정렬
			ListAdjustSort (_state);
			//로컬 저장
			SaveAndLoadBinaryFile (sDBBasicCharacterInfoPath, _state);
			//다음 데이터 불러오기
			ActiveSkillDBLoadAndPutOperation();

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
				activeSkill.m_strAttackType = lDBActiveSkill_GetList [i].Skill_AttackType;
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
			ActiveSkillTypeDBLoadAndPutOperation();
			
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
			PassiveSkillDBLoadAndPutOperation();
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
			PassiveSkillOptionIndexDBLoadAndPutOperation();

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
			BasicSkillDBLoadAndPutOperation();


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
			EquipmentWeaponDBLoadAndPutOperation();
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
			EquipmentArmorDBLoadAndPutOperation();
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
			EquipmentGloveDBLoadAndPutOperation();
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
			EquipmentAccessoryDBLoadAndPutOperation();
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
			EquipmentRandomOptionDBLoadAndPutOperation();
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
				bIsFinishLoadDate = true;
				break;

			default:
				break;
			}

			fileStream.Close ();

			Debug.Log ("Load In Binary Data");
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
				StartCoroutine (GameManager.Instance.DataLoad ());
				GameManager.Instance.LoadScene (E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, false);
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
		switch (_loadState)
		{
		case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:
			
			GameManager.Instance.lDbBasicCharacter.Sort (delegate(DBBasicCharacter A, DBBasicCharacter B) 
				{
					if(A.Index > B.Index) return 1;
					else if (A.Index < B.Index) return -1;
					return 0;
				});
			Debug.Log ("BasicCharacter Sort Confirm!!");
			break;
		case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLDATA:
			GameManager.Instance.lDbActiveSkill.Sort (delegate(DBActiveSkill A, DBActiveSkill B) 
				{
					if(A.m_nIndex > B.m_nIndex) return 1;
					else if (A.m_nIndex < B.m_nIndex) return -1;
					return 0;
				});
			Debug.Log ("ActiveSkill Sort Confirm!!");

			break;
		case E_LOAD_STATE.E_LOAD_GET_ACTIVESKILLTYPEDATA:
			
			GameManager.Instance.lDbActiveSkillType.Sort (delegate(DBActiveSkillType A, DBActiveSkillType B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("ActiveSkillType Sort Confirm!!");
			break;
		case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLDATA:
			GameManager.Instance.lDbPassiveSkill.Sort (delegate(DBPassiveSkill A, DBPassiveSkill B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("PassiveSkill Sort Confirm!!");
			break;
		case E_LOAD_STATE.E_LOAD_GET_PASSIVESKILLOPTIONINDEXDATA:
			GameManager.Instance.lDbPassiveSkillOptionIndex.Sort (delegate(DBPassiveSkillOptionIndex A, DBPassiveSkillOptionIndex B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("PassiveSkillOptionIndex Sort Confirm!!");
			break;
		case E_LOAD_STATE.E_LOAD_GET_BASICSKILLDATA:
			GameManager.Instance.lDbBasickill.Sort (delegate(DBBasicSkill A, DBBasicSkill B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("BasicSkill Sort Confirm!!");
			break;

		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTWEAPONDATA:
			GameManager.Instance.lDbWeapon.Sort (delegate(DBWeapon A, DBWeapon B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("DBweapon Sort Confirm!!");
			break;


		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTARMORDATA:
			GameManager.Instance.lDBArmor.Sort (delegate(DBArmor A, DBArmor B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("DBArmor Sort Confirm!!");
			break;


		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTGLOVEDATA:
			GameManager.Instance.lDBGlove.Sort (delegate(DBGlove A, DBGlove B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("DBGlove Sort Confirm!!");
			break;


		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTACCESSORYDATA:
			GameManager.Instance.lDBAccessory.Sort (delegate(DBAccessory A, DBAccessory B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("DBAccessory Sort Confirm!!");
			break;


		case E_LOAD_STATE.E_LOAD_GET_EQUIPMENTRANDOMOPTIONDATA:
			GameManager.Instance.lDBEquipmentRandomOption.Sort (delegate(DBEquipment_RandomOption A, DBEquipment_RandomOption B) 
				{
					if(A.nIndex > B.nIndex) return 1;
					else if (A.nIndex < B.nIndex) return -1;
					return 0;
				});
			Debug.Log ("DBEquipmet_RandomOption Sort Confirm!!");
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
				StartCoroutine (GetPlayerCountFromDB ());


				//이메일 비교하여 접속한적이 있는지 없는지 비교
				if (sEmail == playerInfo.Get ("Email")) 
				{
					//player DataLoad

				}
				//접속한 적이 있으면 Data Load -> playerInfo
				else 
				{
					//없다면 닉을 치는 창을 띄움
					nickInputObj.SetActive(true);
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



	[DynamoDBTable("CharacterBasicInfoTable")]
	public class DBBaiscCharacter_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 						// Hash key.

		[DynamoDBRangeKey]
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
		public float Betch_Index {	get; set; }				// Character 배치위치 

		[DynamoDBProperty("BasicSkill")]				
		public List<BasicSkill> basicSkill {get; set;}

		[DynamoDBProperty("ActiveSkill")]				
		public List<ActiveSkill> activeSkills {get; set;}

	}

	[DynamoDBTable("PlayersCharacterInfoTable")]
	public class DBPlayersCharacter_ForDBWork
	{
		[DynamoDBHashKey]   
		public string UserEamil { get; set; } 				// Hash key.
		[DynamoDBRangeKey]
		public string UserNick	{ get; set; }

		[DynamoDBProperty("CharactersInfo")]				
		public List<DBBaiscCharacter_ForGet> Characters {get; set;}

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

		[DynamoDBRangeKey]
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
		public int Index { get; set; } 					// Hash key.

		[DynamoDBProperty("C_Index")]
		public int C_Index { get; set; }				// CharacterIndex 

		[DynamoDBProperty("BasicSkill_Name")]
		public string BasicSkill_Name { get; set;}		// BasicSkill_Name

		[DynamoDBProperty("BasicSkill_Type")]
		public string BasicSkill_Type { get; set;}		// BasicSkill_Type

		[DynamoDBProperty("BasicSkill_Class")]
		public int BasicSkill_Class{ get; set;}			// BasicSkill_Class

		[DynamoDBProperty("BasicSkill_Tier")]
		public int BasicSkill_Tier { get; set;}			// BasicSkill_Name

		[DynamoDBProperty("BasicSkill_Job")]
		public string BasicSkill_Job { get; set;}			// BasicSkill_Job

		[DynamoDBProperty("BasicSkill_Attribute")]
		public int BasicSkill_Attribute { get; set;}	// BasicSkill_Attribute

		[DynamoDBProperty("BasicSkill_AttackType")]
		public int BasicSkill_AttackType { get; set;}	// BasicSkill_AttackType

		[DynamoDBProperty("BasicSkill_PhysicMagnification")]
		public int BasicSkill_PhysicMagnification { get; set;}	// BasicSkill_PhysicMagnification

		[DynamoDBProperty("BasicSkill_MagicMagnification")]
		public int BasicSkill_MagicMagnification { get; set;}	// BasicSkill_MagicMagnification

		[DynamoDBProperty("BasicSkill_AttackArea")]
		public float BasicSkill_AttackArea { get; set;}	// BasicSkill_공격범위

		[DynamoDBProperty("BasicSkill_SkillTarget")]
		public string BasicSkill_SkillTarget { get; set;}	// BasicSkill_SkillTarget

		[DynamoDBProperty("BasicSkill_MaxTargetNumber")]
		public int BasicSkill_MaxTargetNumber { get; set;}	// BasicSkill_MaxTargetNumber

		[DynamoDBProperty("BasicSkill_AttackNumber")]
		public int BasicSkill_AttackNumber { get; set;}		// BasicSkill_AttackNumber

		[DynamoDBProperty("BasicSkill_AttackPriority")]
		public string BasicSkill_AttackPriority { get; set;} // BasicSkill_AtttackPriority

		[DynamoDBProperty("BasicSkill_Explanation")]
		public string BasicSkill_Explanation { get; set;}	// BasicSkill_Explanationxw




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
		public int EquipWeapon_RandomOption { get; set;} 

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
		public int EquipArmor_RandomOption { get; set;} 

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
		public int EquipGlove_RandomOption { get; set;} 

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
		public int EquipAccessory_RandomOption { get; set;} 

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





}