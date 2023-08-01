using UnityEngine;
using UnityEngine.UI;

public class SelectionActivator : MonoBehaviour
{
    [SerializeField] Transform content;

    private void OnEnable() => ToggleInteractable(true);

    private void OnDisable() => ToggleInteractable(false);

    private void ToggleInteractable(bool state)
    {
        for (int i = 0; i < content.childCount; i++)
        content.GetChild(i).GetComponent<Button>().interactable = state;
    }
}
