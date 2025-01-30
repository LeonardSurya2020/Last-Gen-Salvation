using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidht = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    [SerializeField]
    private float decorationProbability;

    [SerializeField]
    private GameObject player; // Referensi ke player

    [SerializeField]
    private EnemyVisualizer enemyVisualizer; // Tambahkan visualizer musuh

    [SerializeField]
    private int maxEnemiesPerRoom = 5; // Maksimum jumlah musuh per ruangan
    [SerializeField]
    [Range(0f, 1f)]
    private float enemySpawnProbability = 0.8f; // Peluang spawn musuh di setiap posisi
    [SerializeField]
    private int spawnMargin = 1; // Margin dari tepi ruangan untuk area spawn

    [SerializeField]
    private GameObject teleporterPrefab; // Prefab untuk teleporter

    [SerializeField]
    private GameObject startTeleporterPrefab; // Prefab untuk teleporter

    public float totalRoom;

    private void Start()
    {
        GenerateDungeon();
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidht, dungeonHeight, 0)),
            minRoomWidth, minRoomHeight);

        while (roomsList.Count < totalRoom || roomsList.Count > totalRoom)
        {
            CreateRooms();
            return;
        }

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);

        var firstRoom = roomsList[0];
        Vector2Int firstRoomCenter = (Vector2Int)Vector3Int.RoundToInt(firstRoom.center);

        // Tempatkan teleporter di posisi awal pemain
        PlacePlayer(firstRoomCenter);
        PlaceTeleporterAtPlayerStart(firstRoomCenter); // Tambahkan ini

        List<Vector2Int> roomCenter = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenter);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        DecorationGenerator.CreateDecorations(floor, tilemapVisualizer, decorationProbability);
        DecorationPrefabGenerator.CreatePrefabDecorations(floor, prefabVisualizer, decorationProbability);

        // Spawn musuh di setiap ruangan
        EnemySpawner.SpawnEnemies(roomsList, enemyVisualizer, maxEnemiesPerRoom, enemySpawnProbability, spawnMargin);

        // Letakkan teleporter di ruangan terakhir
        PlaceTeleporter(roomsList);
    }

    private void PlacePlayer(Vector2Int spawnPosition)
    {
        // Cek apakah player sudah diinstansiasi di scene
        GameObject[] existingPlayers = GameObject.FindGameObjectsWithTag("Player"); // Pastikan player diberi tag "Player"
        GameObject existingPlayerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawn"); // Pastikan player diberi tag "Player"
        //Jika ada, hancurkan player lama
        //if (existingPlayer != null)
        //{
        //    Debug.Log("Hancurkan player");
        //    DestroyImmediate(existingPlayer.transform.parent.gameObject);
        //}

        // Jika ada, hancurkan player lama
        foreach (GameObject existingPlayer in existingPlayers)
        {
            if (existingPlayer != null)
            {
                // Jika ada parent, hancurkan parent
                if (existingPlayer.transform.parent != null)
                {
                    Debug.Log("Hancurkan player dengan parent");
                    DestroyImmediate(existingPlayer.transform.parent.gameObject);
                }
                else
                {
                    // Jika tidak ada parent, hancurkan langsung existingPlayer
                    Debug.Log("Hancurkan player langsung");
                    DestroyImmediate(existingPlayer);
                }
            }
        }

        if (existingPlayerSpawner != null)
        {
            // Jika ada parent, hancurkan parent
            if (existingPlayerSpawner.transform.parent != null)
            {
                Debug.Log("Hancurkan player dengan parent");
                DestroyImmediate(existingPlayerSpawner.transform.parent.gameObject);
            }
            else
            {
                // Jika tidak ada parent, hancurkan langsung existingPlayer
                Debug.Log("Hancurkan player langsung");
                DestroyImmediate(existingPlayerSpawner);
            }
        }

        //if (existingPlayer.transform.parent != null)
        //{
        //    DestroyImmediate(existingPlayer.transform.parent.gameObject);
        //}
        //else
        //{
        //    // Jika tidak ada parent, hancurkan langsung existingPlayer
        //    DestroyImmediate(existingPlayer);
        //}

        // Ubah posisi spawn dari koordinat grid ke posisi dunia
        Vector3 worldPosition = tilemapVisualizer.floorTilemap.CellToWorld((Vector3Int)spawnPosition);

        // Cek jika player belum diinstansiasi
        if (player == null)
        {
            Debug.LogError("Player object reference is not set!");
            return;
        }

        // Pastikan objek player ada di scene (instantiate jika belum ada)
        GameObject playerInstance = Instantiate(player, worldPosition, Quaternion.identity);
        playerInstance.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0); // Pastikan Z = 0 untuk 2D
        //playerInstance.tag = "Player";  // Beri tag pada objek player yang baru diinstansiasi
        Debug.Log("Player instantiated at: " + playerInstance.transform.position);
    }

    // Menambahkan teleporter di posisi awal pemain
    private void PlaceTeleporterAtPlayerStart(Vector2Int playerStartPosition)
    {
        if (startTeleporterPrefab == null)
        {
            Debug.LogWarning("Teleporter prefab belum diatur!");
            return;
        }

        // Konversi posisi ke world position (dengan offset)
        Vector3 worldPosition = new Vector3(playerStartPosition.x, playerStartPosition.y + 0.2f, 0);
        Debug.Log("posisi = " + worldPosition);

        // Instansiasi teleporter
        Instantiate(startTeleporterPrefab, worldPosition, Quaternion.identity);
    }

    private void PlaceTeleporter(List<BoundsInt> roomsList)
    {
        if (teleporterPrefab == null)
        {
            Debug.LogWarning("Teleporter prefab belum diatur!");
            return;
        }

        // Tentukan ruangan terakhir (akhir dari daftar ruangan)
        var lastRoom = roomsList[roomsList.Count - 1];

        // Ambil posisi tengah ruangan terakhir
        Vector2Int lastRoomCenter = (Vector2Int)Vector3Int.RoundToInt(lastRoom.center);

        // Konversi ke world position (dengan offset)
        Vector3 worldPosition = new Vector3(lastRoomCenter.x + 0.5f, lastRoomCenter.y + 0.5f, 0);

        // Instansiasi teleporter
        Instantiate(teleporterPrefab, worldPosition, Quaternion.identity);
    }


    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while(roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);

        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while(position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while(position.x != destination.x)
        {
            if(destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int) room.min + new Vector2Int(col, row);
                    floor.Add(position);

                }
            }
        }
        return floor;
    }




}
