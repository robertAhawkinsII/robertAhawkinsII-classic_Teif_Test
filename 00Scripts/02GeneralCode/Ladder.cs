using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    public Transform Player;
    bool insideLadder;
    public float ladderHeight = 3.3f;
    public PlayerTheifControler playerInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerTheifControler>();
        insideLadder = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Lader")
        {
            playerInput.canMove = false;
            insideLadder = !insideLadder;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Lader")
        {
            playerInput.canMove = true;

            if (insideLadder)
            insideLadder = !insideLadder;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (insideLadder && Input.GetKey(KeyCode.W) && playerInput.PlayerCamera.localRotation.x < .3f)
        {
            Player.transform.position += (Vector3.up / ladderHeight);
        }
        if (insideLadder && Input.GetKey(KeyCode.W) && playerInput.PlayerCamera.localRotation.x > .3f)
        {
            Player.transform.position -= (Vector3.up / ladderHeight);
        }

        if(insideLadder && Input.GetKey(KeyCode.Space))
        {
            playerInput.canMove = true;
            insideLadder = !insideLadder;
        }
    }
}
