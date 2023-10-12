using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// ゲームの進行状況、イベント処理を管理するManager
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region property
    public IObservable<bool> IsInGameObserver => _isInGameSubject;
    public IObservable<Unit> GameStartObserver => _gameStartSubject;
    public IObservable<bool> GamePauseObserver => _gamePauseSubject;
    public IObservable<Unit> UpdateComboObserver => _updateComboSubject;
    public Subject<Unit> ResetComboObserver => _resetComboSubject;
    public IObservable<Unit> PlayerDamageObserver => _playerDamageSubject;
    public IObservable<Unit> GameEndObserver => _gameEndSubject;
    public IObservable<Unit> GameResetObserver => _gameResetSubject;
    public IObservable<int> UpdateScoreObserver => _updateScoreSubject;

    protected override bool IsDontDestroyOnLoad => true;
    #endregion

    #region private 
    private bool _isGameStarted = false;
    #endregion

    #region Event
    /// <summary>インゲーム中かどうかを切り替えるSubject</summary>
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    /// <summary>ゲーム開始時のSubject</summary>
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    /// <summary>ゲーム中断時のSubject</summary>
    private Subject<bool> _gamePauseSubject = new Subject<bool>();
    /// <summary>コンボ数を更新するSubject</summary>
    private Subject<Unit> _updateComboSubject = new Subject<Unit>();
    /// <summary>コンボ数をリセットするSubject</summary>
    private Subject<Unit> _resetComboSubject = new Subject<Unit>();
    /// <summary>プレイヤー被弾時のSubject</summary>
    private Subject<Unit> _playerDamageSubject = new Subject<Unit>();
    /// <summary>ゲーム終了時のSubject</summary>
    private Subject<Unit> _gameEndSubject = new Subject<Unit>();
    /// <summary>ゲームの内容をリセットするSubject</summary>
    private Subject<Unit> _gameResetSubject = new Subject<Unit>();
    /// <summary>スコアを更新するSubject</summary>
    private Subject<int> _updateScoreSubject = new Subject<int>();
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        //画面を明転
        FadeManager.Fade(FadeType.In);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => !_isGameStarted)
            .Subscribe(_ =>
            {
                if (Input.anyKeyDown)
                {
                    OnGameStart();
                }
            });
    }
    #endregion

    #region public method
    /// <summary>
    /// ゲームの状態を更新する
    /// </summary>
    /// <param name="nextState"></param>

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void OnGameStart()
    {
        _gameStartSubject.OnNext(Unit.Default);
        _isInGameSubject.OnNext(true);
        _isGameStarted = true;
    }

    /// <summary>
    /// ゲームを再スタートする
    /// </summary>
    public void OnGameReStart()
    {

    }
    /// <summary>
    /// インゲーム中かどうかを切り替える
    /// </summary>
    /// <param name="value">ON/OFF</param>
    public void OnChangeIsInGame(bool value)
    {
        _isInGameSubject.OnNext(value);
    }
    /// <summary>
    /// ゲームを中断する
    /// </summary>
    /// /// <param name="value">ポーズするかどうか</param>
    public void OnGamePause(bool value)
    {
        _gamePauseSubject.OnNext(value);
        _isInGameSubject.OnNext(!value);
    }
    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void OnGameEnd()
    {
        _gameEndSubject.OnNext(Unit.Default);
        _isInGameSubject.OnNext(false);
    }

    /// <summary>
    /// ゲームをリセットする
    /// </summary>
    public void OnGameReset()
    {
        _gameResetSubject.OnNext(Unit.Default);
    }
    #endregion
}