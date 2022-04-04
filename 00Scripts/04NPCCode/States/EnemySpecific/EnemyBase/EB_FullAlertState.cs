using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB_FullAlerState : AlertState
{
    private EnemyBase ai;

    int randomStrafeDir;

    public EB_FullAlerState(Entity entity, FiniteStateMechine stateMechine, string animBoolName
        , D_PlayerDetected stateData, EnemyBase ai) : base(entity, stateMechine, animBoolName, stateData)
    {
        this.ai = ai;
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
        

        if (!isPlayerFullySpotted && !isPlayerinCautionRange)
        {
            ai.setPlayerLastKnownPoint();
            stateMechine.ChangeState(ai.cautionState);
        }

        ai.FacePlayer();

        if (ai.CheckIfPlayerInMaxAgroRange() && !ai.CheckIfPlayerInMinAgroRange())
        {
            ai.moveToTargetPlayer();
        }

        if (isTimeToAttack)
        {
            if (preformLongRangeAttack)
            {
                stateMechine.ChangeState(ai.rangeAttackState);
            }
            else
            {
                stateMechine.ChangeState(ai.attackState);
            }
        }

        if(ai.CheckIfPlayerInMaxAgroRange() && ai.CheckIfPlayerInMaxAgroHeightRange())
        {
            //TODO: make an attack cool down Timer setps 1) stop enemy. 2) enemy ranged_attack. 3) turn nave agent back on

            randomStrafeDir = Random.Range(0, 2);
            randomStrafeStartTime = Random.Range(stateData.t_minStrafe, stateData.t_maxStraif);

            if (waitStrafeTime <= 0)
            {
                if (randomStrafeDir == 0)
                    ai.agent.SetDestination(ai.strafeLeft.position);
                else if (randomStrafeDir == 1)
                    ai.agent.SetDestination(ai.strafeRight.position);
            }
            waitStrafeTime = randomStrafeStartTime;
        }
        else
        {
            waitStrafeTime -= Time.deltaTime;
        }

        if (ai.CheckIfPlayerInMinAgroRange())
        {

            //TODO: make an attack cool down Timer setps 1) stop enemy. 2) enemy attack. 3) turn nave agent back on

            randomStrafeDir = Random.Range(0, 2);
            randomStrafeStartTime = Random.Range(stateData.t_minStrafe, stateData.t_maxStraif);

            if (waitStrafeTime <= 0)
            {
                if (randomStrafeDir == 0)
                    ai.agent.SetDestination(ai.strafeLeft.position);
                else if (randomStrafeDir == 1)
                    ai.agent.SetDestination(ai.strafeRight.position);
            }
            waitStrafeTime = randomStrafeStartTime;
        }
        else
        {
            waitStrafeTime -= Time.deltaTime;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
