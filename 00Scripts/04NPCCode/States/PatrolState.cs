using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    protected D_PatrolState stateData;

    protected int SetPoint;
    protected bool isPlayerFullySpotted;
    protected bool isPlayerinCautionRange;
    protected bool isPlayerHeared;
    protected bool isPlayerCautionHeared;

    public PatrolState(Entity entity, FiniteStateMechine stateMechine, string animBoolName, D_PatrolState stateData) : base(entity, stateMechine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.agent.isStopped = false;
        SetPoint = Random.Range(0, entity.moveSpots.Length);
        entity.agent.SetDestination(entity.moveSpots[SetPoint].position);    
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerinCautionRange = entity.inCausionWithoutLights();
        isPlayerFullySpotted = entity.insightWithoutLights();
        isPlayerHeared = entity.HeardThePlayer();
        isPlayerCautionHeared = entity.HearedANoise();
    }
}
