using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;
    public Text itemInfoText;

    RectTransform rectTransform;

    public string slotType;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupTooltip(ItemData_SO item) {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;

    }
    public void SetupTooltip(ItemData_SO item, string type)
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;
        slotType = type;
    }
    private void Update()
    {
        UpdatePosition();
    }
    private void OnEnable()
    {
        UpdatePosition();
    }

    public void UpdatePosition() {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];

        rectTransform.GetWorldCorners(corners);

        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        if (mousePos.y < height)
        {
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        }
        else if (Screen.width - mousePos.x > width)
        {
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        }
        else {
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;

        }

        //rectTransform.position = mousePos;
    }
}
