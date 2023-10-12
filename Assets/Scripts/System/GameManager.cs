using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// �Q�[���̐i�s�󋵁A�C�x���g�������Ǘ�����Manager
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
    /// <summary>�C���Q�[�������ǂ�����؂�ւ���Subject</summary>
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    /// <summary>�Q�[���J�n����Subject</summary>
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    /// <summary>�Q�[�����f����Subject</summary>
    private Subject<bool> _gamePauseSubject = new Subject<bool>();
    /// <summary>�R���{�����X�V����Subject</summary>
    private Subject<Unit> _updateComboSubject = new Subject<Unit>();
    /// <summary>�R���{�������Z�b�g����Subject</summary>
    private Subject<Unit> _resetComboSubject = new Subject<Unit>();
    /// <summary>�v���C���[��e����Subject</summary>
    private Subject<Unit> _playerDamageSubject = new Subject<Unit>();
    /// <summary>�Q�[���I������Subject</summary>
    private Subject<Unit> _gameEndSubject = new Subject<Unit>();
    /// <summary>�Q�[���̓��e�����Z�b�g����Subject</summary>
    private Subject<Unit> _gameResetSubject = new Subject<Unit>();
    /// <summary>�X�R�A���X�V����Subject</summary>
    private Subject<int> _updateScoreSubject = new Subject<int>();
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        //��ʂ𖾓]
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
    /// �Q�[���̏�Ԃ��X�V����
    /// </summary>
    /// <param name="nextState"></param>

    /// <summary>
    /// �Q�[�����J�n����
    /// </summary>
    public void OnGameStart()
    {
        _gameStartSubject.OnNext(Unit.Default);
        _isInGameSubject.OnNext(true);
        _isGameStarted = true;
    }

    /// <summary>
    /// �Q�[�����ăX�^�[�g����
    /// </summary>
    public void OnGameReStart()
    {

    }
    /// <summary>
    /// �C���Q�[�������ǂ�����؂�ւ���
    /// </summary>
    /// <param name="value">ON/OFF</param>
    public void OnChangeIsInGame(bool value)
    {
        _isInGameSubject.OnNext(value);
    }
    /// <summary>
    /// �Q�[���𒆒f����
    /// </summary>
    /// /// <param name="value">�|�[�Y���邩�ǂ���</param>
    public void OnGamePause(bool value)
    {
        _gamePauseSubject.OnNext(value);
        _isInGameSubject.OnNext(!value);
    }
    /// <summary>
    /// �Q�[�����I������
    /// </summary>
    public void OnGameEnd()
    {
        _gameEndSubject.OnNext(Unit.Default);
        _isInGameSubject.OnNext(false);
    }

    /// <summary>
    /// �Q�[�������Z�b�g����
    /// </summary>
    public void OnGameReset()
    {
        _gameResetSubject.OnNext(Unit.Default);
    }
    #endregion
}