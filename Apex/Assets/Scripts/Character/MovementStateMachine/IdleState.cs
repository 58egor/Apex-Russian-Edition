using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private MovementSM _movementSM;
    public IdleState(MovementSM stateMachine) : base("Idle", stateMachine)
    {
        _movementSM = stateMachine;
    }

    public override void UpdateLogic()
    {
        if (_movementSM.IsOnGround == false)
            _movementSM.SetInAirState();
        
        if (Mathf.Abs(_movementSM.X) > 0 || Mathf.Abs(_movementSM.Y) > 0)
            _movementSM.SetMovingState();

        if (_movementSM.IsCrounching)
            _movementSM.SetCrounchState();
    }

    public override void Enter()
    {
        _movementSM.OnJumpButtonPressed += Jump;
    }

    private void Jump()
    {
        _movementSM.SetJumpState();
    }

    public override void Exit()
    {
        _movementSM.OnJumpButtonPressed -= Jump;
    }

}
