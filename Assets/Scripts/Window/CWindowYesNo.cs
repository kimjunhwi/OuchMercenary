using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CWindowYesNo : CWindow , IPointerDownHandler
{
    public Button m_button_Ads;
    public Button m_button_Goods;
	public Button m_button_delete;

	public Text Title_Text;

	GameObject getInfoGameObject;

	public void Show(string _strTitle, Action<string> _callback)
    {
        base.Show(null, _callback);

		m_button_Ads.onClick.AddListener(OnYes);
		m_button_Goods.onClick.AddListener(OnNo);
		m_button_delete.onClick.AddListener (OnDelete);

		Title_Text.text = _strTitle;
    }
    //public override void Show(GameObject _root, Action<string> _callback)
    //{
    //    base.Show(_root, _callback);
    //}

    public void OnYes()
    {
        base.Close();

        if (callback_func != null)
            callback_func("0");

        Destroy(this.gameObject);
    }
    public void OnNo()
    {
        base.Close();

        if (callback_func != null)
            callback_func("1");

        Destroy(this.gameObject);
    }

	public void OnDelete()
	{
		base.Close ();

		Destroy (this.gameObject);
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		

	}
}
