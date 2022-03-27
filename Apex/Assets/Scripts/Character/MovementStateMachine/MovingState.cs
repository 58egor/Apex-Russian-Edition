using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : BaseState
{
    private MovementSM _movementSM;
    public MovingState(MovementSM stateMachine) : base("Moving", stateMachine)
    {
        _movementSM = stateMachine;
    }
}
