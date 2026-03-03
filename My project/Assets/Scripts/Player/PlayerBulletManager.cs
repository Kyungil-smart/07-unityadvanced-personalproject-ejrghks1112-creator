using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletManager : MonoBehaviour
{
    public static PlayerBulletManager Instance{ get; private set; }
    
    [SerializeField] GameObject _bulletPrefab;

    [SerializeField] private int _defaultSize;
    
    private Stack<PlayerBullet> _pool;
    
    
    void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _pool= new Stack<PlayerBullet>(_defaultSize);
    }

    public void ShootBullet(Vector3 pos, Quaternion rot)
    {
        PlayerBullet bullet;
        
        if (_pool.Count == 0)   bullet = CreateBullet();
        else                    bullet = _pool.Pop();
        
        bullet.gameObject.SetActive(true);
        bullet.transform.position = pos;
        bullet.transform.rotation = rot;
        
        IShootable shootable = bullet as IShootable;
        shootable.OnSpawn();
    }

    public void DespawnBullet(PlayerBullet bullet)
    {
        _pool.Push(bullet);
        bullet.gameObject.SetActive(false);
        
        IShootable shootable = bullet as IShootable;
        shootable.OnDespawn();
    }

    private PlayerBullet CreateBullet()
    {
        GameObject newBullet = Instantiate(_bulletPrefab);
        PlayerBullet bullet = newBullet.GetComponent<PlayerBullet>();
        
        _pool.Push(bullet);
        return bullet;
    }

}
