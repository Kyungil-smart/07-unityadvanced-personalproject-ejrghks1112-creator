using System;
using UnityEngine;
using Random = System.Random;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _monsterPrefabs;
    [SerializeField] private int minMobSpawn = 4;
    [SerializeField] private int maxMobSpawn = 8;
    BaseRoom _baseRoom;


    public void Awake()
    {
        Init();
    }

    void Init()
    {
        _baseRoom = GetComponent<BaseRoom>();
    }

    public void SpawnMob()
    {
        if (_baseRoom._isClear) return;
        if (_baseRoom.isTreasureRoom) return;  
        
        int mobSpawn = UnityEngine.Random.Range(minMobSpawn, maxMobSpawn);

        for (int i = 0; i < mobSpawn; i++)
        { 
            Spawn();
        }
    }

    public void Spawn()
    {
        float randomPosX = UnityEngine.Random.Range(_baseRoom.roomSize.x * 15f, _baseRoom.roomSize.x * 3f);
        float randomPosY = UnityEngine.Random.Range(_baseRoom.roomSize.y * 10f, _baseRoom.roomSize.y * 3f);
        
        Vector3 spawnPos = transform.position + new Vector3(randomPosX, randomPosY, 0);
        
        GameObject randomMobSpawn = _monsterPrefabs[UnityEngine.Random.Range(0, _monsterPrefabs.Length)];
        Instantiate(randomMobSpawn, spawnPos, Quaternion.identity, transform);
    }
}
