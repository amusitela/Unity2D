using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public GameObject panel;
    public Button upgradeWeaponButton;
    public Button upgradeAttributeButton;

    void Start()
    {
        panel.SetActive(false);
        upgradeWeaponButton.onClick.AddListener(UpgradeWeapon);
        upgradeAttributeButton.onClick.AddListener(UpgradeAttribute);
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    void HidePanel()
    {
        panel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    void UpgradeWeapon()
    {
        // Logic to upgrade weapon
        HidePanel();
    }

    void UpgradeAttribute()
    {
        // Logic to upgrade attribute
        HidePanel();
    }
}
