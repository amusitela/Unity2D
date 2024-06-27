using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 30f; // �ӵ��ٶ�
    public int damage = 10; // �ӵ��˺�

    private Vector2 direction; // �ӵ����ƶ�����

    void Start()
    {
        // ���ӵ�����ʱ�Զ������ӵ��Ա����ڴ�й©
        Destroy(gameObject, 5f); // 5��������ӵ�
    }

    void Update()
    {
        // �ӵ�����ָ�������ƶ�
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
            Destroy(gameObject); // ��ײ�������ӵ�
        }
        else if (collision.gameObject.CompareTag("Wall")) // ���Ը�����Ҫ��Ӹ������ײ����
        {
            Destroy(gameObject); // ��ײ��ǽ�ں������ӵ�
        }
    }
}
