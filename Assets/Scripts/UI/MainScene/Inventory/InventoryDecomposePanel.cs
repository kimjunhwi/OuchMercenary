using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class InventoryDecomposePanel : MonoBehaviour
{
    public InventoryDecompose inventoryDecompose;

    public void Init()
    {
        inventoryDecompose = this.gameObject.transform.GetChild(1).gameObject.AddComponent<InventoryDecompose>();
        inventoryDecompose = this.gameObject.transform.GetChild(1).GetComponent<InventoryDecompose>();
        inventoryDecompose.decomposePanel = this;
        inventoryDecompose.Init();
    }
}
