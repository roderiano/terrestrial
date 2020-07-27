using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]private GameObject bulletPrefab = null;
    [SerializeField]private Transform shotPoint = null;
    [SerializeField]private Transform particleShot = null;

    void Start()
    {
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shot();
        }
    }

    void Shot() 
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = (shotPoint.position - transform.position).normalized * 100f;

        Instantiate(particleShot, shotPoint.position, new Quaternion(0, 0, 0, 0));
    }
}
