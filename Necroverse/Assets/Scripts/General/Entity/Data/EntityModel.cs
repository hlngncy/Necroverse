using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player", fileName = "newPlayer", order = 0)]
public class EntityModel : ScriptableObject, IModel
{
    [SerializeField] private string _playerName;
    [SerializeField] private ushort _attackPower;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private short _maxHealth;
    [SerializeField] private short _movementSpeed;
    private int _currentHealth;

    //properties
    public int CurrentHealth => _currentHealth;
    public short MovementSpeed => _movementSpeed;
    public ushort AttackPower => _attackPower;
    public float AttackCooldown => _attackCooldown;
    public string PlayerName
    {
        get => _playerName;
        set => _playerName = value;
    }

    private EntityModel()
    {
        _currentHealth = _maxHealth;
    }
    
    public void OnHurt(HealtInfo healthInfo)
    {
        _currentHealth -= healthInfo.damage;
    }
}