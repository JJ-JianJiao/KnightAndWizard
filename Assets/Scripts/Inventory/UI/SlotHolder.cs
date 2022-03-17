using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum SlotType { BAG, WEAPON, ARMOR, ACTION,CONTAINER01, CONTAINER02, CONTAINER03 }
public class SlotHolder : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;

    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0) {
            UseItem();
        }
    }

    public void UseItem() {

        if (itemUI.GetItem() == null) {
            return;
        }

        if (itemUI.GetItem().itemType == ItemType.Useable  && itemUI.Bag.items[itemUI.Index].amount > 0) {
            GameManager.Instance.playerStates.ApplyHealth(itemUI.GetItem().useableItemData.healHealthValue);

            itemUI.Bag.items[itemUI.Index].amount--;
        }
        UpdateItem();
    }

    public void UpdateItem() {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.CONTAINER01:
                itemUI.Bag = InventoryManager.Instance.ChestContainer01Data;
                break;
            case SlotType.CONTAINER02:
                itemUI.Bag = InventoryManager.Instance.ChestContainer02Data;
                break;
            case SlotType.CONTAINER03:
                itemUI.Bag = InventoryManager.Instance.ChestContainer03Data;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                //TODO: switch weapon
                if (GameManager.Instance.playerStates != null)
                {
                    if (itemUI.Bag.items[itemUI.Index].itemData != null)
                    {
                        if (itemUI.Index == 0)
                            GameManager.Instance.playerStates.ChangeWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                    }
                    else
                    {
                        if (itemUI.Index == 0)
                            GameManager.Instance.playerStates.UnEquipWeapon();
                    }
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                if (GameManager.Instance.playerStates != null)
                {
                    if (itemUI.Bag.items[itemUI.Index].itemData != null)
                    {
                        if (itemUI.Index == 1)
                            GameManager.Instance.playerStates.ChangeRightWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                    }
                    else
                    {
                        if (itemUI.Index == 1)
                            GameManager.Instance.playerStates.UnEquipRightWeapon();
                    }
                }
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
            default:
                break;
        }


        var item = itemUI.Bag.items[itemUI.Index];

        itemUI.SetupItemUI(item.itemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem()) {

            //InventoryManager.Instance.tooltip.SetupTooltip(itemUI.GetItem());
            string type = "";
            if (gameObject.name.Contains("Action Btn")) {
                type = "Action Btn";
            }
            else if (gameObject.name.Contains("Slot Holder"))
            {
                type = "Slot Holder";
            }
            else if (gameObject.name.Contains("EquipmentSlot"))
            {
                type = "EquipmentSlot";
            }

            InventoryManager.Instance.tooltip.SetupTooltip(itemUI.GetItem(), type);
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //Debug.Log(name);

        if(gameObject.name.Contains(InventoryManager.Instance.tooltip.slotType))
            InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }
}
