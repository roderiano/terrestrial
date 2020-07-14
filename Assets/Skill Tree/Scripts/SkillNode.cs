using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{

    [SerializeField] private Skill skill;
    [SerializeField] private SkillNodeStatus status = SkillNodeStatus.Blocked;
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

}
