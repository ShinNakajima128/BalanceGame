using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerPresenter : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private PlayerModel _model = default;

    [SerializeField]
    private PlayerView _view = default;

    [SerializeField]
    private Carrier _carrier = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {
        //‰^‚ñ‚Å‚¢‚é•ó” ‚Ì”‚ª•Ï‰»‚µ‚½Û‚Ìˆ—‚ð“o˜^
        _model.CurrentCarrierAmountObserver
              .TakeUntilDestroy(this)
              .Subscribe(value =>
              {
                  _carrier.ChangeCarrierStatus(value);
              });
        _model.MoveDirectionObserver
              .TakeUntilDestroy(this)
              .Subscribe(value => _carrier.AddForceCarrier(value));

        _model.ResetCarryObserver
              .TakeUntilDestroy(this)
              .Subscribe(value => _carrier.ChangeCarrierStatus(value));

        GameManager.Instance.IsInGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => _model.IsCanMove = value);

        StageManager.Instance.DropTreasureObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => _model.OnResetCarry());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
