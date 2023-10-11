using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TreasureGenerator : MonoBehaviour
{
    #region property
    public static TreasureGenerator Instance { get; private set; }
    public int MaxGenerateNum => _generatePoints.Length;
    #endregion

    #region serialize
    [SerializeField]
    private Treasure _treasurePrefab = default;

    [SerializeField]
    private Transform _treasureParent = default;

    [SerializeField]
    private Transform[] _generatePoints = default;
    #endregion

    #region private
    private ObjectPool<Treasure> _treasurePool;
    private bool[] _isSetArray;
    #endregion

    #region Constant
    #endregion

    #region Event
    private ReactiveProperty<int> _currentGenerateAmountRP = new ReactiveProperty<int>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _isSetArray = new bool[_generatePoints.Length];

        for (int i = 0; i < _isSetArray.Length; i++)
        {
            _isSetArray[i] = false;
        }
        _treasurePool = new ObjectPool<Treasure>(_treasurePrefab, _treasureParent);
    }

    private void Start()
    {
        Initialize();
    }
    #endregion

    #region public method
    public void OnGenerate()
    {
        if (_currentGenerateAmountRP.Value >= MaxGenerateNum)
        {
            return;
        }

        bool isSetCompleted = false;

        while (!isSetCompleted)
        {
            int randomIndex = UnityEngine.Random.Range(0, MaxGenerateNum);

            if (!_isSetArray[randomIndex])
            {
                var treasure = _treasurePool.Rent();
                treasure.transform.position = _generatePoints[randomIndex].position;

                int randomRotateX = UnityEngine.Random.Range(0, 360);
                int randomRotateY = UnityEngine.Random.Range(0, 360);
                int randomRotateZ = UnityEngine.Random.Range(0, 360);

                treasure.transform.eulerAngles = new Vector3(randomRotateX, randomRotateY, randomRotateZ);
                _isSetArray[randomIndex] = true;
                isSetCompleted = true;
            }
        }
    }
    #endregion

    #region private method
    private void Initialize()
    {
        for (int i = 0; i < MaxGenerateNum; i++)
        {
            OnGenerate();
        }
    }
    #endregion

    #region coroutine method
    #endregion
}

[Serializable]
public class GeneratePoint
{
    public Transform Point;
    public bool IsSet = false;
}
