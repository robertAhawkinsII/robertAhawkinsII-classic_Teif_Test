using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB_AttackState : AttackState
{

    EnemyBase ai;

    public EB_AttackState(Entity entity, FiniteStateMechine stateMechine, string animBoolName, D_attackStateData stateData, EnemyBase ai) 
        : base(entity, stateMechine, animBoolName, stateData)
    {
        this.ai = ai;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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
        ai.FacePlayer();

        if (isAttackTimeOver)
        {
            if (isPlayerFullySpotted)
            {
                stateMechine.ChangeState(ai.alertState);
            }

            if (isPlayerinCautionRange)
            {
                stateMechine.ChangeState(ai.cautionState);
            }

            if (!isPlayerinCautionRange && !isPlayerFullySpotted)
            {
                stateMechine.ChangeState(ai.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
