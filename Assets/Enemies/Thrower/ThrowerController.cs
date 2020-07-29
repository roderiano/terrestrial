using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : Enemy
{
    [SerializeField]private Transform reloadPoint;
    [SerializeField]private Transform shotRoot;
    [SerializeField]private Transform leftArmSolverTarget;
    [SerializeField]private GameObject projectilePrefab;
    [SerializeField]private float reloadTime, attackTime;

    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    void Update()
    {   
        if(GetStatus() == EnemyStatus.Idle)
        {
            StartCoroutine(Attack());
        }

        LookTarget();
    }

    void LookTarget() 
    {
        float distance = transform.position.x - target.position.x;
        transform.Find("Sprite").rotation = distance < 0 ? new Quaternion(0, 180, 0, 0) : new Quaternion(0, 0, 0, 0);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(reloadPoint.position, 1f);  
        Gizmos.DrawWireSphere(shotRoot.Find("ShotPoint").position, 1f);  
    }

    IEnumerator Attack()
    {
        float startTime = 0;
        Vector3 center = new Vector3(0, 0, 0);

        SetStatus(EnemyStatus.Attacking);


        // Reload
        startTime = Time.time;
        center = ((leftArmSolverTarget.position + reloadPoint.position) * 0.5f) - new Vector3(0, 1, 0);

        while(leftArmSolverTarget.position != reloadPoint.position)
        {
            Vector3 targetCenter = leftArmSolverTarget.position - center;
            Vector3 reloadCenter = reloadPoint.position - center;
            float fracComplete = (Time.time - startTime) / reloadTime;

            leftArmSolverTarget.position = Vector3.Slerp(targetCenter, reloadCenter, fracComplete * Time.deltaTime);
            leftArmSolverTarget.position += center;
            yield return new WaitForEndOfFrame ();
        }

        // Set shot direction
        Vector2 direction = target.position - shotRoot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotRoot.eulerAngles = rotation.eulerAngles - new Vector3(0f, 0f, 180f);

        //Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, leftArmSolverTarget.position, new Quaternion(0, 0, 0, 0));
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        projectileRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        // Shot
        startTime = Time.time;
        center = ((leftArmSolverTarget.position + reloadPoint.position) * 0.5f) - new Vector3(0, 1, 0);

        while(leftArmSolverTarget.position != shotRoot.Find("ShotPoint").position)
        {
            // Move arms
            Vector3 targetCenter = leftArmSolverTarget.position - center;
            Vector3 shootCenter = shotRoot.Find("ShotPoint").position - center;
            float fracComplete = (Time.time - startTime) / attackTime;

            leftArmSolverTarget.position = Vector3.Slerp(targetCenter, shootCenter, fracComplete * Time.deltaTime);
            leftArmSolverTarget.position += center;
            yield return new WaitForEndOfFrame ();

            // Move projectile
            projectile.transform.position = leftArmSolverTarget.position;
        }

        projectileRigidbody.constraints = RigidbodyConstraints2D.None;
        projectileRigidbody.velocity = (leftArmSolverTarget.position - shotRoot.position).normalized * 50f;

        SetStatus(EnemyStatus.Idle);
    }
}
