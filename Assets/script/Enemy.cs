using System.Collections;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public int health = 50; // ����ֵ
    public int experienceValue = 10; // ��ɱ���˺���һ�õľ���ֵ
    public int damage = 10; // �������ɵ��˺�
    public float speed = 2f; // �ƶ��ٶ�

    public float wanderRadius = 1f; // ����ƶ��İ뾶
    public float wanderInterval = 0.5f; // ����ƶ���ʱ����
    private Vector2 wanderOffset;
    private float wanderTimer;

    private Transform knight; // ���λ��

    private bool facingRight = true; // ���ڸ��ٵ��˵ĵ�ǰ����

    private int baseDamage = 10;
    private int baseHealth = 50;

    private Animator animator;

    private Vector2 previousPosition;

    private SpriteRenderer spriteRenderer;
    public float damageBlinkDuration = 0.1f; // ������˸����ʱ��
    public Color blinkColor = Color.red; // ����ʱ����ɫ
    private Color originalColor; // ԭʼ��ɫ

    public static GameObject damageTextPrefab; // �˺��ı�Ԥ�Ƽ�
    public static Transform canvasTransform;   // Canvas��Transform
    private DamageTextManager damageTextManager; // �����˺��ı��Ĺ�����

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("δ�ҵ� Animator �����");
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        knight = GameObject.FindGameObjectWithTag("Knight").transform;
        if (knight == null)
        {
            Debug.LogError("δ�ҵ����� 'Knight' ��ǩ�Ķ���");
            return;
        }
        wanderOffset = Vector2.zero;
        wanderTimer = wanderInterval;
        previousPosition = transform.position;

        // ȷ����ʼ����������ȷ
        animator.SetBool("isDie", false);
        animator.SetBool("isIdle", false);

        // ��ȡ DamageTextManager
        if (canvasTransform != null)
        {
            damageTextManager = canvasTransform.GetComponent<DamageTextManager>();
        }
    }

    void Update()
    {
        if (health > 0)
        {
            Wander();
            MoveTowardsPlayer();

            // ����Ƿ����ƶ�
            Vector2 currentPosition = transform.position;
            bool isIdle = currentPosition == previousPosition;
            animator.SetBool("isIdle", isIdle);
            previousPosition = currentPosition;
        }
    }

    public void AdjustAttributes(float difficultyMultiplier)
    {
        health = Mathf.RoundToInt(baseHealth * difficultyMultiplier);
        damage = Mathf.RoundToInt(baseDamage * difficultyMultiplier);
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            // ���ѡ��һ���µ�ƫ����
            wanderOffset = Random.insideUnitCircle * wanderRadius;
            wanderTimer = wanderInterval;
        }
    }

    void MoveTowardsPlayer()
    {
        if (knight != null)
        {
            Vector2 targetPosition = (Vector2)knight.position + wanderOffset;
            float step = speed * Time.deltaTime;
            Vector2 currentPosition = transform.position;
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, step);

            // ��鲢����ת��
            if ((newPosition.x > currentPosition.x && !facingRight) || (newPosition.x < currentPosition.x && facingRight))
            {
                Flip();
            }

            transform.position = newPosition;
        }
    }

    void Flip()
    {
        // �л�����
        facingRight = !facingRight;

        // ͨ������x���ϵ�������������ת����
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knight"))
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // ȷ���������������ײ������
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        ShowDamage(damage); // ��ʾ�˺���ֵ
        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashOnDamage());
        }
    }

    private void ShowDamage(int damageAmount)
    {
        if (damageTextPrefab == null || canvasTransform == null)
        {
            return;
        }

        GameObject damageTextInstance = Instantiate(damageTextPrefab, canvasTransform);
        TextMeshProUGUI damageText = damageTextInstance.GetComponent<TextMeshProUGUI>();

        // �����ı�����
        damageText.text = damageAmount.ToString();

        // ���ı�λ��ת������Ļ���꣬������λ�õ�����ͷ��
        Vector3 worldPosition = transform.position + Vector3.up; // ����Y��������ʾ��ͷ���Ϸ�
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        damageTextInstance.transform.position = screenPosition;

        // ע�ᵽ DamageTextManager
        if (damageTextManager != null)
        {
            damageTextManager.RegisterDamageText(damageTextInstance);
        }

        // ���ı�����ʧ
        StartCoroutine(FadeOutAndDestroy(damageTextInstance));
    }

    private IEnumerator FadeOutAndDestroy(GameObject damageTextInstance)
    {
        TextMeshProUGUI damageText = damageTextInstance.GetComponent<TextMeshProUGUI>();
        Color originalColor = damageText.color;
        float duration = 1.0f; // ����ʱ��
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(damageTextInstance);
    }

    private IEnumerator FlashOnDamage()
    {
        spriteRenderer.color = blinkColor;
        yield return new WaitForSeconds(damageBlinkDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetBool("isDie", true);
        }

        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (player != null)
        {
            player.GainExperience(experienceValue);
        }

        Destroy(gameObject, 1f); // �ӳ����ٶ����Բ�����������
    }
}
