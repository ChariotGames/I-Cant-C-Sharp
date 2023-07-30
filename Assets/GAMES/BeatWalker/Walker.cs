
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts._Input;
using Scripts.Games;
using Scripts.Models;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

namespace Scripts.Games
{
    public class Walker : BaseGame
    {
        #region Serialized Fields
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject buttonContainer;
        [SerializeField] private SpriteRenderer spriteWin;
        [SerializeField] private SpriteRenderer spriteLose;
        [SerializeField] private Settings settings;


        #endregion Serialized Fields

        #region Fields
        private float lowerBound;
        private float upperBound;
        private bool buttonPressed = false;
        private bool lost = false;
        private List<GameObject> buttons = new();
        private float instantiationDelay = 1f;
        private float delayBetweenRhythm = 0.5f;
        private int instantiationCount = 0;
        private float _time = 0;
        private List<float> rhythmDelay = new();
        private int repeatNumber; //number of times the rhythm is being repeated
        private float minRange;
        private float maxRange;
        
        

        #endregion Fields

        #region Built-Ins / MonoBehaviours



        private void Awake()
        {
            //button.GetComponent<BasePressElement>().Button = Keys.One.Input;
            //Physics2D.gravity = new Vector2(-9.8f, 0);

            successesToWin = 16;
            failsToLose = 4;
            //Get coords of activation region
            float regionSizeY = GameObject.Find("ActivationRegion").GetComponent<Collider2D>().bounds.size.y;
            float regionPosY = GameObject.Find("ActivationRegion").transform.position.y;

            lowerBound = regionPosY - regionSizeY / 2;
            upperBound = regionPosY + regionSizeY / 2;
            //button.GetComponent<BasePressElement>().Button = _keys.One.Input;
            
            switch(Difficulty)
                {
                    case Difficulty.EASY:
                        PlayerPrefs.SetFloat("speed", 3f);
                        repeatNumber = 2;
                        minRange = 0.5f;
                        maxRange = 2.0f;
                        
                        break;
                    case Difficulty.MEDIUM:
                        PlayerPrefs.SetFloat("speed", 10f);
                        repeatNumber = 4;
                        minRange = 0.1f;
                        maxRange = 0.5f;
                        
                        break;
                    case Difficulty.HARD:
                        PlayerPrefs.SetFloat("speed", 15f);
                    repeatNumber = 8;
                        minRange = 0.0f;
                        maxRange = 0.2f;
                        
                        break;
                    default:
                        PlayerPrefs.SetFloat("speed", 3f);
                        minRange = 0.5f;
                        maxRange = 2.0f;
                        
                        break;
                }
          
        }

        private void OnEnable()
        {
            //InputHandler.ArrowDown += ButtonPress;
            _keys.One.Input.action.performed += ButtonPress;
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());
            //instantiateButton();
            StartCoroutine(InstantiateButtonsWithDelay());
           

        }

        void Update()
        {
            _time += Time.deltaTime;
            //if (!lost)
            //{
            CheckWin();
            //}
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

        

        private void instantiateButton()
        {
            GameObject newButton = Instantiate(button, buttonContainer.transform.position, Quaternion.identity, buttonContainer.transform);
            buttons.Add(newButton);

        }

        
        private void CheckWin()
        {

            for (int i = 0; i < buttons.Count; i++)
            {
                float buttonPosY = buttonContainer.transform.GetChild(i).gameObject.transform.position.y;

                if (buttonPosY < lowerBound)
                {
                    
                    removeClickedButton(i);
                    failed();

                }
                else if (buttonPosY > upperBound && buttonPressed)
                {
                    removeClickedButton(i);
                    failed();
                    
                }
                else if (buttonPosY > lowerBound && buttonPosY < upperBound && buttonPressed)
                {
                    removeClickedButton(i);
                    won();
                    
                }
            }
        }

        private void ButtonPress(InputAction.CallbackContext ctx)
        {
            buttonPressed = true;
            Debug.Log("clicked!");
        }

        private void failed()
        {

            if(base._fails <= 0)
            {
                base.Easier();
            }
            base._successes = 0;
            Debug.Log("Lose");
            lost = true;
            Fail();
            var loseSprite = Instantiate(spriteLose.gameObject, GameObject.Find("ActivationRegion").transform.position, Quaternion.identity, transform.parent);// Instantiate(spriteLose.gameObject, newButton.transform.position, Quaternion.identity, transform.parent);
            loseSprite.SetActive(true);
            Destroy(loseSprite, 1);
        }

        private void won()
        {

            if(base._successes >= 32) 
            {
                base.Harder();
            }
            Debug.Log("Win");
            lost = false;
            Success();
            var winSprite = Instantiate(spriteWin.gameObject, GameObject.Find("ActivationRegion").transform.position, Quaternion.identity, transform.parent);
            winSprite.SetActive(true);
            Destroy(winSprite, 0.5f);
        }


        void removeClickedButton(int i)
        {
            Destroy(buttons[i].gameObject);
            buttons.RemoveAt(i);
            buttonPressed = false;
        }

        
        private IEnumerator InstantiateButtonsWithDelay()
        {
            createRhythm();
            int counter = 0;
            while (true)
            {
                if (instantiationCount == 4 * repeatNumber)
                {
                    //yield return new WaitForSeconds(delayBetweenRhythm);
                    createRhythm();
                    instantiationCount = 0;
                }
                GameObject newButton = Instantiate(button, buttonContainer.transform.position, Quaternion.identity, buttonContainer.transform);
                newButton.GetComponent<SpriteRenderer>().sprite = settings.SelectedCharacter.Preview;
                newButton.GetComponent<SpriteRenderer>().flipX = true;
                //newButton.GetComponent<BasePressElement>().Button = button.GetComponent<BasePressElement>().Button;
                newButton.SetActive(true);


                buttons.Add(newButton);
                instantiationCount++;

                
                yield return new WaitForSeconds(rhythmDelay[counter]);
                counter++;
                if (counter >= 4) counter = 0;
            }
        }

        private void createRhythm()
        {
            rhythmDelay.Clear();
            for (int i = 0; i < 4; i++)
            {
                rhythmDelay.Add(Random.Range(0.25f, 1.5f));
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

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


        #endregion Overarching Methods / Helpers
    }
}