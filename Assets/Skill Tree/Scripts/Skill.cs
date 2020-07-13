using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{

    [SerializeField] private SkillScriptableObject skill;
    [SerializeField] private SkillStatus status = SkillStatus.Blocked;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = transform.Find("NodeButton").GetComponent<Image>();
    }

    void Update()
    {
        RefreshButtonColor();
    }

    public void SetStatus(SkillStatus status)
    {
        this.status = status;
    }

    public SkillStatus GetStatus()
    {
        return status;
    }

    void RefreshButtonColor()
    {
        switch (GetStatus())
        {
            case SkillStatus.Adquired:
                buttonImage.color = Color.green;
                break;
            case SkillStatus.Available:
                buttonImage.color = Color.white;
                break;
            case SkillStatus.Blocked:
                buttonImage.color = Color.red;
                break;
        }
    }
}
