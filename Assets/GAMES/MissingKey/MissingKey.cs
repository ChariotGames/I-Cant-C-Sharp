using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Models;
using System.Collections;
using Scripts.Controllers;

namespace Scripts.Games
{
    public class MissingKey : BaseGame
    {
        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private GameObject buttonContainer;
        [SerializeField] private List<GameObject> buttons, pattern;
        [SerializeField] private SpriteRenderer spriteWin;
        [SerializeField] private SpriteRenderer spriteLose;
        [SerializeField] private AudioSource sound;

        //private GameObject answer;
        private int count = 3;
        private bool _playerPressed = false;
        private const float ROUND_TIME = 5.0f;
        private Bounds _cameraViewportBounds;
        private Camera _mainCamera;
        private float _time = 0;
        private bool timerEnded;
        private int loseCounter = 0;
        private bool _isAnswerScreen, startGame;



        private void Awake()
        {
            _mainCamera = Camera.main;

            SetDifficulty();

            for (int i = 0; i < buttons.Count; i++)
            {
                BasePressElement bpe = buttons[i].GetComponent<BasePressElement>();
                buttons[i].GetComponent<BasePressElement>().Button = _keys.All[i].Input;
            }
        }

        // Start is called before the first frame update
         private IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());
            startGame = true;
            //playfieldWidth = transform.GetComponentInChildren<RectTransform>().rect.width;

            // _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            //_playfieldWidth = _cameraViewportBounds.size.x / 2;
            //Debug.Log(_cameraViewportBounds.max.x);
            GeneratePattern();
            DisplayPattern();

        }

        // Update is called once per frame
        void Update()
        {
            if (!startGame) return;
            _time += Time.deltaTime;
            if (_time >= ROUND_TIME)
            {
                Debug.Log("Time is up!");
                timerEnded = true;

                //TimerEnded();

            }
            CheckWin();

        }
        
        /*
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


        }
        */

        // Subscribes to playerPress()
        private void OnEnable()
        {
            _keys.One.Input.action.performed += PlayerPress;
            _keys.Two.Input.action.performed += PlayerPress;
            _keys.Three.Input.action.performed += PlayerPress;
            _keys.Four.Input.action.performed += PlayerPress;
        }

        private void OnDisable()
        {
            _keys.One.Input.action.performed -= PlayerPress;
            _keys.Two.Input.action.performed -= PlayerPress;
            _keys.Three.Input.action.performed -= PlayerPress;
            _keys.Four.Input.action.performed -= PlayerPress;
        }

        private protected override void SetDifficulty()
        {
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
        }
        
        private void UpdateDifficulty(Difficulty difficulty)
        {
            base.Difficulty = difficulty;
            SetDifficulty();
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
            base.Success();
        }

        private void CheckWin()
        {
            if (_playerPressed)
            {

                if (!Won())
                {
                    failed();
                }
                else
                {
                    won();
                }
                StartCoroutine(NextRound());
                _playerPressed = false;
            }
            else if (timerEnded)
            {
                timerEnded = false;
                _time = 0;
                
                DeleteAll();
                failed();
                
                StartCoroutine(NextRound());
            }

            
        }

        private void won()
        {
            //ScoreOneUp();
            base.Success();
            spriteWin.gameObject.SetActive(true);
        }

        private void failed()
        {
            spriteLose.gameObject.SetActive(true);
            Fail();
        }
    }
}