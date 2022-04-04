using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB_RangeAttackState : AttackState
{
    EnemyBase ai;

    public EB_RangeAttackState(Entity entity, FiniteStateMechine stateMechine, string animBoolName,
        D_attackStateData stateData, EnemyBase ai) : base(entity, stateMechine, animBoolName, stateData)
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
