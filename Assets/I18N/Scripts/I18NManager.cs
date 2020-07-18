using UnityEngine;

public class I18NManager : MonoBehaviour
{
    void Start()
    {
        SetLanguage(Language.Portuguese);
    }

    // Find translation in CSV
    public string GetTranslation(string token) 
    {
        Language language = GetLanguage();

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

    public void SetLanguage(Language language) 
    {
        PlayerPrefs.SetInt("Language", (int)language);
    }

    public Language GetLanguage() 
    {
        return (Language)PlayerPrefs.GetInt("Language");
    }
}
