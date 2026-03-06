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

    private Coroutine _attackingCoroutine;
    private WaitForSeconds AttackCD;
    private PlayerActionInput _input;
    public bool CanAttack;
    public bool IsDead;
    
    private void Awake()
    {
        Init();
    }

    private void OnEnable() 
    {
        _input.PlayerAction.Enable();
        _input.PlayerAction.Attack.started += OnAttackStart;
        _input.PlayerAction.Attack.canceled += OnAttackCancel;
    }

    private void OnDisable()
    {
        _input.PlayerAction.Attack.started -= OnAttackStart;
        _input.PlayerAction.Attack.canceled -= OnAttackCancel;
        _input.PlayerAction.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Portal"))
        {
            SceneManager.Instance.CheckCurrentScene();
        }
    }

    private void OnAttackStart(InputAction.CallbackContext ctx)
    {
        if (_attackingCoroutine != null) StopCoroutine(_attackingCoroutine);

        _attackingCoroutine = StartCoroutine(AttackingCoroutine());
    }

    void OnAttackCancel(InputAction.CallbackContext ctx)
    {
        if(_attackingCoroutine != null) StopCoroutine(_attackingCoroutine);
    }
    

    private void Attack()
    {
        Vector3 bulletPos = GetPos();
        Quaternion bulletRot = GetRot(); 
        PlayerBulletManager.Instance.ShootBullet(bulletPos, bulletRot);
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
        _input = new PlayerActionInput();
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
    
    IEnumerator AttackingCoroutine()
    {
        while (true)
        {
            Attack();
            
            CanAttack = false;
            yield return new WaitForSeconds(_attackCD);
            CanAttack = true;
        }
    }

    private void Die()
    {
        if(IsDead) return;
        
        IsDead = true;
    }
}
