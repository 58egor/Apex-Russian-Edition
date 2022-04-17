using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : BaseState
{
    protected MovementSM _movementSM;
    protected Character _character => _movementSM.Character;
    protected virtual float _multiplier => 1f;
    protected virtual float _multiplierV => 1f;

    private float _speed;
    private float _maxSpeed;
    public MovingState(MovementSM stateMachine,float speed,float maxSpeed) : base("Moving", stateMachine)
    {
        _movementSM = stateMachine;
        _speed = speed;
        _maxSpeed = maxSpeed;
    }

    public override void Enter()
    {
        _movementSM.OnJumpButtonPressed += Jump;
    }

    protected void Jump()
    {
        _movementSM.SetJumpState();
    }

    public override void Exit()
    {
        _movementSM.OnJumpButtonPressed -= Jump;
    }

    public override void UpdateLogic()
    {
        CheckForGround();

       if (_character.Rigidbody.velocity == Vector3.zero)
            _movementSM.SetIdleState();

        if (_movementSM.IsCrounching)
            _movementSM.SetCrounchState();

        if (_movementSM.IsRunning)
            _movementSM.SetRunState();
    }

    protected void CheckForGround()
    {
        if (_movementSM.IsOnGround == false)
            _movementSM.SetInAirState();
    }

    public override void UpdatePhysics()
    {
        MoveCharacter();
    }

    protected void MoveCharacter()
    {
        Vector2 mag = FindVelRelativeToLook();

        CounterMovement(_movementSM.X, _movementSM.Y, mag);

        float x = CheckForMaxSpeed(_movementSM.X, mag.x);
        float y = CheckForMaxSpeed(_movementSM.Y, mag.y);

        _character.Rigidbody.AddForce(_character.transform.forward * y * _speed * Time.deltaTime * _multiplier * _multiplierV);
       _character.Rigidbody.AddForce(_character.transform.right * x * _speed * Time.deltaTime * _multiplier);
    }

    private Vector2 FindVelRelativeToLook()
    {
        float lookAngle = _character.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(_character.Rigidbody.velocity.x, _character.Rigidbody.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;
        float magnitue = _character.Rigidbody.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private float CheckForMaxSpeed(float direction, float mag)
    {
        if (direction > 0 && mag > _maxSpeed) return 0;
        if (direction < 0 && mag < -_maxSpeed) return 0;
        return direction;
    }

    protected virtual void CounterMovement(float x, float y, Vector2 mag)
    {
        if (Math.Abs(mag.x) > _movementSM.Threshold && Math.Abs(x) < 0.05f || (mag.x < -_movementSM.Threshold && x > 0) || (mag.x > _movementSM.Threshold && x < 0))
        {
            _character.Rigidbody.AddForce(_movementSM.CharacterMoveSpeed * _character.transform.right * Time.deltaTime * -mag.x * _movementSM.CounterMovement);
        }
        if (Math.Abs(mag.y) > _movementSM.Threshold && Math.Abs(y) < 0.05f || (mag.y < -_movementSM.Threshold && y > 0) || (mag.y > _movementSM.Threshold && y < 0))
        {
            _character.Rigidbody.AddForce(_movementSM.CharacterMoveSpeed * _character.transform.forward * Time.deltaTime * -mag.y * _movementSM.CounterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(_character.Rigidbody.velocity.x, 2) + Mathf.Pow(_character.Rigidbody.velocity.z, 2))) > _maxSpeed)
        {
            float fallspeed = _character.Rigidbody.velocity.y;
            Vector3 n = _character.Rigidbody.velocity.normalized * _maxSpeed;
            _character.Rigidbody.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

}
