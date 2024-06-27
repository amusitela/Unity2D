using System.Collections;
using UnityEngine;
using TMPro;

public class Enemy2 : MonoBehaviour
{
    public int health = 50; // 生命值
    public int experienceValue = 20; // 击杀敌人后玩家获得的经验值
    public int damage = 10; // 对玩家造成的伤害
    public float speed = 2f; // 移动速度
    public GameObject explosionPrefab; // 爆炸效果预制体
    public GameObject bulletPrefab; // 子弹预制体
    public float shootingInterval = 2f; // 发射子弹的时间间隔
    public float shootingDistance = 10f; // 发射子弹的距离
    public float stopDistance = 2f; // 停止移动的距离

    public float wanderRadius = 1f; // 随机移动的半径
    public float wanderInterval = 0.5f; // 随机移动的时间间隔
    private Vector2 wanderOffset;
    private float wanderTimer;
    private float shootingTimer;

    private Transform knight; // 玩家位置

    private bool facingRight = true; // 用于跟踪敌人的当前朝向

    private int baseDamage = 10;
    private int baseHealth = 50;

    private Animator animator;
    private Vector2 previousPosition;

    private SpriteRenderer spriteRenderer;
    public float damageBlinkDuration = 0.1f; // 受伤闪烁持续时间
    public Color blinkColor = Color.red; // 受伤时的颜色
    private Color originalColor; // 原始颜色

    public static GameObject damageTextPrefab; // 伤害文本预制件
    public static Transform canvasTransform;   // Canvas的Transform
    private DamageTextManager damageTextManager; // 管理伤害文本的管理器

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("未找到 Animator 组件。");
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        knight = GameObject.FindGameObjectWithTag("Knight").transform;
        if (knight == null)
        {
            Debug.LogError("未找到带有 'Knight' 标签的对象。");
            return;
        }
        wanderOffset = Vector2.zero;
        wanderTimer = wanderInterval;
        shootingTimer = shootingInterval;
        previousPosition = transform.position;

        // 确保初始动画参数正确
        animator.SetBool("isDie", false);
        animator.SetBool("isIdle", false);

        // 获取 DamageTextManager
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
            ShootAtPlayer();

            // 检查是否在移动
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
            // 随机选择一个新的偏移量
            wanderOffset = Random.insideUnitCircle * wanderRadius;
            wanderTimer = wanderInterval;
        }
    }

    void MoveTowardsPlayer()
    {
        if (knight != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, knight.position);
            if (distanceToPlayer > stopDistance)
            {
                Vector2 targetPosition = (Vector2)knight.position + wanderOffset;
                float step = speed * Time.deltaTime;
                Vector2 currentPosition = transform.position;
                Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, step);

                // 检查并处理转身
                if ((newPosition.x > currentPosition.x && !facingRight) || (newPosition.x < currentPosition.x && facingRight))
                {
                    Flip();
                }

                transform.position = newPosition;
            }
        }
    }

    void ShootAtPlayer()
    {
        if (knight != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, knight.position);
            shootingTimer -= Time.deltaTime;
            if (distanceToPlayer <= shootingDistance && shootingTimer <= 0f)
            {
                shootingTimer = shootingInterval;
                Vector2 shootingDirection = (knight.position - transform.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                EnemyBullet bulletComponent = bullet.GetComponent<EnemyBullet>();
                if (bulletComponent != null)
                {
                    bulletComponent.damage = damage;
                    bulletComponent.SetDirection(shootingDirection);
                }
            }
        }
    }

    void Flip()
    {
        // 切换朝向
        facingRight = !facingRight;

        // 通过调整x轴上的缩放因子来翻转敌人
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
            Explode(); // 触发爆炸效果
            Destroy(gameObject); // 确保敌人在与玩家碰撞后被销毁
        }
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        ShowDamage(damage); // 显示伤害数值
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

        // 设置文本内容
        damageText.text = damageAmount.ToString();

        // 将文本位置转换到屏幕坐标，并调整位置到敌人头顶
        Vector3 worldPosition = transform.position + Vector3.up; // 调整Y坐标来显示在头顶上方
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        damageTextInstance.transform.position = screenPosition;

        // 注册到 DamageTextManager
        if (damageTextManager != null)
        {
            damageTextManager.RegisterDamageText(damageTextInstance);
        }

        // 让文本逐渐消失
        StartCoroutine(FadeOutAndDestroy(damageTextInstance));
    }

    private IEnumerator FadeOutAndDestroy(GameObject damageTextInstance)
    {
        TextMeshProUGUI damageText = damageTextInstance.GetComponent<TextMeshProUGUI>();
        Color originalColor = damageText.color;
        float duration = 1.0f; // 持续时间
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

        Explode(); // 触发爆炸效果
        Destroy(gameObject, 1f); // 延迟销毁对象以播放死亡动画
    }
}
