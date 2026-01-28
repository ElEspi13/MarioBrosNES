using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class InputManager
{
    private static PlayerInputActions _actions;
    public static PlayerInputActions Actions
    {
               get
        {
            if (_actions == null)
            {
                _actions = new PlayerInputActions();
            }
            return _actions;
        }
    }

    public static void SwitchMap(InputActionMap mapToActivate)
    {
        Actions.Player.Disable();
        mapToActivate.Enable();
    }
}