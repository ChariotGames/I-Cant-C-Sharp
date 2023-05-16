using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private List<Simon> displayPattern, guessPattern, infoPattern;
        [SerializeField] private GameObject[] buttons, infos;
        [SerializeField] private Image timer;

        #endregion

        #region Fields

        private Difficulty currentDifficulty = Difficulty.LVL1;

        private const float BLINK_TIME = 0.50f, TURN_TIME = 5.0f;
        private const int MIN_LENGTH = 3, CHANCE = 3, LVL_CHANGE = 5, COLORS = 4;
        private float animationTime;
        private int checkingIndex = 0, correctGuesses = 0;

        #endregion

        #region Unity Built-Ins

        // Start is called before the first frame update
        void Start()
        {
            animationTime = BLINK_TIME * COLORS;
            StartCoroutine(ActivateButtons(BLINK_TIME));
            GeneratePattern(MIN_LENGTH);
            StartCoroutine(AnimateButtons(buttons, animationTime * 2, animationTime));
        }

        #endregion

        #region Overarching Game Mechanics

        /// <summary>
        /// Autoproperty to the value of currentDifficulty
        /// </summary>
        public Difficulty CurrentDifficulty
        {
            get { return currentDifficulty; }
            set { currentDifficulty = value; }
        }

        /// <summary>
        /// Restarts guessing process and disables player input.
        /// </summary>
        private void ResetTurn()
        {
            StopAllCoroutines();
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
            Simon randomColor;
            while (displayPattern.Count < length)
            {
                randomColor = (Simon)UnityEngine.Random.Range(0, COLORS);
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
            if(displayPattern.Count <= MIN_LENGTH)
            {
                // Only do the extra difficulty after the 3rd round!
                infoPattern.Add(Simon.EMPTY);
                guessPattern.Add(button);
                return;
            }

            int chance = UnityEngine.Random.Range(0, CHANCE);

            if (currentDifficulty == Difficulty.LVL3 && chance < 1)
            {
                // On Level 3 nothing gets added if the chance is right
                infoPattern.Add(Simon.NONE);
                return;
            }

            infoPattern.Add(Simon.EMPTY);
            guessPattern.Add(button);

            if (currentDifficulty != Difficulty.LVL1 && chance > 1)
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
            GameObject icon = infos[Array.FindIndex(infos, obj => obj.name.Equals("Ok"))];
            if (icon != null) icon.GetComponent<SimonButton>().Animate();

            for (int i = 0; i < infoPattern.Count; i++)
            {
                infoPattern[i] = Simon.EMPTY;
            }
        }

        /// <summary>
        /// Toggles if buttons are able to take input.
        /// Depending on the state if it is the player's turn.
        /// </summary>
        /// <param name="isPlayersTurn">State of the player's turn.</param>
        private void PlayerTurn(bool isPlayersTurn)
        {
            foreach (GameObject obj in buttons)
            {
                SimonButton button = obj.GetComponent<SimonButton>();
                if (button != null) button.ToggleInput(obj, isPlayersTurn);
            }
            infos[Array.FindIndex(infos, obj => obj.name.Equals("Input"))].SetActive(isPlayersTurn);
        }

        #endregion

        #region Animations

        /// <summary>
        /// When the game starts, activates each button in order.
        /// </summary>
        private IEnumerator ActivateButtons(float time)
        {
            yield return new WaitForSeconds(time);
            buttons[^1].SetActive(true);
            yield return new WaitForSeconds(time);
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(true);
                SimonButton button = obj.GetComponent<SimonButton>();
                if(button != null) button.Animate();
                yield return new WaitForSeconds(time);
            }
        }

        /// <summary>
        /// Plays the animation of the buttons to play.
        /// </summary>
        /// <param name="buttons">The list of objects to animate.</param>
        /// <param name="delay">The delay of the overal animation.</param>
        /// <param name="duration">The duration of each button's animation.</param>
        /// <returns>An object that can be used to control the coroutine's execution.</returns>
        private IEnumerator AnimateButtons(GameObject[] buttons, float delay, float duration)
        {
            yield return new WaitForSeconds(delay);

            for (int i = 0; i < displayPattern.Count; i++)
            {
                Simon button = displayPattern[i];
                Simon info = infoPattern[i];
                buttons[(int)button].GetComponent<SimonButton>().Animate();
                if (info != Simon.EMPTY)
                {
                    GameObject icon = infos[(int)info % COLORS];
                    icon.GetComponent<SimonButton>().Animate();
                }
                yield return new WaitForSeconds(duration);
            }

            PlayerTurn(true);
            StartCoroutine(AnimateTimer(TURN_TIME));
        }


        /// <summary>
        /// Animates the circular progess bar aka. timer.
        /// </summary>
        /// <param name="duration">The duration of timer.</param>
        /// <returns></returns>
        private IEnumerator AnimateTimer(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float progress = elapsedTime / duration;
                timer.fillAmount = Mathf.Clamp01(progress);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            timer.fillAmount = 1f;
            WrongColor();
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
            if (!(guessPattern[checkingIndex] == color))
            {
                WrongColor();
                return;
            }

            checkingIndex++;

            if (!(checkingIndex >= guessPattern.Count)) return;

            GuessingDone();
        }

        /// <summary>
        /// Checks if the user input was wrong.
        /// If so, restart the current round and inform the Game Controller of lose.
        /// </summary>
        private void WrongColor()
        {
            GameObject icon = infos[Array.FindIndex(infos, obj => obj.name.Equals("None"))];
            if (icon != null) icon.GetComponent<SimonButton>().Animate();
            correctGuesses -= (int)currentDifficulty + 1;
            SendMessageUpwards("LoseCondition", gameObject);
            ResetTurn();
            StartCoroutine(AnimateButtons(buttons, animationTime, BLINK_TIME));
        }

        /// <summary>
        /// Checks if the player is done guessing.
        /// If so, starts the new round an inform the Game Controller of win.
        /// </summary>
        private void GuessingDone()
        {
            correctGuesses++;
            UpdateDifficulty(correctGuesses);
            SendMessageUpwards("WinCondition", gameObject);
            ResetTurn();
            ClearInfoPattern();
            GeneratePattern(displayPattern.Count + 1);
            StartCoroutine(AnimateButtons(buttons, animationTime, animationTime));
        }

        #endregion
    }
}