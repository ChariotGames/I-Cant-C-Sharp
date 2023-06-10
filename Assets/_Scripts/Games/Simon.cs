using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    /// On Easy, each color is added once.
    /// On Medium, there is a chance for a color to be added twice.
    /// On Hard, a color is displayed but has to be skipped in guessing additionally.
    /// </summary>
    public class Simon : BaseGame
    {
        #region Serialized Fields

        [SerializeField] private List<Colors> displayPattern, guessPattern;
        [SerializeField] private List<Modifier> infoPattern;
        [SerializeField] private GameObject buttonsContainer, blue, red, yellow, green, middle;
        [SerializeField] private GameObject inputOverlay, infoOverlay, twice, not, ok;
        [SerializeField] private Image timer;

        #endregion

        #region Fields

        private Dictionary<Colors, GameObject> buttonObjects;
        private const float BLINK_TIME = 0.50f, TURN_TIME = 5.0f;
        private const int MIN_LENGTH = 1, CHANCE = 3, LVL_CHANGE = 5, COLORS = 4;
        private float animationTime;
        private int checkingIndex = 0, correctGuesses = 0, wrongGuesses = 0;

        #endregion

        #region Unity Built-Ins

        private void Awake()
        {
            // Initialize game
            buttonObjects = new();
            buttonObjects.Add(Colors.BLUE, blue);
            buttonObjects.Add(Colors.RED, red);
            buttonObjects.Add(Colors.YELLOW, yellow);
            buttonObjects.Add(Colors.GREEN, green);
        }

        // Start is called before the first frame update
        void Start()
        {
            animationTime = BLINK_TIME * COLORS;
            infoOverlay.SetActive(true);
            StartCoroutine(ActivateButtons(BLINK_TIME));
            GeneratePattern(MIN_LENGTH);
            StartCoroutine(AnimateButtons(animationTime * 2, animationTime));
        }

        #endregion

        #region Overarching Game Mechanics

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
        /// Updates the game difficulty for next spawn and messages to the GameManager.
        /// </summary>
        private void UpdateDifficulty(int rounds)
        {
            if (rounds <= 0)
            {
                base.Easier();
            }

            if (rounds >= LVL_CHANGE)
            {
                base.Harder();
            }

            correctGuesses %= LVL_CHANGE;
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
            Colors randomColor;
            while (displayPattern.Count < length)
            {
                randomColor = (Colors)UnityEngine.Random.Range(0, COLORS) + 1;
                displayPattern.Add(randomColor);
                SetGuessPattern(randomColor);
            }
        }

        /// <summary>
        /// Sets the actual pattern for the user to guess.
        /// Depending on the Level and Modifier it may differ
        /// from the patern the user gets to actually see.
        /// </summary>
        /// <param name="color">Enum of the color to add to the pattern.</param>
        private void SetGuessPattern(Colors color)
        {
            if (displayPattern.Count <= MIN_LENGTH)
            {
                // Only do the extra difficulty after the 3rd round!
                infoPattern.Add(Modifier.NORMAL);
                guessPattern.Add(color);
                return;
            }

            int chance = UnityEngine.Random.Range(0, CHANCE);

            if (base.Difficulty == Difficulty.HARD && chance < 1)
            {
                // On Level 3 nothing gets added if the chance is right
                infoPattern.Add(Modifier.NONE);
                return;
            }

            infoPattern.Add(Modifier.NORMAL);
            guessPattern.Add(color);

            if (base.Difficulty != Difficulty.EASY && chance > 1)
            {
                // On Level 2 the color is doubled
                infoPattern[^1] = Modifier.DOUBLE;
                guessPattern.Add(color);
            }
        }

        /// <summary>
        /// Clears the info pattern for next round, obscuring previous hints.
        /// </summary>
        private void ClearInfoPattern()
        {
            ok.GetComponent<SimonElement>().Animate();

            for (int i = 0; i < infoPattern.Count; i++)
            {
                infoPattern[i] = Modifier.NORMAL;
            }
        }

        /// <summary>
        /// Toggles if buttons are able to take input.
        /// Depending on the state if it is the player's turn.
        /// </summary>
        /// <param name="isPlayersTurn">State of the player's turn.</param>
        private void PlayerTurn(bool isPlayersTurn)
        {
            foreach (GameObject button in buttonObjects.Values)
            {
                button.GetComponent<SimonButton>().ToggleInput(isPlayersTurn);
            }

            inputOverlay.SetActive(isPlayersTurn);
        }

        #endregion

        #region Animations

        /// <summary>
        /// When the game starts, activates each button in order.
        /// </summary>
        private IEnumerator ActivateButtons(float time)
        {
            yield return new WaitForSeconds(time);
            middle.SetActive(true);
            yield return new WaitForSeconds(time);

            buttonsContainer.SetActive(true);
            foreach (GameObject button in buttonObjects.Values)
            {
                button.SetActive(true);
                button.GetComponent<SimonButton>().Animate();
                yield return new WaitForSeconds(time);
            }
        }

        /// <summary>
        /// Plays the animation of the buttons to play.
        /// </summary>
        /// <param name="delay">The delay of the overal animation.</param>
        /// <param name="duration">The duration of each button's animation.</param>
        /// <returns>An object that can be used to control the coroutine's execution.</returns>
        private IEnumerator AnimateButtons(float delay, float duration)
        {
            yield return new WaitForSeconds(delay);

            for (int i = 0; i < displayPattern.Count; i++)
            {
                Colors color = displayPattern[i];
                Modifier info = infoPattern[i];

                buttonObjects[color].GetComponent<SimonButton>().Animate();

                if (info == Modifier.DOUBLE) twice.GetComponent<SimonElement>().Animate();
                if (info == Modifier.NONE) not.GetComponent<SimonElement>().Animate();
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
        public void CheckColor(Colors color)
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
            wrongGuesses++;
            if (wrongGuesses == 3) base.Lose();

            not.GetComponent<SimonElement>().Animate();

            correctGuesses -= (int)base.Difficulty + 1;
            ResetTurn();
            StartCoroutine(AnimateButtons(animationTime, BLINK_TIME));
        }

        /// <summary>
        /// Checks if the player is done guessing.
        /// If so, starts the new round an inform the Game Controller of win.
        /// </summary>
        private void GuessingDone()
        {
            correctGuesses++;
            UpdateDifficulty(correctGuesses);
            if (correctGuesses == 0) base.Win();
            ResetTurn();
            ClearInfoPattern();
            GeneratePattern(displayPattern.Count + 1);
            StartCoroutine(AnimateButtons(animationTime, animationTime));
        }

        #endregion
    }
}