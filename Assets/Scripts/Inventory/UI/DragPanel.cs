using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour,IDragHandler, IPointerDownHandler
{
    RectTransform rectTransform;

    Canvas canvas;

    Canvas InvetoryCanvas;
    Canvas QuestCanvas;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = InventoryManager.Instance.GetComponent<Canvas>();

        InvetoryCanvas = GameObject.Find("Inventory Canvas").GetComponent<Canvas>();
        QuestCanvas = GameObject.Find("Quest Canvas").GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
        if (transform.parent.GetComponent<Canvas>()) {
            //transform.parent.GetComponent<Canvas>().sortingOrder = 1;
            if (transform.parent.name.Contains("Inventory Canvas"))
            {
                InvetoryCanvas.sortingOrder = 1;
                QuestCanvas.sortingOrder = 0;
            }
            else if (transform.parent.name.Contains("Quest Canvas"))
            {
                InvetoryCanvas.sortingOrder = 0;
                QuestCanvas.sortingOrder = 1;
            }
        }
        //Debug.Log(rectTransform.GetSiblingIndex());

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if(!name.Contains("ChestContainer"))
        if (transform.parent.name.Contains("Inventory Canvas"))
        {
            rectTransform.SetSiblingIndex(InventoryManager.Instance.dragSiblingIndex);
        }
    }
}
