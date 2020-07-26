using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]GameObject main = null;
    [SerializeField]GameObject settings = null;
    [SerializeField]GameObject selectSlot = null;

    public void ActiveSettingsScreen() 
    {
        main.SetActive(false);
        settings.SetActive(true);
    }

    public void ActiveMainScreen() 
    {
        main.SetActive(true);
        settings.SetActive(false);
        selectSlot.SetActive(false);
    }

    public void ActiveSelectSlotScreen() 
    {
        main.SetActive(false);
        selectSlot.SetActive(true); 

        for(int iSlot = 1; iSlot < 4; iSlot++)
        {
            Text gameNameText = selectSlot.transform.Find("Slot" + iSlot.ToString() + "/GameName").GetComponent<Text>();
            PlayerData data = SaveSystem.LoadPlayer(iSlot.ToString());

            
            if(data != null)
            {
                gameNameText.text = data.scene;
            }
            else
            {
                gameNameText.text = "New Game";
            }
        }

    }

    public void SaveSettings() 
    {
        //Save language settings
        I18NManager i18nManager = FindObjectOfType(typeof(I18NManager)) as I18NManager;
        Dropdown languageDropdown = settings.transform.Find("LanguageDropdown").GetComponent<Dropdown>();
        i18nManager.SetLanguage((Language)languageDropdown.value);
        ActiveMainScreen();
    }

    public void LoadSettings() 
    {
        //Load language settings
        I18NManager i18nManager = FindObjectOfType(typeof(I18NManager)) as I18NManager;
        Dropdown languageDropdown = settings.transform.Find("LanguageDropdown").GetComponent<Dropdown>();
        languageDropdown.value = (int)i18nManager.GetLanguage(); 

        ActiveSettingsScreen();
    }

    public void LoadGame(string slot) 
    {
        PlayerPrefs.SetString("Slot", slot);

        PlayerData data = SaveSystem.LoadPlayer(slot.ToString());
        
        if(data != null)
        {
            SceneManager.LoadScene(data.scene);
        }
        else
        {
            SceneManager.LoadScene("Valley of Confidence");
        }
        
    }
    
}
