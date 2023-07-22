using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Scripts.Models;


namespace Scripts.Games

{
    public class HumanCalculator : BaseGame
    {
        #region Fields

        [SerializeField] private TextMeshPro equationText;
        [SerializeField] private TextMeshPro leftAnswer;
        [SerializeField] private TextMeshPro rightAnswer;
        [SerializeField] private SpriteRenderer correctAnswer;
        [SerializeField] private SpriteRenderer wrongAnswer;

        private int _missingNumber;
        private int _equationResult;
        private int _remainingLives = 3;
        private float _elapsedTime;
        private float _timeoutStemp;
        private bool _isAnswerScreen;
        private int _currentScore;
        
        private const float _timeoutDelay = 15f;


        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            leftAnswer.GetComponent<BasePressElement>().Button = _keys.One.Input;
            rightAnswer.GetComponent<BasePressElement>().Button = _keys.Two.Input;
        }

        private void Start()
        {
            StartCoroutine(GenerateNewEquation());
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_isAnswerScreen && _elapsedTime >= _timeoutStemp + _timeoutDelay)
            {
                _isAnswerScreen = false;
                CheckAnswer("");
                
            }
            
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public IEnumerator GenerateNewEquation()
        {
            yield return new WaitForSeconds(1f);
            
            wrongAnswer.gameObject.SetActive(false);
            correctAnswer.gameObject.SetActive(false);
            
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    CreateRandomEquation(2, 2, 1, 10, false);
                    break;
                case Difficulty.MEDIUM:
                    CreateRandomEquation(2, 2, 1, 50, false);
                    break;
                case Difficulty.HARD:
                    CreateRandomEquation(3, 3, 1, 100, false);
                    break;
            }
            
        }
        private void CreateRandomEquation(int minEquationLength, int maxEquationLength, int minNumber, int maxNumber, bool allowMultiplication)
        {
            var equationLength = Random.Range(minEquationLength, maxEquationLength);
            var numbers = new int[equationLength];
            var operators = new char[equationLength - 1];
            var equation = new StringBuilder();

            for (var i = 0; i < equationLength; i++)
            {
                numbers[i] = Random.Range(minNumber, maxNumber);
            }

            for (var i = 0; i < operators.Length; i++)
            {
                var operatorIndex = Random.Range(0, allowMultiplication ? 3 : 2);

                switch (operatorIndex)
                {
                    case 0:
                        operators[i] = '+';
                        break;
                    case 1:
                        operators[i] = '-';
                        break;
                    case 2:
                        operators[i] = '*';
                        break;
                }
            }

            _missingNumber = numbers[Random.Range(0, equationLength)];

            Debug.Log(_missingNumber);

            _equationResult = CalculateEquationResult(numbers, operators);

            var missingNumberReplaced = false; 

            for (var i = 0; i < equationLength; i++)
            {
                if (numbers[i] == _missingNumber && !missingNumberReplaced)
                {
                    equation.Append(" ? ");
                    missingNumberReplaced = true;
                }
                else
                {
                    equation.Append(numbers[i]);
                }

                if (i < operators.Length)
                {
                    equation.Append(" ").Append(operators[i]).Append(" ");
                }
            }

            equationText.text = equation + " = " + _equationResult;

            DisplayAnswers();
        }
        
        private int CalculateEquationResult(IReadOnlyList<int> numbers, IReadOnlyList<char> operators)
        {
            var result = numbers[0];

            for (var i = 0; i < operators.Count; i++)
            {
                switch (operators[i])
                {
                    case '+':
                        result += numbers[i + 1];
                        break;
                    case '-':
                        result -= numbers[i + 1];
                        break;
                    case '*':
                        result *= numbers[i + 1];
                        break;
                }
            }

            return result;
        }
        
        private void DisplayAnswers()
        {
            var randomCorrectPos = Random.Range(0, 2);
            var randomNumOffset = Random.Range(1, 21);
            _isAnswerScreen = true;
            _timeoutStemp = _elapsedTime;
            if (randomCorrectPos == 0)
            {
                leftAnswer.text = _missingNumber.ToString();
                rightAnswer.text = (_missingNumber - randomNumOffset).ToString();
            }
            else
            {
                leftAnswer.text = (_missingNumber - randomNumOffset).ToString();
                rightAnswer.text = _missingNumber.ToString();
            }
        }

        public void CheckAnswer(string selectedAnswer)
        {
            if (selectedAnswer == _missingNumber.ToString())
            {
                Debug.Log("Correct");
                correctAnswer.gameObject.SetActive(true);
                _currentScore++;
                base.ScoreUp();
                base.AnimateWin(_currentScore, 5);
                if (_currentScore >= 5)
                {
                    _currentScore = 0;
                    base.Harder();
                    base.Win();
                }
                
            }
            else {
                Debug.Log("Wrong");
                _remainingLives--;
                wrongAnswer.gameObject.SetActive(true);
                base.ScoreDown();
                base.AnimateLose(_remainingLives , 3);
                if (_remainingLives <= 0)
                {
                    Debug.Log("GAME LOST");
                    _remainingLives = 3;
                    base.Easier();
                    base.Lose();
                }
                
            }
            StartCoroutine(GenerateNewEquation());
        }

        #endregion Game Mechanics / Methods
    }
}
