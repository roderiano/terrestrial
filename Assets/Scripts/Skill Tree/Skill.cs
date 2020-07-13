using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    [SerializeField] private SkillScriptableObject skill;
    [SerializeField] private SkillStatus status = SkillStatus.Blocked;

    public void SetStatus(SkillStatus status)
    {
        this.status = status;
    }

    public SkillStatus GetStatus()
    {
        return status;
    }
}
