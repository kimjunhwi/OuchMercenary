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



	public bool bIsSuccessed = false;

	public Button GoogleLogin_Button;
	public Button FaceBookLogin_Button;
	public Button GuestLogin_Button;
	public Button nickConfirm_Button;

	public GameObject nickInputObj;

	public InputField nickInputField;
	public Text loginState_Text;

	public GameObject LoginCategory_Panel;


	private Player m_Player;
	//Aws에서 받는 리스트
	public List<DBBaiscCharacter_ForGet> lDBBasicCheacter_GetList = new List<DBBaiscCharacter_ForGet> ();
	private const int nCharacterCount = 53;
	//정보 불러올때 띄우는 텍스트

	//playerpref 필요 처음 앱을 시작했는지 아닌지


	//Google
	private string AccessTokken_GP;
	//Identity ID pool 
	private string IdentityPoolId = "ap-northeast-2:8958fcc7-adb9-492d-8435-baefabf9c962";

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
		//Init LoadingPanel
		GameManager.Instance.InitLoadingPanel();




		#if UNITY_EDITOR
		//처음 실행과 아닐때의 분기
		if (PlayerPrefs.HasKey ("FirstAppActive"))
		{
			//db에서 데이터 읽어오기
			//다운되어있는  prefab에서 가져오기
			Debug.Log ("LoginSequence");
		}
		else {
			PlayerPrefs.SetString ("FirstAppActive", "True");
			CharacterDBLoadAndPutOperation ();
		}
		#elif UNITY_ANDROID

		//처음 실행과 아닐때의 분기
		if (PlayerPrefs.HasKey ("FirstAppActive"))
		{
			//db에서 데이터 읽어오기
			//다운되어있는  prefab에서 가져오기
			Debug.Log ("LoginSequence");
		}
		else {
			PlayerPrefs.SetString ("FirstAppActive", "True");
			CharacterDBLoadAndPutOperation ();
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


		_ddbClient = Client;

		playerInfo = SyncManager.OpenOrCreateDataset("PlayerInfo");

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


		playerInfo.SynchronizeAsync ();


		nickInputObj.SetActive (false);
	}

	//연동이 되어있으면 이메일과 닉을 체크 해서 연동을 한다
	public void DynamoDBCheck(string _email, string _nick)
	{

	}

	#region LoadFromAwsDB
	//DB에서 연동하여 데이터를 가져온다 Character
	private void CharacterDBLoadAndPutOperation()
	{
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

					Debug.Log(character.C_JobNames + "\n");
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

	IEnumerator isFinishLoadData(E_LOAD_STATE _state)
	{
		int nPotCount = 0;
		string sInputText = null;
		switch (_state) 
		{
		case E_LOAD_STATE.E_LOAD_GET_BASICCHARACTERDATA:
			sInputText = "기본 캐릭터 받아오는중 ";
			break;

			default:
			break;
		}

		while (true) 
		{
			//break 조건
			if (lDBBasicCheacter_GetList.Count == nCharacterCount) 
			{
				loginState_Text.text = "기본 캐릭터 불러오기 완료";
				#if UNITY_EDITOR
				StartCoroutine(GameManager.Instance.DataLoad());


				GameManager.Instance.LoadScene(E_SCENE_INDEX.E_MENU, E_SCENE_INDEX.E_LOGO, canvas );
				LoginCategory_Panel.SetActive (false);

		
				#elif UNITY_ANDROID
				LoginCategory_Panel.SetActive (true);

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

				nickInputObj.SetActive(true);
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
		CharacterDBLoadAndPutOperation ();


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

	//List 정렬 (Index 기준)
	private void CharacterListAdjust()
	{
		GameManager.Instance.lDbBasicCharacter.Sort (delegate(DBBasicCharacter A, DBBasicCharacter B) 
			{
				if(A.Index > B.Index) return 1;
				else if (A.Index < B.Index) return -1;
				return 0;
			});
		Debug.Log (lDBBasicCheacter_GetList);
		Debug.Log ("정렬 완료!");
	}

	[DynamoDBTable("CharacterBasicInfoTable")]
	public class DBBaiscCharacter_ForGet
	{
		[DynamoDBHashKey]   
		public int Index { get; set; } 						// Hash key.

		[DynamoDBRangeKey]
		public int C_Index { get; set; }					// CharacterIndex 

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

	}

}