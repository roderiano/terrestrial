using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnectionRefresh : MonoBehaviour
{
    public Material lineMaterial;
    public Skill skill;

    private GameObject rootNode;
    private Vector3[] nodesPoints;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.material = lineMaterial;
        nodesPoints = new Vector3[2];
    }

    void Update()
    {
        nodesPoints[0] = transform.position;
        nodesPoints[1] = rootNode.transform.position;
        lineRenderer.SetPositions(nodesPoints);

        switch (skill.GetStatus())
        {
            case SkillStatus.Adquired:
                lineRenderer.endColor = Color.green;
                lineRenderer.startColor = Color.green;
                break;
            case SkillStatus.Available:
                lineRenderer.endColor = Color.yellow;
                lineRenderer.startColor = Color.yellow;
                break;
            case SkillStatus.Blocked:
                lineRenderer.endColor = Color.red;
                lineRenderer.startColor = Color.red;
                break;
        }
    }

    public void SetRootNode(GameObject rootNode)
    {
        this.rootNode = rootNode;
    }
}
