using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

public class PlayerModel : MonoBehaviour
{
    #region property
    public IObservable<int> CurrentCarrierAmountObserver => _currentCarrierAmountRP;
    public IObservable<Vector3> MoveDirectionObserver => _moveDirectionSubject;
    public IObservable<int> ResetCarryObserver => _resetCarrySubject;
    public int CurrentCarryAmount => _currentCarrierAmountRP.Value;
    #endregion

    #region serialize
    [SerializeField]
    private float _moveSpeed = 5.0f;

    [SerializeField]
    private float _rotateSpeed = 8.0f;

    [SerializeField]
    private GameObject _playerObject = default;
    #endregion

    #region private
    private PlayerInput _input;
    private Rigidbody _rb;
    private Animator _anim;

    private Vector2 _inputAxis;
    private Vector3 _inputMove;
    private float _currentMoveSpeed = 0;
    private PlayerAnimationType _currentType;
    #endregion

    #region Constant
    private const int MAX_CARRIER_AMOUNT = 5;
    #endregion

    #region Event
    private ReactiveProperty<int> _currentCarrierAmountRP = new ReactiveProperty<int>();
    private Subject<Vector3> _moveDirectionSubject = new Subject<Vector3>();
    private Subject<int> _resetCarrySubject = new Subject<int>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _anim = _playerObject.GetComponent<Animator>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                OnMove();
            });

        this.FixedUpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => _inputMove != Vector3.zero)
            .Subscribe(_ =>
            {
                _moveDirectionSubject.OnNext(_inputMove);
            });
    }

    private void OnEnable()
    {
        _input.actions["Move"].performed += OnMoveInput;
        _input.actions["Move"].canceled += OnMoveStop;
    }

    private void OnDisable()
    {
        _input.actions["Move"].performed -= OnMoveInput;
        _input.actions["Move"].canceled -= OnMoveStop;
    }
    #endregion

    #region public method
    public void AddTreasure()
    {
        _currentCarrierAmountRP.Value++;
        Debug.Log(_currentCarrierAmountRP.Value);
    }

    public bool IsCanCarry()
    {
        return _currentCarrierAmountRP.Value < MAX_CARRIER_AMOUNT;
    }

    public void OnResetCarry()
    {
        _resetCarrySubject.OnNext(0);
        _currentCarrierAmountRP.Value = 0;
    }
    #endregion

    #region private method
    private void OnMove()
    {
        if (_inputAxis == Vector2.zero)
        {
            _rb.velocity = Vector3.zero;
            PlayAnimation(PlayerAnimationType.Idle);
        }
        else
        {
            _inputMove = Vector3.forward * _inputAxis.y + Vector3.right * _inputAxis.x;
            _inputMove = Camera.main.transform.TransformDirection(_inputMove);
            _inputMove.y = 0;

            OnRotate();

            Vector3 velocity = _inputMove.normalized * _moveSpeed;
            velocity.y = _rb.velocity.y;
            _rb.velocity = velocity;

            if (_currentCarrierAmountRP.Value > 0)
            {
                PlayAnimation(PlayerAnimationType.Move_Carry);
            }
            else
            {
                PlayAnimation(PlayerAnimationType.Move_Default);
            }
        }
    }

    private void OnRotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_inputMove);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        var dir = context.ReadValue<Vector2>();
        _inputAxis = dir;
    }
    private void OnMoveStop(InputAction.CallbackContext context)
    {
        _inputAxis = Vector2.zero;
    }

    private void PlayAnimation(PlayerAnimationType type)
    {
        if (_currentType == type)
        {
            return;
        }

        _anim.CrossFadeInFixedTime(type.ToString(), 0.2f);
        _currentType = type;    
    }
    #endregion

    #region coroutine method
    #endregion
}
