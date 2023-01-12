using UnityEngine;

public interface IDamageable
{
    [HideInInspector] public int currentHealth { get; }

    void ApplyDamage(int damage);
}
