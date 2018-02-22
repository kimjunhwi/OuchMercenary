using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLineSlot : MonoBehaviour
{

    public InventorySlot[] inventorySlots;

    private const int inventorySlotCount = 6;

    public void Init()
    {
        inventorySlots = new InventorySlot[inventorySlotCount];
        for(int i=0; i< inventorySlotCount; i++)
        {
            inventorySlots[i] = this.gameObject.transform.GetChild(i).GetComponent<InventorySlot>();
            inventorySlots[i].Init();
        }
    }
}
