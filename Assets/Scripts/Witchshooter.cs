using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witchshooter : MonoBehaviour
{
    public GameObject LovePrefabs;
    public Transform firePoint; // �Ѿ��� ������ ��ġ

    [Header("���� Ÿ�̹� ����")]
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 0.5f;

    public float bulletSpeed = 5f;

    private float timer = 0.0f;
    private float nextSpawnTime;

    public float bulletLifetime = 3f;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            timer = 0.0f;
            SetNextSpawnTime();
            Shoot(); // �Ѿ� �߻�
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(LovePrefabs, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.right * bulletSpeed; // ������ �������� �߻�
        }
        Destroy(bullet, bulletLifetime);
    }
}