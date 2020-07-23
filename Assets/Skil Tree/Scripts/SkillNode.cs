using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SkillNode : MonoBehaviour
{

    [SerializeField] private Skill skill = null;
    [SerializeField] private SkillNodeStatus status = SkillNodeStatus.Blocked;
    
    private Image ledImage;
    private Image adquireCenter;
    private bool buttonPressed;
    private float fillAmount;
    private SkillTreeManager skillTreeManager;

    private void Start()
    {
        buttonPressed = false;
        ledImage = transform.Find("NodeButton/Led").GetComponent<Image>();
        adquireCenter = transform.Find("NodeButton/AdquireCenter").GetComponent<Image>();
        skillTreeManager = FindObjectOfType(typeof(SkillTreeManager)) as SkillTreeManager;
    }

    void Update()
    {
        RefreshButtonStyle();
        AdquireSkill();
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
        
        // Text titleNodeText = transform.Find("NodeButton/Title").GetComponent<Text>();
        // titleNodeText.text =  i18nManager.GetTranslation(skill.titleToken).ToUpper();
        
        switch (GetStatus())
        {
            case SkillNodeStatus.Adquired:
                ledImage.color = Color.green;
                break;
            case SkillNodeStatus.Available:
                ledImage.color = Color.white;
                break;
            case SkillNodeStatus.Blocked:
                ledImage.color = Color.red;
                break;
        }
    }

    public void OnSkillUp()
    {
        fillAmount = 0f;
        buttonPressed = false;
        adquireCenter.fillAmount = 0;
    }

    public void OnSkillDown()
    {
        fillAmount = 0f;
        buttonPressed = true;
        skillTreeManager.SelectSkill(this);
    }

    
    private void AdquireSkill()
    {
        if(status == SkillNodeStatus.Available && buttonPressed)
        {
            fillAmount += 0.01f * Time.deltaTime;

            if(adquireCenter.fillAmount < 1)
            {
                adquireCenter.fillAmount += fillAmount;
            }
            else
            {
                adquireCenter.fillAmount = 0;
                SetStatus(SkillNodeStatus.Adquired);
            }
                
        }
    }

}
