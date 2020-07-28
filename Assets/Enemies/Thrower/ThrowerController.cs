using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : Enemy
{
    [SerializeField]private Transform reloadPoint;
    [SerializeField]private Transform shotRoot;

    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = target.position - shotRoot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            shotRoot.eulerAngles = rotation.eulerAngles - new Vector3(0f, 0f, 180f);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(reloadPoint.position, 1f);  
        Gizmos.DrawWireSphere(shotRoot.Find("ShotPoint").position, 1f);  
    }
}
