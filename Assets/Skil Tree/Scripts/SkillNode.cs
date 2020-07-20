using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{

    [SerializeField] private Skill skill = null;
    [SerializeField] private SkillNodeStatus status = SkillNodeStatus.Blocked;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = transform.Find("NodeButton").GetComponent<Image>();
    }

    void Update()
    {
        RefreshButtonStyle();
    }

    public void SetStatus(SkillNodeStatus status)
    {
        this.status = status;
    }

    public SkillNodeStatus GetStatus()
    {
        return status;
    }

    public Skill GetSkill()
    {
        return skill;
    }

    void RefreshButtonStyle()
    {
        I18NManager i18nManager = FindObjectOfType(typeof(I18NManager)) as I18NManager;
        
        Text titleNodeText = transform.Find("NodeButton/Title").GetComponent<Text>();
        titleNodeText.text =  i18nManager.GetTranslation(skill.titleToken).ToUpper();
        
        switch (GetStatus())
        {
            case SkillNodeStatus.Adquired:
                buttonImage.color = Color.green;
                break;
            case SkillNodeStatus.Available:
                buttonImage.color = Color.white;
                break;
            case SkillNodeStatus.Blocked:
                buttonImage.color = Color.red;
                break;
        }
    }

}
