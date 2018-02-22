using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

struct stEnemyData
{
	public float fCreateTime;
	public float fPlusYPosition;
	public CharacterStats Enemy_Data;
}

struct stWaveData
{
	public float fWaveTime;
	public Queue<stEnemyData> Enemy_Queue;
}

public class BattleManager : MonoBehaviour {

	//시간을 위한 변수들 
	public int nCurMin;
	public float fCurSec;

	public float fPlusTimer = 0.0f;

	private const int nInitTime_Min = 59;
	private const int nInitTime_Sec = 59;

	//스테이지에서 각 웨이브 들을 위한 인덱스 
	public int nStageIndex;
	public int nNowWaveIndex;
	public int nMaxWaveIndex;

	public E_BATTLE_STATE Battle_State = E_BATTLE_STATE.E_NONE;

	public GameObject ResultCharacterUI;

	//공격, 수비 버튼 
	public Button ToggleButton;

	public Transform canvasTransform;
	public Transform bottomTransfrom;
	public Transform StagePanel;
	public Transform characterUI_Parent;
	public BattleResult ResultPanel;

	//위치 임시
	private Vector3 vecPosition;
	private Vector3 m_vecZeroPosition = new Vector3 (-8.5f, 1.0f, 0);

	private float m_fPlusX = 1.5f;
	private float m_fPlusY = 1.5f;

	private bool m_bIsPause = false;
	private bool m_bIsWin = false;

	public Player player;
	public DBStageData stageData;
	public SkillManager skillManager;
	public CharacterManager characterManager;

	public SimpleObjectPool characterPool;
	public SimpleObjectPool damageTextPool;
	public SimpleObjectPool skillObjectPool;
	public SimpleObjectPool ResultCharacterUIPool;
	public SimpleObjectPool TakeItemPool;

	public Transform damagetParentTransfrom;

	public Text StageNameText;
	public Text StageWaveText;
	public Text NextWaveTimeText;

	List<stWaveData> Wave_List = new List<stWaveData> ();
	public List<Character> playCharacter_List = new List<Character>();

	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();

		canvasTransform = GameObject.Find ("Canvas").transform;
		bottomTransfrom = canvasTransform.GetChild(0).transform;
		StagePanel = canvasTransform.GetChild(1).transform;
		characterUI_Parent = canvasTransform.GetChild(2).transform.GetChild(0).transform;
		ToggleButton = canvasTransform.GetChild (3).GetComponent<Button> ();
		ResultPanel = canvasTransform.GetChild(4).GetComponent<BattleResult>();

		ToggleButton.onClick.AddListener (ChangeMode);

		ResultPanel.Init (this);
		ResultPanel.gameObject.SetActive (false);
		skillManager = new SkillManager ();

		characterManager = gameObject.AddComponent<CharacterManager>();

