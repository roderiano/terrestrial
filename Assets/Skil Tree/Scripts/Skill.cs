using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class Skill : ScriptableObject
{
    [Header("Skill Info")]
    public string titleToken;
    public string descriptionToken;
    public Sprite icon;

    [Header("Attributes Hability")]
    [Range(0, 10)]
    public int cost;
    public SkillHability specialHability = SkillHability.None;
}