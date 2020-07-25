using UnityEngine;
using System.IO;
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

    public static void SavePlayer(GameObject player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.ter";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player); 

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.ter";
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

}
