using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create/Data/PlayerData", order = 0)]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private PlayerGroundedData _groundedData;
    [SerializeField] private CombatData _combatData;
    [SerializeField] private PlayerRotationData _playerRotationData;
    
    public PlayerRotationData PlayerRotationData => _playerRotationData;
    public PlayerGroundedData GroundedData => _groundedData;
    public CombatData CombatData => _combatData;
}