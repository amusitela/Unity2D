using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public int health = 100; // 生命值
    public int maxHealth = 100; // 最大生命值
    public int experience = 0; // 经验值
    public int experienceToNextLevel = 100; // 升级所需经验值
    public int level = 1; // 等级
    public GameObject childObject;

    private UIManager uiManager; // UI 管理脚本
    private Animator animator; // 动画控制器
    public bool facingRight = true; // 角色初始面向右侧

     //private bool isWalking = false; 是否在走路
    private float walkSoundTimer = 0f; // 控制走路音效播放的计时器
    public float walkSoundInterval = 0.5f; // 走路音效的播放间隔

    void Start()
    {
        // 获取 UIManager 实例
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("未找到 UIManager 脚本实例。");
        }

        // 获取 Animator 组件
        if (childObject == null)
        {
            childObject = transform.GetChild(0).gameObject;
        }
        animator = childObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("未找到 Animator 组件。");
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // 判断是否移动
        bool isMoving = moveX != 0 || moveY != 0;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            walkSoundTimer -= Time.deltaTime;
            if (walkSoundTimer <= 0f)
            {
                AudioManager.instance.PlaySound(AudioManager.instance.walkClip);
                walkSoundTimer = walkSoundInterval;
            }
        }

        if (!Input.GetButton("Fire1"))
        {
            // 角色转身逻辑
            if (moveX > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveX < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = childObject.transform.localScale; // 确保只翻转子对象的缩放
        scale.x *= -1;
        childObject.transform.localScale = scale;
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }

        if (uiManager != null)
        {
            uiManager.UpdateExperience(experience, experienceToNextLevel);
        }
    }

    void LevelUp()
    {
        level++;
        health = maxHealth;
        experience = 0;
        experienceToNextLevel += experienceToNextLevel;

        if (level > 10)
        {
            level = 10;
            return;
        }

        // 播放升级音效
        AudioManager.instance.PlaySound(AudioManager.instance.levelUpClip);

        // 确保 uiManager 不为 null
        if (uiManager != null)
        {
            uiManager.ShowUpgradePanel();
            uiManager.UpdateLevel(level);
        }
        else
        {
            Debug.LogError("UIManager 为 null，无法显示升级面板。");
        }
    }

    public void UpgradeWeaponStats(string type)
    {
        WeaponController weapon = GetComponentInChildren<WeaponController>();
        if (weapon != null)
        {
            weapon.UpgradeWeaponStats(type);
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        if (uiManager != null)
        {
            uiManager.UpdateHealth(health, maxHealth);
        }
    }

    public void PowerUp(int amount)
    {
        // 在这里实现PowerUp逻辑，例如临时增加移动速度或攻击力
        StartCoroutine(PowerUpCoroutine(amount));
    }

    private IEnumerator PowerUpCoroutine(int amount)
    {
        // 假设这个PowerUp是增加移动速度
        moveSpeed += amount;
        yield return new WaitForSeconds(5);  // 假设PowerUp持续5秒
        moveSpeed -= amount;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(10); // 示例：假设敌人造成10点伤害
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        else
        {
            // 播放受伤音效
            AudioManager.instance.PlaySound(AudioManager.instance.hitClip);
        }

        if (uiManager != null)
        {
            uiManager.UpdateHealth(health, maxHealth);
        }
    }

    void Die()
    {
        animator.SetTrigger("DieTrigger"); // 触发死亡动画

        // 播放死亡音效
        AudioManager.instance.PlaySound(AudioManager.instance.dieClip);

        Invoke("Restart", 3f); // 3秒后调用Restart方法
    }

    private void Restart()
    {
        GameManager.instance.GameOver();
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void StartGame()
    {
        // 重置玩家状态
        health = maxHealth;
        experience = 0;
        level = 1;
        if (uiManager != null)
        {
            uiManager.UpdateHealth(health, maxHealth);
            uiManager.UpdateExperience(experience, experienceToNextLevel);
            uiManager.UpdateLevel(level);
        }
    }
}
