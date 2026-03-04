using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IShootable
{
    
    [Header("총알 속도")]
    [SerializeField] private float _bulletSpeed;
    [Header("총알 유지 시간")]
    [SerializeField] private float _bulletLifeTime;
    [Header("총알 프리펩")]
    [SerializeField] GameObject _playerBulletPrefab;
    
    private float _bulletDamage;
    private Rigidbody2D _rb;
    private PlayerController _playerController;
    private WaitForSeconds bulletLifetime;
    [SerializeField]private float _bulletDontBreakTime;
    
    private void Awake()
    {
       Init();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time - _bulletDontBreakTime < 0.05f) return;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerBulletManager.Instance.DespawnBullet(this);
            
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if(damagable != null) damagable.TakeDamage(_bulletDamage);
        }
        
        if(other.CompareTag("Wall"))
        {
            Debug.Log("벽에 부딛힘");
            PlayerBulletManager.Instance.DespawnBullet(this);
        }
    }

    private void Init()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        bulletLifetime = new WaitForSeconds(_bulletLifeTime);
    }

    public void OnSpawn()
    {
        _bulletDontBreakTime = Time.time;
        
        _bulletDamage = _playerController.playerDamage;
        _rb.linearVelocity = transform.right * _bulletSpeed;
        
        StartCoroutine(bulletCoroutine());
    }

    public void OnDespawn()
    {
        _rb.linearVelocity = Vector3.zero;
        StopCoroutine(bulletCoroutine());
    }

    IEnumerator bulletCoroutine()
    {
        yield return bulletLifetime;
        PlayerBulletManager.Instance.DespawnBullet(this);
    }
}
