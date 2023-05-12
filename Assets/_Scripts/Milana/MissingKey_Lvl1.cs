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
    private GameObject ButtonContainer;
    bool playerPressed = false;
    

    // Start is called before the first frame update
    void Start()
    {
        generatePattern();
        displayPattern();
    }

    // Update is called once per frame
    void Update()
    {
        nextRound();
    }

    // Subscribes to playerPress()
    private void OnEnable()
    {
        InputHandler.RightArrowBtnAction += playerPress;
        InputHandler.LeftArrowBtnAction += playerPress;
        InputHandler.UpArrowBtnAction += playerPress;
        InputHandler.DownArrowBtnAction += playerPress;

    }

    // Creates a new random pattern 
    private void generatePattern()
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

    private void displayPattern()
    {

        float canvasWidth = 1920;
        int count = pattern.Count;
        float step = canvasWidth/(count+1); // Schrittlänge

        for(int i=0; i<count; i++)
        {
            float buttonPosX = pattern[i].transform.localScale.x + step * (count + 1) - canvasWidth / 2;
            Vector3 newPosition = ButtonContainer.transform.position + new Vector3(step * (count + 1) - canvasWidth / 2, 0, 0);
            //float offset = pattern[i].transform.localScale.x  + startPoint * i;
            Instantiate(pattern[i], newPosition, Quaternion.identity, ButtonContainer.transform);
        }
    }

    private bool checkWin()
    {
        
            //go through each button and check if one was deactivated
            for (int i = 0; i < ButtonContainer.transform.childCount; i++)
            {
               
                bool isActive = ButtonContainer.transform.GetChild(i).gameObject.activeInHierarchy;

                //a button on the screen was deactivated -> lose 
                if (!isActive)
                {
                    return false;
                }
                
            }
            
        
        return true;
    }

    private void deleteAll()
    {
        for (int i = 0; i < ButtonContainer.transform.childCount; i++)
        {
            Destroy(ButtonContainer.transform.GetChild(i).gameObject);
        }
    }

    private void playerPress()
    {
        playerPressed = true;
    }

    private void nextRound()
    {
        if (playerPressed)
        {
            deleteAll();
            if (checkWin())
            {
                generatePattern();
                displayPattern();
                
            }
            else
            {
                loseDisplay.SetActive(true);
                InputHandler.RightArrowBtnAction -= playerPress;
                InputHandler.LeftArrowBtnAction -= playerPress;
                InputHandler.UpArrowBtnAction -= playerPress;
                InputHandler.DownArrowBtnAction -= playerPress;
            }
        }
        playerPressed = false;
    }

}
