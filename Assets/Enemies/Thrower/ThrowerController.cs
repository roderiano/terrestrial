using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : Enemy
{
    [SerializeField]private Transform reloadPoint;
    [SerializeField]private Transform shotRoot;
    [SerializeField]private Transform leftArmSolverTarget;

    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    // Update is called once per frame
    void Update()
    {   
        if(GetStatus() == EnemyStatus.Idle)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(reloadPoint.position, 1f);  
        Gizmos.DrawWireSphere(shotRoot.Find("ShotPoint").position, 1f);  
    }

    IEnumerator Attack()
    {
        SetStatus(EnemyStatus.Attacking);

        float startTime = Time.time;
        float attackTime = 0.1f;
        Vector3 center = ((leftArmSolverTarget.position + reloadPoint.position) * 0.5f) - new Vector3(0, 1, 0);

        // Prepare
        while(leftArmSolverTarget.position != reloadPoint.position)
        {
            Vector3 targetCenter = leftArmSolverTarget.position - center;
            Vector3 reloadCenter = reloadPoint.position - center;
            float fracComplete = (Time.time - startTime) / attackTime;

            leftArmSolverTarget.position = Vector3.Slerp(targetCenter, reloadCenter, fracComplete * Time.deltaTime);
            leftArmSolverTarget.position += center;
            yield return new WaitForEndOfFrame ();
        }

        // Set shot direction
        Vector2 direction = target.position - shotRoot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotRoot.eulerAngles = rotation.eulerAngles - new Vector3(0f, 0f, 180f);

        // Shot
        while(leftArmSolverTarget.position != shotRoot.Find("ShotPoint").position)
        {
            Vector3 targetCenter = leftArmSolverTarget.position - center;
            Vector3 shootCenter = shotRoot.Find("ShotPoint").position - center;
            float fracComplete = (Time.time - startTime) / attackTime;

            leftArmSolverTarget.position = Vector3.Slerp(targetCenter, shootCenter, fracComplete * Time.deltaTime);
            leftArmSolverTarget.position += center;
            yield return new WaitForEndOfFrame ();
        }

        SetStatus(EnemyStatus.Idle);
    }
}
