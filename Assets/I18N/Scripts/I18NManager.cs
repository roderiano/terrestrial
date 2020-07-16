using UnityEngine;

public class I18NManager : MonoBehaviour
{
    private I18NEntity[] entities = null;

    void Start()
    {
        FindAllEntities();
        RefreshSceneTranslation(Language.Portuguese);
    }

    void FindAllEntities() 
    {
        entities = (I18NEntity[])FindObjectsOfType(typeof(I18NEntity));
    }

    void RefreshSceneTranslation(Language language)
    {
        string translation = "";
        foreach(I18NEntity entity in entities) 
        {
            translation = GetTranslation(entity.GetToken(), language);
            entity.SetTranslation(translation);
        }   
    }

    string GetTranslation(string token, Language language) 
    {
        string path = "Assets/I18N/Data/TranslationData.csv";
        string[] data = System.IO.File.ReadAllLines(path);

        foreach(string line in data) 
        {
            string[] columns = line.Split(char.Parse(";"));

            if(columns[0] == token)
                return columns[(int)language + 1];
        }

        return null;        
    }
}
