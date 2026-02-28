using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseRoom : MonoBehaviour
{
    public Vector2Int roomSize = new Vector2Int(2, 2);
    [SerializeField] public bool isTreasureRoom = false;
    [SerializeField] List<Door> doorU  = new List<Door>();
    [SerializeField] List<Door> doorD  = new List<Door>();
    [SerializeField] List<Door> doorL  = new List<Door>();
    [SerializeField] List<Door> doorR = new List<Door>();
    private MonsterManager _monster;
    private Door _door;

    private void Awake()
    {
        _monster = GetComponent<MonsterManager>();
        _door = GetComponent<Door>();
    }

    public bool _isClear = false;
    bool _isPlayerInRoom = false;

    public void OnPlayerEnter()
    {
        if (!_isClear && _isPlayerInRoom && !isTreasureRoom)
        {
            Debug.Log("몹 스폰 개시");
            CloseAllDoor();
            _monster.SpawnMob();
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            RoomClear();
        }
    }
    
    public void RoomClear()
    {
        _isClear = true;
        _door.OpenDoorIsRoomCleared();
    }

    public void CloseAllDoor()
    {
        GetAllDoor().ForEach(door => door.CloseDoor());
    }

    public void OpenAllDoor()
    {
        foreach (Door door in GetAllDoor())
        {
            if (door != null && door.gameObject.activeSelf) door.OpenDoor();
        }
    }

    List<Door> GetAllDoor()
    {
        List<Door> doors = new List<Door>();
        if(doorU != null) doors.AddRange(doorU);
        if(doorD != null) doors.AddRange(doorD);
        if(doorL != null) doors.AddRange(doorL);
        if(doorR != null) doors.AddRange(doorR);
        return doors;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 입장");
            _isPlayerInRoom = true;
            OnPlayerEnter();
        }
    }
}
