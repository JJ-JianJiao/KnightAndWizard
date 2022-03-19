using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button saveBtn;
    public Button loadBtn;
    public Button backToMainBtn;

    private void Awake()
    {
        saveBtn.onClick.AddListener(SaveGameData);
        loadBtn.onClick.AddListener(LoadGameData);
        backToMainBtn.onClick.AddListener(BackToMain);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseMenuPanel.activeInHierarchy)
            {
                pauseMenuPanel.SetActive(false);
                Time.timeScale = 1;
            }
            else {
                pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }


    private void SaveGameData()
    {
        //SavePlayerDataPlayerPrefs();
        SaveManager.Instance.SavePlayerProfileXML();
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.SaveData();
        }
        LevelManager.Instance?.SaveData();

        QuestManager.Instance?.SaveQuestSystem();

        Debug.Log("saved");
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void LoadGameData()
    {
        CharacterProfile cp = SaveManager.Instance.LoadPlayerProfileXML(GameManager.Instance.fileName);
        //SaveManager.Instance.SycPlayerData(cp);
        //if (cp.positionX != -1 && cp.positionY != -1 && cp.positionZ != -1 && cp.rotationX != -1 && cp.rotationY != -1 && cp.rotationZ != -1)
        //{
        //    GameManager.Instance.playerStates.transform.SetPositionAndRotation(new Vector3(cp.positionX, cp.positionY, cp.positionZ), Quaternion.Euler(cp.rotationX, cp.rotationY, cp.rotationZ));
        //}
        //if (InventoryManager.Instance != null)
        //{
        //    InventoryManager.Instance.LoadData();
        //}

        //LevelManager.Instance?.LoadData();
        //Debug.Log("Loaded");

        SceneController.Instance.TransitionToLoadGame(cp);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void BackToMain()
    {
        Time.timeScale = 1;
        SceneController.Instance?.TransitionToMain();
    }
}
