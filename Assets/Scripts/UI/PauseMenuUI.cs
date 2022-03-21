using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button saveBtn;
    public Button loadBtn;
    public Button backToMainBtn;
    public Button audioBtn;

    public GameObject audioPanel;

    [Header("AudioPanel")]
    public Slider bgmSlider;
    public Slider effectsSlider;
    public Button audioBackButton;

    public AudioMixerSnapshot pause;
    public AudioMixerSnapshot unpause;

    private void Awake()
    {
        saveBtn.onClick.AddListener(SaveGameData);
        loadBtn.onClick.AddListener(LoadGameData);
        backToMainBtn.onClick.AddListener(BackToMain);
        audioBtn.onClick.AddListener(AuidoOnClick);
        audioBackButton.onClick.AddListener(AudioBackButtonOnClick);

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseMenuPanel.activeInHierarchy)
            {
                pauseMenuPanel.SetActive(false);
                audioPanel.SetActive(false);

                Time.timeScale = 1;
                //unpause.TransitionTo(0.01f);
            }
            else {
                pauseMenuPanel.SetActive(true);

                Time.timeScale = 0;
                //pause.TransitionTo(0.01f);
            }
            LowPass();
        }
    }

    void LowPass() {
        if (Time.timeScale == 0)
        {
            //pause.TransitionTo(0.01f);
            pause.TransitionTo(0.001f);

        }
        else {
            //unpause.TransitionTo(0.01f);
            unpause.TransitionTo(0.001f);


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

    private void AuidoOnClick()
    {
        audioPanel.SetActive(true);
    }


    private void AudioBackButtonOnClick()
    {
        audioPanel.SetActive(false);
    }
}
