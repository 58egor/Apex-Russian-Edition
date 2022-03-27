using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : BaseState
{
    private MovementSM _movementSM;
    private Character _character => _movementSM.Character;
    private float _multiplier = 1f;
    private float _multiplierV = 1f;
    public MovingState(MovementSM stateMachine) : base("Moving", stateMachine)
    {
        _movementSM = stateMachine;
    }

    public override void UpdateLogic()
    {
        if (_movementSM.X == 0 && _movementSM.Y == 0)
            _movementSM.SetIdleState();
    }

    public override void UpdatePhysics()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
       _character.Rigidbody.AddForce(_character.transform.forward * _movementSM.Y * _movementSM.CharacterMoveSpeed * Time.deltaTime * _multiplier * _multiplierV);
       _character.Rigidbody.AddForce(_character.transform.right * _movementSM.X * _movementSM.CharacterMoveSpeed * Time.deltaTime * _multiplier);
    }
}
