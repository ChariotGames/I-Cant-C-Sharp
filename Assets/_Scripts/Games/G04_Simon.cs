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

        [SerializeField] private List<GameObject> buttons, icons;
        [SerializeField] private List<Simon> displayPattern, guessPattern, infoPattern;
        [SerializeField] private GameObject middle, overlay;
        [SerializeField] private Difficulty currentLevel = Difficulty.LVL1;
        private const int MIN_LENGTH = 3, CHANCE = 3;
        private bool isPlayerTurn = false;
        private int checkingIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ActivateButtons());
            GeneratePattern(MIN_LENGTH);
            StartCoroutine(AnimateButtons());
        }

        /// <summary>
        /// When the game starts, activates each button in order.
        /// </summary>
        private IEnumerator ActivateButtons()
        {
            yield return new WaitForSeconds(0.25f);
            middle.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
                button.GetComponent<SimonButton>().Animate();
                yield return new WaitForSeconds(0.25f);
            }
        }

        /// <summary>
        /// Restarts guessing process and disables player input.
        /// </summary>
        private void ResetTurn()
        {
            checkingIndex = 0;
            isPlayerTurn = false;
            overlay.SetActive(isPlayerTurn);
            ToggleInput(isPlayerTurn);
        }

        /// <summary>
        /// Generates a random color and adds it to the pattern.
        /// This is the pattern the user gets to see, but not guess!
        /// </summary>
        /// /// <param name="length">The length of the pattern</param>
        private void GeneratePattern(int length)
        {
            ResetTurn();

            if (displayPattern.Count == length) return;

            Simon randomColor;
            while (displayPattern.Count < length)
            {
                randomColor = (Simon)UnityEngine.Random.Range(0, buttons.Count);
                displayPattern.Add(randomColor);
                SetGuessPattern(randomColor);
            }
        }

        /// <summary>
        /// Sets the actual pattern for the user to guess.
        /// Depending on the Level and Modifiers it may differ
        /// from the patern the user gets to actually see.
        /// </summary>
        /// <param name="button">Enum of the color to add to the pattern.</param>
        private void SetGuessPattern(Simon button)
        {
            int chance = UnityEngine.Random.Range(0, CHANCE);

            if (currentLevel == Difficulty.LVL3 && chance < 1)
            {
                // On Level 3 nothing gets added if the chance is right
                infoPattern.Add(Simon.NONE);
                return;
            }

            infoPattern.Add(Simon.SAME);
            guessPattern.Add(button);

            if (currentLevel != Difficulty.LVL1 && chance > 1)
            {
                // On Level 2 the color is doubled
                infoPattern[^1] = Simon.DOUBLE;
                guessPattern.Add(button);
            }
        }

        /// <summary>
        /// Plays the animation of the buttons to play.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateButtons()
        {
            yield return new WaitForSeconds(0.20f * buttons.Count*2);

            for (int i = 0; i < displayPattern.Count; i++)
            {
                Simon button = displayPattern[i];
                Simon icon = infoPattern[i];
                buttons[(int)button].GetComponent<SimonButton>().Animate();
                if(icon != Simon.SAME) icons[(int)icon%icons.Count].GetComponent<SimonButton>().Animate();
                yield return new WaitForSeconds(1.0f);
            }

            isPlayerTurn = true;
            overlay.SetActive(isPlayerTurn);
            ToggleInput(isPlayerTurn);
        }

        /// <summary>
        /// Toggles if buttons are able to take input.
        /// Depending on the state if it is the player's turn.
        /// </summary>
        /// <param name="state">State of the player's turn.</param>
        private void ToggleInput(bool state)
        {
            foreach (GameObject button in buttons)
            {
                button.GetComponent<SimonButton>().ToggleInput(button, state);
            }
        }

        /// <summary>
        /// This method reacts on user presses and checks
        /// the input against the guessing pattern.
        /// </summary>
        /// <param name="color">The color to check.</param>
        private void CheckColor(Simon color)
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
                ResetTurn();
                StartCoroutine(AnimateButtons());
            }

            if (checkingIndex >= guessPattern.Count)
            {
                Debug.Log("guessing done");
                ClearInfoPattern();
                GeneratePattern(displayPattern.Count + 1);
                StartCoroutine(AnimateButtons());
            }
        }

        private void ClearInfoPattern()
        {
            GameObject icon = icons.Find(obj => obj.name.Equals("IconOk"));
            if (icon != null) icon.GetComponent<SimonButton>().Animate();

            for (int i = 0; i < infoPattern.Count; i++)
            {
                infoPattern[i] = Simon.SAME;
            }
        }
    }
}

