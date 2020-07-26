using UnityEngine;

public class I18NManager : MonoBehaviour 
{
    private Language language;

    private void Start() 
    {
        LoadLanguage();    
    }
    
    // Find translation in CSV
    public string GetTranslation(string token) 
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

    public void SaveLanguage() 
    {
        SaveSystem.SaveI18NSettings(this);
    }

    public void LoadLanguage() 
    {
        I18NData data = SaveSystem.LoadI18NSettings();
        if(data != null)
        {
            language = data.language;
        }
        else
        {
            language = Language.English;
        }
        
    }

    public Language GetLanguage() 
    {
        return language;
    }

    public void SetLanguage(Language language) 
    {
        this.language = language; 
        SaveLanguage();
    }
}
