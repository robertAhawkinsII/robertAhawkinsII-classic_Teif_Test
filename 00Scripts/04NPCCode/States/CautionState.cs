using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionState : State
{
    protected D_PlayerDetected stateData;


    protected bool isPlayerFullySpotted;
    protected bool isPlayerinCautionRange;
    protected bool isPlayerHeared;
    protected bool isPlayerCautionHeared;

    public CautionState(Entity entity, FiniteStateMechine stateMechine,
        string animBoolName, D_PlayerDetected stateData) : base(entity, stateMechine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        entity.agent.isStopped = false;
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
