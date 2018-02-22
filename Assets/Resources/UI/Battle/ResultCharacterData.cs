using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class ResultCharacterData : MonoBehaviour {

	public Image CharacterIcon;
	public Text LevelText;
	public Text NameText;
	public Slider staminaSlider;
	public Slider expSlider;
	CharacterStats charicStats;

	const float m_fChargeSpeed = 10;

	public void SetUp(CharacterStats _charic)
	{
		CharacterIcon = transform.GetChild (0).GetComponent<Image> ();
		LevelText = transform.GetChild (1).GetComponent<Text> ();
		NameText = transform.GetChild (2).GetComponent<Text> ();
		staminaSlider = transform.GetChild (3).GetComponent<Slider> ();
		expSlider = transform.GetChild (4).GetComponent<Slider> ();

		charicStats = _charic;

		CharacterIcon.sprite = ObjectCashing.Instance.LoadSpriteFromCache("UI/BoxImages/Character/" + charicStats.m_sImage);
		LevelText.text = string.Format("Lv.{0}",charicStats.m_nLevel);
		NameText.text = charicStats.m_strCharicName;

		staminaSlider.maxValue = 100;
		staminaSlider.value = 100;

		expSlider.maxValue = 100;
		expSlider.value = charicStats.m_fExp;
	}

	public IEnumerator ExpChargeSlider(float _fPlusValue)
	{
		yield return new WaitForSeconds (0.3f);

		float _fValue = 0;
		float _fSliderValue = expSlider.value;

		while (_fValue <= _fPlusValue) 
		{
			_fValue += Time.deltaTime * m_fChargeSpeed;
			_fSliderValue += Time.deltaTime *m_fChargeSpeed;

			expSlider.value = _fSliderValue;

			//레벨업
			if (expSlider.value >= expSlider.maxValue) 
			{
				expSlider.value = 0;
				_fSliderValue = 0.0f;

				charicStats.m_nLevel++;
			}



			yield return null;
		}

		charicStats.m_fExp = expSlider.value;
	}

	public IEnumerator StaminaDwonSlider(float _fDownSliderValue)
	{
		yield return new WaitForSeconds (0.3f);

		float fDownValue = staminaSlider.value - _fDownSliderValue;

		if (fDownValue < 0)
			fDownValue = 0;

		while (staminaSlider.value != fDownValue) 
		{
			staminaSlider.value =  Mathf.Lerp (staminaSlider.value, fDownValue, Time.deltaTime * m_fChargeSpeed);

			if (staminaSlider.value < 0) 
			{
				Debug.Log ("스태미나가 0이 됨 사용불가 ");
				break;
			}

			yield return null;
		}


	}
}
