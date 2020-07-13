using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{

    [SerializeField] private Material lineMaterial;

    void Start()
    {
        CreateLinesRelation();
    }

    void Update()
    {
        RefreshTreeSkill();
    }

    void CreateLinesRelation()
    {
        foreach (Transform child in transform)
        {
            DrawLine(child);
        }
    }

    void DrawLine(Transform node)
    {
        foreach (Transform child in node)
        {
            if (child.tag == "Skill Node")
            {
                NodeConnectionRefresh childConnectionLine = child.gameObject.AddComponent<NodeConnectionRefresh>();
                childConnectionLine.SetRootNode(node.gameObject);
                childConnectionLine.lineMaterial = lineMaterial;
                childConnectionLine.skill = child.GetComponent<Skill>();
                DrawLine(child);
            }
        }
    }

    void RefreshTreeSkill()
    {
        foreach (Transform child in transform)
        {
            RefreshNode(child);
        }
    }

    void RefreshNode(Transform node)
    {
        foreach (Transform child in node)
        {
            if (child.tag == "Skill Node")
            {
                Skill skillNode = node.GetComponent<Skill>();
                Skill skillChild = child.GetComponent<Skill>();

                switch (skillNode.GetStatus())
                {
                    case SkillStatus.Adquired:
                        if (skillChild.GetStatus() != SkillStatus.Adquired)
                            skillChild.SetStatus(SkillStatus.Available);
                        break;
                    case SkillStatus.Available:
                        skillChild.SetStatus(SkillStatus.Blocked);
                        break;
                    case SkillStatus.Blocked:
                        skillChild.SetStatus(SkillStatus.Blocked);
                        break;
                    case SkillStatus.None:
                        skillNode.SetStatus(SkillStatus.Available);
                        break;
                }

                RefreshNode(child);
            }
        }
    }
}
