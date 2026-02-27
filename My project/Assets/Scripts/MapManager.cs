using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] public GameObject[] mapPrefabs;

    private float roomWidth = 17f;
    private float roomHeight = 13f;
    
    private int _maxRoomInMap = 5;
    public StageType currentStage;
    private List<Vector2Int> roomPos;
    private Dictionary<Vector2Int, GameObject> roomGrid;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        DrawMap();
    }

    void Init()
    {
        SetMaxRoomInMap();
        roomPos = new List<Vector2Int>();
        roomGrid = new Dictionary<Vector2Int, GameObject>();
        CreateRoom();
    }

    void DrawMap()
    {
        foreach (var pos in roomPos)
        {
            if (roomGrid.TryGetValue(pos, out GameObject map))
            {
                Vector2 spawnPos = new Vector2(pos.x * roomWidth, pos.y * roomHeight);
                Instantiate(map, spawnPos, Quaternion.identity);
            }
        }
    }
    
    void SetMaxRoomInMap()
    {
        switch(currentStage)
        {
            case StageType.Stage1:
                _maxRoomInMap = 7;
                break;
            case StageType.Stage2:
                _maxRoomInMap = 10;
                break;
            case StageType.Stage3:
                _maxRoomInMap = 15;
                break;
            case StageType.testStage:
                _maxRoomInMap = 200;
                break;
        }
    }

    bool CanPlaceRoom(Vector2Int pos, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int checkPos = pos + new Vector2Int(x, y);
                if (roomGrid.ContainsKey(checkPos)) return false;
            }
        }
        return true;
    }
    
    void CreateRoom()
    {
        int createCount = 0;
        Vector2Int currentPos = Vector2Int.zero;
        
        GameObject firstMap = mapPrefabs[0];
        Vector2Int firstMapSize = firstMap.GetComponent<BaseRoom>().roomSize;
        
        OccupyGrid(currentPos, firstMapSize, firstMap);
        roomPos.Add(currentPos);

        int loopSafe = 0;
        while (createCount < _maxRoomInMap && loopSafe < 100)
        {
            loopSafe++;
            
            currentPos = roomPos[UnityEngine.Random.Range(0, roomPos.Count)];
            Vector2Int nextRoomPos = GetNeighborRoom(currentPos);
            
            int randomIndex = UnityEngine.Random.Range(1, mapPrefabs.Length);
            GameObject selectedPrefab = mapPrefabs[randomIndex];
            Vector2Int roomSize = selectedPrefab.GetComponent<BaseRoom>().roomSize;

            if (CanPlaceRoom(nextRoomPos, roomSize))
            {
                OccupyGrid(nextRoomPos, roomSize, selectedPrefab);
                roomPos.Add(nextRoomPos);
                createCount++;
            }
        }
        Vector2Int GetNeighborRoom(Vector2Int pos)
        {
            Vector2Int[] directions = 
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left, 
                Vector2Int.right, 
            };
            
            return pos + directions[UnityEngine.Random.Range(0, directions.Length)];
        }

        void OccupyGrid(Vector2Int pos, Vector2Int size, GameObject map)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int checkPos = pos + new Vector2Int(x, y);
                    if (!roomGrid.ContainsKey(checkPos))
                    {
                        roomGrid.Add(checkPos, map);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (roomGrid == null) return;
        foreach (var pair in roomGrid)
        {
            Vector2Int pos = pair.Key;
            Vector3 worldPos = new Vector3(pos.x * roomWidth, pos.y * roomHeight, 0);
            
            Gizmos.color = Color.yellowNice;
            Gizmos.DrawWireCube(worldPos, new Vector3(roomWidth, roomHeight, .1f));
            
        }
    }
}

public enum StageType
{
    Stage1,
    Stage2,
    Stage3,
    testStage
}
