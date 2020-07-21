using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class I18NData
{
    public Language language;

    public I18NData(I18NManager i18NManager)
    {
        language = i18NManager.GetLanguage();
    }
}
