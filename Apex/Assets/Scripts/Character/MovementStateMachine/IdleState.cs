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
}
