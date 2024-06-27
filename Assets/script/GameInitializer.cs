using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject damageTextPrefab; // 伤害文本预制件
    public Transform canvasTransform;   // Canvas的Transform

    void Awake()
    {
        // 在游戏启动时设置 Enemy 类的静态变量
        Enemy.damageTextPrefab = damageTextPrefab;
        Enemy.canvasTransform = canvasTransform;

        Enemy2.damageTextPrefab = damageTextPrefab;
        Enemy2.canvasTransform = canvasTransform;
    }
}
