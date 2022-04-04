using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : State
{
    protected D_PlayerDetected stateData;


    protected bool isPlayerFullySpotted;
    protected bool isPlayerinCautionRange;

    protected float randomStrafeStartTime;
    protected float waitStrafeTime;


    protected bool isTimeToAttack;
    protected bool preformLongRangeAttack;

    public AlertState(Entity entity, FiniteStateMechine stateMechine,
        string animBoolName, D_PlayerDetected stateData) : base(entity, stateMechine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerinCautionRange = entity.inCausionWithoutLights();
        isPlayerFullySpotted = entity.insightWithoutLights();
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

        if(Time.time >= startTime + stateData.attackStartTime)
        {
            if (!entity.CheckIfPlayerInMaxAgroRange()
               && !entity.CheckIfPlayerInMaxAgroHeightRange()
               && !entity.CheckIfPlayerInMinAgroRange())
            {
                isTimeToAttack = false;
                preformLongRangeAttack = false;
            }

            if (entity.CheckIfPlayerInMaxAgroRange() 
                && entity.CheckIfPlayerInMaxAgroHeightRange() 
                && !entity.CheckIfPlayerInMinAgroRange())
            {
                isTimeToAttack = true;
                preformLongRangeAttack = true;
            }

            if (entity.CheckIfPlayerInMaxAgroRange()
                && !entity.CheckIfPlayerInMaxAgroHeightRange()
                && entity.CheckIfPlayerInMinAgroRange())
            {
                isTimeToAttack = true;
                preformLongRangeAttack = false;
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();        
    }
}