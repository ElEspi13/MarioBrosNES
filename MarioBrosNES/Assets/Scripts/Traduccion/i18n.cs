using System;
using System.Collections.Generic;
using UnityEngine;

public class i18n
{
    private Dictionary<string, string> translationsDictionary;
    public TextAsset jsonFile;
    public event Action OnLanguageChanged;

    public i18n()
    {
        LoadLanguage("es"); 
    }

    public void LoadLanguage(string langCode)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"i18n/_{langCode}");
        if (jsonFile == null)
        {
            Debug.LogWarning($"No se encontró el archivo de idioma: {langCode}");
            return;
        }

        TranslationsWrapper content = JsonUtility.FromJson<TranslationsWrapper>(jsonFile.text);
        this.translationsDictionary = ConvertListToDictionary(content.traducciones);
        OnLanguageChanged?.Invoke();
    }

    private Dictionary<string, string> ConvertListToDictionary(List<Translations> traducciones)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (var item in traducciones)
        {
            dict[item.key] = item.value;
        }
        return dict;
    }

    public string GetTranslation(string key)
    {
        if (translationsDictionary != null && translationsDictionary.ContainsKey(key))
            return translationsDictionary[key];
        return key;
    }
}