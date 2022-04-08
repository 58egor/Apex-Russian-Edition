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
        if (_movementSM.IsJumpAvailable && _movementSM.IsJumpButtonPressed)
            _movementSM.SetJumpState();

        if (Mathf.Abs(_movementSM.X) > 0 || Mathf.Abs(_movementSM.Y) > 0)
            _movementSM.SetMovingState();
    }

}
