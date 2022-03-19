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
    private bool takeQuest;

    private void Awake()
    {
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece, DialogueOption option) {
        dialoguePiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeQuest = option.takeQuest;
    }

    public void OnOptionClicked() {

        if (dialoguePiece.quest != null) {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(dialoguePiece.quest)
            };
            if (takeQuest) {
                //add into quest list
                if (QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //if finished, get reward
                    if (QuestManager.Instance.GetTask(newTask.questData).IsComplete) {
                        newTask.questData.GiveRewards();
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else {
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;

                    foreach (var requireItem in newTask.questData.RequireTargetName())
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }

        if (nextPieceID == "") {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
        else{
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currenData.dialogueIndex[nextPieceID]);
        }
    }
}
