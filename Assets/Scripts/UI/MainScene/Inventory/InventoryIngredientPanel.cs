using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIngredientPanel : MonoBehaviour
{
    public Text Explanation_Text;

    public void Init()
    {
        Explanation_Text = this.gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    public void SetInfo(string _string)
    { 
        Explanation_Text.text = _string;
    }
}
