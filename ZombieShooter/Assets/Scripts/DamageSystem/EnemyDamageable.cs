using UnityEngine;
using UnityEngine.Events;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] public int maxHealth;
    [HideInInspector] public int currentHealth { get; private set; }
    
    [SerializeField] private GameObject bloodEffect;

    public UnityEvent Damaged, OnDeath;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        Instantiate(bloodEffect, transform.position, Quaternion.identity);

        Damaged.Invoke();

        if (currentHealth <= 0)
            Dead();
    }

    private void Dead()
    {
        OnDeath.Invoke();
        Destroy(this.gameObject);
    }
}
