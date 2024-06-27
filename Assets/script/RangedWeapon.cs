using UnityEngine;

public class RangedWeapon : Weapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    public override void Use()
    {
        // �����ӵ��߼�
        Debug.Log("ʹ��Զ������");
        Shoot();
    }

    private void Shoot()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.shootClip);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed;
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.damage = damage;
        }
    }
}
