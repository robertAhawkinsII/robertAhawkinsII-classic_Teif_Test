using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : Interactable
{

    private enum DoorState {  Close, Still, Open}
    private DoorState doorState;

    private bool doorOpen = false;

    public bool Locked;
    

    bool isPaused = false;

    bool interacted;

    [SerializeField] float speed = 0.1f;

    [SerializeField]
    Quaternion currentRot;

    BoxCollider phiysicsBox;

    // Start is called before the first frame update
    void Awake()
    {
        canLift = false;
        phiysicsBox = GetComponent<BoxCollider>();
    }

    public void SetOpener(GameObject target)
    {
        theUser = target;
    }

    [SerializeField] GameObject theUser;

    public override void Interact()
    {
        if (Locked)
            {
                Debug.Log("Locked!");
                return;
            }
            else
            {
                if (!doorOpen && doorState == DoorState.Close)
                {
                    //animationPausePoint = 0;
                    doorState = DoorState.Open;
                doorOpen = true;
            }
                else if (!doorOpen && doorState == DoorState.Open) //while door is opening
                {
                    doorState = DoorState.Still;
                    doorOpen = true;
                }
                else if (doorOpen && doorState == DoorState.Open)
                {
                    //animationPausePoint = 0;
                    doorState = DoorState.Close;
                }
                else if (doorOpen && doorState == DoorState.Close) // while door is opening
                {
                    doorState = DoorState.Still;
                    doorOpen = false;
                }
                else if (doorOpen && doorState == DoorState.Still)
                {
                    //animationPausePoint = 0;
                    doorState = DoorState.Close;
                doorOpen = false;
            }
                else if (!doorOpen && doorState == DoorState.Still)
                {
                    //animationPausePoint = 0;
                    doorState = DoorState.Open;
                }

                switch (doorState)
                {
                    case DoorState.Close:
                    Debug.Log("Closed!");
                    //phiysicsBox.enabled = false;
                    this.transform.DORotate(new Vector3(0, 0, 0), 1.3f, RotateMode.Fast);                      
                        break;
                    case DoorState.Still:
                    Debug.Log("Stopped!");
                    currentRot = transform.rotation;
                    //phiysicsBox.enabled = true;
                    this.transform.DORotate(new Vector3(0, currentRot.y * 100, 0), 1.3f, RotateMode.Fast);
                    break;
                    case DoorState.Open:
                    Debug.Log("Open!");
                    //phiysicsBox.enabled = false;
                    this.transform.DORotate(new Vector3(0,90, 0), 1.3f, RotateMode.Fast);
                        break;
                }

            }
    }

    // Update is called once per frame
    void Update()
    {
        if(doorState == DoorState.Open)
        {
            if(transform.localRotation.y >= .7f)
            {
                doorOpen = true;
            }
        }
        if(doorState == DoorState.Close)
        {
            if(transform.localRotation.y <= 0)
            {
                doorOpen = false;
            }
        }
    }

    private void FixLater()
    {
        if (Locked)
        {
            Debug.Log("Locked!");
            return;
        }
        else
        {
            if (!doorOpen && doorState == DoorState.Close)
            {
                //animationPausePoint = 0;
                doorState = DoorState.Open;
            }
            else if (!doorOpen && doorState == DoorState.Open) //while door is opening
            {
                doorState = DoorState.Still;
                doorOpen = true;
            }
            else if (doorOpen && doorState == DoorState.Open)
            {
                //animationPausePoint = 0;
                doorState = DoorState.Close;
            }
            else if (doorOpen && doorState == DoorState.Close) // while door is opening
            {
                doorState = DoorState.Still;
                doorOpen = false;
            }
            else if (doorOpen && doorState == DoorState.Still)
            {
                //animationPausePoint = 0;
                doorState = DoorState.Close;
            }
            else if (!doorOpen && doorState == DoorState.Still)
            {
                //animationPausePoint = 0;
                doorState = DoorState.Open;
            }

            switch (doorState)
            {
                case DoorState.Close:
                    Debug.Log("Closed!");
                    this.transform.DORotate(new Vector3(0, 0, 0), .7f, RotateMode.Fast);
                    break;
                case DoorState.Still:
                    Debug.Log("Stopped!");
                    currentRot = transform.rotation;

                    this.transform.DORotate(new Vector3(0, currentRot.y * 100, 0), .7f, RotateMode.Fast);
                    break;
                case DoorState.Open:
                    Debug.Log("Open!");
                    this.transform.DORotate(new Vector3(0, 90, 0), .7f, RotateMode.Fast);
                    break;
            }

        }
    }
}
