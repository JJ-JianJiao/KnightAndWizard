using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Playables;

public class SelectCharacterUI : MonoBehaviour
{
    private string playerName;
    private int staminaPoints;
    private int strengthPoints;
    private int agilityPoints;
    private int intellectPoints;
    private int totalPoints = 9;

    public string currentCharacter;

    [Header("All TMP UI")]
    public TMP_InputField characterNameInput;
    public TMP_Text staminaText;
    public TMP_Text strengthText;
    public TMP_Text agilityText;
    public TMP_Text intellectText;
    public TMP_Text currentPointText;


    [Header("All Button UI")]
    public Button closeBtn;
    public Button startBtn;
    public Button staminaLeftBtn;
    public Button staminaRightBtn;
    public Button strengthLeftBtn;
    public Button strengthRightBtn;
    public Button agilityLeftBtn;
    public Button agilityRightBtn;
    public Button intellectLeftBtn;
    public Button intellectRightBtn;

    public Button maxProfileAlertBtn;

    [Header("Two Characters")]
    public GameObject wizardCharacter;
    public GameObject knightCharacter;

    private CanvasGroup canvasGroup;



    //public PlayableDirector knightStartGameDirector;
    //public PlayableDirector wizardStartGameDirector;
    public PlayableDirector startGameDirector;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        closeBtn.onClick.AddListener(CloseBtnOnClick);
        staminaLeftBtn.onClick.AddListener(StaminaLeftBtnOnClick);
        staminaRightBtn.onClick.AddListener(StaminaRightBtnOnClick);
        strengthLeftBtn.onClick.AddListener(StrengthLeftBtnOnClick);
        strengthRightBtn.onClick.AddListener(StrengthRightBtnOnClick);
        if (currentCharacter == "Knight")
        {
            agilityLeftBtn.onClick.AddListener(AgilityLeftBtnOnClick);
            agilityRightBtn.onClick.AddListener(AgilityRightBtnOnClick);
        }
        else if (currentCharacter == "Wizard")
        {
            intellectLeftBtn.onClick.AddListener(IntellectLeftBtnOnClick);
            intellectRightBtn.onClick.AddListener(IntellectRightBtnOnClick);
        }
        startBtn.onClick.AddListener(PlayNewGameTimeline);

        startGameDirector.stopped += StartNewGame;

