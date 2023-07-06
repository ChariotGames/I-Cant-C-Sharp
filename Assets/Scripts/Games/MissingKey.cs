using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
{
    public class MissingKey : BaseGame
    {
        [SerializeField] private List<GameObject> buttons, pattern;
        [SerializeField] private int count = 3;

        //private GameObject answer;
        [SerializeField] private GameObject loseDisplay, buttonContainer;
        private bool _playerPressed = false;
        private float _playfieldWidth;
        private const float ROUND_TIME = 5.0f;
        private Bounds _cameraViewportBounds;
        private Camera _mainCamera;
        private float _time = 0;
        private bool timerEnded;


        private void Awake()
        {
            _mainCamera = Camera.main;
            for (int i = 0; i < buttons.Count; i++)
            {
                BasePressElement bpe = buttons[i].GetComponent<BasePressElement>();
                buttons[i].GetComponent<BasePressElement>().Button = keys.All[i].Input;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //playfieldWidth = transform.GetComponentInChildren<RectTransform>().rect.width;
            
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            _playfieldWidth = _cameraViewportBounds.size.x/2;
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
                //timerEnded = true;
                TimerEnded();
            }

            NextRound();
        }

        private void TimerEnded()
        {
            _time = 0;
            Lose();
            
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
            keys.One.Input.action.performed += PlayerPress;
            keys.Two.Input.action.performed += PlayerPress;
            keys.Three.Input.action.performed += PlayerPress;
            keys.Four.Input.action.performed += PlayerPress;
            //InputHandler.ArrowRight += PlayerPress;
            //InputHandler.ArrowLeft += PlayerPress;
            //InputHandler.ArrowUp += PlayerPress;
            //InputHandler.ArrowDown += PlayerPress;
        }

        private void OnDisable()
        {
            keys.One.Input.action.performed -= PlayerPress;
            keys.Two.Input.action.performed -= PlayerPress;
            keys.Three.Input.action.performed -= PlayerPress;
            keys.Four.Input.action.performed -= PlayerPress;
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

        private void PlayerPress(InputAction.CallbackContext ctx)
        {
            _playerPressed = true;
            _time = 0;
        }

        private void NextRound()
        {
            if (_playerPressed)
            {
                DeleteAll();
                if (CheckWin())
                {

                    Win();
                    GeneratePattern();
                    DisplayPattern();
                }
                else
                {
                    Lose();
                    GeneratePattern();
                    DisplayPattern();
                    //loseDisplay.SetActive(true);
                    //InputHandler.ArrowRight -= PlayerPress;
                    //InputHandler.ArrowLeft -= PlayerPress;
                    //InputHandler.ArrowUp -= PlayerPress;
                    //InputHandler.ArrowDown -= PlayerPress;
                   
                    
                }
            }
            _playerPressed = false;
        }
    }
}