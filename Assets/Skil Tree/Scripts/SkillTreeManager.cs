using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillTreeManager : MonoBehaviour
{
    [Header("Node Line Connection")]
    [SerializeField] private Material lineMaterial = null;
    
    [Header("UI Components")]
    [SerializeField] private Text cost = null;
    [SerializeField] private Text title = null;
    [SerializeField] private Text memories = null;
    [SerializeField] private Text description = null;
    [SerializeField] private GameObject canvasTreeViewUI = null;

    private GameObject contentUI = null;
    private SkillNode selectedSkillNode;
    private PlayerController playerController;
    private List<SkillType> adquiredSkills = new List<SkillType>();

    void Start()
    {
        // Configure line redenderes relation
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        contentUI = canvasTreeViewUI.transform.Find("Viewport/Content").gameObject;
        CreateLinesRelation();
        LoadSkillTree();
    }

    void Update()
    {
        RefreshTreeSkill();
    }

    void CreateLinesRelation()
    {
        foreach (Transform child in contentUI.transform)
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
                childNodeLineConnectionRefresh.SetSkill(child.GetComponent<SkillNode>());
                DrawLine(child);
            }
        }
    }

    public void AdquireSkillType(SkillType skillType) 
    {
        adquiredSkills.Add(skillType);
    }

    void RefreshTreeSkill()
    {   
        memories.text = playerController.GetMemoriesAmount().ToString();
        
        GameObject contentUI = canvasTreeViewUI.transform.Find("Viewport/Content").gameObject;
        foreach (Transform child in contentUI.transform)
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
                SkillNode skillNode = node.GetComponent<SkillNode>();
                SkillNode skillChild = child.GetComponent<SkillNode>();

                switch (skillNode.GetStatus())
                {
                    case SkillNodeStatus.Adquired:
                        if (skillChild.GetStatus() != SkillNodeStatus.Adquired)
                            skillChild.SetStatus(SkillNodeStatus.Available);
                        break;
                    case SkillNodeStatus.Available:
                        skillChild.SetStatus(SkillNodeStatus.Blocked);
                        break;
                    case SkillNodeStatus.Blocked:
                        skillChild.SetStatus(SkillNodeStatus.Blocked);
                        break;
                    case SkillNodeStatus.None:
                        skillNode.SetStatus(SkillNodeStatus.Available);
                        break;
                }

                RefreshNode(child);
            }
        }
    }

    public void SelectSkill(SkillNode skillNode)
    {
        selectedSkillNode = skillNode;
        RefreshInfo(skillNode);
    }

    // Refresh UI info
    public void RefreshInfo(SkillNode skillNode)
    {
        I18NManager translatinManager = (I18NManager)FindObjectOfType(typeof(I18NManager));
        
        title.text = translatinManager.GetTranslation(selectedSkillNode.GetSkill().titleToken).ToUpper();
        description.text = translatinManager.GetTranslation(selectedSkillNode.GetSkill().descriptionToken);
        cost.text = "COST " + selectedSkillNode.GetSkill().cost.ToString();
    }

    public void ActivateSelectedSkill()
    {
        if (selectedSkillNode.GetStatus() == SkillNodeStatus.Available)
        {
            selectedSkillNode.SetStatus(SkillNodeStatus.Adquired);
            RefreshInfo(selectedSkillNode);
        }
    }

    public IEnumerator EnableSkillTree() 
    {
        canvasTreeViewUI.SetActive(true);
        LoadSkillTree();
        
        while(!Input.GetKeyDown(KeyCode.Escape)) 
        {
            yield return null;
        }

        canvasTreeViewUI.SetActive(false);
    }

    public bool SkillIsAdquired(SkillType type) 
    {
        if(type == SkillType.None)
        {
            Debug.LogError("SkillTreeManager.SkillIsAdquired(): type None doesn't exists.");
            return false;
        }
        
        return adquiredSkills.Contains(type);
    }

    public void SaveSkillTree() 
    {
        SaveSystem.SaveSkillTree(adquiredSkills, PlayerPrefs.GetString("Slot"));
    }

    public void LoadSkillTree() 
    {
        SkillTreeData data = SaveSystem.LoadSkillTree(PlayerPrefs.GetString("Slot"));

        if(data != null)
        {
            adquiredSkills.Clear();
            foreach(int skillType in data.equipedSkillTypes) 
            {
                adquiredSkills.Add((SkillType)skillType);
            }

            SkillNode[] skillNodes = FindObjectsOfType(typeof(SkillNode)) as SkillNode[];
            foreach(SkillNode node in skillNodes) 
            {
                if(adquiredSkills.Contains(node.GetSkill().type))
                    node.SetStatus(SkillNodeStatus.Adquired);
            }
        }
    }

}