		BattleState_set (E_BATTLE_STATE.E_INIT);
	}

	void Update()
	{
		BattleState_update ();
	}

	//-------------------------------------------------------------------------------------
	void BattleState_set(E_BATTLE_STATE _STATE)
	{
		Battle_State = _STATE;
		switch (Battle_State)
		{
		case E_BATTLE_STATE.E_INIT: BattleState_init_set(); break;
		case E_BATTLE_STATE.E_PLAY: BattleState_play_set(); break;
		case E_BATTLE_STATE.E_RESULT: BattleState_result_set(); break;
		}
	}
	//-------------------------------------------------------------------------------------
	void BattleState_update()
	{
		switch (Battle_State)
		{
		case E_BATTLE_STATE.E_INIT: BattleState_init_update(); break;
		case E_BATTLE_STATE.E_PLAY: BattleState_play_update(); break;
		case E_BATTLE_STATE.E_RESULT: BattleState_result_update(); break;
		}
	}

	//초기화---------------------------------------------------
	void BattleState_init_set()
	{
		nCurMin 		= 0;
		fCurSec 		= 0f;

		fPlusTimer 		= 0.0f;

		nStageIndex 	= 0;
		nNowWaveIndex 	= 0;
		nMaxWaveIndex 	= 0;

		stageData = GameManager.Instance.lDBStageData [0];

		StageInit ();

		characterManager.SortingCharacterLayer();

		StartCoroutine(EnemySpawning ());

		StartCoroutine (Timer (Wave_List [nNowWaveIndex].fWaveTime));

		BattleState_set (E_BATTLE_STATE.E_PLAY);
	}

	void BattleState_play_set()
	{
		Debug.Log ("Play");
	}

	void BattleState_result_set()
	{
		Debug.Log ("Result");

		ResultPanel.SetUp (m_bIsWin);
	}


	//배틀씬에 대한 업데이트------------------------------------------
	void BattleState_init_update()
	{

	}

	void BattleState_play_update()
	{
		characterManager.Actions ();
	}

	void BattleState_result_update()
	{

	}

	public void StageInit()
	{
		Wave_List.Clear ();
		characterManager.Remove_all ();

		// Stage에 관한 데이터를 가져와서 초기화 및 대입
		string[] strWaves = stageData.strWaveTimes.Split (',');

		//최고 웨이브 인덱스를 저장
		nMaxWaveIndex = strWaves.Length - 1;

		string[] strWaveEnemies = stageData.strEnemySpawnIndexs.Split ('/');
		string[] strWaveCreateTimes = stageData.strCreateTimes.Split ('/');
		string[] strWavePlusYPosition = stageData.strYPositions.Split ('/');

		for (int nIndex = 0; nIndex < strWaves.Length; nIndex++) 
		{
			stWaveData _stWave;
			stEnemyData _stEnemyData;

			_stWave.Enemy_Queue = new Queue<stEnemyData> ();

			string[] strEnemies = strWaveEnemies [nIndex].Split (','); 
			string[] strCreateTimes = strWaveCreateTimes [nIndex].Split (',');
			string[] strPlusYPosition = strWavePlusYPosition [nIndex].Split (',');

			for (int nEnemyIndex = 0; nEnemyIndex < strEnemies.Length; nEnemyIndex++) {

				_stEnemyData.fCreateTime = float.Parse (strCreateTimes [nEnemyIndex]);
				_stEnemyData.fPlusYPosition = float.Parse (strPlusYPosition [nEnemyIndex]);
				_stEnemyData.Enemy_Data = GameManager.Instance.SummonCharacter( int.Parse (strEnemies [nEnemyIndex]));

				_stWave.Enemy_Queue.Enqueue (_stEnemyData);
			}
			_stWave.fWaveTime = float.Parse(strWaves [nIndex]);

			Wave_List.Add (_stWave);
		}


		///플레이어 캐릭터 셋팅
		for (int nIndex = 0; nIndex < player.TEST_MY_HERO.Count; nIndex++) 
		{
			CharacterStats characterStats = player.TEST_MY_HERO [nIndex];

			if (characterStats.m_nBatchIndex == -1)
				continue;

			GameObject characterObject = characterPool.GetObject ();

			Character charic = null;

			//각 직업에 맞는 클래스를 추가해줌
			//if(characterStats.m_strJob.Contains("Assassin")) 
			if (characterStats.m_strJob.Contains ("warrior"))		charic = characterObject.AddComponent<Warrior> ();
			else if (characterStats.m_strJob.Contains ("assassin") || characterStats.m_strJob.Contains("clown"))	charic = characterObject.AddComponent<Assasin> ();
			else if (characterStats.m_strJob.Contains ("archer"))	charic = characterObject.AddComponent<Archer> ();
			else if (characterStats.m_strJob.Contains ("priest"))	charic = characterObject.AddComponent<Priest> ();
			else if (characterStats.m_strJob.Contains ("commander"))charic = characterObject.AddComponent<Commander> ();
			else if (characterStats.m_strJob.Contains ("knight"))	charic = characterObject.AddComponent<Knight> ();
			else if (characterStats.m_strJob.Contains ("wizard") || characterStats.m_strJob.Contains ("mage")) 	charic = characterObject.AddComponent<Wizard> ();
			else if( characterStats.m_strJob.Contains("mechanic")) 	charic = characterObject.AddComponent<Mechanic> ();

			//4x4배치이므로 x,y위치를 구하고 그 위치를 저장
			int nValue = characterStats.m_nBatchIndex % 4;
			int nDight = characterStats.m_nBatchIndex / 4;

			vecPosition = new Vector3(m_vecZeroPosition.x + (nValue * m_fPlusX), m_vecZeroPosition.y - (nDight * m_fPlusX),0.0f);

			//캐릭터에 대한 내용을 초기화 해줌
			charic.Setup (characterStats, characterManager, skillManager,this, E_Type.E_Hero,vecPosition);


			playCharacter_List.Add (charic);
			//캐릭터매니저에 추가
			characterManager.Add (charic);
		}
	}

	public IEnumerator EnemySpawning()
	{
		yield return new WaitForSeconds (0.1f);

		//마지막 웨이브가 0일 경우 종료 스폰 종료
		while (Wave_List [nMaxWaveIndex].Enemy_Queue.Count != 0) 
		{
			if (Wave_List [nNowWaveIndex].Enemy_Queue.Count == 0) 
			{

				yield return null;
			}
			else if (Wave_List [nNowWaveIndex].Enemy_Queue.Peek ().fCreateTime < fPlusTimer ) 
			{
				stEnemyData _charicData = Wave_List [nNowWaveIndex].Enemy_Queue.Dequeue ();

				CreateEnemyCharacter (_charicData.Enemy_Data, _charicData.fPlusYPosition);

				fPlusTimer -=10000;

				yield return null;
			}

			yield return null;
		}
	}

	public void CreateEnemyCharacter(CharacterStats _charicData,float _fPlusYPosition)
	{
		CharacterStats characterStats = _charicData;

		GameObject characterObject = characterPool.GetObject ();

		Character charic = null;

		if (characterStats.m_strJob.Contains ("warrior"))		charic = characterObject.AddComponent<Enemy_Warrior> ();
		else if (characterStats.m_strJob.Contains ("assassin") || characterStats.m_strJob.Contains("clown"))	charic = characterObject.AddComponent<Enemy_Assasin> ();
		else if (characterStats.m_strJob.Contains ("archer"))	charic = characterObject.AddComponent<Enemy_Archer> ();
		else if (characterStats.m_strJob.Contains ("priest"))	charic = characterObject.AddComponent<Enemy_Priest> ();
		else if (characterStats.m_strJob.Contains ("knight"))	charic = characterObject.AddComponent<Enemy_Knight> ();
		else if (characterStats.m_strJob.Contains ("wizard") || characterStats.m_strJob.Contains ("mage")) 	charic = characterObject.AddComponent<Enemy_Wizard> ();
		else if( characterStats.m_strJob.Contains("mechanic")) 	charic = characterObject.AddComponent<Enemy_Mechanic> ();

		vecPosition = new Vector3 (7 , 5 - _fPlusYPosition, 0);

		charic.Setup (characterStats, characterManager, skillManager,this, E_Type.E_Enemy,vecPosition);

		characterManager.Add (charic);
	}

	public void SortDamageTextLayer()
	{
		if (damagetParentTransfrom.childCount > 1) 
		{
			for (int nIndex = 0; nIndex < damagetParentTransfrom.childCount - 1; nIndex++) 
			{
				for (int nNextIndex = 1; nNextIndex < damagetParentTransfrom.childCount -1; nNextIndex++) 
				{
					if (damagetParentTransfrom.GetChild (nNextIndex - 1).position.y > damagetParentTransfrom.GetChild (nNextIndex).position.y) 
					{
						damagetParentTransfrom.SetSiblingIndex (nNextIndex);
					}
				}
			}
		}
	}


	public IEnumerator Timer(float _fTime)
	{
		int second = 0;
		nCurMin = 0;
		fCurSec = _fTime;

		while (fCurSec > 60) 
		{
			nCurMin++;
			fCurSec -= 60.0f;
		}
			
		while (true) 
		{
			if (E_BATTLE_STATE.E_RESULT == Battle_State)
				yield break;

			fCurSec -= Time.deltaTime;
			fPlusTimer += Time.deltaTime;

			second = (int)fCurSec;

			if(second < 10)
				NextWaveTimeText.text = nCurMin.ToString () + ":" +"0"+second.ToString ();
			else
				NextWaveTimeText.text = nCurMin.ToString () + ":" + second.ToString ();

			if (nCurMin == 0 && second <= 0f) 
			{
				fPlusTimer = 0f;
				nNowWaveIndex++;

				StageWaveText.text = nNowWaveIndex.ToString ();

				if (nNowWaveIndex == nMaxWaveIndex) 
				{
					NextWaveTimeText.text = "00:00";
					break;
				} 
				else 
				{
					nCurMin = 0;
					fCurSec = Wave_List[nNowWaveIndex].fWaveTime;

					while (fCurSec > 60) 
					{
						nCurMin++;
						fCurSec -= 60.0f;
					}

					continue;
				}
			}

			if (nCurMin != 0 && second == 0f) 
			{
				fCurSec = 59f;
				nCurMin--;
			}

			yield return null;
		}

		yield  break;
	}

	public void CharacterDie(E_Type _type)
	{
		if (Battle_State == E_BATTLE_STATE.E_RESULT)
			return;

		if (_type == E_Type.E_Enemy) 
		{
			m_bIsWin = true;
			BattleState_set (E_BATTLE_STATE.E_RESULT);
			return;

//			if (characterManager.SearchTypeCount(_type) == 0) 
//			{
//				fPlusTimer = 0f;
//				nNowWaveIndex++;
//
//				if (nNowWaveIndex > nMaxWaveIndex) 
//				{
//					m_bIsWin = true;
//					BattleState_set (E_BATTLE_STATE.E_RESULT);
//					return;
//				}
//
//				nCurMin = 0;
//				fCurSec =Wave_List[nNowWaveIndex].fWaveTime;
//
//				while (fCurSec > 60) 
//				{
//					nCurMin++;
//					fCurSec -= 60.0f;
//				}
//			}
		} 
		else if (_type == E_Type.E_Hero) 
		{
			if (characterManager.SearchTypeCount (_type) == 0) 
			{
				m_bIsWin = false;
				BattleState_set (E_BATTLE_STATE.E_RESULT);
			}
		}

	}

	// 플레이어 캐릭터들의 (공격 or 수비) 모드를 바꾼다. ------------------------------------------ 
	public void ChangeMode()
	{
		characterManager.PlayerCharacterChangeMode ();
	}
}

