using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class SceneLoader : MonoBehaviour
{
    #region property
    public static SceneLoader Instance { get; private set; }
    #endregion

    #region serialize
    [SerializeField]
    private float _loadTime = 1.0f;
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
    #endregion

    #region public method
    public static void LoadScene(SceneType type)
    {
        Instance.StartCoroutine(Instance.LoadCoroutine(type));
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    private IEnumerator LoadCoroutine(SceneType type)
    {
        yield return new WaitForSeconds(_loadTime);

        SceneManager.LoadScene(type.ToString());
    }
    #endregion
}
