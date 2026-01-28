using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializerScene1 : MonoBehaviour
{
    private void Awake()
    {
        InputManager.SwitchMap(InputManager.Actions.Player);
    }
}

