using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    #region property
    public static StageManager Instance { get; private set; }
    public IObservable<int> CarryCompleteObserver => _carryCompleteSubject;
    public IObservable<int> GenerateTreasureObserver => _generateTreasureSubject;
    public IObservable<int> CurrentLimitTimeObserver => _currentLimitTimeRP;
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
    #endregion

    #region private
    private CanvasGroup _currentGroup;

    private int _currentCarryAmount = 0;
    private int _currentComboAmount = 0;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<int> _carryCompleteSubject = new Subject<int>();
    private Subject<int> _generateTreasureSubject = new Subject<int>();

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

        _currentLimitTimeRP.TakeUntilDestroy(this)
                           .Subscribe(value => _stageView.LimitTimeView(value));
    }
    #endregion

    #region public method
    public void OnCarryComplete(int carryAmount)
    {
        _carryCompleteSubject.OnNext(carryAmount);
        _generateTreasureSubject.OnNext(carryAmount);
    }
    #endregion

    #region private method
    #endregion

    #region unitask method
    private async UniTaskVoid CountLimitTimeAsync()
    {
        for (int i = 0; i < 3; i++)
        {
            await UniTask.Delay(1000);
            Debug.Log(3 - i);
        }

        await UniTask.Delay(1000);

        Debug.Log("Start!");

        ChangeViewGroup(GameState.InGame);

        for (int i = 0; i < _limitTime; i++)
        {
            await UniTask.Delay(1000);
            _currentLimitTimeRP.Value--;
        }

        Debug.Log("ゲーム終了");

        await UniTask.Delay(1000);

        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            CameraManager.Instance.ChangeCamera(CameraType.Result);
            ChangeViewGroup(GameState.Result);
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
