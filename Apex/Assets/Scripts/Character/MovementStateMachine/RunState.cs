using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovingState
{
    public RunState(MovementSM stateMachine, float speed,float maxSpeed) : base(stateMachine, speed, maxSpeed)
    {
        Name = "Run";
    }

    public override void UpdateLogic()
    {
        CheckForGround();

        if (_character.Rigidbody.velocity == Vector3.zero)
            _movementSM.SetIdleState();

        if (_movementSM.IsCrounching)
            _movementSM.SetCrounchState();
    }
}
