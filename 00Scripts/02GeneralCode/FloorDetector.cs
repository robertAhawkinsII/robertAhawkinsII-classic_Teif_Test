using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{


    public CurentFloorStanding StandingFloor;
    public void CheckFloor()
    {
        RaycastHit _hit;

        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out _hit, 2.5f))
        {
                      
            var currentStandingLayer = _hit.transform.gameObject.layer;


            if (_hit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
                StandingFloor = CurentFloorStanding.Ground;
            else if (currentStandingLayer == LayerMask.NameToLayer("Stone"))
                StandingFloor = CurentFloorStanding.Stone;
            else if (currentStandingLayer == LayerMask.NameToLayer("Wood"))
                StandingFloor = CurentFloorStanding.Wood;
            else if (currentStandingLayer == LayerMask.NameToLayer("Carpet"))
                StandingFloor = CurentFloorStanding.Carpet;
        }
    }
}

public enum CurentFloorStanding
{
    Wood, Ground, Stone, Carpet
}