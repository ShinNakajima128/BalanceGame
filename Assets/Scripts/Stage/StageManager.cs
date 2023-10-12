using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

public class StageManager : MonoBehaviour
{
    #region property
    public static StageManager Instance { get; private set; }
    public IObservable<int> CarryCompleteObserver => _carryCompleteSubject;
    public IObservable<int> GenerateTreasureObserver => _generateTreasureSubject;
    public IObservable<Unit> DropTreasureObserver => _dropTreasureSubject;
    public IObservable<int> CurrentLimitTimeObserver => _currentLimitTimeRP;
    public IObservable<int> CurrentCarryAmountObsercer => _currentCarryAmountRP;
    public IObservable<int> ComboAmountObserver => _currentComboAmountRP;
    #endregion

    #region serialize
    [SerializeField]
    private int _limitTime = 60;

    [SerializeField]
    private StageView _stageView = default;

    [Tooltip("インゲーム画面のCanvasGroup")]
    [SerializeField]
    private CanvasGroup _inGameGroup = default;

    [Tooltip("リザルト画面のCanvasGroup")]
    [SerializeField]
    private CanvasGroup _resultGroup = default;

    [SerializeField]
    private TextMeshProUGUI _countDownTMP = default;
    #endregion

    #region private
    private CanvasGroup _currentGroup;

    private ReactiveProperty<int> _currentCarryAmountRP = new ReactiveProperty<int>(0);
    private ReactiveProperty<int> _currentComboAmountRP = new ReactiveProperty<int>(0);
    private int _maxComboAmount = 0;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<int> _carryCompleteSubject = new Subject<int>();
    private Subject<int> _generateTreasureSubject = new Subject<int>();
    private Subject<Unit> _dropTreasureSubject = new Subject<Unit>();

    private ReactiveProperty<int> _currentLimitTimeRP = new ReactiveProperty<int>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _currentLimitTimeRP.Value = _limitTime;
    }

    private void Start()
    {
        FadeManager.Fade(FadeType.In);
        CountLimitTimeAsync().Forget();

        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => ResetAmount());

        _currentLimitTimeRP.TakeUntilDestroy(this)
                           .Subscribe(value => _stageView.LimitTimeView(value));

        _currentCarryAmountRP.TakeUntilDestroy(this)
                             .Subscribe(amount => _stageView.CarryAmountView(amount));

        _currentComboAmountRP.TakeUntilDestroy(this)
                             .Subscribe(amount => _stageView.ComboAmountView(amount));
    }
    #endregion

    #region public method
    public void OnCarryComplete(int carryAmount)
    {
        _currentCarryAmountRP.Value += carryAmount;
        _currentComboAmountRP.Value++;

        if (_maxComboAmount < _currentComboAmountRP.Value)
        {
            _maxComboAmount = _currentCarryAmountRP.Value;
        }

        _carryCompleteSubject.OnNext(carryAmount);
        _generateTreasureSubject.OnNext(carryAmount);
    }

    public void ResetCombo()
    {
        if (_maxComboAmount < _currentComboAmountRP.Value)
        {
            _maxComboAmount = _currentCarryAmountRP.Value;
        }
        _currentComboAmountRP.Value = 0;
    }
    public void OnDropTreasure()
    {
        _dropTreasureSubject.OnNext(Unit.Default);
        ResetCombo();
    }

    public (int, int) GetCurrentResultAmount()
    {
        return (_currentCarryAmountRP.Value, _maxComboAmount);
    }
    #endregion

    #region private method
    private void ResetAmount()
    {
        _currentCarryAmountRP.Value = 0;
        _currentComboAmountRP.Value = 0;
    }
    #endregion

    #region unitask method
    private async UniTaskVoid CountLimitTimeAsync()
    {
        for (int i = 0; i < 3; i++)
        {
            await UniTask.Delay(1000);
            _countDownTMP.text = $"{3 - i}";
        }

        await UniTask.Delay(1000);

        _countDownTMP.text = "Start!!";

        await UniTask.Delay(1000);

        _countDownTMP.text = "";
        GameManager.Instance.OnGameStart();
        ChangeViewGroup(GameState.InGame);

        for (int i = 0; i < _limitTime; i++)
        {
            await UniTask.Delay(1000);
            _currentLimitTimeRP.Value--;
        }

        Debug.Log("ゲーム終了");
        GameManager.Instance.OnGameEnd();

        await UniTask.Delay(1000);

        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            CameraManager.Instance.ChangeCamera(CameraType.Result, 0f);
            ChangeViewGroup(GameState.Result);
            ResultManager.Instance.OnResultView();
        });
    }
    #endregion
    /// <summary>
    /// ゲーム画面を切り替える
    /// </summary>
    /// <param name="state">ゲームの状態</param>
    public void ChangeViewGroup(GameState state)
    {
        if (_currentGroup != null)
        {
            _currentGroup.alpha = 0;
            _currentGroup.blocksRaycasts = false;
            _currentGroup.interactable = false;
        }

        switch (state)
        {
            case GameState.InGame:
                _currentGroup = _inGameGroup;
                break;
            case GameState.Result:
                _currentGroup = _resultGroup;
                break;
            default:
                break;
        }

        _currentGroup.alpha = 1;
        _currentGroup.blocksRaycasts = true;
        _currentGroup.interactable = true;
    }

    public void FadeoutGroup()
    {
        ViewGroupAsync(_currentGroup, 1).Forget();
    }

    #region unitask method
    private async UniTaskVoid ViewGroupAsync(CanvasGroup group, float fadeTime = 0)
    {
        await DOTween.To(() => group.alpha,
                         x => group.alpha = x,
                         1f,
                         fadeTime).AsyncWaitForCompletion();
    }
    #endregion
}
