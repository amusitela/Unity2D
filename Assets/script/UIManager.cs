using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // DivTopLeft 面板中的 TextMeshProUGUI 组件
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI levelText;

    // DivTopLeft 面板中的 Slider 组件
    public Slider healthSlider;
    public Slider experienceSlider;
    public Slider levelSlider;

    public GameObject gameOverPanel;
    public GameObject upgradePanel;
    public GameObject pausePanel; 
    public Button damageUpgradeButton;
    public Button fireRateUpgradeButton;
    public Button exitButton; 

    public PlayerController player;

    void Start()
    {
        // 检查 UI 文本元素是否已分配
        if (healthText == null || experienceText == null || levelText == null)
        {
            Debug.LogError("UI 文本元素未分配。");
        }

        // 检查 Slider 元素是否已分配
        if (healthSlider == null || experienceSlider == null || levelSlider == null)
        {
            Debug.LogError("Slider 元素未分配。");
        }

        if (gameOverPanel == null || upgradePanel == null || pausePanel == null)
        {
            Debug.LogError("面板未分配。");
        }

        // 确保面板在游戏开始时隐藏
        gameOverPanel.SetActive(false);
        upgradePanel.SetActive(false);
        pausePanel.SetActive(false);

        // 设置按钮点击事件
        if (damageUpgradeButton != null)
        {
            damageUpgradeButton.onClick.AddListener(() => UpgradeWeapon("damage"));
        }
        else
        {
            Debug.LogError("DamageUpgradeButton 未分配。");
        }

        if (fireRateUpgradeButton != null)
        {
            fireRateUpgradeButton.onClick.AddListener(() => UpgradeWeapon("fireRate"));
        }
        else
        {
            Debug.LogError("FireRateUpgradeButton 未分配。");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("ExitButton 未分配。");
        }
    }

    void Update()
    {
        if (player != null)
        {
            UpdateHealth(player.health, player.maxHealth);
            UpdateExperience(player.experience, player.experienceToNextLevel);
            UpdateLevel(player.level);

            // 暂停游戏
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    public void SetPlayer(PlayerController playerController)
    {
        player = playerController;
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void UpdateExperience(int currentExperience, int experienceToNextLevel)
    {
        if (experienceSlider != null)
        {
            experienceSlider.value = (float)currentExperience / experienceToNextLevel;
        }
        experienceText.text = currentExperience + "/" + experienceToNextLevel;
    }

    public void UpdateLevel(int level)
    {
        if (levelSlider != null)
        {
            levelSlider.value = (float)level / 10; // 假设最大等级为10
        }
        levelText.text = level + "/10";
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowUpgradePanel()
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0; // 暂停游戏
    }

    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1; // 恢复游戏
    }

    private void UpgradeWeapon(string type)
    {
        if (player != null)
        {
            player.UpgradeWeaponStats(type);
            HideUpgradePanel();
        }
    }

    private void TogglePause()
    {
        if (pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1; // 恢复游戏
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0; // 暂停游戏
        }
    }

    private void ExitGame()
    {
        GameManager.instance.GameOver();
    }
}
