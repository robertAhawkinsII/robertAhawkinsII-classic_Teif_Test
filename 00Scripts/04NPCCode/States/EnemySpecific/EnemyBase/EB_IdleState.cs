using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB_IdleState : IdleState
{

    private EnemyBase aI_Base;

    public EB_IdleState(Entity entity, FiniteStateMechine stateMechine, string animBoolName, 
        D_IdleState stateData, EnemyBase aI_Base) : base(entity, stateMechine, animBoolName, stateData)
    {
        this.aI_Base = aI_Base;
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
            stateMechine.ChangeState(aI_Base.alertState);
        }

        if (isPlayerCautionHeared)
        {
            aI_Base.setPlayerLastKnownPoint();
            stateMechine.ChangeState(aI_Base.noiseCaution);
        }
        if (isPlayerinCautionRange)
        {
            aI_Base.setPlayerLastKnownPoint();
            stateMechine.ChangeState(aI_Base.cautionState);
        }

        if (isIdleTimeOver)
        {
            stateMechine.ChangeState(aI_Base.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
