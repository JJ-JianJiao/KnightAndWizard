using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestContainer : MonoBehaviour
{

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseUp()
    {

        if (Vector3.Distance(transform.position, GameManager.Instance.playerStates.transform.position) < 2.2)
        {
            Debug.Log("Please open the chest, container!");
            anim.SetBool("Open", true);
        }
    }

    public void CloseContainer() {
        anim.SetBool("Open", false);
    }

}
