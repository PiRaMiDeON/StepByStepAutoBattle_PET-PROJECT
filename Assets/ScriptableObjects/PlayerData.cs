using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create new player data")]
public class PlayerData : ScriptableObject
{
    [Header("Player parametres")]
    public int HealthCount;
    public int ProtectionCount;
    public int MeleeDamage;
    public int RangeDamage;
    public int Cooldown;
}

public enum AttackType
{
    Melee = 1,
    Range = 2
}

