using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    [SerializeField] private GameObject[] _monsterPrefabs;
    [SerializeField] private GameObject[] _bossMonsterPrefabs;
    [SerializeField] private int minMobSpawn = 4;
    [SerializeField] private int maxMobSpawn = 8;
    [SerializeField] private Dictionary<BaseRoom, List<GameObject>> _inRoomMonster = new Dictionary<BaseRoom, List<GameObject>>();


    public void Awake()
    {
        Instance = this;
    }

    public void SpawnMob(BaseRoom room)
    {
        if (room._isClear == true) return;
        if (room.isTreasureRoom == true) return;
        
        if (!_inRoomMonster.ContainsKey(room))
        {
            _inRoomMonster[room] = new List<GameObject>();
        }

        int mobSpawn = 0;
        if (room.isBossRoom == true)     mobSpawn = 1;
        else                             mobSpawn = UnityEngine.Random.Range(minMobSpawn, maxMobSpawn);

        for (int i = 0; i < mobSpawn; i++)
        { 
            Spawn(room);
        }
    }

    public void Spawn(BaseRoom room)
    {
        Vector3 spawnPos = Vector3.zero;
        int loopSafe = 0;
        bool canSpawn = false;

        while (!canSpawn && loopSafe < 10)
        {
            
            float randomPosX = UnityEngine.Random.Range(room.roomSize.x * 15f, room.roomSize.x * 3f);
            float randomPosY = UnityEngine.Random.Range(room.roomSize.y * 10f, room.roomSize.y * 3f);

            if (room.isBossRoom == true)
                spawnPos =
                    room.transform.position +
                    new Vector3
                    (
                        (room.roomSize.x * 18f) / 2,
                        (room.roomSize.y * 13f) / 2,
                        0
                    );
            else spawnPos = room.transform.position + new Vector3(randomPosX, randomPosY, 0);
            
            Collider2D hit = Physics2D.OverlapCircle(spawnPos,0.2f, LayerMask.GetMask("Wall"));
            if (hit == null) canSpawn = true;
            loopSafe++;
        }

        GameObject randomMobSpawn = null;
        
        if(room.isBossRoom == true)    randomMobSpawn = _bossMonsterPrefabs[UnityEngine.Random.Range(0, _bossMonsterPrefabs.Length)];
        else                           randomMobSpawn = _monsterPrefabs[UnityEngine.Random.Range(0, _monsterPrefabs.Length)];
        
        GameObject spawnMob = Instantiate(randomMobSpawn, spawnPos, Quaternion.identity, transform);
        Monster _monster = spawnMob.GetComponent<Monster>();
        
        if (_monster != null) _monster.SetRoom(room);
        
        _inRoomMonster[room].Add(spawnMob);
        Debug.Log($"{spawnMob.name} 생성됨. 리스트 개수: {_inRoomMonster.Count}");
    }
    
    public void CheckLeftMonster(BaseRoom room, GameObject monster)
    {
        if (_inRoomMonster.ContainsKey(room))
        {
            _inRoomMonster[room].Remove(monster);
             Debug.Log($"남은 몹 수 : {_inRoomMonster.Count}");
             
             if (_inRoomMonster[room].Count <= 0)
             {
                 room.RoomClear();
                 _inRoomMonster.Remove(room);
             }
        }
    }
}
