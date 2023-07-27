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

            /**
            for(int i=0; i<buttons.Count; i++)
            {
                if(!usedButtons.Contains(buttons[i]))
                {
                    answer = buttons[i];
                }
            }
            */
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, usedButtons.Count);
                pattern.Add(usedButtons[randomIndex]);
            }
        }

        private void DisplayPattern()
        {
            //float canvasWidth = 1920;
            int count = pattern.Count;
            int x, y;
            x = pattern.Count / 2;
            y = pattern.Count - x;
            base.RunTimer(ROUND_TIME);
            //for (int c = 0; c < count; c++)
            //{

            //    GameObject obj = Instantiate(pattern[c], buttonContainer.transform);

            //    for (int i = -x; i <= x; i++)
            //    {
            //        for (int j = -y; j <= y; j++)
            //        {
            //            //float buttonPosX = pattern[i].transform.localScale.x - 1 + x * (i + 1) - _playfieldWidth / 2;


            //            obj.transform.Translate(x * 1.5f, y * 1.5f, 0);
            //        }
            //    }
            //}

            //float step = _playfieldWidth / (count + 1); // Schrittlänge
            for (int i = 0; i < count; i++)
            {

                // float buttonPosX = pattern[i].transform.localScale.x - 1 + step * (i + 1) - _playfieldWidth / 2;

                //Vector3 newPosition = ButtonContainer.transform.position + new Vector3(step * (count + 1) - playfieldWidth / 2, 0, 0);
                //float offset = pattern[i].transform.localScale.x  + startPoint * i;
                GameObject obj = Instantiate(pattern[i], new Vector3(0, 0, 0), Quaternion.identity, buttonContainer.transform);
                obj.GetComponent<BasePressElement>().Button = pattern[i].GetComponent<BasePressElement>().Button;
                obj.SetActive(true);
            }
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
            //Debug.Log("Clicked!");
            _playerPressed = true;
            _time = 0;
            sound.time = 0.7f;
            sound.Play();
            DeleteAll();
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
                    winCounter = 0;
                    loseCounter++;
                    Debug.Log("loseCounter: " + loseCounter);


                    spriteLose.gameObject.SetActive(true);
                    Fail();
                    
                    if (base._fails <= 0)
                    {
                        Debug.Log("Lost a heart!");
                        loseCounter = 0;
                        base.Easier();
                        //Lose();
                        

                    }
                    
                }
                else
                {

                    winCounter++;
                    Debug.Log("winCounter: " + winCounter);
                    ScoreOneUp();

                    spriteWin.gameObject.SetActive(true);
                    //Success();

                }
                StartCoroutine(NextRound());
                _playerPressed = false;
            }
            else if (timerEnded)
            {
                timerEnded = false;
                _time = 0;
                winCounter = 0;
                loseCounter++;
                Debug.Log("loseCounter: " + loseCounter);

                DeleteAll();
                spriteLose.gameObject.SetActive(true);
                Fail();
                
                if (base._fails <= 0)
                {
                    Debug.Log("Lost a heart!");
                    loseCounter = 0;
                    base.Easier();
                    
                }
                
                StartCoroutine(NextRound());
            }
        }
    }
}