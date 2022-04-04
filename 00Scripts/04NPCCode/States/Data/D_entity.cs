using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_entity : ScriptableObject
{
    public bool PlayerSpoted = false;
    public bool PlayerisinLOS = false; //Line of sight; check
    public float FOVAngle = 160f;
    public float losRadius = 45f; //how far they can see

    //sight + memory
    public float memoryStartTime = 10f;

    //hearing

    public float noiseTravelDistance = 5f;
    public float spinSpeed = 3f;
    
    public float spinTime = 3f;

    //when they are in swinging distance and seeing you
    public float distToPlayer = 5.0f; // strafeRadius

    public float t_minStrafe = .01f;
    public float t_maxStraif = .05f;

    public int randomStrafeDir;

    //while Chasing 
    public float chaseRadius = 20f;

    public float facePlayerFactor = 20f;

    //waitTime at waypoint for potrolling
    public float startWaitTime = 1f;

}
