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

    private bool _isCheckGroundAvailable = true;

    private const float _additionalValue = 3f;

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
        StartCoroutine(DisableJumpingForTime(JumpCooldown));

        //TODO костыль, придумать как лучше после прыжка оставаться в состоянии в воздухе
        StartCoroutine(DisableCheckGround((Time.deltaTime * _additionalValue)));
    }

    public void SetInAirState()
    {
        ChangeState(_inAirState);
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
        StopAllCoroutines();
    }

    IEnumerator DisableJumpingForTime(float time)
    {
        IsJumpAvailable = false;
        yield return new WaitForSeconds(time);
        IsOnGround = false;
        IsJumpAvailable = true;
    }

    IEnumerator DisableCheckGround(float time)
    {
        _isCheckGroundAvailable = false;
        if (_disableGroundCoroutine == null)
            _disableGroundCoroutine = StartCoroutine(StopGrounded(time));

        yield return new WaitForSeconds(time);
        _isCheckGroundAvailable = true;
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
        if (GroundLayer != (GroundLayer | (1 << layer)) || _isCheckGroundAvailable == false) 
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
                    _disableGroundCoroutine = StartCoroutine(StopGrounded(Time.deltaTime * _additionalValue));
            }
        }
    }

    IEnumerator StopGrounded(float time)
    {
        yield return new WaitForSeconds(time);
        IsOnGround = false;
    }
}
