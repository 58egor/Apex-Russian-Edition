using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : MovingState
{
    protected override float _multiplier => 0.5f;
    protected override float _multiplierV => 0.5f;

    public InAirState(MovementSM stateMachine) : base(stateMachine)
    {
        Name = "InAir";
    }

    public override void UpdatePhysics()
    {
        MoveCharacter();
    }

    public override void UpdateLogic()
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }
}
