using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    public void RefreshUI() {
        if (this.name.Contains("equipment")) 
            Debug.Log("::");
        for (int i = 0; i < slotHolders.Length; i++)
        {
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }
    }

    public void RefreshUI(SlotType slotType)
    {
        for (int i = 0; i < slotHolders.Length; i++)
        {
            slotHolders[i].slotType = slotType;

            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }
    }
}
