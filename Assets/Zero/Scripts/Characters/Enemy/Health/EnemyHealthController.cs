using System;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private HealthCurrentData healthTemplate;
    private HealthCurrentData _healthCurrentData;
    private bool _isInitialized;

    public HealthCurrentData HealthCurrentData => _healthCurrentData;
    public bool IsDead => _healthCurrentData != null && _healthCurrentData.CurrentHP <= 0f;
    

    #region Unity Methods

    private void OnEnable()
    {
        CreateRuntimeHealthData();
        GameEventManager.MainInstance.AddEvent<float,Transform>(EventNames.TakeDamager, TakeDamage);
    }

    private void OnDisable()
    {
        _isInitialized = false;
        GameEventManager.MainInstance.RemoveEvent<float,Transform>(EventNames.TakeDamager, TakeDamage);
    }

    private void Start()
    {
        InitializeHealth();
    }

    private void Update()
    {
        // DebugTakeDamage();
    }

    #endregion
    
    

    #region Main Methods

    public void InitializeHealth()
    {
        if (_healthCurrentData == null)
            CreateRuntimeHealthData();

        if (_healthCurrentData == null)
            return;

        _healthCurrentData.Initialize();
        _isInitialized = true;
    }

    public void ApplyDamage(float damage)
    {
        if (!_isInitialized)
            InitializeHealth();

        if (_healthCurrentData == null)
            return;

        _healthCurrentData.TakeDamage(damage);
    }

    private void TakeDamage(float damage, Transform target)
    {
        if (target == transform)
            ApplyDamage(damage);
    }

    private void CreateRuntimeHealthData()
    {
        if (healthTemplate == null)
        {
            Debug.LogWarning($"EnemyHealthController on {name} is missing healthTemplate.", this);
            _healthCurrentData = null;
            return;
        }

        _healthCurrentData = Instantiate(healthTemplate);
    }

    #endregion
    
    

    #region Debug

    // public float debugDamage;
    // public float currentHp;
    // public float maxHp;
    //
    // public void DebugTakeDamage()
    // {
    //     currentHp = _healthCurrentData.CurrentHP;
    //     maxHp = _healthCurrentData.MaxHP;
    //     TakeDamage(debugDamage, transform);
    // }

    #endregion
}
