using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
 using System.Text.RegularExpressions;

public class I18NEntity
{
    public Object obj;
    public string token;

    public I18NEntity(Object obj, string token)
    {
        this.obj = obj;
        this.token = token;
    }
}

public class I18NManager : MonoBehaviour
{
    [SerializeField]
    private List<I18NEntity> entities = new List<I18NEntity>();
    
    void Start()
    {
        FindAllEntities();
        RefreshSceneTranslation(Language.Portuguese);
    }

    // Find all I18NEntity in scene when start
    void FindAllEntities() 
    {
        List<Object> objects = new List<Object>();
        
        // Get all object of type Text and add to list of objects
        Object[] arrayObjects = FindObjectsOfType(typeof(Text));
        foreach(Object obj in arrayObjects)
            objects.Add(obj);


        // Verify if obj is a I18Entity and add to entity list
        string token = "";
        foreach(Object obj in objects) 
        {
            switch(obj) 
            {
                case Text text:
                    token = GetToken(((Text)obj).text);
                    entities.Add(new I18NEntity(obj, token));
                break;
            }
        }   
    }

    void RefreshSceneTranslation(Language language)
    {
        foreach(I18NEntity entity in entities) 
        {
            string translation = GetTranslation(entity.token, language);
            if(translation != null)
            {
                switch(entity.obj) 
                {
                    case Text textComponent:
                        textComponent.text = translation;
                    break;
                }
            }
        }  
    }

    
    // Find translation in CSV
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

    // Find token by regex
    string GetToken(string text) 
    {
        string token = Regex.Match(text, "(token.)(\\S*)").ToString().Replace("token.", "").Trim();
        token = token != "" ? token : null;
        return token;
    }
}
