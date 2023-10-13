using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TreasureGenerator : MonoBehaviour
{
    #region property
    public static TreasureGenerator Instance { get; private set; }
    #endregion

    #region serialize
    [SerializeField]
    private Treasure _treasurePrefab = default;

    [SerializeField]
    private Transform _treasureParent = default;

    [SerializeField]
    private DropTreasure _dropTreasurePrefab = default;

    [SerializeField]
    private Transform _dropTreasureParent = default;

    [SerializeField]
    private Transform[] _generatePoints = default;

    [SerializeField]
    private Transform _dropPoint = default;
    #endregion

    #region private
    private ObjectPool<Treasure> _treasurePool;
    private ObjectPool<DropTreasure> _dropTreasurePool;
    private bool[] _isSetArray;
    #endregion

    #region Constant
    private const int MAX_GENERATE_AMOUNT = 5;
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
        _dropTreasurePool = new ObjectPool<DropTreasure>(_dropTreasurePrefab, _dropTreasureParent);
    }

    private void Start()
    {
        Initialize();
        StageManager.Instance.GenerateTreasureObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => OnGenerateTreasure(value));

        StageManager.Instance.DropTreasureObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => OnGenerateTreasure(value));

        StageManager.Instance.DropTreasureObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => OnGenerateDropTreasure(value));
    }
    #endregion

    #region public method
    public void OnGenerateTreasure(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GenerateTreasure();
        }
    }

    public void OnGenerateDropTreasure(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GenerateDropTreasure();
        }
    }
    #endregion

    #region private method
    private void GenerateTreasure()
    {
        bool isSetCompleted = false;

        while (!isSetCompleted)
        {
            int randomIndex = UnityEngine.Random.Range(0, _generatePoints.Length);

            if (!_isSetArray[randomIndex])
            {
                var treasure = _treasurePool.Rent();
                treasure.transform.position = _generatePoints[randomIndex].position;

                treasure.GetTreasureObserver
                        .TakeUntilDisable(treasure)
                        .Subscribe(_ =>
                        {
                            int index = randomIndex;
                            _isSetArray[index] = false;
                            _currentGenerateAmountRP.Value--;
                        });

                int randomRotateX = UnityEngine.Random.Range(0, 360);
                int randomRotateY = UnityEngine.Random.Range(0, 360);
                int randomRotateZ = UnityEngine.Random.Range(0, 360);

                treasure.transform.eulerAngles = new Vector3(randomRotateX, randomRotateY, randomRotateZ);
                _isSetArray[randomIndex] = true;
                isSetCompleted = true;
                _currentGenerateAmountRP.Value++;
            }
        }
    }
    private void GenerateDropTreasure()
    {
        var drop = _dropTreasurePool.Rent();

        drop.transform.position = _dropPoint.position;
    }
    private void Initialize()
    {
        OnGenerateTreasure(MAX_GENERATE_AMOUNT);
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
