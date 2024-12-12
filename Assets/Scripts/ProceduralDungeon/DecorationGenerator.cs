using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DecorationGenerator
{
    public static void CreateDecorations(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer, float decorationProbability)
    {
        var decorationPositions = FindRandomDecorations(floorPositions, decorationProbability);
        PlaceDecorations(tilemapVisualizer, decorationPositions);
    }

    private static HashSet<Vector2Int> FindRandomDecorations(HashSet<Vector2Int> floorPositions, float decorationProbability)
    {
        HashSet<Vector2Int> decorationPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            // Decide randomly if this position should have a decoration
            if (UnityEngine.Random.value < decorationProbability)
            {
                decorationPositions.Add(position);
            }
        }

        return decorationPositions;
    }

    private static void PlaceDecorations(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> decorationPositions)
    {
        foreach (var position in decorationPositions)
        {
            tilemapVisualizer.PaintRandomDecoration(position);
        }
    }
}
