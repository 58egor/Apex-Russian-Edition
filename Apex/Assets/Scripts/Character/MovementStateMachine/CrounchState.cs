using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrounchState : MovingState
{
    public CrounchState(MovementSM stateMachine, float speed,float maxSpeed) : base(stateMachine, speed, maxSpeed)
    {
        Name = "Crounch";
    }

    public override void Enter()
    {
        _character.transform.localScale = _movementSM.crouchScale;
        _character.transform.position = new Vector3(_character.transform.position.x, _character.transform.position.y - 0.5f, _character.transform.position.z);
        _movementSM.OnJumpButtonPressed += Jump;
    }

    public override void Exit()
    {
        _character.transform.localScale = _movementSM._playerScale;
        _character.transform.position = new Vector3(_character.transform.position.x, _character.transform.position.y + 0.5f, _character.transform.position.z);
        _movementSM.OnJumpButtonPressed -= Jump;
    }

    public override void UpdatePhysics()
    {
        MoveCharacter();
    }

    public override void UpdateLogic()
    {
        CheckForGround();

        if (_movementSM.IsCrounching == false)
            _movementSM.SetIdleState();
    }
}
