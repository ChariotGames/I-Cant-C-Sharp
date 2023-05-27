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
    private string playerMode;
    private bool isClicked;

    private void Awake()
    {
        singlePlayerButton.SetActive(false);
        multiplayerButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
        startButton.GetComponentInChildren<Button>().onClick.AddListener(OnStartButtonClick);
        singlePlayerButton.GetComponentInChildren<Button>().onClick.AddListener(OnSinglePlayerButtonClick);
        multiplayerButton.GetComponentInChildren<Button>().onClick.AddListener(OnMultiplayerButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Doesn't work .___.
       /* if (isClicked)
        {
            startButton.SetActive(false);
            optionsButton.SetActive(false);
            quitButton.SetActive(false);
            singlePlayerButton.SetActive(true);
            multiplayerButton.SetActive(true);
            isClicked = false;
        }*/
    }

   
    public void OnStartButtonClick() 
    {
        Debug.Log("Clicked Start!");
        //isClicked = true;
        startButton.SetActive(false);
        optionsButton.SetActive(false);
        quitButton.SetActive(false);
        singlePlayerButton.SetActive(true);
        multiplayerButton.SetActive(true);
    }

    private void OnSinglePlayerButtonClick()
    {
        playerMode = "Single Player";
    }

    private void OnMultiplayerButtonClick()
    {
        playerMode = "Multiplayer";
    }
}
