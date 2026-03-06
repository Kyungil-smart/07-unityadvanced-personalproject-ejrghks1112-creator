using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class BaseRoom : MonoBehaviour
{
    public Vector2Int roomSize = new Vector2Int(2, 2);
    [SerializeField] public bool isTreasureRoom;
    [SerializeField] public bool isBossRoom;
    [SerializeField] List<Door> doorU  = new List<Door>();
    [SerializeField] List<Door> doorD  = new List<Door>();
    [SerializeField] List<Door> doorL  = new List<Door>();
    [SerializeField] List<Door> doorR = new List<Door>();
    private MonsterManager _monster;
    [SerializeField] private Portal _portal;
    [SerializeField] public CinemachineCamera _roomCamera;

    private void Awake()
    {
       Init();
    }

    void Init()
    {
        _monster = GetComponent<MonsterManager>();
    }

    public bool _isClear = false;
    bool _isPlayerInRoom = false;

    public void OnPlayerEnter()
    {
        if (!_isClear && !isTreasureRoom)
        {
            CloseAllDoor();
            MonsterManager.Instance.SpawnMob(this);
        }
    }

    public void RoomClear()
    {
        _isClear = true;
        if(isBossRoom == true && _portal != null) _portal.gameObject.SetActive(true);
        OpenDoor();
    }

    public void CloseAllDoor()
    {
        foreach (Door door in GetAllDoor())
        {
            if (door != null && door._isOpen)
            {
                door.CloseDoor();
            }
        }
    }

    public void OpenDoor()
    {
        foreach (Door door in GetAllDoor())
        {
            if (door != null && door._isOpen) door.OpenDoor();
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
            _roomCamera.Priority = 11;
            _roomCamera.Lens.OrthographicSize = 14f;
            _roomCamera.Target.TrackingTarget = other.transform;
            
            _isPlayerInRoom = true;
            OnPlayerEnter();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _roomCamera.Priority = 10;
        }
    }
}
