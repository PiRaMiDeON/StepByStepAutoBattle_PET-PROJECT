using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Create new enemy data")]
public class EnemyData : ScriptableObject
{
    public int HealthCount;
    public int ProtectionCount;
    public int SpawnRate;
    public int DamageCount;
    public int Cooldown;
}
