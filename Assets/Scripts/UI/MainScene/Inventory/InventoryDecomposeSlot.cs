using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class InventoryDecomposeSlot : MonoBehaviour
{
    public Image Ingredient_Image;
    public Text IngredientCount_Text;
 

    public void Init()
    {
        Ingredient_Image = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        IngredientCount_Text = this.gameObject.transform.GetChild(1).GetComponent<Text>();
     
    }
	

    public void SetInfo(int _Count , Sprite _sprite)
    {
      
        Ingredient_Image.sprite = _sprite;
        IngredientCount_Text.text = string.Format("{0}", _Count);
    }
}
