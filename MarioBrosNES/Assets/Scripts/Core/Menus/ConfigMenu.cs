using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigMenu : MonoBehaviour
{
    public TextMeshProUGUI[] options;
    public RectTransform cursor;
    public GameObject parentMenu; // El menú principal para volver

    private int selectedIndex = 0;
    private bool canNavigate = true;
    public float inputBuffer = 0.2f;

    private void OnEnable()
    {
        InputManager.SwitchMap(InputManager.Actions.UIcontrolls);

        InputManager.Actions.UIcontrolls.Navigate.performed += OnNavigate;
        InputManager.Actions.UIcontrolls.Submit.performed += OnSubmit;
        InputManager.Actions.UIcontrolls.Cancel.performed += OnCancel;

        UpdateSelection();
    }

    private void OnDisable()
    {
        InputManager.Actions.UIcontrolls.Navigate.performed -= OnNavigate;
        InputManager.Actions.UIcontrolls.Submit.performed -= OnSubmit;
        InputManager.Actions.UIcontrolls.Cancel.performed -= OnCancel;
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

        cursor.position = new Vector3(
            cursor.position.x,
            options[selectedIndex].transform.position.y,
            cursor.position.z
        );
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        ExecuteOption(selectedIndex);
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        ReturnToParentMenu();
    }

    private void ExecuteOption(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Instance.stats.lives=10;
                GameManager.Instance.stats.difficulty = GameStats.Dificulty.Easy;
                break;

            case 1:
                GameManager.Instance.stats.lives = 3;
                GameManager.Instance.stats.difficulty = GameStats.Dificulty.Normal;
                break;

            case 2:
                GameManager.Instance.stats.lives = 1;
                GameManager.Instance.stats.difficulty = GameStats.Dificulty.Hard;
                break;

            case 3:
                Main.translateManager.LoadLanguage("es");
                break;
            case 4:
                Main.translateManager.LoadLanguage("en");
                break;

            default:
                Debug.LogWarning("Opción de submenú no asignada");
                break;
        }
    }

    private void ReturnToParentMenu()
    {
        if (parentMenu != null)
            parentMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}