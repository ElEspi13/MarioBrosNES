using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


/// <summary>
/// Provee acceso estático a las acciones de entrada (singleton de PlayerInputActions).
/// </summary>
public class InputManager
{
    private static PlayerInputActions _actions;
    /// <summary>
    /// Obtiene la instancia singleton de <see cref="PlayerInputActions"/>.
    /// </summary>
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

    /// <summary>
    /// Cambia el mapa de acciones activo por el indicado.
    /// </summary>
    /// <param name="mapToActivate">Mapa de acciones a activar</param>
    public static void SwitchMap(InputActionMap mapToActivate)
    {
        Actions.Player.Disable();
        mapToActivate.Enable();
    }

    /// <summary>
    /// Desactiva el mapa de acciones indicado, dejando sin acciones activas. Útil para pausar el juego o deshabilitar la entrada temporalmente.
    /// </summary>
    /// <param name="mapToActivate"></param>
    public static void DisableMap(InputActionMap mapToActivate)
    {
        mapToActivate.Disable();
    }

}