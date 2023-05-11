using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    /// <summary>
    /// Game script that controls and simulates the 
    /// electronic guessing & memory game: Simon.
    /// 
    /// Color patterns are generated and displayed at random,
    /// each round adding a varying number of colors to the
    /// existing pattern.
    /// 
    /// On Level 1, each color is added once.
    /// On Level 2, there is a chance for a color to be added twice.
    /// On Level 3, a color is displayed but has to be skipped in guessing.
    /// </summary>
    public class G04_Simon : MonoBehaviour
    {
        [SerializeField] private GameObject buttons;
        [SerializeField] private List<Colors> displayPattern, guessPattern;
        [SerializeField] private const int MIN_LENGTH = 3, CHANCE = 3;
        [SerializeField] private Difficulty currentLevel = Difficulty.LVL1;
        private bool isPlayerTurn = false;
        int checkingIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ActivateButtons());
            GeneratePattern(MIN_LENGTH);
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// When the game starts, activates each button in order.
        /// </summary>
        private IEnumerator ActivateButtons()
        {
            foreach (Transform child in buttons.transform)
            {
                child.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.25f);
            }
        }

        /// <summary>
        /// This method reacts on user presses and checks
        /// the input against the guessing pattern.
        /// </summary>
        /// <param name="color">Enum of the color to check.</param>
        private void CheckColor(Colors color)
        {
            if (!isPlayerTurn)
            {
                return;
            }

            if (guessPattern[checkingIndex] == color)
            {
                Debug.Log("correct color");
                checkingIndex++;
            }
            else
            {
                Debug.Log("wrong color");
                isPlayerTurn = false;
                StartCoroutine(AnimateButtons());
            }

            if (checkingIndex >= guessPattern.Count)
            {
                checkingIndex = 0;
                isPlayerTurn = false;
                ToggleInput(isPlayerTurn);
                Debug.Log("guessing done");
                GeneratePattern(displayPattern.Count + 1);
            }
        }

        /// <summary>
        /// Generates a random color and adds it to the pattern.
        /// This is the pattern the user gets to see, but not guess!
        /// </summary>
        /// /// <param name="length">The length of the pattern</param>
        private void GeneratePattern(int length)
        {
            Colors randomColor;
            while (displayPattern.Count < length)
            {
                randomColor = (Colors)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Colors)).Length);
                displayPattern.Add(randomColor);
                SetGuessPattern(randomColor);
            }
            StartCoroutine(AnimateButtons());
        }

        /// <summary>
        /// Sets the actual pattern for the user to guess.
        /// Depending on the Level and Modifiers it may differ
        /// from the patern the user gets to actually see.
        /// </summary>
        /// <param name="color">Enum of the color to add to the pattern.</param>
        private void SetGuessPattern(Colors color)
        {
            int chance = UnityEngine.Random.Range(0, CHANCE);

            if (currentLevel == Difficulty.LVL3 && chance == 0)
            {
                // On Level 3 nothing gets added if the chance is right
                return;
            }

            if (currentLevel == Difficulty.LVL2 && chance == 0)
            {
                // On Level 2 the color is doubled
                guessPattern.Add(color);
            }

            guessPattern.Add(color);
        }

        /// <summary>
        /// Plays the animation of the buttons to play.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateButtons()
        {
            yield return new WaitForSeconds(0.20f * buttons.transform.childCount*2);

            foreach (Colors color in displayPattern)
            {
                SimonButton button = GameObject.Find(color.ToString()).GetComponent<SimonButton>();

                button.ButtonPressed();
                yield return new WaitForSeconds(1.0f);
            }

            isPlayerTurn = true;
            ToggleInput(isPlayerTurn);
        }

        private void ToggleInput(bool state)
        {
            foreach (Transform child in buttons.transform)
            {
                SimonButton button = child.gameObject.GetComponent<SimonButton>();
                if (state)
                {
                    button.AllowInput(child.gameObject.name);
                }else
                {
                    button.DisableInput(child.gameObject.name);
                }
            }
            
        }
    }
}

