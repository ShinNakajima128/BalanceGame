using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Carrier : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _pushAmount = 5.0f;
    [SerializeField]
    private GameObject[] _treasureObjects = default;
    #endregion

    #region private
    private CapsuleCollider _collider;
    private Rigidbody _rb;

    private Vector3 _originPosition;
    private Vector3 _originRotation;
    private float _currentPushAmount;
    private int _currentTreasureAmount;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
        _originPosition = transform.localPosition;
        _originRotation = transform.localEulerAngles;

        _currentPushAmount = _pushAmount;
    }

    private void Start()
    {
        ChangeCarrierStatus(0);
    }
    #endregion

    #region public method
    public void ChangeCarrierStatus(int treasureAmount)
    {
        switch (treasureAmount)
        {
            case 0:
                foreach (var t in _treasureObjects)
                {
                    t.gameObject.SetActive(false);
                }
                transform.localPosition = _originPosition;
                transform.localEulerAngles = _originRotation;
                _rb.velocity = Vector3.zero;
                _rb.useGravity = false;
                break;
            case 1:
                _treasureObjects[0].SetActive(true);
                _collider.center = new Vector3(0f, 0.5f, 0f);
                _collider.height = 1f;
                _rb.useGravity = true;
                break;
            case 2:
                _treasureObjects[1].SetActive(true);
                _collider.center = new Vector3(0f, 0.75f, 0f);
                _collider.height = 1.5f;
                break;
            case 3:
                _treasureObjects[2].SetActive(true);
                _collider.center = new Vector3(0f, 1f, 0f);
                _collider.height = 2f;
                break;
            case 4:
                _treasureObjects[3].SetActive(true);
                _collider.center = new Vector3(0f, 1.35f, 0f);
                _collider.height = 2.75f;
                break;
            case 5:
                _treasureObjects[4].SetActive(true);
                _collider.center = new Vector3(0f, 1.75f, 0f);
                _collider.height = 3.5f;
                break;
            default:
                break;
        }
        _currentTreasureAmount = treasureAmount;
    }
    public void AddForceCarrier(Vector3 dir)
    {
        //3Ç¬à»è„â^ÇÒÇ≈Ç¢ÇÈèÍçáÇÕåXÇ≠èàóùÇçsÇ§
        if (_treasureObjects[2].activeSelf)
        {
            var rot = new Vector3(dir.x, 0, dir.z);
            _rb.angularVelocity = rot.normalized * _pushAmount;

            Debug.Log(dir);
        }
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
