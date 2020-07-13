using UnityEngine;
using UnityEditor;
using System.Linq;

public class CreateTool
{
    [MenuItem("Terrestrial/Skill Tree/Create Skill Node")]
    static void CreateSkillNode()
    {
        Object nodePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Skill Tree/Node.prefab", typeof(GameObject));
        GameObject go = PrefabUtility.InstantiatePrefab(nodePrefab) as GameObject;

        if (go != null)
        {
            if (Selection.activeTransform != null)
                go.transform.SetParent(Selection.activeTransform);

            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
        }
     }

    [MenuItem("GameObject/UI/Terrestrial/Skill Node")]
    static void CreateSkillNoteByCreateMenu()
    {
        CreateSkillNode();
    }

}