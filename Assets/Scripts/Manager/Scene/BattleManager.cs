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


	public int nCurMin;
	public float fCurSec;

	public float fPlusTimer = 0.0f;

	private const int nInitTime_Min = 59;
	private const int nInitTime_Sec = 59;

	public int nStageIndex;
	public int nNowWaveIndex;
	public int nMaxWaveIndex;

	List<stWaveData> Wave_List = new List<stWaveData> ();

	DBStageData stageData;

	//공격, 수비 버튼 
	public Button ToggleButton;

	//위치 임시
	private Vector3 vecPosition;
	private Vector3 m_vecZeroPosition = new Vector3 (-8.5f, 1.0f, 0);

	private float m_fPlusX = 1.5f;
	private float m_fPlusY = 1.5f;

	private bool m_bIsPause = false;

	public Player player;
	public SkillManager skillManager;
	public CharacterManager characterManager;

	public SimpleObjectPool characterPool;
	public SimpleObjectPool damageTextPool;

	public Transform damagetParentTransfrom;
	public Transform characterUI_Parent;

	public Text StageNameText;
	public Text StageWaveText;
	public Text StageTimeText;

	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();

		ToggleButton.onClick.AddListener (ChangeMode);

		skillManager = new SkillManager ();

		characterManager = gameObject.AddComponent<CharacterManager>();



		for (int nIndex = 0; nIndex < 4; nIndex++) 
		{
			
		}

		characterManager.SortingCharacterLayer();
	}

	void Update()
	{
		if(m_bIsPause == false)
		{
			characterManager.Actions();
		}
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
			string[] strCreateTimes = strWaveEnemies [nIndex].Split (',');
			string[] strPlusYPosition = strWavePlusYPosition [nIndex].Split (',');

			for (int nEnemyIndex = 0; nEnemyIndex < strEnemies.Length; nEnemyIndex++) {

				_stEnemyData.fCreateTime = float.Parse (strCreateTimes [nIndex]);
				_stEnemyData.fPlusYPosition = float.Parse (strPlusYPosition [nIndex]);
				_stEnemyData.Enemy_Data = new CharacterStats(GameManager.Instance.lDbBasicCharacter [int.Parse (strEnemies [nIndex])]);

				_stWave.Enemy_Queue.Enqueue (_stEnemyData);
			}
			_stWave.fWaveTime = float.Parse(strWaves [nIndex]);

			Wave_List.Add (_stWave);
		}


		///플레이어 캐릭터 셋팅
		for (int nIndex = 2; nIndex < player.LIST_HERO.Count; nIndex++) 
		{
			CharacterStats characterStats = player.LIST_HERO [nIndex];

			GameObject characterObject = characterPool.GetObject ();

			Character charic = null;

			//각 직업에 맞는 클래스를 추가해줌
			//if(characterStats.m_strJob.Contains("Assassin")) 
			if (characterStats.m_strJob.Contains ("Warrior")) 		charic = characterObject.AddComponent<Warrior> ();
			else if (characterStats.m_strJob.Contains ("Archer"))	charic = characterObject.AddComponent<Archer> ();
			else if (characterStats.m_strJob.Contains ("Priest"))	charic = characterObject.AddComponent<Priest> ();
			else if (characterStats.m_strJob.Contains ("Commander"))charic = characterObject.AddComponent<Commander> ();
			else if (characterStats.m_strJob.Contains ("Knight"))	charic = characterObject.AddComponent<Knight> ();
			else if (characterStats.m_strJob.Contains ("Wizard")) 	charic = characterObject.AddComponent<Wizard> ();
			else if( characterStats.m_strJob.Contains("Mechanic")) 	charic = characterObject.AddComponent<Mechanic> ();

			//4x4배치이므로 x,y위치를 구하고 그 위치를 저장
			int nValue = characterStats.m_nBatchIndex / 4;
			int nDight = characterStats.m_nBatchIndex % 4;

			vecPosition = new Vector3(m_vecZeroPosition.x + (nValue * m_fPlusX), m_vecZeroPosition.y - (nDight * m_fPlusX),0.0f);

			//캐릭터에 대한 내용을 초기화 해줌
			charic.Setup (characterStats, characterManager, skillManager,this, E_Type.E_Hero,vecPosition);

			//캐릭터매니저에 추가
			characterManager.Add (charic);
		}
	}

	public IEnumerator EnemySpawning()
	{
		yield return new WaitForSeconds (3.0f);

		while (Wave_List [nMaxWaveIndex].Enemy_Queue.Count != 0) 
		{
			if (Wave_List [nNowWaveIndex].Enemy_Queue.Count == 0) 
			{
				nNowWaveIndex++;

				if (nNowWaveIndex > nMaxWaveIndex) 
				{
					break;
				}
			}

			if (nNowWaveIndex != nMaxWaveIndex)
				StartCoroutine (Timer (Wave_List [nNowWaveIndex].fWaveTime));

			if (Wave_List [nNowWaveIndex].Enemy_Queue.Peek ().fCreateTime < fPlusTimer) 
			{
				stEnemyData _charicData = Wave_List [nNowWaveIndex].Enemy_Queue.Dequeue ();

				CreateEnemyCharacter (_charicData.Enemy_Data, _charicData.fPlusYPosition);
			}
		}
	}

	public void CreateEnemyCharacter(CharacterStats _charicData,float _fPlusYPosition)
	{
		CharacterStats characterStats = _charicData;

		GameObject characterObject = characterPool.GetObject ();

		Character charic = null;

		if (characterStats.m_strJob.Contains ("Warrior")) 		charic = characterObject.AddComponent<Enemy_Warrior> ();
		else if (characterStats.m_strJob.Contains ("Archer"))	charic = characterObject.AddComponent<Enemy_Archer> ();
		else if (characterStats.m_strJob.Contains ("Priest"))	charic = characterObject.AddComponent<Enemy_Priest> ();
		else if (characterStats.m_strJob.Contains ("Commander"))charic = characterObject.AddComponent<Commander> ();
		else if (characterStats.m_strJob.Contains ("Knight"))	charic = characterObject.AddComponent<Enemy_Knight> ();
		else if (characterStats.m_strJob.Contains ("Wizard")) 	charic = characterObject.AddComponent<Enemy_Wizard> ();
		else if( characterStats.m_strJob.Contains("Mechanic")) 	charic = characterObject.AddComponent<Enemy_Mechanic> ();

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

		while (fCurSec < 60) 
		{
			nCurMin++;
			fCurSec -= 60.0f;
		}


		while (nCurMin >= 0f) 
		{
			fCurSec -= Time.deltaTime;
			fPlusTimer += Time.deltaTime;

			second = (int)fCurSec;

			if(second < 10)
				StageTimeText.text = nCurMin.ToString () + ":" +"0"+second.ToString ();
			else
				StageTimeText.text = nCurMin.ToString () + ":" + second.ToString ();

			if (nCurMin == 0 && second <= 0f)
				break;	

			if (nCurMin != 0 && second == 0f) 
			{
				fCurSec = 59f;
				nCurMin--;
			}

			yield return null;
		}

		nNowWaveIndex++;

		yield  break;
	}

	// 플레이어 캐릭터들의 (공격 or 수비) 모드를 바꾼다. ------------------------------------------ 
	public void ChangeMode()
	{
		characterManager.PlayerCharacterChangeMode ();
	}
}

