using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenerCheckTrigger : MonoBehaviour
{
    [SerializeField] Door parentDoor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Character" || other.tag == "Player")
        {
            parentDoor.SetOpener(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        parentDoor.SetOpener(null);
    }
}
