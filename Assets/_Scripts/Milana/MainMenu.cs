using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _Scripts;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject gamesContainer, templateGameButton;
    [SerializeField] private Settings settings;
    [SerializeField] private Camera mainCamera;
    private List<string> playerMode;
    private List<GameObject> gameButtons;
    //private Bounds _cameraViewportBounds;
    //private float _playfieldWidth;

    private void Start()
    {
        gameButtons = new();
        gameObject.GetComponent<CanvasScaler>().scaleFactor = mainCamera.pixelWidth / 1920.0f;
        //_cameraViewportBounds = new Bounds(mainCamera.transform.position, mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - mainCamera.ViewportToWorldPoint(Vector3.zero));
        //_playfieldWidth = _cameraViewportBounds.size.x;
    }

    public void ActivateSubMenu(GameObject menu)
    {
        menu.SetActive(true);
    }
    public void DeactivateSubMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void FillGamesContainer()
    {
        if (gamesContainer.transform.childCount != 0) return;

        foreach (var game in settings.Games)
        {
            gameButtons.Add(Instantiate(templateGameButton, gamesContainer.transform));
            GameObject gameButton = GameObject.Find("GameButton");
            gameButton.GetComponentInChildren<TMP_Text>().text = game.AssetID.ToString()[4..];
            
        }
        SortButtons();
    }

    private void SortButtons()
    {
        foreach (var button in gameButtons)
        {
            //TODO Button pos anordnen
        }
    }
}
