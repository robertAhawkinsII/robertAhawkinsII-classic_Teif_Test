using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB_CautionAlertState : CautionState
{

    private EnemyBase ai;
    private Vector3 lastknownPoint;
    float destinationDis;

    public EB_CautionAlertState(Entity entity, FiniteStateMechine stateMechine, string animBoolName,
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
            ai.setPlayerLastKnownPoint();
            stateMechine.ChangeState(ai.alertState);
        }

        if (isPlayerCautionHeared)
        {
            ai.setPlayerLastKnownPoint();
            stateMechine.ChangeState(ai.noiseCaution);
        }

        if (!isPlayerinCautionRange && destinationDis < 0.5f)
        {
            stateMechine.ChangeState(ai.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
