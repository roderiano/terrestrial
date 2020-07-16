using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
 using System.Text.RegularExpressions;

public class I18NEntity
{
    public Object obj;
    public string token;
    public string attribute;

    public I18NEntity(Object obj, string token, string attribute = null)
    {
        this.obj = obj;
        this.token = token;
        this.attribute = attribute;
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

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
            RefreshSceneTranslation(Language.English);   
    }

    // Find all I18NEntity in scene when start
    void FindAllEntities() 
    {
        List<Object> objects = new List<Object>();
        
        // Get all object of type Text and add to list of objects
        Object[] arrayObjects;
        
        arrayObjects = FindObjectsOfType(typeof(Text));
        foreach(Object obj in arrayObjects)
            objects.Add(obj);

        arrayObjects = FindObjectsOfType(typeof(SkillNode));
        foreach(Object obj in arrayObjects)
            objects.Add(obj);

        // Verify if obj is a I18Entity and add to entity list
        string token = "";
        foreach(Object obj in objects) 
        {
            switch(obj) 
            {
                case Text textComponent:
                    token = GetToken(((Text)obj).text);
                    entities.Add(new I18NEntity(obj, token));
                break;

                case SkillNode skillNodeComponent:
                    token = GetToken(((SkillNode)obj).GetSkill().titleToken);
                    entities.Add(new I18NEntity(obj, token, "title"));

                    token = GetToken(((SkillNode)obj).GetSkill().descriptionToken);
                    entities.Add(new I18NEntity(obj, token, "description"));
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

                    case SkillNode skillNodeComponent:
                        if(entity.attribute == "title")
                            skillNodeComponent.SetTitle(translation);
                        else if(entity.attribute == "description")
                            skillNodeComponent.SetDescription(translation);
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
