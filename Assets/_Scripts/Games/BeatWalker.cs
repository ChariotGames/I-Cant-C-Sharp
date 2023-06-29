using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Input;

namespace _Scripts.Games
{
    public class BeatWalker : Game
    {
        #region Serialized Fields
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject buttonContainer;


        #endregion Serialized Fields

        #region Fields
        private float leftBound;
        private float rightBound;
        private bool buttonPressed = false;
        private bool lost = false;
        private List<GameObject> buttons = new List<GameObject>();
        private float instantiationDelay = 2f;
        private float delayBetweenRhythm = 1f;
        private int instantiationCount = 0;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            //Physics2D.gravity = new Vector2(-9.8f, 0);

            //Get coords of activation region
            float regionSizeX = GameObject.Find("ActivationRegion").gameObject.GetComponent<Collider2D>().bounds.size.x;
            float regionPosX = GameObject.Find("ActivationRegion").gameObject.transform.position.x;

            leftBound = regionPosX - regionSizeX / 2;
            rightBound = regionPosX + regionSizeX / 2;
        }

        private void OnEnable()
        {
            InputHandler.LeftArrowBtnAction += ButtonPress;
        }

        void Start()
        {
            //instantiateButton();
            StartCoroutine(InstantiateButtonsWithDelay());


        }

        void Update()
        {
            if(!lost)
            {
                loseCondition();
            } 
            //TODO wird nach ein mal lose immer noch aufgerufen
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(buttonContainer.transform.position, new Vector3(4f, 5f, 0));
        }



        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        private void ButtonPress()
        {
            buttonPressed = true;
            Debug.Log("clicked!");
        }

        private void instantiateButton()
        {
            GameObject newButton = Instantiate(button, buttonContainer.transform.position, Quaternion.identity, buttonContainer.transform);
            buttons.Add(newButton);
            
        }

        private void loseCondition()
        {
           
            for (int i = 0; i < buttons.Count; i++)
            {
                float buttonPosX = buttonContainer.transform.GetChild(i).gameObject.transform.position.x;

                if (buttonPosX < leftBound)
                {
                    Debug.Log("Lose");
                    lost = true;
                    //buttons.RemoveAt(i);
                    DestroyAll();

                    buttonPressed = false;
                    StopGame();
                }
                else if (buttonPosX > rightBound && buttonPressed)
                {
                    Debug.Log("Lose");
                    lost = true;
                    //Destroy(buttons[i].gameObject);
                    //buttons.RemoveAt(i);
                    DestroyAll();
                    buttonPressed = false;
                    StopGame();

                }
                else if (buttonPosX > leftBound && buttonPosX < rightBound && buttonPressed)
                {
                    Debug.Log("Win");
                    lost = false;

                    Destroy(buttons[i].gameObject);
                    buttons.RemoveAt(i);
                    buttonPressed = false;
                }
            }
         }

        void StopGame()
        {
            Time.timeScale = 0;
        }

        private void DestroyAll()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i].gameObject);
                buttons.RemoveAt(i);
            }
        }

        private IEnumerator InstantiateButtonsWithDelay()
        {
            while (lost == false)
            {
                if(instantiationCount == 4)
                {
                    yield return new WaitForSeconds(delayBetweenRhythm);
                    instantiationCount = 0;
                }
                GameObject newButton = Instantiate(button, buttonContainer.transform.position, Quaternion.identity, buttonContainer.transform);

                buttons.Add(newButton);
                instantiationCount++;
                yield return new WaitForSeconds(instantiationDelay);
            }
        }

        private void createRhythm()
        {
            if (instantiationCount == 4)
            {

            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers



        #endregion Overarching Methods / Helpers
    }
}