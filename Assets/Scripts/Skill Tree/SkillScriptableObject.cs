using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillScriptableObject : ScriptableObject
{
    [Header("Skill Info")]
    public string title;
    public Image icon;
    [TextArea]
    public string description;

    [Header("Attributes")]
    [Range(-10, 10)]
    public int speed;
    [Range(-10, 10)]
    public int health;
    [Range(-10, 10)]
    public int stealth;
    [Range(-10, 10)]
    public int vitality;

    [Header("Special Hability")]
    public SkillSpecialHability specialHability = SkillSpecialHability.None;


}