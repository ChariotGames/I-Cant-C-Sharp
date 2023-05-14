#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

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
        #region Serialized Fields

        [SerializeField] private List<GameObject> buttons, infos;
        [SerializeField] private List<Simon> displayPattern, guessPattern, infoPattern;
        [SerializeField] private GameObject middle, inputOverlay;
        [SerializeField] private Image timer;

        #endregion

        #region Fields

        private Difficulty CurrentLevel { get; set; } = Difficulty.LVL1;

        private const float WAIT_TIME = 0.50f, TURN_TIME = 5.0f;
        private const int MIN_LENGTH = 3, CHANCE = 3, LVL_CHANGE = 5;
        private int checkingIndex = 0, correctGuesses = 0;

        #endregion

        #region Overarching Game Mechanics

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ActivateButtons());
            GeneratePattern(MIN_LENGTH);
            StartCoroutine(AnimateButtons(buttons));
        }

        /// <summary>
        /// Restarts guessing process and disables player input.
        /// </summary>
        private void ResetTurn()
        {
            checkingIndex = 0;
            timer.fillAmount = 0f;
            PlayerTurn(false);
        }

        /// <summary>
        /// Updates the game difficulty for next spawn and messages to the GameController.
        /// </summary>
        private void UpdateDifficulty(int rounds)
        {
            if (rounds <= 0)
            {
                SendMessageUpwards("SetDifficulty", new object[] { gameObject, Difficulty.LVL1 });
                correctGuesses = 0;
            }

            if (rounds >= LVL_CHANGE)
            {
                SendMessageUpwards("SetDifficulty", new object[] { gameObject, (Difficulty)(rounds / LVL_CHANGE) });
            }
        }

        #endregion

        #region Pattern Setups

        /// <summary>
        /// Generates a random color and adds it to the pattern.
        /// This is the pattern the user gets to see, but not guess!
        /// </summary>
        /// /// <param name="length">The length of the pattern</param>
        private void GeneratePattern(int length)
        {
            ResetTurn();

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
            if(displayPattern.Count <= 3)
            {
                // Only do the extra difficulty after the 3rd round!
                infoPattern.Add(Simon.SAME);
                guessPattern.Add(button);
                return;
            }

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
        /// Toggles if buttons are able to take input.
        /// Depending on the state if it is the player's turn.
        /// </summary>
        /// <param name="isPlayersTurn">State of the player's turn.</param>
        private void PlayerTurn(bool isPlayersTurn)
        {
            foreach (GameObject button in buttons)
            {
                button.GetComponent<SimonButton>().ToggleInput(button, isPlayersTurn);
            }
            inputOverlay.SetActive(isPlayersTurn);
        }

        #endregion

        #region Animations

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
        /// Plays the animation of the buttons to play.
        /// </summary>
        /// <param name="buttons">The list of objects to animate.</param>
        /// <returns>An object that can be used to control the coroutine's execution.</returns>
        private IEnumerator AnimateButtons(List<GameObject> buttons)
        {
            yield return new WaitForSeconds(WAIT_TIME * buttons.Count * 2);

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

            PlayerTurn(true);
            StartCoroutine(AnimateTimer());
        }

        private IEnumerator AnimateTimer()
        {
            float elapsedTime = 0f;
            while (elapsedTime < TURN_TIME)
            {
                float progress = elapsedTime / TURN_TIME;
                timer.fillAmount = Mathf.Clamp01(progress);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            timer.fillAmount = 1f;
            CheckColor(Simon.NONE);
        }

        #endregion

        #region Game Checks

        /// <summary>
        /// This method reacts on user presses and checks
        /// the input against the guessing pattern.
        /// </summary>
        /// <param name="color">The color to check.</param>
        private void CheckColor(Simon color)
        {
            StopAllCoroutines();
            bool colorPick = guessPattern[checkingIndex] == color;

            CorrectColor(colorPick);

            WrongColor(colorPick);

            GuessingDone(checkingIndex >= guessPattern.Count);
        }

        /// <summary>
        /// Checks if the user input was correct.
        /// If so, update the counter and reset the timer.
        /// </summary>
        /// <param name="isCorrect"></param>
        private void CorrectColor(bool isCorrect)
        {
            if (!isCorrect) return;

            checkingIndex++; //correct color
        }

        /// <summary>
        /// Checks if the user input was wrong.
        /// If so, restart the current round and inform the Game Controller of lose.
        /// </summary>
        /// <param name="isWrong"></param>
        private void WrongColor(bool isWrong)
        {
            if (!isWrong) return;

            GameObject icon = infos.Find(obj => obj.name.Equals("IconNone"));
            if (icon != null) icon.GetComponent<SimonButton>().Animate();
            correctGuesses -= (int)CurrentLevel + 1;
            SendMessageUpwards("LoseCondition", gameObject);
            ResetTurn();
            StartCoroutine(AnimateButtons(buttons));
        }

        /// <summary>
        /// Checks if the player is done guessing.
        /// If so, starts the new round an inform the Game Controller of win.
        /// </summary>
        /// <param name="isDone"></param>
        private void GuessingDone(bool isDone)
        {
            if (!isDone) return;

            correctGuesses++;
            UpdateDifficulty(correctGuesses);
            SendMessageUpwards("WinCondition", gameObject);
            ClearInfoPattern();
            GeneratePattern(displayPattern.Count + 1);
            StartCoroutine(AnimateButtons(buttons));
        }

        #endregion
    }
}