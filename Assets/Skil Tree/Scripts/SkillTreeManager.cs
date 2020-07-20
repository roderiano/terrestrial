using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillTreeManager : MonoBehaviour
{
    [Header("Node Line Connection")]
    [SerializeField] private Material lineMaterial = null;
    [Header("UI Components")]
    
    [SerializeField] private Text title = null;
    [SerializeField] private Text description = null;
    [SerializeField] private Text actionButtonText = null;
    [SerializeField] private Image actionButtonImage = null;
    [SerializeField] private GameObject canvasTreeViewUI = null;

    private GameObject contentUI = null;
    

    private SkillNode selectedSkillNode;

    void Start()
    {
        // Configure line redenderes relation
        contentUI = canvasTreeViewUI.transform.Find("Viewport/Content").gameObject;
        CreateLinesRelation();
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

    void RefreshTreeSkill()
    {
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

        switch (skillNode.GetStatus())
        {
            case SkillNodeStatus.Adquired:
                actionButtonImage.color = Color.green;
                actionButtonText.text = "ADQUIRED";
                break;
            case SkillNodeStatus.Available:
                actionButtonImage.color = Color.white;
                actionButtonText.text = "AVAILABLE";
                break;
            case SkillNodeStatus.Blocked:
                actionButtonImage.color = Color.red;
                actionButtonText.text = "BLOCKED";
                break;
        }
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
        
        while(!Input.GetKeyDown(KeyCode.Escape)) 
        {
            yield return null;
        }

        canvasTreeViewUI.SetActive(false);
    }

}
