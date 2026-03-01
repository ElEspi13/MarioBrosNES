using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la navegación y selección de un menú usando el Input System.
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("Opciones del menú")]
    [SerializeField] private TextMeshProUGUI[] options;
    [SerializeField] private RectTransform cursor;


    [SerializeField] private GameObject subMenu; 
    private int selectedIndex = 0;
    private bool canNavigate = true;

    private void OnEnable()
    {
        // Cambiar al mapa de UI
        InputManager.SwitchMap(InputManager.Actions.UIcontrolls);

        // Suscribirse a los eventos
        InputManager.Actions.UIcontrolls.Navigate.performed += OnNavigate;
        InputManager.Actions.UIcontrolls.Submit.performed += OnSubmit;

        UpdateSelection();
    }

    private void OnDisable()
    {
        InputManager.Actions.UIcontrolls.Navigate.performed -= OnNavigate;
        InputManager.Actions.UIcontrolls.Submit.performed -= OnSubmit;
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (!canNavigate) return;

        Vector2 input = ctx.ReadValue<Vector2>();

        if (input.y > 0.5f)
            selectedIndex--;
        else if (input.y < -0.5f)
            selectedIndex++;

        if (selectedIndex < 0)
            selectedIndex = options.Length - 1;
        else if (selectedIndex >= options.Length)
            selectedIndex = 0;

        UpdateSelection();

    }

    private void UpdateSelection()
    {
        if (cursor == null || options.Length == 0) return;

        RectTransform optionRect = options[selectedIndex].rectTransform;

        // Calculamos la posición izquierda del texto en world space
        Vector3 leftEdgeWorld = optionRect.position;
        leftEdgeWorld.x -= optionRect.rect.width * optionRect.pivot.x;

        // Movemos el cursor
        cursor.position = new Vector3(
            leftEdgeWorld.x,            // X
            optionRect.position.y,      // Y
            cursor.position.z
        );
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        ExecuteOption(selectedIndex);
    }

    /// <summary>
    /// Ejecuta la acción según el índice seleccionado.
    /// </summary>
    private void ExecuteOption(int index)
    {
        switch (index)
        {
            case 0:

                GameManager.Instance.ChangeLevel(1);
                break;

            case 1:
                if (subMenu != null)
                    subMenu.SetActive(true);
                gameObject.SetActive(false);
                break;

            case 2:
                Debug.Log("Saliendo del juego...");
                Application.Quit();
                break;

        }
    }

    
}