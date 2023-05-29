using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    //private SceneChanger sceneChanger = new SceneChanger();
    private string currentScene;
    public GameObject buttonPanel;
    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject quitButton;
    public GameObject singlePlayerButton;
    public GameObject multiplayerButton;
    public GameObject selectGameButton;
    public GameObject endlessModeButton;
    public GameObject backButton;
    public GameObject simonSays;
    public GameObject missingKey;
    public GameObject autoRunner;
    public GameObject SAB;
    public GameObject tankGame;
    public GameObject game0;
    public GameObject game1;
    public GameObject game2;
    public GameObject game3;
    public GameObject game4;

    private List<string> playerMode;

    private void Awake()
    {
        currentScene = "MainMenu";
        startButton.SetActive(true);
        optionsButton.SetActive(true);
        quitButton.SetActive(true);
        /*singlePlayerButton.SetActive(false);
        multiplayerButton.SetActive(false);
        selectGameButton.SetActive(false);
        endlessModeButton.SetActive(false);
        backButton.SetActive(false);
        */
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
        endlessModeButton.GetComponentInChildren<Button>().onClick.AddListener(OnEndlessModeButtonClick);
        backButton.GetComponentInChildren<Button>().onClick.AddListener(OnBackButtonClick);

        simonSays.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        missingKey.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        autoRunner.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        SAB.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        tankGame.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        game0.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        game1.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        game2.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        game3.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
        game4.GetComponentInChildren<Button>().onClick.AddListener(OnGameButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

   
    public void OnStartButtonClick() 
    {
        currentScene = "Player Menu";
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
        //TODO implement
    }

    public void OnSinglePlayerButtonClick()
    {
        currentScene = "Mode Menu";
        singlePlayerButton.SetActive(false);
        multiplayerButton.SetActive(false);
        selectGameButton.SetActive(true);
        endlessModeButton.SetActive(true);
    }

    public void OnMultiplayerButtonClick()
    {
        //playerMode.Add("Multiplayer");
    }

    public void OnSelectGameButtonClick()
    {
        currentScene = "Games Menu";
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
   
    public void OnEndlessModeButtonClick()
    {
        
        //sceneChanger.ChangeScene(1);
    }

    public void OnBackButtonClick()
    {
        switch(currentScene)
        {
            case "Main Menu":
                Debug.Log("No going back!");
                break;
            case "Player Menu":
                startButton.SetActive(true);
                optionsButton.SetActive(true);
                quitButton.SetActive(true);
                singlePlayerButton.SetActive(false);
                multiplayerButton.SetActive(false);
                backButton.SetActive(false);
                currentScene = "Main Menu";
                break;
            case "Mode Menu":
                singlePlayerButton.SetActive(true);
                multiplayerButton.SetActive(true);
                selectGameButton.SetActive(false);
                endlessModeButton.SetActive(false);
                currentScene = "Player Menu";
                break;
            case "Games Menu":
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
                currentScene = "Mode Menu";
                break;
            default:
                Debug.Log("Something");
                break;
        }
    }

    public void OnGameButtonClick()
    {
        
    }
}
