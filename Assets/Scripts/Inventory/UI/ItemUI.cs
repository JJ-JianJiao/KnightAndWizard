using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public TMP_Text amount = null;

    public InventoryData_SO Bag { get; set; }
    public int Index { get; set; } = -1;

    public void SetupItemUI(ItemData_SO item, int itemAmount) {

        if (itemAmount == 0) {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }


        if (item != null)
        {
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString("00");

            icon.gameObject.SetActive(true);
        }
        else {
            icon.gameObject.SetActive(false);
        }
    }

    public ItemData_SO GetItem() {
        return Bag.items[Index].itemData;
    }
}
