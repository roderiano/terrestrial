using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLineConnectionRefresh : MonoBehaviour
{

    private float lineWidth;
    private SkillNode skill;
    private GameObject rootNode;
    private Vector3[] nodesPoints;
    private Material lineMaterial;
    private LineRenderer lineRenderer;

    void Start()
    {
        // Set initial config for the line
        lineWidth = 0.5f;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        nodesPoints = new Vector3[2];
        lineRenderer.sortingOrder = 2000;
    }

    void Update()
    {
        // Update the position and color of connection line renderer
        if (skill != null)
        {
            // Refresh start/end points of line connection
            nodesPoints[0] = transform.position;
            nodesPoints[1] = rootNode.transform.position;
            lineRenderer.SetPositions(nodesPoints);

            // Refresh color of line by skill status
            switch (skill.GetStatus())
            {
                case SkillNodeStatus.Adquired:
                    lineRenderer.endColor = Color.green;
                    lineRenderer.startColor = Color.green;
                    break;
                case SkillNodeStatus.Available:
                    lineRenderer.endColor = Color.white;
                    lineRenderer.startColor = Color.white;
                    break;
                case SkillNodeStatus.Blocked:
                    lineRenderer.endColor = Color.red;
                    lineRenderer.startColor = Color.red;
                    break;
            }
        }
    }

    public void SetRootNode(GameObject rootNode)
    {
        this.rootNode = rootNode;
    }

    public void SetLineMaterial(Material material)
    {
        lineMaterial = material;
    }

    public void SetSkill(SkillNode skill)
    {
        this.skill = skill;
    }
}
