using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected D_attackStateData stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerFullySpotted;
    protected bool isPlayerinCautionRange;


    protected bool isAttackTimeOver;

    public AttackState(Entity entity, FiniteStateMechine stateMechine, string animBoolName, D_attackStateData stateData) :
        base(entity, stateMechine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckIfPlayerInMinAgroRange();
        isPlayerinCautionRange = entity.inCausionWithoutLights();
        isPlayerFullySpotted = entity.insightWithoutLights();
    }

    public override void Enter()
    {
        base.Enter();
        entity.agent.isStopped = true;
        //attack play
    }

    public override void Exit()
    {
        base.Exit();
        entity.agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time > startTime + stateData.attackTime)
        {
            isAttackTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
