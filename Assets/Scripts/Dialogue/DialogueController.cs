using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO dialogueData;
    public bool canTalk = false;
    //public float talkDistance;

    public Collider talkTrigger;


    //private void Update()
    //{
    //    if (canTalk && Input.GetMouseButton(1)) {
    //        OpenDialogue();
    //    }
    //}

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canTalk)
            {
                OpenDialogue();
            }
        }
    }



    void OpenDialogue() {
        //Open Dialogue UI panel
        //transfer the dialogueData to Dialogue UI Panel
        DialogueUI.Instance.UpdateDialogueData(dialogueData, this.gameObject);
        DialogueUI.Instance.UpdateMainDialogue(dialogueData.dialoguePieces[0]);
    }

    public void CloseTalkTrigger() {
        canTalk = false;
        talkTrigger.enabled = false;
    }

}
