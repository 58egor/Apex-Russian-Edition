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
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    public Vector3 NormalVector { get; private set; } = Vector3.up;
    public bool IsJumpAvailable { get; private set; } = true;
    public bool IsOnGround { get; set; } = true;

    [SerializeField] private float _maxSlopeAngle = 35f;

    private IdleState _idleState;
    private MovingState _movingState;
    private JumpState _jumpState;
    private InAirState _inAirState;
    private Coroutine _disableGroundCoroutine;

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
        _inAirState = new InAirState(this);
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

    public void SetInAirState() => ChangeState(_inAirState);

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
        StopAllCoroutines();
    }

    IEnumerator DisableJumpingForTime()
    {
        IsJumpAvailable = false;
        yield return new WaitForSeconds(JumpCooldown);
        IsOnGround = false;
        IsJumpAvailable = true;
    }

    public void JumpButtonIsProssed() => OnJumpButtonPressed?.Invoke();

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < _maxSlopeAngle;
    }

    private bool cancellingGrounded;

    private void OnCollisionStay(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (GroundLayer != (GroundLayer | (1 << layer))) 
            return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.contacts[i].normal;

            if (IsFloor(normal))
            {
                IsOnGround = true;
                cancellingGrounded = false;
                NormalVector = normal;

                if (_disableGroundCoroutine != null)
                {
                    StopCoroutine(_disableGroundCoroutine);
                    _disableGroundCoroutine = null;
                }
            }

            if (!cancellingGrounded)
            {
                cancellingGrounded = true;
                if(_disableGroundCoroutine == null)
                    _disableGroundCoroutine = StartCoroutine(StopGrounded(Time.deltaTime * 3f));
            }
        }
    }

    IEnumerator StopGrounded(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.LogError("kek");
        IsOnGround = false;
    }
}
