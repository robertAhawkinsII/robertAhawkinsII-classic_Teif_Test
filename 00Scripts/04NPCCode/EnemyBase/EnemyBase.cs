using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : Entity
{
    [SerializeField]Gamemanager gmWorldConstant;
    [SerializeField] CharacterValues stats;

    public CharacterValues ReadableStats { get { return stats; } }

    private int randomSpot;

    //when the night starts activate this new patrole point assign script
    public void AssignNightlyDuties(Transform[] schedule)
    {
        moveSpots = new Transform[0];
        moveSpots = schedule;
    }


    public EB_IdleState idleState { get; private set; }
    public EB_PatrolState patrolState { get; private set; }
    public EB_CautionAlertState cautionState { get; private set; }
    public EB_FullAlerState alertState { get; private set; }
    public EB_AttackState attackState { get; private set; }
    public EB_RangeAttackState rangeAttackState { get; private set; }
    public EB_NoiseCautionState noiseCaution { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PatrolState patrolStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectionData;
    [SerializeField]
    private D_attackStateData attackStateData;

    private void Awake()
    {
        stats = GetComponent<CharacterValues>();
    }

    public override void Start()
    {
        base.Start();

        patrolState = new EB_PatrolState(this, stateMechine, "Patrol", patrolStateData, this);
        idleState = new EB_IdleState(this, stateMechine, "Idle", idleStateData, this);
        cautionState = new EB_CautionAlertState(this, stateMechine, "Caution", playerDetectionData, 
            this, lastKnownPosition);
        noiseCaution = new EB_NoiseCautionState(this, stateMechine, "Caution", playerDetectionData,
            this, lastKnownPosition);

        alertState = new EB_FullAlerState(this, stateMechine, "Aleart", playerDetectionData, this);
        attackState = new EB_AttackState(this, stateMechine, "Attack", attackStateData, this);
        rangeAttackState = new EB_RangeAttackState(this, stateMechine, "Range_Attack", attackStateData, this);

        stateMechine.Initialize(patrolState);
        waitTime = entityData.startWaitTime;
        randomSpot = UnityEngine.Random.Range(0, moveSpots.Length);
        gmWorldConstant = Gamemanager.instance;
    }

    

    public override void Update()
    {
        if (gmWorldConstant == null)
            return;
        base.Update(); 

        var playerCheck = gmWorldConstant.PlayerRef;      
        //if(distance <= losRadius)
        //{
        //    //check lineOSight;
        //    PlayerisinLOS = inFOV(aiEyes.transform, playerCheck.transform, FOVAngle, losRadius);
        //    StateCheckBasedonoutsideLOS(playerCheck.stealthLevel, distance);
        //    PlayerSpoted = inFOV(aiEyes.transform, playerCheck.transform, FOVAngle, losLightingFactor);
        //}
        //
        //if (nav.isActiveAndEnabled)
        //{
        //    if(!PlayerSpoted && !aiMemorizesPlayer && !aiHeardPlayer)
        //    {
        //        
        //        NoiseCheck();
        //
        //        StopCoroutine(AiMemory());
        //    }
        //    else if(aiHeardPlayer && PlayerSpoted && !aiMemorizesPlayer)
        //    {
        //        //look around functions
        //        canSpin = true;
        //        //go to noise Position();
        //        GoToNoisePosition();
        //    }
        //    else if (PlayerSpoted)
        //    {
        //        aiMemorizesPlayer = true;
        //
        //        FacePlayer();
        //
        //        ChasePlayer();
        //    }
        //    else if(aiMemorizesPlayer && !PlayerSpoted)
        //    {
        //        ChasePlayer();
        //
        //        StartCoroutine(AiMemory());
        //    }
        //}
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region Enemy Patrole Codes //May deleat later but Repurpose them
    float curRangeOfSound;
    public void setRangeOfSoundHeared(float noise) => curRangeOfSound = noise;

    //TODO: Transfer the noise scripts and make them bools 

    public void NoiseCheck()
    {
        //var thPlayer = gmWorldConstant.PlayerRef;
        //float distance = Vector3.Distance(thPlayer.transform.position, transform.position);
        //
        //if(distance <= noiseTravelDistance)
        //{
        //
        //    Debug.Log("distance" + distance + " , sound dis" + (thPlayer.noiseMaker.NoiseRadius - distance));
        //    if(distance >= thPlayer.noiseMaker.NoiseRadius - distance && distance < (thPlayer.noiseMaker.NoiseRadius * 2) - distance)
        //    {
        //        Debug.Log("WhatWasThat!?");
        //        curRangeOfSound = thPlayer.noiseMaker.NoiseRadius;
        //        //causion mode engage
        //        FacePlayer();
        //        aiHeardPlayer = true; // maybe?
        //    }
        //    else if(distance <= (thPlayer.noiseMaker.NoiseRadius * .7f) - distance) //70%
        //    {
        //        Debug.Log("Who's There?!");
        //        curRangeOfSound = thPlayer.noiseMaker.NoiseRadius;
        //        //still in causion
        //        noisePosition = thPlayer.transform.position;
        //        aiHeardPlayer = true;
        //    }
        //    else if(distance <= (thPlayer.noiseMaker.NoiseRadius * .3f) - distance)//30%
        //    {
        //        Debug.Log("Show Yourself!!");
        //        curRangeOfSound = thPlayer.noiseMaker.NoiseRadius;
        //        //alert mode
        //        noisePosition = thPlayer.transform.position;
        //
        //        aiHeardPlayer = true;
        //    }           
        //}
        //else
        //{
        //    aiHeardPlayer = false;
        //    canSpin = false;
        //}
    }

    public void GoToNoisePosition()
    {
        //agent.SetDestination(noisePosition);
        //
        //if (Vector3.Distance(transform.position, noisePosition) <= curRangeOfSound 
        //    && canSpin == true)
        //{
        //    isSpiningTime += Time.deltaTime;
        //
        //    transform.Rotate(Vector3.up * spinSpeed, Space.World);
        //
        //    if(isSpiningTime >= spinTime)
        //    {
        //        canSpin = false;
        //        aiHeardPlayer = false;
        //        isSpiningTime = 0;
        //    }
        //}
    }

    IEnumerator AiMemory()
    {
        increaseingMemoryTime = 0;

        while (increaseingMemoryTime < entityData.memoryStartTime)
        {
            increaseingMemoryTime += Time.deltaTime;
            aiMemorizesPlayer = true;
            yield return null;
        }

        aiHeardPlayer = false;
        aiMemorizesPlayer = false;
    }

    public void FacePlayer()
    {
        Vector3 direction = (gmWorldConstant.PlayerRef.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * entityData.facePlayerFactor);
    }

    #endregion
    //debugging
    #region debuging
    [SerializeField] GameObject debugEyes;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, entityData.chaseRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, entityData.noiseTravelDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, entityData.losRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(entityData.FOVAngle, debugEyes.transform.up) * debugEyes.transform.forward * entityData.losRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-entityData.FOVAngle, debugEyes.transform.up) * debugEyes.transform.forward * entityData.losRadius;

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(debugEyes.transform.position, fovLine1);
        Gizmos.DrawRay(debugEyes.transform.position, fovLine2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(debugEyes.transform.position, 
            (new Vector3(gmWorldConstant.PlayerRef.transform.position.x, 1.5f,
            gmWorldConstant.PlayerRef.transform.position.z
            )-debugEyes.transform.position).normalized * entityData.losRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(debugEyes.transform.position, debugEyes.transform.forward * entityData.losRadius);
    }
    #endregion
}
