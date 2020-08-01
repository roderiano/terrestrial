using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public bool isActive; 

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(isActive)
        {
            Destroy(this.gameObject);
        } 
    }
}
