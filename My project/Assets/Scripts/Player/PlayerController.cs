using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("플레이어 체력")]
    [SerializeField] public float playerMaxHP;
    public float playerCurrentHP;

    [Header("플레이어 데미지")] 
    [SerializeField] public float playerDamage;

    [Header("공격 쿨타임")] 
    [SerializeField] public float _attackCD;
    
    [Header("총구 위치")]
    [SerializeField] private GameObject _muzzlePoint;
    [Header("총알 프리펩")]
    [SerializeField] private GameObject _playerBulletPrefab;
    [SerializeField] private PlayerBulletManager _bulletManagerPrefab;

    private WaitForSeconds AttackCD;
    // private PlayerActionInput _input;
    public bool CanAttack;
    public bool IsDead;
    
    private void Awake()
    {
        Init();
    }

    /* private void OnEnable()
    {
        _input.PlayerAction.Enable();
        _input.PlayerAction.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        _input.PlayerAction.Attack.performed -= OnAttack;
        _input.PlayerAction.Disable();
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log("공격시도");
        Attack();
    }
    */

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("공격시도");
        if (!CanAttack) return;
        
        Vector3 bulletPos = GetPos();
        Quaternion bulletRot = GetRot(); 
        PlayerBulletManager.Instance.ShootBullet(bulletPos, bulletRot);
        StartCoroutine(AttackCDCoroutine());
        CanAttack = false;
    }
    
    Vector3 GetPos()
    {
        return _muzzlePoint.transform.position;
    }
    
    Quaternion GetRot()
    {
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - GetPos()).normalized;
        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        return Quaternion.Euler(0f, 0f, angle);
    }
    
    private void Init()
    {
        // _input = new PlayerActionInput();
        playerCurrentHP = playerMaxHP;
        CanAttack = true;

        if (PlayerBulletManager.Instance == null)
        {
            Instantiate(_bulletManagerPrefab);
        }
    }

    public void TakeDamage(float damage)
    {
        playerCurrentHP -= damage;

        if (playerCurrentHP <= 0) Die();
    }
    
    IEnumerator AttackCDCoroutine()
    {
        yield return new WaitForSeconds(_attackCD);
        CanAttack = true;
    }

    private void Die()
    {
        if(IsDead) return;
        
        IsDead = true;
    }
}
