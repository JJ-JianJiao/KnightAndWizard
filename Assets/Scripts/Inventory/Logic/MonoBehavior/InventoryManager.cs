using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    //TODO: add the modol to save data at last
    [Header("Inventory Data")]

    public InventoryData_SO inventoryTemplate;

    public InventoryData_SO inventoryData;

    public InventoryData_SO actionData;

    public InventoryData_SO actionTemplate;

    public InventoryData_SO equipmentData;

    public InventoryData_SO equipmentTemplate;


    public InventoryData_SO ChestContainer01Template;

    public InventoryData_SO ChestContainer01Data;

    public InventoryData_SO ChestContainer02Template;

    public InventoryData_SO ChestContainer02Data;

    public InventoryData_SO ChestContainer03Template;

    public InventoryData_SO ChestContainer03Data;



    [Header("ContainerS")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;

    public ContainerUI chestContainer01UI;
    public ContainerUI chestContainer02UI;
    public ContainerUI chestContainer03UI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;


    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statsPanel;

    bool isBagOpen = false;
    bool isStatsOpen = false;

    [Header("Stats Text")]
    public Text healthText;
    public Text attackText;

    [Header("Tooltip")]
    public ItemTooltip tooltip;

    public int dragSiblingIndex = 2;

    protected override void Awake()
    {
        base.Awake();
        if (inventoryTemplate != null) {
            inventoryData = Instantiate(inventoryTemplate);
        }
        if (actionTemplate != null)
        {
            actionData = Instantiate(actionTemplate);
        }
        if (equipmentTemplate != null)
        {
            equipmentData = Instantiate(equipmentTemplate);
        }
        if (ChestContainer01Template != null)
        {
            ChestContainer01Data = Instantiate(ChestContainer01Template);
        }
        if (ChestContainer02Template != null)
        {
            ChestContainer02Data = Instantiate(ChestContainer02Template);
        }

        if (ChestContainer03Template != null)
        {
            ChestContainer03Data = Instantiate(ChestContainer03Template);
        }

    }

    private void Start()
    {
        //Load data
        //LoadData();

        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) {
            isBagOpen = !isBagOpen;
            bagPanel.SetActive(isBagOpen);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isStatsOpen = !isStatsOpen;
            statsPanel.SetActive(isStatsOpen);
        }

        UpdateStatsText(GameManager.Instance.playerStates.MaxHealth, GameManager.Instance.playerStates.attackData.minDamage,
            GameManager.Instance.playerStates.attackData.maxDamage);
    }

    public void UpdateStatsText(int health, int minAttack, int maxAttack) {
        healthText.text = health.ToString();
        attackText.text = minAttack + " - " + maxAttack;
    }

    public void CloseBagPanel() {
        isBagOpen = false;
        bagPanel.SetActive(isBagOpen);
    }

    public void CloseStatsPanel() {
        isStatsOpen = false;
        statsPanel.SetActive(isStatsOpen);
    }

    #region check the drag item is in the range of Slot
    public bool CheckInventoryUI(Vector3 position) {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position)) {
                return true;
            }
        }
        return false;
    }

    public bool CheckChestContainer01UI(Vector3 position)
    {
        if (chestContainer01UI)
        {
            for (int i = 0; i < chestContainer01UI.slotHolders.Length; i++)
            {
                RectTransform t = chestContainer01UI.slotHolders[i].transform as RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }

    public bool CheckChestContainer02UI(Vector3 position)
    {
        if (chestContainer02UI)
        {
            for (int i = 0; i < chestContainer02UI.slotHolders.Length; i++)
            {
                RectTransform t = chestContainer02UI.slotHolders[i].transform as RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }

    public bool CheckChestContainer03UI(Vector3 position)
    {
        if (chestContainer03UI)
        {
            for (int i = 0; i < chestContainer03UI.slotHolders.Length; i++)
            {
                RectTransform t = chestContainer03UI.slotHolders[i].transform as RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }

    public bool CheckActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    #endregion


    #region save inventory Data

    public void SaveData() {

        //SaveManager.Instance.Save(inventoryData, inventoryData.name);
        //SaveManager.Instance.Save(actionData, actionData.name);
        //SaveManager.Instance.Save(equipmentData, equipmentData.name);

        SaveManager.Instance.SavePlayerInventoryFileToXML(inventoryData, inventoryData.name);
        SaveManager.Instance.SavePlayerInventoryFileToXML(actionData, actionData.name);
        SaveManager.Instance.SavePlayerInventoryFileToXML(equipmentData, equipmentData.name);
        SaveManager.Instance.SavePlayerInventoryFileToXML(ChestContainer01Data, ChestContainer01Data.name);
        SaveManager.Instance.SavePlayerInventoryFileToXML(ChestContainer02Data, ChestContainer02Data.name);
        SaveManager.Instance.SavePlayerInventoryFileToXML(ChestContainer03Data, ChestContainer03Data.name);

    }

    public void LoadData() {
        SaveManager.Instance.LoadPlayerInventoryFileToXML(inventoryData, inventoryData.name);
        SaveManager.Instance.LoadPlayerInventoryFileToXML(actionData, actionData.name);
        SaveManager.Instance.LoadPlayerInventoryFileToXML(equipmentData, equipmentData.name);
        SaveManager.Instance.LoadPlayerInventoryFileToXML(ChestContainer01Data, ChestContainer01Data.name);
        SaveManager.Instance.LoadPlayerInventoryFileToXML(ChestContainer02Data, ChestContainer02Data.name);
        SaveManager.Instance.LoadPlayerInventoryFileToXML(ChestContainer03Data, ChestContainer03Data.name);

        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();

    }

    #endregion
}
