using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon[] weapons;
    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;
    private PlayerController playerController;

    void Start()
    {
        // 获取PlayerController，父对象的父对象
        if (transform.parent != null && transform.parent.parent != null)
        {
            playerController = transform.parent.parent.GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            Debug.LogError("未找到 PlayerController 脚本实例。");
        }

        EquipWeapon(currentWeaponIndex);
    }

    void Update()
    {
        // 将武器位置设置为 WeaponController 的位置
        if (currentWeapon != null)
        {
            currentWeapon.transform.position = transform.position;
            currentWeapon.transform.rotation = transform.rotation;
        }

        Aim();
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.switchClip);
            SwitchWeapon();
        }

        if (Input.GetButton("Fire1") && Time.time > currentWeapon.nextFireTime)
        {
            currentWeapon.Use();
            currentWeapon.nextFireTime = Time.time + currentWeapon.fireRate;
        }
    }

    void Aim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (playerController != null)
        {
            if (playerController.facingRight)
            {
                angle = Mathf.Clamp(angle, -90f, 90f);
                Vector3 weaponScale = currentWeapon.transform.localScale;
                weaponScale.y = Mathf.Abs(weaponScale.y);
                currentWeapon.transform.localScale = weaponScale;
            }
            else
            {
                if (angle > 0)
                {
                    angle = Mathf.Clamp(angle, 90f, 180f);
                }
                else
                {
                    angle = Mathf.Clamp(angle, -180f, -90f);
                }
                Vector3 weaponScale = currentWeapon.transform.localScale;
                weaponScale.y = -Mathf.Abs(weaponScale.y);
                currentWeapon.transform.localScale = weaponScale;
            }
        }

        currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void SwitchWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }
        EquipWeapon(currentWeaponIndex);
    }

    void EquipWeapon(int index)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeapon = weapons[index];
        currentWeapon.transform.position = transform.position;  // 将武器位置设置为WeaponController的位置
        currentWeapon.transform.rotation = transform.rotation;  // 将武器旋转设置为WeaponController的旋转
        currentWeapon.gameObject.SetActive(true);
    }

    public void UpgradeWeaponStats(string type)
    {
        currentWeapon.UpgradeWeaponStats(type);
    }
}
