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

    private bool needHeal;
    private bool needAlly;

    private GameObject dialogueObj;

    private void Awake()
    {
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece, DialogueOption option, GameObject currentObj) {
        dialoguePiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeQuest = option.takeQuest;

        needHeal = option.needHeal;
        needAlly = option.needAlly;

        dialogueObj = currentObj;
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
                        AudioManager.Instance.PlaySfx("Quest Complete");
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

            if (needHeal)
            {
                if (dialogueObj != null && dialogueObj.name.Equals("Priest")) {
                    dialogueObj.GetComponent<Priest>().StartHeal();
                    //DialogueUI.Instance.dialoguePanel.SetActive(false);
                }
            }
            else if (needAlly) {
                if (dialogueObj != null && dialogueObj.name.Equals("FriendKnight"))
                {
                    dialogueObj.GetComponent<FriendKnight>().JoinTeam();
                    dialogueObj.GetComponent<DialogueController>().CloseTalkTrigger();
                    //DialogueUI.Instance.dialoguePanel.SetActive(false);
                }
                else if (dialogueObj != null && dialogueObj.name.Equals("BigBrother"))
                {
                    dialogueObj.GetComponent<BigBrother>().JoinTeam();
                    dialogueObj.GetComponent<DialogueController>().CloseTalkTrigger();
                    //DialogueUI.Instance.dialoguePanel.SetActive(false);
                }
            }
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
        else{
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currenData.dialogueIndex[nextPieceID]);
        }

    }


}
