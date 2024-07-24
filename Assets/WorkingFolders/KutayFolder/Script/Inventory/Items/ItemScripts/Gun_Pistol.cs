using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun_Pistol : ItemScript
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public override void Fire()
    {
        base.Fire();
        firePoint = GameObject.FindWithTag("FirePoint").transform;
        Debug.Log("Firing pistol");
        Shoot();
    }
    
    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
