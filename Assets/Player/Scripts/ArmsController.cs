using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsController : MonoBehaviour
{
    [SerializeField]private Transform spineTargetRoot; 

    private Transform riggedPlayer;

    void Start()
    {
        riggedPlayer = transform.Find("riggedPlayer");
    }

    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        
        spineTargetRoot.eulerAngles = riggedPlayer.rotation.y == 1 ? rotation.eulerAngles - new Vector3(0, 0, 180) : rotation.eulerAngles;
    }
}
