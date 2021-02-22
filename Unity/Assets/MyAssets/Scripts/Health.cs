using UnityEngine;

public class Health : MonoBehaviour
{
    public Characteristics values = new Characteristics();

    public string Name()
    {
        return values.name;
    }

    public int HealthAmount()
    {
        return values.GetHealth();
    }

    public int Damage()
    {
        return values.GetDamage();
    }

    public int Defense()
    {
        return values.GetDefense();
    }

    public void Substract(int damage)
    {
        values.SetHealth(values.GetHealth() - damage * (100 / (values.GetDefense() + 100)));
    }
}
