using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Opciones del menú")]
    [SerializeField] private TextMeshProUGUI[] options;
    [SerializeField] private RectTransform cursor;

    private int selectedIndex = 0;

    private void OnEnable()
    {
        InputManager.Actions.UIcontrolls.Navigate.performed += OnNavigate;
        InputManager.Actions.UIcontrolls.Submit.performed += OnSubmit;
    }

    private void OnDisable()
    {
        InputManager.Actions.UIcontrolls.Navigate.performed -= OnNavigate;
        InputManager.Actions.UIcontrolls.Submit.performed -= OnSubmit;
    }

    private void Update()
    {
        
    }

    public void TogglePause()
    {

        gameObject.SetActive(true);

            Time.timeScale = 0f;
            InputManager.SwitchMap(InputManager.Actions.UIcontrolls);
            selectedIndex = 0;
            UpdateSelection();

    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {

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

        Vector3 leftEdgeWorld = optionRect.position;
        leftEdgeWorld.x -= optionRect.rect.width * optionRect.pivot.x/0.5f;

        cursor.position = new Vector3(
            leftEdgeWorld.x,
            optionRect.position.y,
            cursor.position.z
        );
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        ExecuteOption(selectedIndex);
    }

    private void ExecuteOption(int index)
    {
        switch (index)
        {
            case 0: 
                Time.timeScale = 1f;
                gameObject.SetActive(false);
                InputManager.SwitchMap(InputManager.Actions.Player);
                break;

            case 1: 
                Time.timeScale = 1f;
                InputManager.SwitchMap(InputManager.Actions.UIcontrolls);
                SceneManager.LoadScene(0);
                GameManager.Instance.ExitGame();
                GameManager.Instance.ResetPlayerPosition();
                gameObject.SetActive(false);
                
                break;
        }
    }
}