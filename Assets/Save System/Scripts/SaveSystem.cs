using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    
    public static void SaveI18NSettings(I18NManager i18nManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/i18n.ter";
        FileStream stream = new FileStream(path, FileMode.Create);

        I18NData data = new I18NData(i18nManager); 

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static I18NData LoadI18NSettings()
    {
        string path = Application.persistentDataPath + "/i18n.ter";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            I18NData data = formatter.Deserialize(stream) as I18NData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void SavePlayer(GameObject player, string slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player" + slot + ".ter";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player); 

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(string slot)
    {
        string path = Application.persistentDataPath + "/player" + slot + ".ter";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void SaveSkillTree(List<SkillType> skills, string slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/skillTree" + slot + ".ter";
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillTreeData data = new SkillTreeData(skills); 

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SkillTreeData LoadSkillTree(string slot)
    {
        string path = Application.persistentDataPath + "/skillTree" + slot + ".ter";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            SkillTreeData data = formatter.Deserialize(stream) as SkillTreeData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

}
