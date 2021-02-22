using System;
using System.Collections.Generic;

[Serializable]
public class Characteristics
{
    public int damage = 4;
    public int defense = 0;
    public int maxHealth = 10;
    public bool attacking = false;
    private int health = 10;
    public string name;

    public void SetHealth(int value)
    {
        health = value;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetDamage()
    {
        if (attacking)
            return damage;
        return 0;
    }

    public int GetDefense()
    {
        return defense;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
