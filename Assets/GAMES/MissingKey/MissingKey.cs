using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Models;
using System.Collections;

namespace Scripts.Games
{
    public class MissingKey : BaseGame
    {
        [SerializeField] private List<GameObject> buttons, pattern;
        [SerializeField] private SpriteRenderer spriteWin;
        [SerializeField] private SpriteRenderer spriteLose;
        [SerializeField] private AudioSource sound;
        private int count = 3;

        //private GameObject answer;
        [SerializeField] private GameObject buttonContainer;
        private bool _playerPressed = false;
        private float _playfieldWidth;
        private const float ROUND_TIME = 5.0f;
        private Bounds _cameraViewportBounds;
        private Camera _mainCamera;
        private float _time = 0;
        private bool timerEnded;
        private int winCounter = 0;
        private int loseCounter = 0;
        private bool _isAnswerScreen;


        private void Awake()
        {
            _mainCamera = Camera.main;

            switch (Difficulty)
            {
                case Difficulty.EASY:
                    count = 3;
                    break;
                case Difficulty.MEDIUM:
                    count = 6;
                    break;
                case Difficulty.HARD:
                    count = 9;
                    break;
                default:
                    count = 3;
                    break;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                BasePressElement bpe = buttons[i].GetComponent<BasePressElement>();
                buttons[i].GetComponent<BasePressElement>().Button = _keys.All[i].Input;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //playfieldWidth = transform.GetComponentInChildren<RectTransform>().rect.width;

            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            _playfieldWidth = _cameraViewportBounds.size.x / 2;
            //Debug.Log(_cameraViewportBounds.max.x);
            GeneratePattern();
            DisplayPattern();

        }

        // Update is called once per frame
        void Update()
        {
            _time += Time.deltaTime;
            if (_time >= ROUND_TIME)
            {
                Debug.Log("Time is up!");
                timerEnded = true;

                //TimerEnded();

            }
            CheckWin();

        }

        private void TimerEnded()
        {

            _time = 0;

            if (loseCounter >= failsToLose)
            {
                base.Harder();
                Lose();
            }
            else
            {
                spriteLose.gameObject.SetActive(true);
                NextRound();
            }


            //loseDisplay.SetActive(true);
            //DeleteAll();
            //InputHandler.ArrowRight -= PlayerPress;
            //InputHandler.ArrowLeft -= PlayerPress;
            //InputHandler.ArrowUp -= PlayerPress;
            //InputHandler.ArrowDown -= PlayerPress;
        }

        // Subscribes to playerPress()
        private void OnEnable()
        {
            _keys.One.Input.action.performed += PlayerPress;
            _keys.Two.Input.action.performed += PlayerPress;
            _keys.Three.Input.action.performed += PlayerPress;
            _keys.Four.Input.action.performed += PlayerPress;
            //InputHandler.ArrowRight += PlayerPress;
            //InputHandler.ArrowLeft += PlayerPress;
            //InputHandler.ArrowUp += PlayerPress;
            //InputHandler.ArrowDown += PlayerPress;
        }

        private void OnDisable()
        {
            _keys.One.Input.action.performed -= PlayerPress;
            _keys.Two.Input.action.performed -= PlayerPress;
            _keys.Three.Input.action.performed -= PlayerPress;
            _keys.Four.Input.action.performed -= PlayerPress;
        }

        // Creates a new random pattern 
        private void GeneratePattern()
        {
            pattern.Clear();
            List<GameObject> usedButtons = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, buttons.Count);
                usedButtons.Add(buttons[randomIndex]);
            }

            
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, usedButtons.Count);
                pattern.Add(usedButtons[randomIndex]);
            }
        }

        private void DisplayPattern()
        {
            
            int count = pattern.Count;
            int x, y;
            //x = pattern.Count / 2;
            //y = pattern.Count - x;
            base.RunTimer(ROUND_TIME);
           
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(pattern[i], new Vector3(0, 0, 0), Quaternion.identity, buttonContainer.transform);
                obj.GetComponent<BasePressElement>().Button = pattern[i].GetComponent<BasePressElement>().Button;
                obj.SetActive(true);
            }

            _isAnswerScreen = true;
        }

        private bool Won()
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

        private void PlayerPress(InputAction.CallbackContext ctx)
        {
            if(_isAnswerScreen) {
                //Debug.Log("Clicked!");
                _playerPressed = true;
                _time = 0;
                sound.time = 0.7f;
                sound.Play();
                DeleteAll();
                _isAnswerScreen = false;
            }
        }

        private IEnumerator NextRound()
        {
            yield return new WaitForSeconds(1);
            spriteWin.gameObject.SetActive(false);
            spriteLose.gameObject.SetActive(false);

            if (buttonContainer.transform.childCount == 0)
            {
                GeneratePattern();
                DisplayPattern();
            }


        }

        private void ScoreOneUp()
        {
            //so oft gewonnen, neues Spiel und difficulty hochsetzen (?)
            if (base._successes >= successesToWin)
            {
              /**  Debug.Log("You won the game!");
                winCounter = 0;
                base.Harder();
                Win();*/
              base.Harder();
            }
            //base.ScoreUp();
            base.Success();
        }

        private void CheckWin()
        {
            if (_playerPressed)
            {

                if (!Won())
                {
                    //winCounter = 0;
                    //loseCounter++;
                    //Debug.Log("loseCounter: " + loseCounter);


                    failed();
                    
                }
                else
                {

                    //winCounter++;
                    //Debug.Log("winCounter: " + winCounter);
                    won();
                    //Success();

                }
                StartCoroutine(NextRound());
                _playerPressed = false;
            }
            else if (timerEnded)
            {
                timerEnded = false;
                _time = 0;
                //winCounter = 0;
                //loseCounter++;
                //Debug.Log("loseCounter: " + loseCounter);

                DeleteAll();
                failed();
                
                StartCoroutine(NextRound());
            }

            
        }

        private void won()
        {
            ScoreOneUp();

            spriteWin.gameObject.SetActive(true);
        }

        private void failed()
        {
            spriteLose.gameObject.SetActive(true);
            base._successes = 0;
            Fail();

            if (base._fails <= 0)
            {
                Debug.Log("Lost a heart!");
                loseCounter = 0;
                base.Easier();
                //Lose();


            }
        }
    }
}