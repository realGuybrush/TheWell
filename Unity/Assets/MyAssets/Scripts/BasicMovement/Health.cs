using System;
using UnityEngine;

[Serializable]
public class Health
{
    [SerializeField]
    private float _damage = 4;

    [SerializeField]
    private float _defense = 0;

    [SerializeField]
    private float _maxHealth = 10;

    [SerializeField]
    private float _health = 10;

    public void LoadValues(float damage, float defense, float health) {
        _damage = damage;
        _defense = defense;
        _health = health;
    }

    public float GetDamage(float damage) {
        float totalDamageReceived = damage * (100 / (_defense + 100));
        if (totalDamageReceived > _health)
            totalDamageReceived = _health;
        _health -= totalDamageReceived;
        return totalDamageReceived;
    }

    public float damage => _damage;

    public float defense => _defense;

    public float maxHealth => _maxHealth;

    public float health => _health;

}
