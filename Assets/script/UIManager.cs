using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // DivTopLeft ����е� TextMeshProUGUI ���
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI levelText;

    // DivTopLeft ����е� Slider ���
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
        // ��� UI �ı�Ԫ���Ƿ��ѷ���
        if (healthText == null || experienceText == null || levelText == null)
        {
            Debug.LogError("UI �ı�Ԫ��δ���䡣");
        }

        // ��� Slider Ԫ���Ƿ��ѷ���
        if (healthSlider == null || experienceSlider == null || levelSlider == null)
        {
            Debug.LogError("Slider Ԫ��δ���䡣");
        }

        if (gameOverPanel == null || upgradePanel == null || pausePanel == null)
        {
            Debug.LogError("���δ���䡣");
        }

        // ȷ���������Ϸ��ʼʱ����
        gameOverPanel.SetActive(false);
        upgradePanel.SetActive(false);
        pausePanel.SetActive(false);

        // ���ð�ť����¼�
        if (damageUpgradeButton != null)
        {
            damageUpgradeButton.onClick.AddListener(() => UpgradeWeapon("damage"));
        }
        else
        {
            Debug.LogError("DamageUpgradeButton δ���䡣");
        }

        if (fireRateUpgradeButton != null)
        {
            fireRateUpgradeButton.onClick.AddListener(() => UpgradeWeapon("fireRate"));
        }
        else
        {
            Debug.LogError("FireRateUpgradeButton δ���䡣");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("ExitButton δ���䡣");
        }
    }

    void Update()
    {
        if (player != null)
        {
            UpdateHealth(player.health, player.maxHealth);
            UpdateExperience(player.experience, player.experienceToNextLevel);
            UpdateLevel(player.level);

            // ��ͣ��Ϸ
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
            levelSlider.value = (float)level / 10; // �������ȼ�Ϊ10
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
        Time.timeScale = 0; // ��ͣ��Ϸ
    }

    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1; // �ָ���Ϸ
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
            Time.timeScale = 1; // �ָ���Ϸ
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0; // ��ͣ��Ϸ
        }
    }

    private void ExitGame()
    {
        GameManager.instance.GameOver();
    }
}
