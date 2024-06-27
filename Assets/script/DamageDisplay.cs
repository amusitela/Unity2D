using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageDisplay : MonoBehaviour
{
    public GameObject damageTextPrefab; // 伤害文本预制件
    public Transform canvasTransform;   // Canvas的Transform

    // 调用此方法来显示伤害
    public void ShowDamage(float damageAmount, Vector3 position)
    {
        // 创建伤害文本实例
        GameObject damageTextInstance = Instantiate(damageTextPrefab, canvasTransform);
        Text damageText = damageTextInstance.GetComponent<Text>();

        // 设置文本内容
        damageText.text = damageAmount.ToString();

        // 将文本位置转换到屏幕坐标，并调整位置到敌人头顶
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        screenPosition.y += 50; // 调整Y坐标来显示在头顶上方
        damageTextInstance.transform.position = screenPosition;

        // 让文本逐渐消失
        StartCoroutine(FadeOutAndDestroy(damageTextInstance));
    }

    // 让伤害文本逐渐消失的协程
    private IEnumerator FadeOutAndDestroy(GameObject damageTextInstance)
    {
        Text damageText = damageTextInstance.GetComponent<Text>();
        Color originalColor = damageText.color;
        float duration = 0.01f; // 持续时间
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
}
