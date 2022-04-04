using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB_NoiseCautionState : CautionState
{

    private EnemyBase ai;
    private Vector3 lastknownPoint;
    float destinationDis;

    public EB_NoiseCautionState(Entity entity, FiniteStateMechine stateMechine, string animBoolName,
        D_PlayerDetected stateData, EnemyBase ai, Vector3 lastKnownPoint) : base(entity, stateMechine, animBoolName, stateData)
    {
        this.ai = ai;
        lastknownPoint = lastKnownPoint;
    }

    public override void Enter()
    {
        base.Enter();
        ai.agent.SetDestination(ai.lastKnownPosition);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        destinationDis = Vector3.Distance(entity.agent.transform.position, ai.lastKnownPosition);

        
        if (isPlayerFullySpotted || isPlayerHeared)
        {
            stateMechine.ChangeState(ai.alertState);
        }

        if (isPlayerCautionHeared)
        {
            ai.setPlayerLastKnownPoint();
            stateMechine.ChangeState(ai.noiseCaution);
        }

        if (!isPlayerinCautionRange && destinationDis < ai.DopplerVar)
        {
            stateMechine.ChangeState(ai.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
