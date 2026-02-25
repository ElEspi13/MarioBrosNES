using System;
using System.Collections.Generic;
using UnityEngine;

public class i18n
{
    private Dictionary<string, string> translationsDictionary;
    public i18n()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"i18n/_es");
        TranslationsWrapper content= JsonUtility.FromJson<TranslationsWrapper>(jsonFile.text);
        this.translationsDictionary= this.ConvertListToDictionary(content.traducciones);
    }

    private Dictionary<string,string> ConvertListToDictionary(List<Translations> traducciones)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (var item in traducciones)
        {
            dict.Add(item.key, item.value);
        }
        return dict;
    }
    public string GetTranslation(string key)
    {
        if (this.translationsDictionary.ContainsKey(key))
        {
            return this.translationsDictionary[key];
        }
        return key;
    }
}
