using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] public GameObject[] mapPrefabs;
    [SerializeField] private GameObject bossRoomPrefab;
    [SerializeField] private GameObject treasureRoomPrefab;

    private float roomWidth = 17f;
    private float roomHeight = 13f;

    private int _maxRoomInMap = 5;
    public StageType currentStage;
    private List<Vector2Int> roomPos;
    private Dictionary<Vector2Int, GameObject> roomGrid;
    Vector2Int bossRoomPos = new Vector2Int(-100, -100);
    Vector2Int treasurePos = new Vector2Int(-100, -100);
    private List<Vector2Int> endPos;

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
        endPos = new List<Vector2Int>();
        CreateRoom();
        CreateBossRoom();
        CreateTreasureRoom();
    }

    void DrawMap()
    {
        foreach (var pos in roomPos)
        {
            GameObject room = null;
            if (pos == Vector2Int.zero) room = mapPrefabs[0];
            else if (pos == bossRoomPos) room = bossRoomPrefab;
            else if (pos == treasurePos) room = treasureRoomPrefab;
            else
            {
                if (roomGrid.TryGetValue(pos, out GameObject map)) room = map;
            }

            if (room != null)
            {
                Vector2 spawnPos = new Vector2(pos.x * roomWidth, pos.y * roomHeight);
                Instantiate(room, spawnPos, Quaternion.identity);
            }
        }
    }

    void SetMaxRoomInMap()
    {
        switch (currentStage)
        {
            case StageType.Stage1:
                _maxRoomInMap = 9;
                break;
            case StageType.Stage2:
                _maxRoomInMap = 12;
                break;
            case StageType.Stage3:
                _maxRoomInMap = 18;
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
        while (createCount < _maxRoomInMap && loopSafe < 300)
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
    }

    Vector2Int GetNeighborRoom(Vector2Int pos)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, };

            return pos + directions[UnityEngine.Random.Range(0, directions.Length)];
    }

    void OccupyGrid(Vector2Int pos, Vector2Int size, GameObject map)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                    Vector2Int checkPos = pos + new Vector2Int(x, y);
                    if (!roomGrid.ContainsKey(checkPos)) roomGrid.Add(checkPos, map); 
            }
        }
    }


    void CreateBossRoom()
    {
        float maxDistance = 0f;

        foreach (var pos in roomPos)
        { 
            if (pos == Vector2Int.zero) continue;

            float distance = Mathf.Abs(pos.x) + Mathf.Abs(pos.y);
            if (distance > maxDistance) 
            {
                    maxDistance = distance;
                    bossRoomPos = pos;
            }
        }

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, };
        Vector2Int bossRoomSize = bossRoomPrefab.GetComponent<BaseRoom>().roomSize;

        foreach (var dir in directions)
        { 
            Vector2Int checkPos = bossRoomPos + dir;
            if (CanPlaceRoom(checkPos, bossRoomSize))
            {
                    bossRoomPos = checkPos;
                    roomPos.Add(bossRoomPos);
                    OccupyGrid(bossRoomPos, bossRoomSize, bossRoomPrefab);
                    return;
            }
        }
    }

    void CreateTreasureRoom()
    {
        endPos.Clear();

        foreach (var pos in roomPos)
        {
            if (pos == Vector2Int.zero) continue;
            if (pos == bossRoomPos) continue;

            int neighbor = 0;
            Vector2Int[] dir = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (var direction in dir)
            {
                if (roomGrid.ContainsKey(pos + direction))
                {
                    neighbor++;
                }
            }

            if (neighbor == 1)
            {
                endPos.Add(pos);
            }
        }

        if (endPos.Count > 0)
        {
            treasurePos = endPos[UnityEngine.Random.Range(0, endPos.Count)];
        }
        else if (endPos.Count == 0)
        {
            foreach (var pos in roomPos)
            {
                if (pos == Vector2Int.zero || pos == bossRoomPos) continue;

                int neighbor = 0;
                Vector2Int[] dir = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                foreach (var d in dir)
                {
                    if (roomGrid.ContainsKey(pos + d))
                        neighbor++;

                    if (neighbor == 2) endPos.Add(pos);
                }

                if (endPos.Count > 0)
                {
                    treasurePos = endPos[UnityEngine.Random.Range(0, endPos.Count)];
                }
                else
                {
                    treasurePos = roomPos.Find(p => p != Vector2Int.zero && p != bossRoomPos);
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

                if (pos == treasurePos)
                {
                    Gizmos.color = Color.red;
                }
                else if (pos == bossRoomPos)
                {
                    Gizmos.color = Color.cadetBlue;
                }
                else
                {
                    Gizmos.color = Color.yellowNice;
                }

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
