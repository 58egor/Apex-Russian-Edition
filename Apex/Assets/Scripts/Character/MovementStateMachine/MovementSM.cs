using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSM : StateMachine
{
    private IdleState _idleState;
    private MovingState _movingState;
    private Character _character;

    public void Init(Character character)
    {
        _character = character;
    }

    private void Awake()
    {
        _idleState = new IdleState(this);
        _movingState = new MovingState(this);
    }

    protected override BaseState GetInitialState()
    {
       return _idleState;
    }
}
