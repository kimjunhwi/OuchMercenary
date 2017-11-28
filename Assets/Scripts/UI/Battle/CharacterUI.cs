using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour {

	//캐릭터 UI관련
	
	private Image CharacterIcon;
	private Slider healthSlider;
	private Text DpsText;

	private float fDpsDamage;

	void Awake()
	{
		CharacterIcon 	= transform.GetChild(0).GetComponent<Image>();
		healthSlider 	= transform.GetChild(1).GetComponent<Slider>();
		DpsText 		= transform.GetChild(2).GetComponent<Text>();

		fDpsDamage = 0.0f;
	}

	//아이콘 추가시 변경
	//public void SetUp(string _CharacterIcon,float _fHealth)

	public void SetUp(float _fHealth)
	{
		healthSlider.maxValue = _fHealth;
		healthSlider.value = _fHealth;

		DpsText.text = string.Format("Dps : 0");
	}

	public void PlusDps(float _fDamage)
	{
		fDpsDamage += _fDamage;

		DpsText.text = string.Format("Dps : {0}",fDpsDamage);
	}

	public void ChangeHealth(float _fHealth)
	{
		healthSlider.value = _fHealth;
	}
}
