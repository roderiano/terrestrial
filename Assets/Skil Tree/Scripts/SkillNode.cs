using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
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
    private PlayerController playerController;

    private void Start()
    {
        buttonPressed = false;
        ledImage = transform.Find("NodeButton/Led").GetComponent<Image>();
        adquireCenter = transform.Find("NodeButton/AdquireCenter").GetComponent<Image>();
        skillTreeManager = FindObjectOfType(typeof(SkillTreeManager)) as SkillTreeManager;
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    void Update()
    {
        RefreshButtonStyle();
        AdquireSkillTrigger();
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
        Image iconSkillNode = transform.Find("NodeButton/Icon").GetComponent<Image>();
        iconSkillNode.sprite = skill.icon;
        
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

    public void SetStatus(SkillNodeStatus status) 
    {
        this.status = status;
    }
    
    private void AdquireSkillTrigger()
    {
        if(status == SkillNodeStatus.Available && buttonPressed && skill.cost <= playerController.GetMemoriesAmount())
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
                skillTreeManager.AdquireSkillType(skill.type);
                playerController.AddMemories(-skill.cost);
            }
                
        }
    }

}
