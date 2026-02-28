using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : MonoBehaviour
{
    public Vector2Int roomSize = new Vector2Int(2, 2);
    [SerializeField] public bool isTreasureRoom = false;
    [SerializeField] List<Door> doorU  = new List<Door>();
    [SerializeField] List<Door> doorD  = new List<Door>();
    [SerializeField] List<Door> doorL  = new List<Door>();
    [SerializeField] List<Door> doorR = new List<Door>();

    private bool _isClear = false;
    bool _isPlayerInRoom = false;

    public void OnPlayerEnter()
    {
        if (!_isClear && _isPlayerInRoom)
        {
            CloseAllDoor();
            //적 스폰
        }
    }

    public void RoomClear()
    {
        _isClear = true;
        OpenAllDoor();
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
        if(other.CompareTag("Player"))
        {
            _isPlayerInRoom = true;
        }
            
    }

    public void SetDoor(bool up, bool down, bool left, bool right)
    {
        if (!up) FakeDoor(doorU);
        if (!down) FakeDoor(doorD);
        if (!left) FakeDoor(doorL);
        if (!right) FakeDoor(doorR);
    }

    void FakeDoor(List<Door> doors)
    {
        foreach (Door door in doors)
        {
            if(door != null) door.gameObject.SetActive(true);
        }
        doors.Clear();
    }
}
