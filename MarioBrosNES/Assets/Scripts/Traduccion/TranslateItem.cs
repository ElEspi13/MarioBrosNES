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
    }
    void Start()
    {
        
        this.Texto.text = Main.translateManager.GetTranslation(this.key);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
