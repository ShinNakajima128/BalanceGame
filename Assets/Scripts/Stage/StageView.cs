using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class StageView : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private TextMeshProUGUI _limitTimeTMP = default;

    [SerializeField]
    private TextMeshProUGUI _currentCarryAmountTMP = default;

    [SerializeField]
    private TextMeshProUGUI _comboAmountTMP = default;
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

    }
    #endregion

    #region public method
    public void LimitTimeView(int value)
    {
        _limitTimeTMP.text = value.ToString();
    }
    public void CarryAmountView(int amount)
    {
        _currentCarryAmountTMP.text = $"{amount}コ";
    }

    public void ComboAmountView(int amount)
    {
        if (amount <= 0)
        {
            _comboAmountTMP.text = "";
        }
        else
        {
            _comboAmountTMP.text = $"{amount}コンボ！";
        }
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
