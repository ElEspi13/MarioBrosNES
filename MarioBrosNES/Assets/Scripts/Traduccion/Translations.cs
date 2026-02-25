using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Translations 
{
    
    public string key;
    public string value;


}

[Serializable]
public class TranslationsWrapper
{
    public List<Translations> traducciones;

}
