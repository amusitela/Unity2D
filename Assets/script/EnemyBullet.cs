using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 30f; // 子弹速度
    public int damage = 10; // 子弹伤害

    private Vector2 direction; // 子弹的移动方向

    void Start()
    {
        // 在子弹创建时自动销毁子弹以避免内存泄漏
        Destroy(gameObject, 5f); // 5秒后销毁子弹
    }

    void Update()
    {
        // 子弹沿着指定方向移动
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Knight"))
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // 碰撞后销毁子弹
        }
        else if (collision.gameObject.CompareTag("Wall")) // 可以根据需要添加更多的碰撞处理
        {
            Destroy(gameObject); // 碰撞到墙壁后销毁子弹
        }
    }
}
