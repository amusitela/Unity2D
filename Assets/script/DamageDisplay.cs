using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageDisplay : MonoBehaviour
{
    public GameObject damageTextPrefab; // �˺��ı�Ԥ�Ƽ�
    public Transform canvasTransform;   // Canvas��Transform

    // ���ô˷�������ʾ�˺�
    public void ShowDamage(float damageAmount, Vector3 position)
    {
        // �����˺��ı�ʵ��
        GameObject damageTextInstance = Instantiate(damageTextPrefab, canvasTransform);
        Text damageText = damageTextInstance.GetComponent<Text>();

        // �����ı�����
        damageText.text = damageAmount.ToString();

        // ���ı�λ��ת������Ļ���꣬������λ�õ�����ͷ��
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        screenPosition.y += 50; // ����Y��������ʾ��ͷ���Ϸ�
        damageTextInstance.transform.position = screenPosition;

        // ���ı�����ʧ
        StartCoroutine(FadeOutAndDestroy(damageTextInstance));
    }

    // ���˺��ı�����ʧ��Э��
    private IEnumerator FadeOutAndDestroy(GameObject damageTextInstance)
    {
        Text damageText = damageTextInstance.GetComponent<Text>();
        Color originalColor = damageText.color;
        float duration = 0.01f; // ����ʱ��
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
