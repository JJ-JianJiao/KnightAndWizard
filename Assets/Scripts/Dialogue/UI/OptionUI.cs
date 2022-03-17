using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    public Button currentBtn;
    private DialoguePiece dialoguePiece;

    private string nextPieceID;

    private void Awake()
    {
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece, DialogueOption option) {
        dialoguePiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
    }

    public void OnOptionClicked() {
        if (nextPieceID == "") {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
        else{
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currenData.dialogueIndex[nextPieceID]);
        }
    }
}
