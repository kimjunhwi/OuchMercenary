using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostGetSlot : MonoBehaviour
{
    //캐릭터의 정확한 정보는 없어도 된다
    //postSlot 이나 패널에서 정보를 받아와서 넣는다.

    public Image image;

    public void InitSlot()
    {
        image = this.gameObject.transform.GetChild(0).GetComponent<Image>();
    }

 

}
