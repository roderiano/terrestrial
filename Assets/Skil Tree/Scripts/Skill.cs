using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class Skill : ScriptableObject
{
    [Header("Skill Info")]
    public string titleToken;
    public string descriptionToken;
    public Image icon;

    [Header("Attributes Hability")]
    [Range(0, 10)]
    public int cost;
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