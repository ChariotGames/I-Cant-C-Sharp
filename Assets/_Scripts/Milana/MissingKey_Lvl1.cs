using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Input;


public class MissingKey_Lvl1 : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buttons;
    [SerializeField]
    private List<GameObject> pattern;
    [SerializeField]
    private int count = 3;
    //private GameObject answer;
    [SerializeField]
    private GameObject loseDisplay;
    [SerializeField]
    private GameObject buttonContainer;
    private bool _playerPressed = false;
    private float _playfieldWidth;
    private Bounds _cameraViewportBounds;
    private Camera _mainCamera;


    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        //playfieldWidth = transform.GetComponentInChildren<RectTransform>().rect.width;
        _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
        _playfieldWidth = _cameraViewportBounds.size.x;
        //Debug.Log(_cameraViewportBounds.max.x);
        GeneratePattern();
        DisplayPattern();
    }

    // Update is called once per frame
    void Update()
    {
        NextRound();
    }

    // Subscribes to playerPress()
    private void OnEnable()
    {
        InputHandler.RightArrowBtnAction += PlayerPress;
        InputHandler.LeftArrowBtnAction += PlayerPress;
        InputHandler.UpArrowBtnAction += PlayerPress;
        InputHandler.DownArrowBtnAction += PlayerPress;

    }

    // Creates a new random pattern 
    private void GeneratePattern()
    {
        pattern.Clear();
        List<GameObject> usedButtons = new List<GameObject>();
        for (int i=0; i<3; i++)
        {
            int randomIndex = Random.Range(0, buttons.Count);
            usedButtons.Add(buttons[randomIndex]);
        }

        /**
        for(int i=0; i<buttons.Count; i++)
        {
            if(!usedButtons.Contains(buttons[i]))
            {
                answer = buttons[i];
            }
        }
        */
        for(int i=0; i<count; i++)
        {
            int randomIndex = Random.Range(0, usedButtons.Count);
            pattern.Add(usedButtons[randomIndex]);
        }
    }

    private void DisplayPattern()
    {

        //float canvasWidth = 1920;
        int count = pattern.Count;
        float step = _playfieldWidth/(count+1); // Schrittlänge
        for(int i=0; i<count; i++)
        {
            float buttonPosX = pattern[i].transform.localScale.x - 1 + step * (i + 1) - _playfieldWidth / 2;
            
            //Vector3 newPosition = ButtonContainer.transform.position + new Vector3(step * (count + 1) - playfieldWidth / 2, 0, 0);
            //float offset = pattern[i].transform.localScale.x  + startPoint * i;
            Instantiate(pattern[i], new Vector3(buttonPosX, 0, 0), Quaternion.identity, buttonContainer.transform);
        }
    }

    private bool CheckWin()
    {
        
            //go through each button and check if one was deactivated
            for (int i = 0; i < buttonContainer.transform.childCount; i++)
            {
               
                bool isActive = buttonContainer.transform.GetChild(i).gameObject.activeInHierarchy;

                //a button on the screen was deactivated -> lose 
                if (!isActive)
                {
                    return false;
                }
                
            }
            
        
        return true;
    }

    private void DeleteAll()
    {
        for (int i = 0; i < buttonContainer.transform.childCount; i++)
        {
            Destroy(buttonContainer.transform.GetChild(i).gameObject);
        }
    }

    private void PlayerPress()
    {
        _playerPressed = true;
    }

    private void NextRound()
    {
        if (_playerPressed)
        {
            DeleteAll();
            if (CheckWin())
            {
                GeneratePattern();
                DisplayPattern();
                
            }
            else
            {
                loseDisplay.SetActive(true);
                InputHandler.RightArrowBtnAction -= PlayerPress;
                InputHandler.LeftArrowBtnAction -= PlayerPress;
                InputHandler.UpArrowBtnAction -= PlayerPress;
                InputHandler.DownArrowBtnAction -= PlayerPress;
            }
        }
        _playerPressed = false;
    }

}
