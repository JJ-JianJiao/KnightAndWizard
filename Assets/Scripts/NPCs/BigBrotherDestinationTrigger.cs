using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBrotherDestinationTrigger : MonoBehaviour
{
    public Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("BigBrother") && other.CompareTag("NPC")) {
            other.GetComponent<BigBrother>().AtDestinationPoint(destination);
            QuestManager.Instance.FinishQuest("Save Big Brother");
            //LevelManager.Instance.isBigBrotherSaved = true;
        }
    }
}
