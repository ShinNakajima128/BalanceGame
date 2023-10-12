using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class ResultView : MonoBehaviour
{
    #region serialize
    [SerializeField]
    private TextMeshProUGUI _carryAmountTMP = default;

    [SerializeField]
    private TextMeshProUGUI _maxComboAmountTMP = default;

    [SerializeField]
    private TextMeshProUGUI _totalScoreTMP = default;
    #endregion

    #region public method
    public void CarryAmountView(int amount)
    {
        _carryAmountTMP.text = $"{amount}ÉR";
    }
    public void MaxComboAmountView(int amount)
    {
        _maxComboAmountTMP.text = $"{amount}";
    }
    public void TotalScoreView(int amount)
    {
        _totalScoreTMP.text = $"{amount}";
    }
    #endregion
}
