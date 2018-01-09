using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class BattleManager : MonoBehaviour {

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

	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();
 
		ToggleButton.onClick.AddListener (ChangeMode);

		skillManager = new SkillManager ();

		characterManager = gameObject.AddComponent<CharacterManager>();

		///임시
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

		for (int nIndex = 0; nIndex < 4; nIndex++) 
		{
			CharacterStats characterStats = player.LIST_HERO [nIndex];

			GameObject characterObject = characterPool.GetObject ();

			Character charic = null;

			if (characterStats.m_strJob.Contains ("Warrior")) 		charic = characterObject.AddComponent<Enemy_Warrior> ();
			else if (characterStats.m_strJob.Contains ("Archer"))	charic = characterObject.AddComponent<Enemy_Archer> ();
			else if (characterStats.m_strJob.Contains ("Priest"))	charic = characterObject.AddComponent<Enemy_Priest> ();
			else if (characterStats.m_strJob.Contains ("Commander"))charic = characterObject.AddComponent<Commander> ();
			else if (characterStats.m_strJob.Contains ("Knight"))	charic = characterObject.AddComponent<Enemy_Knight> ();
			else if (characterStats.m_strJob.Contains ("Wizard")) 	charic = characterObject.AddComponent<Enemy_Wizard> ();
			else if( characterStats.m_strJob.Contains("Mechanic")) 	charic = characterObject.AddComponent<Enemy_Mechanic> ();

			vecPosition = new Vector3 (7 , 5 - nIndex * 0.8F, 0);

			charic.Setup (characterStats, characterManager, skillManager,this, E_Type.E_Enemy,vecPosition);

			characterManager.Add (charic);
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

	// 플레이어 캐릭터들의 (공격 or 수비) 모드를 바꾼다. ------------------------------------------ 
	public void ChangeMode()
	{
		characterManager.PlayerCharacterChangeMode ();
	}
}

