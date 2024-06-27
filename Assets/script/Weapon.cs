using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public float nextFireTime;

    public abstract void Use();

    public virtual void UpgradeWeaponStats(string type)
    {
        if (type == "damage")
        {
            damage += 5;
        }
        else if (type == "fireRate")
        {
            fireRate -= 0.05f;
            if (fireRate < 0.1f) fireRate = 0.1f;
        }
    }
}
