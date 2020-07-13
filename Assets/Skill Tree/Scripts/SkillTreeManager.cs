using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{

    [SerializeField] private Material lineMaterial;

    void Start()
    {
        // Configure line redenderes relation
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
            //Draw lines and your childrens
            DrawLine(child);
        }
    }

    void DrawLine(Transform node)
    {
        foreach (Transform child in node)
        {
            if (child.tag == "Skill Node")
            {
                // Configure the line connection refresh
                NodeLineConnectionRefresh childNodeLineConnectionRefresh = child.gameObject.AddComponent<NodeLineConnectionRefresh>();
                childNodeLineConnectionRefresh.SetRootNode(node.gameObject);
                childNodeLineConnectionRefresh.SetLineMaterial(lineMaterial);
                childNodeLineConnectionRefresh.SetSkill(child.GetComponent<Skill>());
                DrawLine(child);
            }
        }
    }

    void RefreshTreeSkill()
    {
        foreach (Transform child in transform)
        {
            // Refresh skill status of children by status of root node
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
