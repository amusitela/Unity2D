using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // �ƶ��ٶ�
    public int health = 100; // ����ֵ
    public int maxHealth = 100; // �������ֵ
    public int experience = 0; // ����ֵ
    public int experienceToNextLevel = 100; // �������辭��ֵ
    public int level = 1; // �ȼ�
    public GameObject childObject;

    private UIManager uiManager; // UI ����ű�
    private Animator animator; // ����������
    public bool facingRight = true; // ��ɫ��ʼ�����Ҳ�

     //private bool isWalking = false; �Ƿ�����·
    private float walkSoundTimer = 0f; // ������·��Ч���ŵļ�ʱ��
    public float walkSoundInterval = 0.5f; // ��·��Ч�Ĳ��ż��

    void Start()
    {
        // ��ȡ UIManager ʵ��
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("δ�ҵ� UIManager �ű�ʵ����");
        }

        // ��ȡ Animator ���
        if (childObject == null)
        {
            childObject = transform.GetChild(0).gameObject;
        }
        animator = childObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("δ�ҵ� Animator �����");
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

        // �ж��Ƿ��ƶ�
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
            // ��ɫת���߼�
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
        Vector3 scale = childObject.transform.localScale; // ȷ��ֻ��ת�Ӷ��������
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

        // ����������Ч
        AudioManager.instance.PlaySound(AudioManager.instance.levelUpClip);

        // ȷ�� uiManager ��Ϊ null
        if (uiManager != null)
        {
            uiManager.ShowUpgradePanel();
            uiManager.UpdateLevel(level);
        }
        else
        {
            Debug.LogError("UIManager Ϊ null���޷���ʾ������塣");
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
        // ������ʵ��PowerUp�߼���������ʱ�����ƶ��ٶȻ򹥻���
        StartCoroutine(PowerUpCoroutine(amount));
    }

    private IEnumerator PowerUpCoroutine(int amount)
    {
        // �������PowerUp�������ƶ��ٶ�
        moveSpeed += amount;
        yield return new WaitForSeconds(5);  // ����PowerUp����5��
        moveSpeed -= amount;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(10); // ʾ��������������10���˺�
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
            // ����������Ч
            AudioManager.instance.PlaySound(AudioManager.instance.hitClip);
        }

        if (uiManager != null)
        {
            uiManager.UpdateHealth(health, maxHealth);
        }
    }

    void Die()
    {
        animator.SetTrigger("DieTrigger"); // ������������

        // ����������Ч
        AudioManager.instance.PlaySound(AudioManager.instance.dieClip);

        Invoke("Restart", 3f); // 3������Restart����
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
        // �������״̬
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
