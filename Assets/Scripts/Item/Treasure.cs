using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Treasure : PoolableBase
{
    #region property
    public IObservable<Unit> GetTreasureObserver => _getTreasureSubject;
    #endregion

    #region serialize
    #endregion

    #region Event
    private Subject<Unit> _getTreasureSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Start()
    {
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                var player = x.gameObject.GetComponent<PlayerModel>();

                if (player != null && player.IsCanCarry())
                {
                    Use(player);
                    _getTreasureSubject.OnNext(Unit.Default);
                    gameObject.SetActive(false);
                }
            });
    }
    #endregion

    #region private method
    private void Use(PlayerModel player)
    {
        player.AddTreasure();
    }
    #endregion
}
