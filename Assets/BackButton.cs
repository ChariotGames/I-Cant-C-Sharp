using Scripts._Input;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour
{
    private void OnEnable()
    {
        InputHandler.ButtonEast += SelectBackButton;
    }

    private void SelectBackButton()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    private void OnDisable()
    {
        InputHandler.ButtonEast -= SelectBackButton;
    }
}
