using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteAlertPanelUI : MonoBehaviour
{
    public Button yesDelete;
    public Button noDelete;
    //[HideInInspector]
    public string deleteFileName;

    public GameObject deleteObject;

    private void Awake()
    {
        yesDelete.onClick.AddListener(DeletePlayerProfile);
        noDelete.onClick.AddListener(CancelDeleteProfile);
    }

    private void CancelDeleteProfile()
    {
        this.transform.root.GetChild(0).GetComponent<CanvasGroup>().interactable = true;
        gameObject.SetActive(false);
    }

    public void DeletePlayerProfile()
    {
        if (SaveManager.Instance.DeleteSelectedPlayerProfile(deleteFileName)) {
            //Destroy(deleteObject);
            transform.root.GetComponent<ContinueGameUI>().RefreshBoard();
        }
        this.transform.root.GetChild(0).GetComponent<CanvasGroup>().interactable = true;
        gameObject.SetActive(false);
    }
}
