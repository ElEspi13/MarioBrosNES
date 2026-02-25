using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inicializador de la escena 1: configura el mapa de entrada por defecto en Awake.
/// </summary>
public class InitializerScene1 : MonoBehaviour
{
    private void Awake()
    {
        InputManager.SwitchMap(InputManager.Actions.Player);
        GameManager.Instance.ResetPlayerPosition();
    }
}

