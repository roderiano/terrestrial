using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillTreeData
{
    public int[] equipedSkillTypes;

    public SkillTreeData(List<SkillType> adquiredSkillsTypes)
    {
        equipedSkillTypes = new int[adquiredSkillsTypes.Count];
        foreach(SkillType type in adquiredSkillsTypes)
        {
            equipedSkillTypes[adquiredSkillsTypes.IndexOf(type)] = (int)type;
        }
    }
}