        maxProfileAlertBtn.gameObject.SetActive(false);
        //wizardStartGameDirector.stopped += StartNewGame;
    }

    private void OnEnable()
    {
        if (SaveManager.Instance.LoadPlayersProfles().Length >= 6) {
            maxProfileAlertBtn.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        ResetBoard();
    }

    private void Update()
    {
        if (totalPoints == 0 && characterNameInput.text != "")
            startBtn.interactable = true;
        else
            startBtn.interactable = false;
    }

    private void CloseBtnOnClick() {
        this.gameObject.SetActive(false);
        if (currentCharacter == "Knight")
        {
            wizardCharacter.GetComponent<Collider>().enabled = true;
            knightCharacter.transform.GetChild(3).gameObject.SetActive(false);
        }
        else if (currentCharacter == "Wizard") {
            knightCharacter.GetComponent<Collider>().enabled = true;
            wizardCharacter.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void StaminaLeftBtnOnClick() {
        if (staminaText.text == "0")
        {
            return;
        }
        else {
            staminaText.text = (int.Parse(staminaText.text) - 1).ToString();
            totalPoints += 1;
            currentPointText.text = totalPoints.ToString();
        }
    }

    private void StaminaRightBtnOnClick()
    {
        if (totalPoints == 0)
        {
            return;
        }
        else
        {
            staminaText.text = (int.Parse(staminaText.text) + 1).ToString();
            totalPoints -= 1;
            currentPointText.text = totalPoints.ToString();
        }
    }

    private void StrengthLeftBtnOnClick()
    {
        if (strengthText.text == "0")
        {
            return;
        }
        else
        {
            strengthText.text = (int.Parse(strengthText.text) - 1).ToString();
            totalPoints += 1;
            currentPointText.text = totalPoints.ToString();
        }
    }

    private void StrengthRightBtnOnClick()
    {
        if (totalPoints == 0)
        {
            return;
        }
        else
        {
            strengthText.text = (int.Parse(strengthText.text) + 1).ToString();
            totalPoints -= 1;
            currentPointText.text = totalPoints.ToString();
        }
    }


    private void AgilityLeftBtnOnClick()
    {
        if (agilityText.text == "0")
        {
            return;
        }
        else
        {
            agilityText.text = (int.Parse(agilityText.text) - 1).ToString();
            totalPoints += 1;
            currentPointText.text = totalPoints.ToString();
        }
    }

    private void AgilityRightBtnOnClick()
    {
        if (totalPoints == 0)
        {
            return;
        }
        else
        {
            agilityText.text = (int.Parse(agilityText.text) + 1).ToString();
            totalPoints -= 1;
            currentPointText.text = totalPoints.ToString();
        }
    }


    private void IntellectLeftBtnOnClick()
    {
        if (intellectText.text == "0")
        {
            return;
        }
        else
        {
            intellectText.text = (int.Parse(intellectText.text) - 1).ToString();
            totalPoints += 1;
            currentPointText.text = totalPoints.ToString();
        }
    }

    private void IntellectRightBtnOnClick()
    {
        if (totalPoints == 0)
        {
            return;
        }
        else
        {
            intellectText.text = (int.Parse(intellectText.text) + 1).ToString();
            totalPoints -= 1;
            currentPointText.text = totalPoints.ToString();
        }
    }

    private void ResetBoard()
    {
        characterNameInput.text = "";
        staminaText.text = "0";
        strengthText.text = "0";
        if (currentCharacter == "Knight")
            agilityText.text = "0";
        else if (currentCharacter == "Wizard")
            intellectText.text = "0";
        totalPoints = 9;
        currentPointText.text = "9";
        maxProfileAlertBtn.gameObject.SetActive(false);
    }


    private void PlayNewGameTimeline()
    {


        canvasGroup.alpha = 0;
        if (currentCharacter == "Knight")
        {
            knightCharacter.GetComponent<Collider>().enabled = false;
            knightCharacter.transform.GetChild(3).gameObject.SetActive(false);
        }
        else if (currentCharacter == "Wizard")
        {
            wizardCharacter.GetComponent<Collider>().enabled = false;
            wizardCharacter.transform.GetChild(2).gameObject.SetActive(false);
        }
        startGameDirector.Play();

    }


    private void StartNewGame(PlayableDirector obj)
    {        
        //TODO: get all data from board
        //save the data into the player class
        //add the player class into the playersList
        //save the data into file
        Debug.Log("Run SceneController");

        CharacterProfile characterProfile = GetCharacterProfileData();

        SceneController.Instance.SetCurrentPlayerPrefab(currentCharacter);
        SceneController.Instance.TransitionToFirstLevel(characterProfile);

    }

    private CharacterProfile GetCharacterProfileData()
    {
        CharacterProfile cp = new CharacterProfile(currentCharacter);

        //cp.fileName = "Empty";
        cp.name = characterNameInput.text;
        cp.stamina = int.Parse(staminaText.text);
        cp.strength = int.Parse(strengthText.text);
        if (currentCharacter == "Knight")
            cp.agility = int.Parse(agilityText.text);
        else if (currentCharacter == "Wizard")
            cp.intellect = int.Parse(intellectText.text);


        //TODO: this is just for test
        //cp.currentHealth = 12;
        //cp.currentExp = 10;
        //cp.minDamage = 100;
        //cp.maxDamage = 110;
        //cp.currentExp = 50;
        return cp;
    }

    internal void CloseAllBoard()
    {
        ResetBoard();
        CloseBtnOnClick();
    }

}
