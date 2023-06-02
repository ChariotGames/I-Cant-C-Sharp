using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using _Scripts;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject templateMenuButton, templateGameButton, subMenus, gamesContainer;
    [SerializeField] private List<GameAsset> games;
    private List<string> playerMode;
    private List<GameObject> gameButtons;

    private void Start()
    {
        //fillGamesContainer();
    }

    public void ActivateSubMenu(int menuName)
    {
        for (int i = 0; i < subMenus.transform.childCount; i++)
        {
            Transform child = subMenus.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
        /*
        foreach (var menu in subMenus.transform.GetComponentsInChildren<GameObject>())
        {
            menu.SetActive(false);
        }*/
        subMenus.transform.GetChild(menuName).gameObject.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Bye bye!");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }

    public void fillGamesContainer()
    {
        //List<GameAsset> games = getGames();
        foreach (var game in games)
        {
            gameButtons.Add(Instantiate(templateGameButton, gamesContainer.transform));
            GameObject obj = GameObject.Find("GameButton");
            obj.GetComponentInChildren<TMP_Text>().text = game.AssetID.ToString()[4..];
            
        }
        sortButtons();
    }

    private void sortButtons()
    {
        foreach (var button in gameButtons)
        {
            //TODO Button pos anordnen
        }
    }

   private List<GameAsset> getGames()
    {
        //TODO load assets from folder
        return new List<GameAsset>();
    }

    /*
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
    */

}
