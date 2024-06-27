using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject damageTextPrefab; // �˺��ı�Ԥ�Ƽ�
    public Transform canvasTransform;   // Canvas��Transform

    void Awake()
    {
        // ����Ϸ����ʱ���� Enemy ��ľ�̬����
        Enemy.damageTextPrefab = damageTextPrefab;
        Enemy.canvasTransform = canvasTransform;

        Enemy2.damageTextPrefab = damageTextPrefab;
        Enemy2.canvasTransform = canvasTransform;
    }
}
