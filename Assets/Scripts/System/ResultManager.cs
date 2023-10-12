using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class ResultManager : MonoBehaviour
{
    #region property
    public static ResultManager Instance { get; private set; }
    #endregion

    #region serialize
    [SerializeField]
    private int _carryAmountMultiValue = 1000;

    [SerializeField]
    private int _maxComboAmountMultiValue = 1000;

    [SerializeField]
    private ResultView _resultView = default;
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
        Instance = this;
    }

    private void Start()
    {

    }
    #endregion

    #region public method
    public void OnResultView()
    {
        OnResultAsync().Forget();
    }
    #endregion

    #region private method
    #endregion
    
    #region unitask method
    private async UniTaskVoid OnResultAsync()
    {
        await UniTask.Delay(1000);

        //タプルを使用して一度に複数の値を取得
        (var carryAmount, var maxComboAmount) = StageManager.Instance.GetCurrentResultAmount();

        _resultView.CarryAmountView(carryAmount);

        await UniTask.Delay(1000);

        _resultView.MaxComboAmountView(maxComboAmount);

        await UniTask.Delay(1000);

        _resultView.TotalScoreView((carryAmount * _carryAmountMultiValue) + (maxComboAmount * _maxComboAmountMultiValue));
    }
    #endregion
}
