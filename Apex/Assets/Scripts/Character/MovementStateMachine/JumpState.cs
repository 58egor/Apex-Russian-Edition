using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    private MovementSM _movementSM;
    private Character _character => _movementSM.Character;
    public JumpState(MovementSM stateMachine) : base("Jump", stateMachine)
    {
        _movementSM = stateMachine;
    }

    public override void Enter()
    {
        _character.Rigidbody.AddForce(Vector2.up * _movementSM.JumpForce * 1.5f);
        _character.Rigidbody.AddForce(_movementSM.NormalVector * _movementSM.JumpForce * 0.5f);

        //If jumping while falling, reset y velocity.
        Vector3 vel = _character.Rigidbody.velocity;
        if (_character.Rigidbody.velocity.y < 0.5f)
            _character.Rigidbody.velocity = new Vector3(vel.x, 0, vel.z);
        else if (_character.Rigidbody.velocity.y > 0)
            _character.Rigidbody.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

        _movementSM.SetInAirState();
    }
}
