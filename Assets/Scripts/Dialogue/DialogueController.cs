using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO dialogueData;
    public bool canTalk = false;
    //public float talkDistance;



    //private void Update()
    //{
    //    if (canTalk && Input.GetMouseButton(1)) {
    //        OpenDialogue();
    //    }
    //}

    private void OnMouseUp()
    {
        if (canTalk)
        {
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
