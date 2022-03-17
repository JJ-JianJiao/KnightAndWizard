using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            //TODO: add the item to my bag
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //equip weapon
            //GameManager.Instance.playerStates.EquipWeapon(itemData);

            if (gameObject.name == "ChestWithCoins") {
                LevelManager.Instance.isChestPickUp = true;
            }
                
            Destroy(gameObject);
        }
    }
}
