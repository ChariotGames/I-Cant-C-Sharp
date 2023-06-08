using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Minigame
{
    public string name;
}

public class MenuButtonHandler : MonoBehaviour
{
    
    private MenuButtonHandler.Scene currentScene;
    [SerializeField] private TMP_Text mainTitle;
    [SerializeField] private TMP_Text playerTitle;
    [SerializeField] private TMP_Text modeTitle;
    [SerializeField] private TMP_Text gamesTitle;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject singlePlayerButton;
    [SerializeField] private GameObject multiplayerButton;
    [SerializeField] private GameObject selectGameButton;
    [SerializeField] private GameObject endlessModeButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject simonSays;
    [SerializeField] private GameObject missingKey;
    [SerializeField] private GameObject autoRunner;
    [SerializeField] private GameObject SAB;
    [SerializeField] private GameObject tankGame;
    [SerializeField] private GameObject game0;
    [SerializeField] private GameObject game1;
    [SerializeField] private GameObject game2;
    [SerializeField] private GameObject game3;
    [SerializeField] private GameObject game4;

    private List<string> playerMode;

    enum Scene
    {
        MainMenu,
        PlayerMenu,
        ModeMenu,
        GamesMenu
    }

    private void Awake()
    {
        currentScene = Scene.MainMenu;
        mainTitle.gameObject.SetActive(true);
        startButton.SetActive(true);
        optionsButton.SetActive(true);
        quitButton.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        startButton.GetComponentInChildren<Button>().onClick.AddListener(OnStartButtonClick);
        optionsButton.GetComponentInChildren<Button>().onClick.AddListener(OnOptionsButtonClick);
        quitButton.GetComponentInChildren<Button>().onClick.AddListener(OnQuitButtonClick);

        singlePlayerButton.GetComponentInChildren<Button>().onClick.AddListener(OnSinglePlayerButtonClick);
        multiplayerButton.GetComponentInChildren<Button>().onClick.AddListener(OnMultiplayerButtonClick);

        selectGameButton.GetComponentInChildren<Button>().onClick.AddListener(OnSelectGameButtonClick);
        
        backButton.GetComponentInChildren<Button>().onClick.AddListener(OnBackButtonClick);
    }
   
    public void OnStartButtonClick() 
    {
        currentScene = Scene.PlayerMenu;
        playerTitle.gameObject.SetActive(true);
        mainTitle.gameObject.SetActive(false);
        startButton.SetActive(false);
        optionsButton.SetActive(false);
        quitButton.SetActive(false);
        singlePlayerButton.SetActive(true);
        multiplayerButton.SetActive(true);
        backButton.SetActive(true);
    }

    public void OnOptionsButtonClick()
    {
        //TODO implement
    }

    public void OnQuitButtonClick()
    {
        Debug.Log("Bye bye!");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }

    public void OnSinglePlayerButtonClick()
    {
        currentScene = Scene.ModeMenu;
        modeTitle.gameObject.SetActive(true);
        playerTitle.gameObject.SetActive(false);
        singlePlayerButton.SetActive(false);
        multiplayerButton.SetActive(false);
        selectGameButton.SetActive(true);
        endlessModeButton.SetActive(true);
    }

    public void OnMultiplayerButtonClick()
    {
        //TODO implement
    }

    public void OnSelectGameButtonClick()
    {
        currentScene = Scene.GamesMenu;
        gamesTitle.gameObject.SetActive(true);
        modeTitle.gameObject.SetActive(false);
        selectGameButton.SetActive(false);
        endlessModeButton.SetActive(false);
        simonSays.SetActive(true);
        missingKey.SetActive(true);
        autoRunner.SetActive(true);
        SAB.SetActive(true);
        tankGame.SetActive(true);
        game0.SetActive(true);
        game1.SetActive(true);
        game2.SetActive(true);
        game3.SetActive(true);
        game4.SetActive(true);
    }

    public void OnBackButtonClick()
    {
        switch(currentScene)
        {
            case Scene.MainMenu:
                Debug.Log("No going back!");
                break;
            case Scene.PlayerMenu:
                mainTitle.gameObject.SetActive(true);
                playerTitle.gameObject.SetActive(false);
                startButton.SetActive(true);
                optionsButton.SetActive(true);
                quitButton.SetActive(true);
                singlePlayerButton.SetActive(false);
                multiplayerButton.SetActive(false);
                backButton.SetActive(false);
                currentScene = Scene.MainMenu;
                break;
            case Scene.ModeMenu:
                modeTitle.gameObject.SetActive(false);
                playerTitle.gameObject.SetActive(true);
                singlePlayerButton.SetActive(true);
                multiplayerButton.SetActive(true);
                selectGameButton.SetActive(false);
                endlessModeButton.SetActive(false);
                currentScene = Scene.PlayerMenu;
                break;
            case Scene.GamesMenu:
                gamesTitle.gameObject.SetActive(false);
                modeTitle.gameObject.SetActive(true);
                selectGameButton.SetActive(true);
                endlessModeButton.SetActive(true);
                simonSays.SetActive(false);
                missingKey.SetActive(false);
                autoRunner.SetActive(false);
                SAB.SetActive(false);
                tankGame.SetActive(false);
                game0.SetActive(false);
                game1.SetActive(false);
                game2.SetActive(false);
                game3.SetActive(false);
                game4.SetActive(false);
                currentScene = Scene.ModeMenu;
                break;
            default:
                break;
        }
    }

}
