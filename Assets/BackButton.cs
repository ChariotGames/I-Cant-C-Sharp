using Scripts._Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    private Button backButton;
    private void OnEnable()
    {
        InputHandler.ButtonEast += SelectBackButton;
        backButton = GetComponent<Button>();

    }

    private void SelectBackButton()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        backButton.onClick.Invoke();
    }

    private void OnDisable()
    {
        InputHandler.ButtonEast -= SelectBackButton;
    }
}
