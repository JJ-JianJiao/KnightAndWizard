using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController dialogueController;
    QuestData_OS currentQuest;


    public DialogueData_SO dialogueStart;
    public DialogueData_SO dialogueProgress;
    public DialogueData_SO dialogueComplete;
    public DialogueData_SO dialogueFinish;

    #region Get quest stats
    public bool IsStarted
    {
        get {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;
            }
            else return false;
        }
    }
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;
            }
            else return false;
        }
    }

    public bool IsFinish
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;
            }
            else return false;
        }
    }

    #endregion

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }

    private void Start()
    {
        dialogueController.dialogueData = dialogueStart;
        currentQuest = dialogueController.dialogueData.GetQuest();
    }

    private void Update()
    {
        if (IsStarted) {
            if (IsComplete)
            {
                dialogueController.dialogueData = dialogueComplete;
            }
            else {
                dialogueController.dialogueData = dialogueProgress;
            }
        }

        if (IsFinish) {
            dialogueController.dialogueData = dialogueFinish;
        }
    }
}
