using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class DropTreasure : PoolableBase
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    private Renderer _tureasureRenderer;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _tureasureRenderer = GetComponent<Renderer>();
    }
    private void OnEnable()
    {
        _tureasureRenderer.material.SetFloat("_Opacity", 1f);
        VanishAsync().Forget();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion

    #region unitask method
    private async UniTaskVoid VanishAsync()
    {
        await UniTask.Delay(1000);

        float ditherAmount = 1.0f;

        await DOTween.To(() =>
                     ditherAmount,
                     x => ditherAmount = x,
                     0f,
                     1.5f)
                     .OnUpdate(() =>
                     {
                         _tureasureRenderer.material.SetFloat("_Opacity", ditherAmount);
                     })
                     .AsyncWaitForCompletion();

        gameObject.SetActive(false);
    }
    #endregion
}
