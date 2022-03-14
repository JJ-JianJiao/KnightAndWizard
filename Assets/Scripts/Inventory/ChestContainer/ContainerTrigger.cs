using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player")) {
            transform.root.GetComponent<ChestContainer>().CloseContainer();
        }
    }
}
