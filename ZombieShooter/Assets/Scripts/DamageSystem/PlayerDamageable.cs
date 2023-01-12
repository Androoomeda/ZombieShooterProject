using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] public int maxHealth;
    [HideInInspector] public int currentHealth { get; private set; }

    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private HealthUI healthUI;

    public UnityEvent Damaged, OnDeath;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public bool TryHeal(int healthCount)
    {
        if (currentHealth == maxHealth)
            return false;

        Heal(healthCount);
        return true;
    }

    private void Heal(int healthCount)
    {
        currentHealth += healthCount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        healthUI.ShowHearts(healthCount);
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.FadeHearts(damage);

        Instantiate(bloodEffect, transform.position, Quaternion.identity);

        Damaged.Invoke();

        if (currentHealth <= 0)
            Dead();
    }

    private void Dead()
    {
        OnDeath.Invoke();
    }
}
