using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage;

    private Vector2 direction;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //AudioManager.instance.PlaySound(AudioManager.instance.explodeClip);
            Enemy enemy = collision.GetComponent<Enemy>();
            Enemy2 enemy2 = collision.GetComponent<Enemy2>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            };
            if (enemy2 != null)
            {
                enemy2.TakeDamage(damage);
            };
            Destroy(gameObject);  // 确保子弹在击中敌人后被销毁
        }
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);  // 确保子弹在击中其他物体后被销毁
        }
    }
}

