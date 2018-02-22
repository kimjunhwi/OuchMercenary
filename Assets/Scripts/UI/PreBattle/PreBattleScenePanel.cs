using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ReadOnlys;

public class PreBattleScenePanel : MonoBehaviour {

	public enum E_SORT_TYPE
	{
		E_LEVEL_SORT = 0,
		E_TIER_SORT,
		E_ENHANCE_SORT,
	}


	public Button StartBattle_Button;
	public Button RepeatBattle_Button;

	public Toggle All_Characters_Toggle;
	public Toggle Commander_Characters_Toggle;
	public Toggle Close_Characters_Toggle;
	public Toggle Range_Characters_Toggle;
	public Toggle Favorite_Characters_Toggle;

	public Transform contentsPanel;

	public Button Level_Sort_Button;
	public Button Tier_Sort_Button;
	public Button Enhance_Sort_Button;

	public Button Information_Button;

	private const string sInfoText = "전투준비";


	public GameObject InfoGameObject;
	public StageInfoPanel stageInfoPanel;

	public GameObject prepareCharicObject;
	public MercenaryDispatchPanel mercenaryDispath;

	int nBeforeTypeIndex= -1 ;

	Player player;

	public List<PrepareCharacter> list_all_Character = new List<PrepareCharacter>();

	void Awake()
	{
		SetUp ();
	}


	public void SetUp()
	{
		StartBattle_Button = transform.GetChild (0).GetComponent<Button> ();
		RepeatBattle_Button = transform.GetChild (1).GetComponent<Button> ();

		StartBattle_Button.onClick.AddListener (StartBattle_Click);

		All_Characters_Toggle = transform.GetChild (2).GetComponent<Toggle> ();
		Commander_Characters_Toggle = transform.GetChild (3).GetComponent<Toggle> ();
		Close_Characters_Toggle = transform.GetChild (4).GetComponent<Toggle> ();
		Range_Characters_Toggle = transform.GetChild (5).GetComponent<Toggle> ();
		Favorite_Characters_Toggle = transform.GetChild (6).GetComponent<Toggle> ();

		All_Characters_Toggle.onValueChanged.AddListener((x) => SetCharacterSlot(((int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL)));
		Commander_Characters_Toggle.onValueChanged.AddListener((x) => SetCharacterSlot(((int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER)));
		Close_Characters_Toggle.onValueChanged.AddListener((x) => SetCharacterSlot(((int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_MELEE)));
		Range_Characters_Toggle.onValueChanged.AddListener((x) => SetCharacterSlot(((int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_RANGE)));
		Favorite_Characters_Toggle.onValueChanged.AddListener((x) => SetCharacterSlot(((int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_FAVORITE)));

		Level_Sort_Button = transform.GetChild (9).GetChild (0).GetComponent<Button> ();
		Tier_Sort_Button = transform.GetChild (9).GetChild (1).GetComponent<Button> ();
		Enhance_Sort_Button = transform.GetChild (9).GetChild (2).GetComponent<Button> ();
		Information_Button = transform.GetChild (9).GetChild (3).GetComponent<Button> ();

		Level_Sort_Button.onClick.AddListener(delegate (){SortCharacterType ((int)E_SORT_TYPE.E_LEVEL_SORT); });
		Tier_Sort_Button.onClick.AddListener (delegate (){SortCharacterType ((int)E_SORT_TYPE.E_TIER_SORT); });
		Enhance_Sort_Button.onClick.AddListener (delegate (){SortCharacterType ((int)E_SORT_TYPE.E_ENHANCE_SORT); });

		stageInfoPanel = InfoGameObject.GetComponent<StageInfoPanel> ();

		stageInfoPanel.Init (this);

		//상세정보창 클릭 등록
		Information_Button.onClick.AddListener (InformationButton);

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

	void StartBattle_Click()
	{
		if (mercenaryDispath.IsStartCheck () == false)
			return;

		GameManager.Instance.LoadScene(ReadOnlys.E_SCENE_INDEX.E_BATTLE, E_SCENE_INDEX.E_MENU, true);
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

	public void InformationButton()
	{
		if (InfoGameObject.activeSelf == false) {
			if (mercenaryDispath.read_Batch_Charic != null) {
				InfoGameObject.SetActive (true);
				stageInfoPanel.SetUp (mercenaryDispath.read_Batch_Charic.charicData);
			}
		} else {
			
			mercenaryDispath.ChangeSpriteToDispatchingImage (true);
			InfoGameObject.SetActive (false);
		}

	}

	public void SetCharacterSlot(int _nIndex)
	{
		RemoveIcon ();

		nBeforeTypeIndex = _nIndex;

		//처음에는 무조건 전체 탭에 있는 캐릭터들
		switch (nBeforeTypeIndex) 
		{
		case (int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_TOTAL:
			{
				for (int i = 0; i < list_all_Character.Count; i++) {
					list_all_Character [i].gameObject.GetComponent<RectTransform> ().SetSiblingIndex (i);
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

	public void SortCharacterType(int _nSortType)
	{
		RemoveIcon ();

		//처음에는 무조건 전체 탭에 있는 캐릭터들
		switch (_nSortType) 
		{
		case (int)E_SORT_TYPE.E_LEVEL_SORT:
			{
				Debug.Log ("Level Sort");
			}
			break;
		case (int)E_SORT_TYPE.E_TIER_SORT:
			{
				Debug.Log ("Tier");

				list_all_Character.Sort((delegate(PrepareCharacter x, PrepareCharacter y) {
					return x.charicData.m_nTier.CompareTo( y.charicData.m_nTier ); // 작은 순서대로 정렬.
				}));
			}
			break;
		case (int)E_SORT_TYPE.E_ENHANCE_SORT:
			{
				Debug.Log ("Enhance Sort");
			}
			break;
		}

		SetCharacterSlot (nBeforeTypeIndex);
	}
}
