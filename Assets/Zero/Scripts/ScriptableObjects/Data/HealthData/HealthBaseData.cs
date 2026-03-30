using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "Create/Health/HealthBaseData", order = 0)]
public class HealthBaseData : ScriptableObject
{
    [SerializeField] private float _maxHP;
    
    public float MaxHP => _maxHP;
}