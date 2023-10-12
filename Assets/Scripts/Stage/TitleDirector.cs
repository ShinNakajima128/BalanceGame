using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class TitleDirector : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _moveSpeed = 5.0f;
    
    [SerializeField]
    private Transform _shipTrans = default;

    [SerializeField]
    private Transform _shipTargetPoint = default;

    [SerializeField]
    private Transform _waveTrans = default;

    [SerializeField]
    private Transform _waveTargetPoint = default;

    [SerializeField]
    private Transform _explotionPoint = default;

    [SerializeField]
    private CanvasGroup _titleGroup = default;
    #endregion

    #region private
    private Tween _shipTween;
    private Tween _waveTween;
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
        _titleGroup.alpha = 1;
        OnWaveMove();

        GameManager.Instance.GameStartObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => OnShipMove());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void OnWaveMove()
    {
        _waveTween = _waveTrans.DOMove(_waveTargetPoint.position, _moveSpeed)
                               .SetEase(Ease.Linear)
                               .SetLoops(-1);
    }

    private void OnShipMove()
    {
        ShipMoveAsync().Forget();
    }
    #endregion
    
    #region coroutine method

    private async UniTaskVoid ShipMoveAsync()
    {
        _waveTween.Kill();
        _waveTween = null;
        LetterboxController.ActivateLetterbox(true, 1.0f);
        _titleGroup.alpha = 0;

        await _shipTrans.DOMove(_shipTargetPoint.position, _moveSpeed)
                               .SetEase(Ease.Linear)
                               .AsyncWaitForCompletion();


        Collider[] colliders = Physics.OverlapSphere(_explotionPoint.position, 10);

        foreach (var c in colliders)
        {
            var rb = c.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = true;
                rb.AddExplosionForce(2000, _explotionPoint.position, 10.0f);
            }
        }

        await _shipTrans.DOShakePosition(1.0f).AsyncWaitForCompletion();

        FadeManager.Fade(FadeType.Out, () => 
        {
            SceneLoader.LoadScene(SceneType.InGame);
        });
    }
    #endregion
}
