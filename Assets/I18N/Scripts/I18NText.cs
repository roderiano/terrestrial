using UnityEngine;
using UnityEngine.UI;

public class I18NText : MonoBehaviour
{
    [SerializeField]private string token = null;
    
    void FixedUpdate()
    {
        I18NManager translationManager = (I18NManager)FindObjectOfType(typeof(I18NManager));
        GetComponent<Text>().text = translationManager.GetTranslation(token); 
    }
}
