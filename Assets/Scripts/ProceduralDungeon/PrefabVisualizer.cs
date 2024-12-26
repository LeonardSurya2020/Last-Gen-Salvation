using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabVisualizer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> decorationPrefabs; // List prefab dekorasi

    private Dictionary<Vector2Int, GameObject> instantiatedDecorations = new Dictionary<Vector2Int, GameObject>();

    public void PlaceRandomDecoration(Vector2Int position, float probability)
    {
        if (Random.value < probability && decorationPrefabs.Count > 0)
        {
            // Pilih prefab dekorasi secara acak
            GameObject randomPrefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Count)];

            // Konversi posisi grid ke world position
            Vector3 worldPosition = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);

            // Cek jika posisi ini belum memiliki dekorasi
            if (!instantiatedDecorations.ContainsKey(position))
            {
                GameObject decorationInstance = Instantiate(randomPrefab, worldPosition, Quaternion.identity);
                decorationInstance.transform.parent = this.transform; // Menjadikan PrefabVisualizer sebagai parent
                instantiatedDecorations.Add(position, decorationInstance);
            }
        }
    }

    //public void ClearDecorations()
    //{

    //    // Hapus semua dekorasi yang sudah diinstansiasi
    //    foreach (var decoration in instantiatedDecorations.Values)
    //    {
    //        Debug.Log("clear prefab terpanggil");
    //        DestroyImmediate(decoration);
    //    }
    //    instantiatedDecorations.Clear();
    //}

    public void ClearDecorations()
    {
        Debug.Log("ClearDecorations terpanggil");

        // Hapus semua child dari PrefabVisualizer
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // Kosongkan dictionary
        instantiatedDecorations.Clear();
    }
}
