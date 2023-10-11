using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
    private Collider _collider;
    private Rigidbody _rb;

    private float _currentPushAmount;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();

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
                break;
            case 1:
                _treasureObjects[0].SetActive(true);
                break;
            case 2:
                _treasureObjects[1].SetActive(true);
                break;
            case 3:
                _treasureObjects[2].SetActive(true);
                break;
            case 4:
                _treasureObjects[3].SetActive(true);
                break;
            case 5:
                _treasureObjects[4].SetActive(true);
                break;
            default:
                break;
        }
    }
    public void AddForceCarrier(Vector3 dir)
    {
        //3Ç¬à»è„â^ÇÒÇ≈Ç¢ÇÈèÍçáÇÕåXÇ≠èàóùÇçsÇ§
        if (_treasureObjects[2].activeSelf)
        {
            _rb.AddForce(dir * _currentPushAmount, ForceMode.Force);

            Debug.Log(_rb.velocity.magnitude);
        }
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
