using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    public LayerMask doorLayerMask;

    public void CheckNeighborRoom()
    {
        Vector3 doorWorldPos = transform.position;
        Vector3 direction = transform.up;
        Vector3 check = doorWorldPos + direction;
        
        Collider2D hit = Physics2D.OverlapCircle(check, 0.2f, doorLayerMask);

        if (hit != null)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoorIsRoomCleared()
    {
        CheckNeighborRoom();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Vector3 checkPoint = transform.position + transform.up;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, checkPoint);
        Gizmos.DrawWireSphere(checkPoint, 0.5f);
    }

    public void OpenDoor()
    {
        if(door != null) door.SetActive(false);
    }

    public void CloseDoor()
    {
        if(door != null) door.SetActive(true);
    }
}
