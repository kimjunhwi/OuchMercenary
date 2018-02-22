using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAbilitySlot : MonoBehaviour {

    public Text AbilityExplanation_Text;
    public Text AbilityValue_Text;

	public void Init()
    {
        AbilityExplanation_Text = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        AbilityValue_Text = this.gameObject.transform.GetChild(2).GetComponent<Text>();
    }

    public void SetInfo(InventorySlot _invenSlot)
    {

    }
}
