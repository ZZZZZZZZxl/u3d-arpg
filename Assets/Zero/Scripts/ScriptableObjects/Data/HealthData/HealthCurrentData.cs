using UnityEngine;

[CreateAssetMenu(fileName = "HealthCurrentData", menuName = "Create/Health/HealthCurrentData", order = 0)]
public class HealthCurrentData : ScriptableObject
{
    [SerializeField] private HealthBaseData _healthBaseData;
    
    private float _currentHP;
    private float _maxHP;
    
    public float CurrentHP=> _currentHP;
    public float MaxHP => _maxHP;
    
    public void Initialize()
    {
        _maxHP = _healthBaseData.MaxHP;
        _currentHP = _maxHP;
    }
    
    public void TakeDamage(float damage)
    {
        _currentHP -= damage;
        if (_currentHP < 0)
        {
            _currentHP = 0;
        }

        if (_currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }
    }
}