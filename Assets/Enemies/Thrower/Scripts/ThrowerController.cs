using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class ThrowerController : Enemy
{
    [Header("General Settings")]
    [SerializeField]private Transform shotRoot;
    [SerializeField]private Transform reloadPoint;
    [SerializeField]private Transform armSolverTarget;
    [SerializeField]private GameObject projectilePrefab;

    [Header("Attack Settings")]
    [SerializeField]private float reloadTime;
    [SerializeField]private float attackTime;
    [SerializeField]private float distanceToAttack;
    [SerializeField]private float distanceToEscape;
    [SerializeField]private float attackForce;
    [SerializeField]private float speed;

    private Transform body;
    private Transform target;
    private Transform actualMovePoint;
    private Transform[] movePoints = new Transform[2];
    private float distanceBetweenTarget;
    private Animator animator;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;  
        movePoints[0] = transform.Find("StartPoint");
        movePoints[1] = transform.Find("EndPoint");
        body = transform.Find("Body");

        actualMovePoint = movePoints[0];
        animator = body.GetComponent<Animator>();
    }

    void Update()
    {   
        distanceBetweenTarget = Vector2.Distance(body.position, target.position);
        Debug.Log(distanceBetweenTarget);
        
        if(GetStatus() != EnemyStatus.Attacking && distanceBetweenTarget > distanceToAttack)
        {
            StartCoroutine(GetDistanceToAttack());    
        }
        else  if(GetStatus() != EnemyStatus.Attacking && distanceBetweenTarget < distanceToEscape)
        {
            StartCoroutine(Escape());    
        }
        else if(GetStatus() == EnemyStatus.Idle && distanceBetweenTarget < distanceToAttack)
        {
            if(Physics2D.Linecast(shotRoot.Find("ShotPoint").position, target.transform.position))
            {
                StartCoroutine(Attack());
            } 
        }
    }

    void LookAtPoint(Transform point) 
    {
        float distance = body.position.x - point.position.x;
        body.rotation = distance < 0 ? new Quaternion(0, 0, 0, 0) : new Quaternion(0, 180, 0, 0);
    }

    IEnumerator GetDistanceToAttack()
    {
        SetStatus(EnemyStatus.Moving);
        animator.SetBool("isGettingDistanceToAttack", true);

        Rigidbody2D rb;
        rb = body.GetComponent<Rigidbody2D>();

        actualMovePoint = Vector2.Distance(movePoints[0].position, target.position) < Vector2.Distance(movePoints[1].position, target.position) ? movePoints[0] : movePoints[1];

        while(Vector2.Distance(actualMovePoint.position, body.position) > 5f)
        {
            rb.MovePosition(body.position + (actualMovePoint.position - body.position).normalized * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame ();

            LookAtPoint(actualMovePoint);
            
            if(distanceBetweenTarget > distanceToAttack - 5f)
                break;
        }

        animator.SetBool("isGettingDistanceToAttack", false);
        SetStatus(EnemyStatus.Idle);
    }

    IEnumerator Escape()
    {
        SetStatus(EnemyStatus.Moving);
        animator.SetBool("isEscaping", true);

        Rigidbody2D rb;
        rb = body.GetComponent<Rigidbody2D>();

        actualMovePoint = Vector2.Distance(movePoints[0].position, target.position) < Vector2.Distance(movePoints[1].position, target.position) ? movePoints[1] : movePoints[0];

        while(Vector2.Distance(actualMovePoint.position, body.position) > 5f)
        {
            rb.MovePosition(body.position + (actualMovePoint.position - body.position).normalized * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame ();

            LookAtPoint(target);
            
            if(distanceBetweenTarget > distanceToAttack - 5f)
                break;
        }

        animator.SetBool("isEscaping", false);
        SetStatus(EnemyStatus.Idle);
    }

    IEnumerator Attack()
    {
        SetStatus(EnemyStatus.Attacking);

        float startTime = 0;
        Vector3 center = new Vector3(0, 0, 0);
        LimbSolver2D limbSolver = armSolverTarget.GetComponent<LimbSolver2D>();

        // Reload
        limbSolver.weight = 1;
        startTime = Time.time;
        center = ((armSolverTarget.position + reloadPoint.position) * 0.5f) - new Vector3(0, 1, 0);

        while(armSolverTarget.position != reloadPoint.position)
        {
            Vector3 targetCenter = armSolverTarget.position - center;
            Vector3 reloadCenter = reloadPoint.position - center;
            float fracComplete = (Time.time - startTime) / reloadTime;

            armSolverTarget.position = Vector3.Slerp(targetCenter, reloadCenter, fracComplete * Time.deltaTime);
            armSolverTarget.position += center;

            yield return new WaitForEndOfFrame ();

            // Look to player
            LookAtPoint(target);
        }

        //Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, armSolverTarget.position, new Quaternion(0, 0, 0, 0));
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        projectileRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        // Set shot direction
        Vector2 direction = target.position - shotRoot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotRoot.eulerAngles = rotation.eulerAngles;

        // Shot
        startTime = Time.time;
        center = ((armSolverTarget.position + reloadPoint.position) * 0.5f) - new Vector3(0, 1, 0);
        while(armSolverTarget.position != shotRoot.Find("ShotPoint").position)
        {
            // Move arms
            Vector3 targetCenter = armSolverTarget.position - center;
            Vector3 shootCenter = shotRoot.Find("ShotPoint").position - center;
            float fracComplete = (Time.time - startTime) / attackTime;

            armSolverTarget.position = Vector3.Slerp(targetCenter, shootCenter, fracComplete * Time.deltaTime);
            armSolverTarget.position += center;
            yield return new WaitForEndOfFrame ();

            // Move projectile
            projectile.transform.position = armSolverTarget.position;
        }

        // Set shoot velocity
        projectileRigidbody.constraints = RigidbodyConstraints2D.None;
        projectileRigidbody.velocity = (armSolverTarget.position - shotRoot.position).normalized * attackForce;
        projectile.GetComponent<EnemyProjectile>().isActive = true;

        limbSolver.weight = 0;
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
