using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsController : MonoBehaviour
{
    [SerializeField]private Transform spineTargetRoot = null; 
    
    private PlayerController playerController;

    private Transform riggedPlayer;

    void Start()
    {
        riggedPlayer = transform.Find("riggedPlayer");
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if(playerController.GetStatus() == PlayerStatus.Moving) 
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            spineTargetRoot.eulerAngles = riggedPlayer.rotation.y == 1 ? rotation.eulerAngles : rotation.eulerAngles - new Vector3(0f, 0f, 180f);
        }
    }
}
