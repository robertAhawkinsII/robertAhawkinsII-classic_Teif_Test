using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;

    protected float idleTime;

    protected bool isIdleTimeOver;

    protected bool isPlayerFullySpotted;
    protected bool isPlayerinCautionRange;
    protected bool isPlayerHeared;
    protected bool isPlayerCautionHeared;

    public IdleState(Entity entity, FiniteStateMechine stateMechine, string animBoolName, D_IdleState stateData) : base(entity, stateMechine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.agent.isStopped = true;
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();
        entity.agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time > startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
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

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
