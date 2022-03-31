using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSM : StateMachine
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public Character Character { get; private set; }
    [field: SerializeField] public float CharacterMoveSpeed { get; private set; }
    [field: SerializeField] public float MaxCharacterMoveSpeed { get; private set; }

    private IdleState _idleState;
    private MovingState _movingState;

    public void Init(Character character)
    {
        Character = character;
    }

    public void SetXY(float x,float y)
    {
        X = x;
        Y = y;
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

    public void SetMovingState() => ChangeState(_movingState);

    public void SetIdleState() => ChangeState(_idleState);

    private void FixedUpdate()
    {
        AddExtraGravity();
    }

    private void AddExtraGravity()
    {
        Character.Rigidbody.AddForce(Vector3.down * Time.deltaTime * 10);
    }
}
