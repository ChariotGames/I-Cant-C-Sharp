using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{

    public GameObject buttonPanel;
    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject quitButton;
    public GameObject singlePlayerButton;
    public GameObject multiplayerButton;
    public GameObject selectGameButton;
    public GameObject endlessModeButton;
    public GameObject backButton;
    private List<string> playerMode;

    private void Awake()
    {
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
        
        //startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
        startButton.GetComponentInChildren<Button>().onClick.AddListener(OnStartButtonClick);
        singlePlayerButton.GetComponentInChildren<Button>().onClick.AddListener(OnSinglePlayerButtonClick);
        multiplayerButton.GetComponentInChildren<Button>().onClick.AddListener(OnMultiplayerButtonClick);
        selectGameButton.GetComponentInChildren<Button>().onClick.AddListener(OnSelectGameButtonClick);
        endlessModeButton.GetComponentInChildren<Button>().onClick.AddListener(OnEndlessModeButtonClick);
        backButton.GetComponentInChildren<Button>().onClick.AddListener(OnBackButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

   
    public void OnStartButtonClick() 
    {
        startButton.SetActive(false);
        optionsButton.SetActive(false);
        quitButton.SetActive(false);
        singlePlayerButton.SetActive(true);
        multiplayerButton.SetActive(true);
    }

    public void OnSinglePlayerButtonClick()
    {
        
        Debug.Log("Single Player clicked!");
        singlePlayerButton.SetActive(false);
        multiplayerButton.SetActive(false);
        selectGameButton.SetActive(true);
        endlessModeButton.SetActive(true);
        backButton.SetActive(true);

    }

    public void OnMultiplayerButtonClick()
    {
        //playerMode.Add("Multiplayer");
    }

    public void OnSelectGameButtonClick()
    {

    }

    public void OnEndlessModeButtonClick()
    {

    }
    public void OnBackButtonClick()
    {

    }
}
