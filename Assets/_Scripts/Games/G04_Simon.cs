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

        [SerializeField] private List<GameObject> buttons, infos;
        [SerializeField] private List<Simon> displayPattern, guessPattern, infoPattern;
        [SerializeField] private GameObject middle, overlay;
        private Difficulty CurrentLevel { get; set; } = Difficulty.LVL1;
        private Color middleOriginal;

        private const float WAIT_TIME = 0.50f;
        private const int MIN_LENGTH = 3, CHANCE = 3, LVL_CHANGE = 5;
        private int checkingIndex = 0, correctGuesses = 0;
        private bool isPlayerTurn = false;

        // Start is called before the first frame update
        void Start()
        {
            middleOriginal = middle.GetComponent<SpriteRenderer>().color;
            StartCoroutine(ActivateButtons());
            GeneratePattern(MIN_LENGTH);
            StartCoroutine(AnimateButtons(buttons));
        }

        /// <summary>
        /// When the game starts, activates each button in order.
        /// </summary>
        private IEnumerator ActivateButtons()
        {
            yield return new WaitForSeconds(WAIT_TIME);
            middle.SetActive(true);
            yield return new WaitForSeconds(WAIT_TIME);
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
                button.GetComponent<SimonButton>().Animate();
                yield return new WaitForSeconds(WAIT_TIME);
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
            middle.GetComponent<SpriteRenderer>().color = middleOriginal;
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

            if (CurrentLevel == Difficulty.LVL3 && chance < 1)
            {
                // On Level 3 nothing gets added if the chance is right
                infoPattern.Add(Simon.NONE);
                return;
            }

            infoPattern.Add(Simon.SAME);
            guessPattern.Add(button);

            if (CurrentLevel != Difficulty.LVL1 && chance > 1)
            {
                // On Level 2 the color is doubled
                infoPattern[^1] = Simon.DOUBLE;
                guessPattern.Add(button);
            }
        }

        /// <summary>
        /// Plays the animation of the buttons to play.
        /// </summary>
        /// <param name="buttons">The list of objects to animate.</param>
        /// <returns>An object that can be used to control the coroutine's execution.</returns>
        private IEnumerator AnimateButtons(List<GameObject> buttons)
        {
            yield return new WaitForSeconds(WAIT_TIME * buttons.Count*2);

            for (int i = 0; i < displayPattern.Count; i++)
            {
                Simon button = displayPattern[i];
                Simon info = infoPattern[i];
                buttons[(int)button].GetComponent<SimonButton>().Animate();
                if (info != Simon.SAME)
                {
                    GameObject icon = infos[(int)info % 4];
                    icon.GetComponent<SimonButton>().Animate();
                }
                yield return new WaitForSeconds(WAIT_TIME * buttons.Count);
            }

            isPlayerTurn = true;
            overlay.SetActive(isPlayerTurn);
            middle.GetComponent<SpriteRenderer>().color = middleOriginal + middleOriginal;
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
            if (guessPattern[checkingIndex] == color)
            {
                checkingIndex++; //correct color
            }
            else //wrong color
            {
                GameObject icon = infos.Find(obj => obj.name.Equals("IconNone"));
                if (icon != null) icon.GetComponent<SimonButton>().Animate();
                correctGuesses-=(int)CurrentLevel+1;
                ResetTurn();
                StartCoroutine(AnimateButtons(buttons));
            }

            if (checkingIndex >= guessPattern.Count)
            {
                //guessing done
                correctGuesses++;
                UpdateLevel(correctGuesses);
                ClearInfoPattern();
                GeneratePattern(displayPattern.Count + 1);
                StartCoroutine(AnimateButtons(buttons));
            }
        }

        /// <summary>
        /// Clears the info pattern for next round, obscuring previous hints.
        /// </summary>
        private void ClearInfoPattern()
        {
            GameObject icon = infos.Find(obj => obj.name.Equals("IconOk"));
            if (icon != null) icon.GetComponent<SimonButton>().Animate();

            for (int i = 0; i < infoPattern.Count; i++)
            {
                infoPattern[i] = Simon.SAME;
            }
        }

        /// <summary>
        /// Updates the game level for next spawn and messages to the GameController.
        /// </summary>
        private void UpdateLevel(int rounds)
        {
            if(rounds <= 0)
            {
                SendMessageUpwards("SetLevel", new object[] { gameObject, Difficulty.LVL1 });
                correctGuesses = 0;
            }

            if(rounds >= LVL_CHANGE)
            {
                SendMessageUpwards("SetLevel", new object[] { gameObject, (Difficulty)(rounds/ LVL_CHANGE) });
            }
        }
    }
}

