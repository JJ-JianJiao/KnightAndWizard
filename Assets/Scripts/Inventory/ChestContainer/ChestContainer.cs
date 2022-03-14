using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestContainer : MonoBehaviour
{

    Animator anim;
    public GameObject chestContainerUIPrefab;
    public Transform InventoryUI;

    public GameObject currentChestUI;

    public Button closeCurrentChestBtn;

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
        InventoryManager.Instance.chestContainer01UI = currentChestUI.transform.GetChild(2).GetComponent<ContainerUI>();
        InventoryManager.Instance.chestContainer01UI.RefreshUI();
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
