using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationPrefabGenerator : MonoBehaviour
{
    public static void CreatePrefabDecorations(HashSet<Vector2Int> floorPositions, PrefabVisualizer prefabVisualizer, float decorationProbability)
    {
        prefabVisualizer.ClearDecorations();
        var decorationPositions = FindRandomPrefabDecorations(floorPositions, decorationProbability);
        PlacePrefabDecorations(prefabVisualizer, decorationPositions);
    }

    private static HashSet<Vector2Int> FindRandomPrefabDecorations(HashSet<Vector2Int> floorPositions, float decorationProbability)
    {
        HashSet<Vector2Int> decorationPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            // Secara acak tentukan apakah posisi ini akan memiliki dekorasi
            if (UnityEngine.Random.value < decorationProbability)
            {
                decorationPositions.Add(position);
            }
        }

        return decorationPositions;
    }

    private static void PlacePrefabDecorations(PrefabVisualizer prefabVisualizer, HashSet<Vector2Int> decorationPositions)
    {
        foreach (var position in decorationPositions)
        {
            prefabVisualizer.PlaceRandomDecoration(position, 1.0f); // Selalu panggil PlaceRandomDecoration
        }
    }
}
