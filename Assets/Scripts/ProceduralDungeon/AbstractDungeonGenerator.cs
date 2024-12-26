using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    protected PrefabVisualizer prefabVisualizer = null;

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        prefabVisualizer.ClearDecorations();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();


}
