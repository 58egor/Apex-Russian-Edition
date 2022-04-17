using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InAirState : MovingState
{
    protected override float _multiplier => 1f;
    protected override float _multiplierV => 1f;

    public InAirState(MovementSM stateMachine,float speed,float maxSpeed) : base(stateMachine,speed, maxSpeed)
    {
        Name = "InAir";
    }

    public override void UpdatePhysics()
    {
       // MoveCharacter();
    }

    public override void UpdateLogic()
    {
        if (_movementSM.IsOnGround == true)
            _movementSM.SetIdleState();
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        DOTween.Kill(this);
    }
}
