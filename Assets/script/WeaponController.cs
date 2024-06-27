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
        // ��ȡPlayerController��������ĸ�����
        if (transform.parent != null && transform.parent.parent != null)
        {
            playerController = transform.parent.parent.GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            Debug.LogError("δ�ҵ� PlayerController �ű�ʵ����");
        }

        EquipWeapon(currentWeaponIndex);
    }

    void Update()
    {
        // ������λ������Ϊ WeaponController ��λ��
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
        currentWeapon.transform.position = transform.position;  // ������λ������ΪWeaponController��λ��
        currentWeapon.transform.rotation = transform.rotation;  // ��������ת����ΪWeaponController����ת
        currentWeapon.gameObject.SetActive(true);
    }

    public void UpgradeWeaponStats(string type)
    {
        currentWeapon.UpgradeWeaponStats(type);
    }
}
