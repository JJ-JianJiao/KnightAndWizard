using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;

        //record item data
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);

    }

    public void OnDrag(PointerEventData eventData)
    {
        //follow mourse position
        transform.position = eventData.position;
        //if (EventSystem.current.IsPointerOverGameObject())
        //    Debug.Log(true);
        //else
        //{
        //    Debug.Log(false);
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // put the item down and exchange item data
        //whether the pointer points UI item


        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckActionUI(eventData.position) ||
                InventoryManager.Instance.CheckEquipmentUI(eventData.position) ||
                InventoryManager.Instance.CheckChestContainer01UI(eventData.position) ||
                InventoryManager.Instance.CheckInventoryUI(eventData.position)) {

                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }
                //Debug.Log(eventData.pointerEnter.gameObject);

                if (targetHolder != InventoryManager.Instance.currentDrag.originalHolder)
                {
                    switch (targetHolder.slotType)
                    {
                        case SlotType.BAG:
                            SwapItem();
                            break;
                        case SlotType.CONTAINER01:
                            SwapItem();
                            break;
                        case SlotType.WEAPON:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                                SwapItem();
                            break;
                        case SlotType.ARMOR:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                                SwapItem();
                            break;
                        case SlotType.ACTION:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable)
                                SwapItem();
                            break;
                        default:
                            break;
                    }
                }
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        else { 
            //TODO: desotry the item or drag the item to the world
        }

        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);

        RectTransform t = transform as RectTransform;

        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem() {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];

        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;

        if (isSameItem && targetItem.itemData.stackable)
        {

            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
