using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float swingDuration = 0.3f;
    public float swingAngle = 45f;
    private bool isSwinging = false;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    public override void Use()
    {
        if (!isSwinging)
        {
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing()
    {
        isSwinging = true;
        float elapsedTime = 0f;
        float halfDuration = swingDuration / 2f;
        Quaternion startRotation = Quaternion.Euler(0, 0, swingAngle);
        Quaternion endRotation = Quaternion.Euler(0, 0, -swingAngle);

        AudioManager.instance.PlaySound(AudioManager.instance.swordClip);

        // 旋转到最大角度
        while (elapsedTime < halfDuration)
        {
            transform.localRotation = Quaternion.Lerp(initialRotation, startRotation, (elapsedTime / halfDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        // 旋转回初始位置
        while (elapsedTime < halfDuration)
        {
            transform.localRotation = Quaternion.Lerp(startRotation, initialRotation, (elapsedTime / halfDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSwinging = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            Enemy2 enemy2 = collision.GetComponent<Enemy2>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            if (enemy2 != null)
            {
                enemy2.TakeDamage(damage);
            }
        }
        else if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
