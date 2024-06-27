using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextManager : MonoBehaviour
{
    public float maxDisplayTime = 1.5f; // �����ı����ڵ��ʱ��

    private List<DamageTextInstance> activeDamageTexts = new List<DamageTextInstance>();

    void Update()
    {
        // ÿ֡������л�Ծ���˺��ı�
        for (int i = activeDamageTexts.Count - 1; i >= 0; i--)
        {
            if (Time.time - activeDamageTexts[i].creationTime > maxDisplayTime)
            {
                Destroy(activeDamageTexts[i].gameObject);
                activeDamageTexts.RemoveAt(i);
            }
        }
    }

    public void RegisterDamageText(GameObject damageText)
    {
        activeDamageTexts.Add(new DamageTextInstance(damageText));
    }

    private class DamageTextInstance
    {
        public GameObject gameObject;
        public float creationTime;

        public DamageTextInstance(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.creationTime = Time.time;
        }
    }
}
