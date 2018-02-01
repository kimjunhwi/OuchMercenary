using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ReadOnlys;

public class PreBattleScenePanel : MonoBehaviour {

	private const string sInfoText = "전투준비";
	public Transform contentsPanel;

	public GameObject InfoGameObject;
	public StageInfoPanel stageInfoPanel;

	public SimpleObjectPool simpleCharicPool;
	public MercenaryDispatchPanel mercenaryDispath;

	public Button StartBattle_Button;

	Player player;

	void Awake()
	{
		player = GameManager.Instance.GetPlayer ();

		RemoveIcon ();
	}

	private void OnEnable()
	{
		AddButton ();
	}

	private void OnDisable()
	{
		RemoveIcon ();
	}

	private void RemoveIcon()
	{
		while (contentsPanel.childCount > 0)
		{
			GameObject toRemove = contentsPanel.GetChild(0).gameObject;
			simpleCharicPool.ReturnObject(toRemove);
		}
	}

	private void AddButton()
	{
		for (int i = 0; i < player.TEST_MY_HERO.Count; i++) {
			
			CharacterStats item = player.TEST_MY_HERO [i];

			GameObject newButton = simpleCharicPool.GetObject ();
			newButton.transform.SetParent (contentsPanel, false);
			newButton.transform.localScale = Vector3.one;

			PrepareCharacter sampleButton = newButton.GetComponent<PrepareCharacter> ();
			sampleButton.SetUp (this, item);

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

			}
			break;
		case (int)E_PREPAREBATTLE_CHARCTERTYPE.E_PREPAREBATTLE_CHARCTERTYPE_COMMANDER:
			{

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
