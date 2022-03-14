using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;

    public PlayableDirector director;
    public PlayableDirector selectCharactersDirector;

    public GameObject knightChracterBoard;
    public GameObject wizardChracterBoard;

    public GameObject continueGameUI;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        quitBtn.onClick.AddListener(QuitGame);
        //newGameBtn.onClick.AddListener(NewGame);
        newGameBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueGame);

        //director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
        //selectCharactersDirector.stopped += NewGame;


    }



    void PlayTimeLine() {
        //director.Play();
        selectCharactersDirector.Play();
    }

    void QuitGame() {
        Application.Quit();
        Debug.Log("Quit game");
    }

    void NewGame(PlayableDirector obj) {
        PlayerPrefs.DeleteAll();
        //switch scene
        SceneController.Instance.TransitionToFirstLevel();

    }

    void ContinueGame() {
        //switch scene and load data
        //SceneController.Instance.TransitionToLoadGame();
        continueGameUI.SetActive(true);
    }

    public void DisplayKnightCharacterBoard() {
        knightChracterBoard.SetActive(true);
    }

    public void DisplayWizardCharacterBoard()
    {
        wizardChracterBoard.SetActive(true);
    }

 
}
