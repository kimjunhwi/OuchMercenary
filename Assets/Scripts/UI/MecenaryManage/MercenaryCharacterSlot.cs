using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ReadOnlys;

public class MercenaryCharacterSlot : ButtonUIBase
{
    public Image ActiveSlot_Image;
    public Text Name_Text;
    public Image CharacterImage;
    public Text Level_Text;
    public Slider Hp_Slider;
    public Text Hp_Text;
    public Vector2 initSlotPosition;

    public DBBasicCharacter character;
    public MercenaryTotalCharacter mercenaryTotalCharacterPanel;


    public void Init(MercenaryTotalCharacter _merTotalCharacterPanel)
    {
        character = new DBBasicCharacter();
        ActiveSlot_Image = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        Name_Text = this.gameObject.transform.GetChild(1).GetComponent<Text>();
        CharacterImage = this.gameObject.transform.GetChild(2).GetComponent<Image>();
        Level_Text = this.gameObject.transform.GetChild(3).GetComponent<Text>();
        Hp_Slider = this.gameObject.transform.GetChild(4).GetComponent<Slider>();
        Hp_Text = this.gameObject.transform.GetChild(5).GetComponent<Text>();
        initSlotPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
        mercenaryTotalCharacterPanel = _merTotalCharacterPanel;
    }

    public void SetInfo(DBBasicCharacter _character)
    {
        Name_Text.text = _character.C_JobNames;
        Level_Text.text = string.Format("{0}",_character.Levels);
        Hp_Slider.maxValue = 100;
        Hp_Slider.value = _character.Health / 2;
        Hp_Text.text = string.Format("{0} / {1}", Hp_Slider.maxValue, Hp_Slider.value);
      
        CharacterImage.sprite = GameManager.Instance.MercenaryManage_CharacterBoxImageList.Find(x => x.name == _character.m_sImage);
        character = _character;
    }

    #region Events
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.dragging == false)
        {
            //슬롯의 어디를 클릭하던지 액티브 상태가 되게 만든다.
            if (this.gameObject.name == eventData.pointerCurrentRaycast.gameObject.name || Name_Text.name == eventData.pointerCurrentRaycast.gameObject.name
            || CharacterImage.name == eventData.pointerCurrentRaycast.gameObject.name || Level_Text.name == eventData.pointerCurrentRaycast.gameObject.name ||
            Hp_Slider.name == eventData.pointerCurrentRaycast.gameObject.name || Hp_Text.name == eventData.pointerCurrentRaycast.gameObject.name)
            {
                //다른 슬롯에 액티브가 된것이 있는지 없는지 체크하여 해제한다.

                mercenaryTotalCharacterPanel.nSelectedJobIndex = character.C_Index;
                mercenaryTotalCharacterPanel.curSelectedCharacter = character;
                ActiveSlot_Image.gameObject.SetActive(true);
                Debug.Log("캐릭터 슬롯 액티브!!!");
            }
        }
           
        

        //해당 슬롯의 이미지를 누를시.
        //    Debug.Log("Cliked");
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Cliked");
    }




    #endregion
}
