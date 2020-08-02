using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    
    private float startPos;
    public float parallaxEffect;
    private Camera mainCam;

    void Start()
    {
        startPos = transform.position.x;
        mainCam = Camera.main;
    }

    void FixedUpdate()
    {
        float dist = (mainCam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
