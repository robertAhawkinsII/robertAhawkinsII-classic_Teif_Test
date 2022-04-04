using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB_PatrolState : PatrolState
{
    private EnemyBase ai;
    float destinationDis;

    public EB_PatrolState(Entity entity, FiniteStateMechine stateMechine, string animBoolName,
        D_PatrolState stateData, EnemyBase aI_) : base(entity, stateMechine, animBoolName, stateData)
    {
        ai = aI_;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

       

        if (isPlayerFullySpotted || isPlayerHeared)
        {
            stateMechine.ChangeState(ai.alertState);
        }

        if (isPlayerCautionHeared)
        {
            ai.setPlayerLastKnownPoint();
            stateMechine.ChangeState(ai.noiseCaution);
        }

        if (isPlayerinCautionRange)
        {
            ai.setPlayerLastKnownPoint();
            stateMechine.ChangeState(ai.cautionState);
        }

        destinationDis = Vector3.Distance(entity.agent.transform.position, ai.moveSpots[SetPoint].position);       
        if ( destinationDis <= 0.5f)
        {
            stateMechine.ChangeState(ai.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
