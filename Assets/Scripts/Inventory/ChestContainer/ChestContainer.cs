using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestContainer : MonoBehaviour
{

    Animator anim;
    public GameObject chestContainerUIPrefab;
    public Transform InventoryUI;

    public GameObject currentChestUI;

    public Button closeCurrentChestBtn;

    public TMP_Text title;

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    private void OnMouseUp()
    {

        if (Vector3.Distance(transform.position, GameManager.Instance.playerStates.transform.position) < 2.2)
        {
            Debug.Log("Please open the chest, container!");
            if (anim.GetBool("Open") == false)
            {
                anim.SetBool("Open", true);
                GenerateChestContainer();
            }
        }
    }

    public void CloseContainer() {
        anim.SetBool("Open", false);
        if(currentChestUI)
            CloseChestContainer();
    }

    void GenerateChestContainer() {
        currentChestUI = Instantiate(chestContainerUIPrefab, InventoryUI);
        currentChestUI.transform.SetSiblingIndex(++InventoryManager.Instance.dragSiblingIndex);
        closeCurrentChestBtn = currentChestUI.transform.GetChild(1).GetComponent<Button>();
        closeCurrentChestBtn.onClick.AddListener(CloseChestContainer);

        title = currentChestUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        if (name.Contains("ChestContainer1")) {
            title.text = "Chest 01";
            InventoryManager.Instance.chestContainer01UI = currentChestUI.transform.GetChild(2).GetComponent<ContainerUI>();
            InventoryManager.Instance.chestContainer01UI.RefreshUI(SlotType.CONTAINER01);
        }
        else if (name.Contains("ChestContainer2")) {
            title.text = "Chest 02";
            InventoryManager.Instance.chestContainer02UI = currentChestUI.transform.GetChild(2).GetComponent<ContainerUI>();
            InventoryManager.Instance.chestContainer02UI.RefreshUI(SlotType.CONTAINER02);
        }
        else if (name.Contains("ChestContainer3"))
        {
            title.text = "Chest 03";
            InventoryManager.Instance.chestContainer03UI = currentChestUI.transform.GetChild(2).GetComponent<ContainerUI>();
            InventoryManager.Instance.chestContainer03UI.RefreshUI(SlotType.CONTAINER03);
        }

        //InventoryManager.Instance.chestContainer01UI = currentChestUI.transform.GetChild(2).GetComponent<ContainerUI>();
        //InventoryManager.Instance.chestContainer01UI.RefreshUI();
    }

    void CloseChestContainer() {
        if (anim.GetBool("Open") == true)
        {
            anim.SetBool("Open", false);
        }
        InventoryManager.Instance.dragSiblingIndex--;
        GameObject.Destroy(currentChestUI);
    }

}
