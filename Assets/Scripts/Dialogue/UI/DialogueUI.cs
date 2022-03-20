using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]
    public Image icon;
    public Text mainText;
    public Button nextButton;

    public GameObject dialoguePanel;
    [Header("Options")]
    public RectTransform optionPanel;
    public GameObject optionPrefab;

    [Header("Data")]
    public DialogueData_SO currenData;
    int currentIndex = 0;

    public GameObject currentObj;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    //private void LateUpdate()
    //{
    //    StartCoroutine("UpdateLayout", dialoguePanel.GetComponent<RectTransform>());
    //}

    IEnumerator UpdateLayout(RectTransform rect)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    void ContinueDialogue() {
        if (currentIndex < currenData.dialoguePieces.Count)
            UpdateMainDialogue(currenData.dialoguePieces[currentIndex]);
        else
            dialoguePanel.SetActive(false);
    }

    public void UpdateDialogueData(DialogueData_SO data, GameObject dialogueObj) {
        currenData = data;
        currentIndex = 0;

        currentObj = dialogueObj;
    }

    public void UpdateMainDialogue(DialoguePiece piece) {
        ++currentIndex;
        dialoguePanel.SetActive(true);
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else icon.enabled = false;

        mainText.text = "";
        mainText.text = piece.text;




        if (piece.options.Count == 0 && currenData.dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            //nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);

        }
        else
        {
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
            nextButton.interactable = false;
        }

        //Create current options
        CreateOptions(piece);
        StartCoroutine("UpdateLayout", dialoguePanel.GetComponent<RectTransform>());
    }

    void CreateOptions(DialoguePiece piece) {
        if (optionPanel.childCount > 0) {
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.GetComponent<OptionUI>().UpdateOption(piece, piece.options[i], currentObj);
        }
    }
}
