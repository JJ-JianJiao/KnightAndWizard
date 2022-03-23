using System;
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

    public bool willTalk;

    public bool isDialogueOpen;

    private void Awake()
    {
        isDialogueOpen = false;
        MouseManager.Instance.ClearAllClickTarget += ClearTalkTarget;
    }


    //private void Update()
    //{
    //    if (canTalk && Input.GetMouseButton(1)) {
    //        OpenDialogue();
    //    }
    //}

    private void Update()
    {
        if (willTalk && !isDialogueOpen) {
            if (canTalk) {
                OpenDialogue();
            }
        }
    }

    private void OnMouseUp()
    {
        //if (!EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (canTalk)
        //    {
        //        OpenDialogue();
        //    }
        //}
        if (!EventSystem.current.IsPointerOverGameObject())
            willTalk = true;
    }



    void OpenDialogue() {
        //Open Dialogue UI panel
        //transfer the dialogueData to Dialogue UI Panel
        DialogueUI.Instance.UpdateDialogueData(dialogueData, this.gameObject);
        DialogueUI.Instance.UpdateMainDialogue(dialogueData.dialoguePieces[0]);

        GameManager.Instance.playerStates.GetComponent<PlayerController>().StopMoving();

        isDialogueOpen = true;
    }

    public void CloseTalkTrigger() {
        canTalk = false;
        //isDialogueOpen = false;
        talkTrigger.enabled = false;
    }


    private void ClearTalkTarget()
    {
        willTalk = false;
    }

}
