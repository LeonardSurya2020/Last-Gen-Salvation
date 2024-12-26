using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static void SpawnEnemies(
        List<BoundsInt> roomsList,
        EnemyVisualizer enemyVisualizer,
        int maxEnemiesPerRoom,
        float spawnProbability,
        int spawnMargin)
    {
        enemyVisualizer.ClearEnemies(); // Bersihkan musuh lama

        for (int i = 0; i < roomsList.Count; i++)
        {
            // Skip ruangan pertama
            if (i == 0) continue;

            // Ambil ruangan saat ini
            var room = roomsList[i];

            // Tentukan jumlah musuh untuk ruangan ini secara acak
            int enemyCount = Random.Range(1, maxEnemiesPerRoom + 1);

            // Dapatkan posisi lantai dalam ruangan ini dengan margin
            HashSet<Vector2Int> roomFloorPositions = GetRoomFloorPositions(room, spawnMargin);

            // Pilih posisi acak untuk musuh
            List<Vector2Int> enemyPositions = SelectRandomPositions(roomFloorPositions, enemyCount, spawnProbability);

            // Tempatkan musuh di posisi yang dipilih
            foreach (var position in enemyPositions)
            {
                GameObject enemyPrefab = enemyVisualizer.GetRandomEnemyPrefab();
                if (enemyPrefab != null)
                {
                    enemyVisualizer.PlaceEnemy(position, enemyPrefab);
                }
            }
        }
    }

    private static HashSet<Vector2Int> GetRoomFloorPositions(BoundsInt room, int margin)
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        // Hindari tepi ruangan dengan margin
        for (int col = margin; col < room.size.x - margin; col++)
        {
            for (int row = margin; row < room.size.y - margin; row++)
            {
                Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                floorPositions.Add(position);
            }
        }

        return floorPositions;
    }

    private static List<Vector2Int> SelectRandomPositions(
        HashSet<Vector2Int> floorPositions,
        int count,
        float spawnProbability)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>(floorPositions);
        List<Vector2Int> selectedPositions = new List<Vector2Int>();

        for (int i = 0; i < count; i++)
        {
            if (availablePositions.Count == 0) break;

            // Pilih posisi acak dengan probabilitas tertentu
            int randomIndex = Random.Range(0, availablePositions.Count);
            if (Random.value <= spawnProbability) // Spawn dengan peluang tertentu
            {
                selectedPositions.Add(availablePositions[randomIndex]);
            }
            availablePositions.RemoveAt(randomIndex);
        }

        return selectedPositions;
    }
}
