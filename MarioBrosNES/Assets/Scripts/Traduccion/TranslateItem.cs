using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TranslateItem : MonoBehaviour
{
    public string key;
    private TextMeshProUGUI Texto;

    void Awake()
    {
        this.Texto = GetComponent<TextMeshProUGUI>();
        Main.translateManager.OnLanguageChanged += UpdateText;
    }
    void Start()
    {
        
        this.Texto.text = Main.translateManager.GetTranslation(this.key);
    }


    private void UpdateText()
    {
        if (Main.translateManager != null)
            Texto.text = Main.translateManager.GetTranslation(key);
    }
}
