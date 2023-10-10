using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

public class PlayerModel : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _moveSpeed = 5.0f;

    [SerializeField]
    private float _rotateSpeed = 8.0f;
    #endregion

    #region private
    private PlayerInput _input;
    private Animator _anim;
    private Rigidbody _rb;

    private Vector2 _inputAxis;
    private Vector3 _inputMove;
    private float _currentMoveSpeed = 0;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                OnRotate();
            });
        this.FixedUpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                OnMove();
            });

        this.OnCollisionEnterAsObservable()
            .Subscribe(x => Debug.Log(x.gameObject.name));
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
    #endregion

    #region private method
    private void OnMove()
    {
        if (_inputAxis == Vector2.zero)
        {
            _rb.velocity = Vector3.zero;
        }
        else
        {
            _inputMove = Vector3.forward * _inputAxis.y + Vector3.right * _inputAxis.x;
            _inputMove = Camera.main.transform.TransformDirection(_inputMove);

            Vector3 velocity = _inputMove.normalized * _moveSpeed;
            velocity.y = _rb.velocity.y;
            _rb.velocity = velocity;
        }
        Debug.Log(_rb.velocity);
    }

    private void OnRotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_inputMove);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed); ;

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
    #endregion

    #region coroutine method
    #endregion
}
