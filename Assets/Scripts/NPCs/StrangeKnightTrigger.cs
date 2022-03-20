using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeKnightTrigger : MonoBehaviour
{
    DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = transform.parent.GetComponent<DialogueController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dialogueController.dialogueData != null)
        {
            dialogueController.canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            dialogueController.canTalk = true;
        }
    }
}
