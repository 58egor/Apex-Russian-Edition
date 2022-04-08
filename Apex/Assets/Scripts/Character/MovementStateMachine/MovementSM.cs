using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class MovementSM : StateMachine
{
    public float X { get; private set; }
    public float Y { get; private set; }

    public event Action OnJumpButtonPressed;
    public bool IsJumpButtonPressed { get; set; } = false;
    public Character Character { get; private set; }
    [field: SerializeField] public float CharacterMoveSpeed { get; private set; }
    [field: SerializeField] public float MaxCharacterMoveSpeed { get; private set; }
    [field: SerializeField] public float CounterMovement { get;private set; }
    [field: SerializeField] public float Threshold { get; private set; }
    [field: SerializeField] public float JumpCooldown { get; private set; } = 0.25f;
    [field: SerializeField] public float JumpForce { get; private set; } = 550f;
    public Vector3 NormalVector { get; private set; } = Vector3.up;
    public bool IsJumpAvailable { get; private set; } = true;

    private IdleState _idleState;
    private MovingState _movingState;
    private JumpState _jumpState;

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
        _jumpState = new JumpState(this);
    }

    protected override BaseState GetInitialState()
    {
       return _idleState;
    }

    public void SetMovingState() => ChangeState(_movingState);

    public void SetIdleState() => ChangeState(_idleState);

    public void SetJumpState()
    {
        ChangeState(_jumpState);
        StartCoroutine(DisableJumpingForTime());
    }

    private void FixedUpdate()
    {
        AddExtraGravity();
    }

    private void AddExtraGravity()
    {
        Character.Rigidbody.AddForce(Vector3.down * Time.deltaTime * 10);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }

    IEnumerator DisableJumpingForTime()
    {
        IsJumpAvailable = false;
        yield return new WaitForSeconds(JumpCooldown);
        IsJumpAvailable = true;
    }

    public void JumpButtonIsProssed() => OnJumpButtonPressed?.Invoke();
}
