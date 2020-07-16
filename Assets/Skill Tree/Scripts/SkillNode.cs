using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{

    [SerializeField] private Skill skill = null;
    [SerializeField] private SkillNodeStatus status = SkillNodeStatus.Blocked;

    private string title;
    private string description;

    private Image buttonImage;

    private void Start()
    {
        buttonImage = transform.Find("NodeButton").GetComponent<Image>();
    }

    void Update()
    {
        RefreshButtonColor();
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

    void RefreshButtonColor()
    {
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

    public string GetTitle() {
        return title;
    }

    public string GetDescription() {
        return description;
    }

    public void SetTitle(string title) {
        this.title = title;
    }

    public void SetDescription(string description) {
        this.description = description;
    }

}
