using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class DefenceStageButton : MonoBehaviour {

	bool bIsActive = false;

	public Image buttonImage;
	public Sprite activeSprite;
	public Sprite unActiveSprite;

	public int nStageIndex;

	public StageDefensePanel defencePanel;
	public DefenceStageInfo defenceInfo;

	public void SetUpSprite(bool _bIsSetup)
	{
		buttonImage.sprite = (_bIsSetup == true) ? activeSprite : unActiveSprite;

		if (_bIsSetup == true) 
		{
			defenceInfo.nStageIndex = nStageIndex;
		}

		bIsActive = _bIsSetup;
	}

	public void Click()
	{
		if (nStageIndex == defenceInfo.nStageIndex)
			return;

		defencePanel.AllDisableDefenceButtons ();

		bIsActive = (bIsActive == false) ? true : false;

		if (bIsActive) 
		{
			defenceInfo.SetUpStage (nStageIndex);

			buttonImage.sprite = activeSprite;
		}
	}
}
