using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualizer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemyPrefabs; // Daftar prefab musuh

    private Dictionary<Vector2Int, GameObject> instantiatedEnemies = new Dictionary<Vector2Int, GameObject>();

    public void PlaceEnemy(Vector2Int position, GameObject enemyPrefab)
    {
        // Konversi posisi grid ke world position
        Vector3 worldPosition = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);

        // Cek jika posisi ini belum memiliki musuh
        if (!instantiatedEnemies.ContainsKey(position))
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, worldPosition, Quaternion.identity);
            enemyInstance.transform.parent = this.transform; // Jadikan EnemyVisualizer sebagai parent
            instantiatedEnemies.Add(position, enemyInstance);
        }
    }

    public void ClearEnemies()
    {
        // Hapus semua musuh yang diinstansiasi
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // Kosongkan dictionary
        instantiatedEnemies.Clear();
    }

    public GameObject GetRandomEnemyPrefab()
    {
        if (enemyPrefabs.Count > 0)
        {
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        }
        return null;
    }
}
