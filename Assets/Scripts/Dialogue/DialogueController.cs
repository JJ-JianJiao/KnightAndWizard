using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO dialogueData;
    bool canTalk = false;
    //public float talkDistance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dialogueData != null) {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            canTalk = false;
        }
    }

    private void Update()
    {
        if (canTalk && Input.GetMouseButton(1)) {
            OpenDialogue();
        }
    }

    void OpenDialogue() {
        //Open Dialogue UI panel
        //transfer the dialogueData to Dialogue UI Panel
        DialogueUI.Instance.UpdateDialogueData(dialogueData);
        DialogueUI.Instance.UpdateMainDialogue(dialogueData.dialoguePieces[0]);
    }

}
