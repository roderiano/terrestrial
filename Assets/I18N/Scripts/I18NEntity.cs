using UnityEngine;
using UnityEngine.UI;

public class I18NEntity : MonoBehaviour
{
    [SerializeField] private string token = null;
    private Text component;

    void Start()
    {
        component = GetComponent<Text>();
    }

    public void SetTranslation(string translation) 
    {
        component.text = translation;
    }

    public string GetToken() 
    {
        return token;
    }
}
