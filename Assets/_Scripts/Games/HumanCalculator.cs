using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class HumanCalculator : BaseGame
    {
        #region Fields

        [SerializeField] private TextMeshPro equationText;
        [SerializeField] private TextMeshPro leftAnswer;
        [SerializeField] private TextMeshPro rightAnswer;

        private int missingNumber;
        private int equationResult;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Start()
        {
            GenerateNewEquation();
        }

        
        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public void GenerateNewEquation()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    CreateRandomEquation(2, 6, 1, 1000, false);
                    break;
                case Difficulty.MEDIUM:
                    CreateRandomEquation(4, 8, 1, 1000, false);
                    break;
                case Difficulty.HARD:
                    CreateRandomEquation(2, 6, 1, 1000, true);
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

            equation.Append(numbers[0]);
            for (var i = 0; i < operators.Length; i++)
            {
                if (operators[i] == '*' && i > 0 && operators[i - 1] != '*')
                {
                    equation.Insert(0, "(").Append(")");
                }
                equation.Append(" ").Append(operators[i]).Append(" ").Append(numbers[i + 1]);
            }

            missingNumber = numbers[Random.Range(0, equationLength)];
            
            Debug.Log(missingNumber);

            equationResult = CalculateEquationResult(numbers, operators);

            equationText.text = equation.ToString().Replace(missingNumber.ToString(), "?") + " = " + equationResult;
            
            DisplayAnswers();
        }

        private int CalculateEquationResult(int[] numbers, char[] operators)
        {
            int result = numbers[0];

            for (var i = 0; i < operators.Length; i++)
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
            var randomNumOffset = Random.Range(1, 5); 

            if (randomCorrectPos == 0)
            {
                leftAnswer.text = missingNumber.ToString();
                rightAnswer.text = (missingNumber - randomNumOffset).ToString();
            }
            else
            {
                leftAnswer.text = (missingNumber - randomNumOffset).ToString();
                rightAnswer.text = missingNumber.ToString();
            }
        }

        public void CheckAnswer(string selectedAnswer)
        {
            if (selectedAnswer == missingNumber.ToString())
            {
                Debug.Log("Correct");
            }
            else
            {
                Debug.Log("Wrong");
            }
        }

        #endregion Game Mechanics / Methods
    }
}
