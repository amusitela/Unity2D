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
    public Tilemap enemyGeneratorTilemap; // ���õ�Tilemap
    private List<Vector3> spawnablePositions; // �洢�����ɵ��˵�λ��
    private float currentSpawnRate;
    private float difficultyMultiplier = 1f; // �Ѷ�ϵ������ʼΪ1

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
            Debug.LogError("Tilemap����δ���ã�");
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
                    // Debug.Log("�������λ��: " + place);
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
            currentSpawnRate = Mathf.Max(currentSpawnRate, 0.5f); // ȷ�����ɼ����С��0.5��
            difficultyMultiplier += 0.1f; // �Ѷ�ϵ������
            // Debug.Log("��ǰ�����ٶ�: " + currentSpawnRate);
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

            // ��ȡ���˵� Enemy �� Enemy2 �ű��������� difficultyMultiplier ��������
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
            // Debug.Log("���ɵ�����: " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("û���ҵ�����λ�ã�");
        }
    }
}
