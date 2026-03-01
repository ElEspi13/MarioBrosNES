using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main
{
    public static Main Instance;
    public static i18n translateManager;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        translateManager = new i18n();
    }

}
