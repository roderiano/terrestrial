using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]GameObject mainMenu;
    [SerializeField]GameObject settingsMenu;

    private void ActiveSettingsMenu() 
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    private void ActiveMainMenu() 
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void SaveSettings() 
    {
        //Save language settings
        I18NManager i18nManager = FindObjectOfType(typeof(I18NManager)) as I18NManager;
        Dropdown languageDropdown = settingsMenu.transform.Find("languageDropdown").GetComponent<Dropdown>();
        i18nManager.SetLanguage((Language)languageDropdown.value);
        ActiveMainMenu();
    }

    public void LoadSetting() 
    {
        //Load language settings
        I18NManager i18nManager = FindObjectOfType(typeof(I18NManager)) as I18NManager;
        Dropdown languageDropdown = settingsMenu.transform.Find("languageDropdown").GetComponent<Dropdown>();
        languageDropdown.value = (int)i18nManager.GetLanguage(); 
        Debug.Log(i18nManager.GetLanguage());

        ActiveSettingsMenu();
    }

    public void LoadGame() 
    {
        PlayerData data = SaveSystem.LoadPlayerSettings();
        SceneManager.LoadScene(data.scene);
    }
}
