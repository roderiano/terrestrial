using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : Enemy
{
    [Header("General Settings")]
    [SerializeField]private Transform shotRoot;
    [SerializeField]private Transform reloadPoint;
    [SerializeField]private Transform leftArmSolverTarget;
    [SerializeField]private GameObject projectilePrefab;

    [Header("Attack Settings")]
    [SerializeField]private float reloadTime;
    [SerializeField]private float attackTime;
    [SerializeField]private float attackDistance;

    [Header("Move Settings")]
    [SerializeField]private float speed;
    [SerializeField]private float distanceToEscape;

    private Transform body;
    private Transform target;
    private Transform actualMovePoint;
    private Transform[] movePoints = new Transform[2];

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;  
        movePoints[0] = transform.Find("StartPoint");
        movePoints[1] = transform.Find("EndPoint");
        body = transform.Find("Body");

        actualMovePoint = movePoints[0];
    }

    void Update()
    {   
        float distanceBetweenTarget = Vector2.Distance(body.position, target.position);
        
        if(GetStatus() == EnemyStatus.Idle)
        {
            if(distanceBetweenTarget < distanceToEscape)
            {
                StartCoroutine(Escape());
            }
            else if(Physics2D.Linecast(shotRoot.Find("ShotPoint").position, target.transform.position) && distanceBetweenTarget < attackDistance)
            {
                LookTarget();
                StartCoroutine(Attack());
            }
        }
        
    }

    void LookTarget() 
    {
        float distance = body.position.x - target.position.x;
        body.rotation = distance < 0 ? new Quaternion(0, 180, 0, 0) : new Quaternion(0, 0, 0, 0);
    }

    IEnumerator Escape()
    {
        SetStatus(EnemyStatus.Moving);

        Rigidbody2D rb;
        rb = body.GetComponent<Rigidbody2D>();

        actualMovePoint = actualMovePoint == movePoints[0] ? movePoints[1] : movePoints[0];
        while(Vector2.Distance(actualMovePoint.position, body.position) > 10f)
        {
            rb.MovePosition(body.position + (actualMovePoint.position - body.position).normalized * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame ();
        }

        SetStatus(EnemyStatus.Idle);
    }

    IEnumerator Attack()
    {
        SetStatus(EnemyStatus.Attacking);

        float startTime = 0;
        Vector3 center = new Vector3(0, 0, 0);

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

        //Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, leftArmSolverTarget.position, new Quaternion(0, 0, 0, 0));
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        projectileRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        // Wait a time
        yield return new WaitForSeconds(1.5f);

        // Set shot direction
        Vector2 direction = target.position - shotRoot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotRoot.eulerAngles = rotation.eulerAngles - new Vector3(0f, 0f, 180f);

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

        // Set shoot velocity
        projectileRigidbody.constraints = RigidbodyConstraints2D.None;
        projectileRigidbody.velocity = (leftArmSolverTarget.position - shotRoot.position).normalized * 50f;
        projectile.GetComponent<EnemyProjectile>().isActive = true;

        SetStatus(EnemyStatus.Idle);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        if(movePoints[0] != null && movePoints[1] != null)
        {
            Gizmos.DrawLine(movePoints[0].position, movePoints[1].position);
        }

        
        if(reloadPoint != null && shotRoot != null)
        {
            Gizmos.DrawWireSphere(reloadPoint.position, 0.5f);  
            Gizmos.DrawWireSphere(shotRoot.Find("ShotPoint").position, 0.5f);  
        }
        
    }
}
