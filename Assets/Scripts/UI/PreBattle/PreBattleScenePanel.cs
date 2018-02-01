using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ReadOnlys;

public class PreBattleScenePanel : MonoBehaviour {

	public Button StartBattle_Button;
	public Button RepeatBattle_Button;

	public Toggle All_Characters_Toggle;
	public Toggle Commander_Characters_Toggle;
	public Toggle Close_Characters_Toggle;
	public Toggle Range_Characters_Toggle;
	public Toggle Favorite_Characters_Toggle;

	public Transform contentsPanel;

	public Button Sort_Tier_Button;
	public Button Sort_Enhance_Button;
	public Button Sort_Level_Button;
	public Button InformationIamge;



	private const string sInfoText = "전투준비";


	public GameObject InfoGameObject;
	public StageInfoPanel stageInfoPanel;

	public GameObject prepareCharicObject;
	public MercenaryDispatchPanel mercenaryDispath;


	Player player;

	public List<PrepareCharacter> list_all_Character = new List<PrepareCharacter>();

	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();

		for (int nIndex = 0; nIndex < player.TEST_MY_HERO.Count; nIndex++) {

			CharacterStats item = player.TEST_MY_HERO [nIndex];

            item.m_nBatchIndex = -1;

			GameObject obj = Instantiate (prepareCharicObject);
			obj.transform.SetParent (contentsPanel, false);
			obj.transform.localScale = Vector3.one;

			PrepareCharacter sampleButton = obj.GetComponent<PrepareCharacter> ();
			sampleButton.SetUp (this, item);

			list_all_Character.Add (sampleButton);
		}

		RemoveIcon ();
	}

	private void OnEnable()
	{
		SetCharacterSlot (0);
	}

	private void OnDisable()
	{
		RemoveIcon ();
	}

	private void RemoveIcon()
	{
		int nCount = contentsPanel.childCount;

		for (int nIndex = 0; nIndex < nCount; nIndex++) 
		{
			GameObject toRemove = contentsPanel.GetChild (nIndex).gameObject;
			toRemove.SetActive (false);
		}
	}



	public void SetCharacterSlot(int _nIndex)
	{
		RemoveIcon ();

		//처음에는 무조건 전체 탭에 있는 캐릭터들
		switch (_nIndex) 
		{
		case (int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL:
			{
				for (int i = 0; i < list_all_Character.Count; i++) {
					list_all_Character [i].gameObject.SetActive (true);
				}
			}
			break;
		case (int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER:
			{
				for (int i = 0; i < list_all_Character.Count; i++) {
					CharacterStats item = list_all_Character [i].charicData;

					if (item.m_nCharacterIndex != 1006)
						continue;

					list_all_Character [i].gameObject.SetActive (true);
				}
			}
			break;
		case (int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE:
			{

			}
			break;
		case (int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE:
			{

			}
			break;
		case (int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE:
			{

			}
			break;
		}
	}
}
