using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EndlessMapGenerator : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float initialSpawnRate = 2f;
    public float difficultyIncreaseInterval = 30f;
    public float spawnRateDecreaseAmount = 0.1f;
    public Tilemap enemyGeneratorTilemap; // 引用到Tilemap
    private List<Vector3> spawnablePositions; // 存储可生成敌人的位置
    private float currentSpawnRate;
    private float difficultyMultiplier = 1f; // 难度系数，初始为1

    void Start()
    {
        spawnablePositions = new List<Vector3>();
        currentSpawnRate = initialSpawnRate;
        FindSpawnablePositions();
        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    void FindSpawnablePositions()
    {
        if (enemyGeneratorTilemap == null)
        {
            Debug.LogError("Tilemap引用未设置！");
            return;
        }

        BoundsInt bounds = enemyGeneratorTilemap.cellBounds;
        TileBase[] allTiles = enemyGeneratorTilemap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int localPlace = new Vector3Int(x, y, 0);
                Vector3 place = enemyGeneratorTilemap.CellToWorld(localPlace);

                if (enemyGeneratorTilemap.HasTile(localPlace))
                {
                    spawnablePositions.Add(place);
                    // Debug.Log("添加生成位置: " + place);
                }
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            currentSpawnRate -= spawnRateDecreaseAmount;
            currentSpawnRate = Mathf.Max(currentSpawnRate, 0.5f); // 确保生成间隔不小于0.5秒
            difficultyMultiplier += 0.1f; // 难度系数增加
            // Debug.Log("当前生成速度: " + currentSpawnRate);
        }
    }

    void SpawnEnemy()
    {
        if (spawnablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnablePositions.Count);
            Vector3 spawnPosition = spawnablePositions[randomIndex];
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyObject = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);

            // 获取敌人的 Enemy 或 Enemy2 脚本，并根据 difficultyMultiplier 调整属性
            Enemy enemy1 = enemyObject.GetComponent<Enemy>();
            Enemy2 enemy2 = enemyObject.GetComponent<Enemy2>();

            if (enemy1 != null)
            {
                enemy1.AdjustAttributes(difficultyMultiplier);
            }
            else if (enemy2 != null)
            {
                enemy2.AdjustAttributes(difficultyMultiplier);
            }
            // Debug.Log("生成敌人于: " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("没有找到生成位置！");
        }
    }
}
