using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float _mobMoveSpeed;
    [SerializeField] private float _detectRange;
    [SerializeField]private GameObject _player;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (_player == null) return;
        
        float distance = Vector2.Distance(transform.position, _player.transform.position);

        if (distance <= _detectRange)
        {
            Vector2 direction = (_player.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Wall"));

            if (hit.collider == null)
            {
                _rb.linearVelocity = direction * _mobMoveSpeed;
            }

            else
            {
                Debug.Log("벽 감지");
            }
        }
        else _rb.linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
