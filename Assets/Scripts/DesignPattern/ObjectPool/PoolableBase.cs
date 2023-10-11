using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PoolableBase : MonoBehaviour, IPoolable
{

    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    protected virtual void OnDisable()
    {
        ReturnPool();
    }
    #endregion

    #region public method
    public void ReturnPool()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    #endregion
}
