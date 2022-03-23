using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType { 
        SameScene, DiffrentScene
    }
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;

    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;

    public bool willTrans;

    private void Awake()
    {
        MouseManager.Instance.ClearAllClickTarget += ClearTransInteract;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E) && canTrans)
        //{
        //    SceneController Portal
        //    SceneController.Instance.TransitionToDestination(this);
        //}
        if (willTrans && canTrans)
        {
            //SceneController Portal
            willTrans = false;
            GameManager.Instance.playerStates.GetComponent<PlayerController>().StopMoving();
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    private void OnMouseUp()
    {
        willTrans = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }

    private void ClearTransInteract() {
        willTrans = false;
    }
}
