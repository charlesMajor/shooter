using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletPoolSize = 40;

    [SerializeField] private float missileSpeed = 75f;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private int missilePoolSize = 5;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private PlayerController player;

    private float lastBulletShotTime = 0f;
    private float bulletShootCooldown = 0.1f;
    private float lastMissileShotTime = 0f;
    private float missileShootCooldown = 0.7f;

    private ObjectPool bulletPool;
    private ObjectPool missilePool;
    private GameManager gameManager;

    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip tripleShoot;
    [SerializeField] private AudioClip missileSound;
    private AudioSource audio;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        bulletPool = new ObjectPool(bulletPrefab, bulletPoolSize);
        missilePool = new ObjectPool(missilePrefab, missilePoolSize);

        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandlePlayerShoot();
    }

    private void HandlePlayerShoot()
    {
        lastBulletShotTime += Time.deltaTime;
        lastMissileShotTime += Time.deltaTime;

        bool shootingBullet = Input.GetButton("Fire1");
        bool shootingMissile = Input.GetButton("Fire2");
        if (gameManager.IsGamepadPresent())
        {
            shootingBullet = Input.GetButton("Fire1 Gamepad");
            shootingMissile = Input.GetButton("Fire2 Gamepad");
        }

        if (shootingBullet)
            shootingMissile = false;

        if (shootingBullet && lastBulletShotTime >= bulletShootCooldown && lastMissileShotTime >= missileShootCooldown)
        {
            ShootBullet();
            lastBulletShotTime = 0f;
        }
        else if (shootingMissile && lastMissileShotTime >= missileShootCooldown && player.CanShootMissile())
        {
            ShootMissile();
            lastMissileShotTime = 0f;
        }
    }

    private void ShootBullet()
    {
        GetAvaibleBulletRigidbody().velocity = bulletSpeed * transform.root.forward;

        if (player.IsBonusActive())
        {
            GetAvaibleBulletRigidbody().velocity = bulletSpeed * (transform.forward * 0.5f + transform.right * 0.5f);
            GetAvaibleBulletRigidbody().velocity = bulletSpeed * (transform.forward * 0.5f - transform.right * 0.5f);
            audio.PlayOneShot(tripleShoot);
        }
        else
        {
            audio.PlayOneShot(shoot);
        }
    }

    private void ShootMissile()
    {
        audio.PlayOneShot(missileSound);
        GetAvaibleMissileRigidbody().velocity = missileSpeed * transform.root.forward;
        player.ShootMissile();
    }

    private Rigidbody GetAvaibleBulletRigidbody()
    {
        GameObject bullet = bulletPool.GetObjectFromPool(projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        return bullet.GetComponent<Rigidbody>();
    }

    private Rigidbody GetAvaibleMissileRigidbody()
    {
        GameObject missile = missilePool.GetObjectFromPool(projectileSpawnPoint.position, transform.rotation);
        return missile.GetComponent<Rigidbody>();
    }
}