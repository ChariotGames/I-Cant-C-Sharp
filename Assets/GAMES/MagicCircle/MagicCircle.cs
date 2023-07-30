using System;
using Scripts.Models;
using System.Collections;
using Scripts.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    /// <summary>
    /// A circle appears and larger rings are closing in on it. Press a defined button once a ring hits the circle.
    /// Easy: Press button A once a ring hits the circle.
    /// Medium: Rings have different colors. Adjust your input in regard to the color of the ring.
    /// Hard: Rings can be thicker. Hold a button according to the length of the ring.
    /// </summary>
    public class MagicCircle : BaseGame
    {
        #region Serialized Fields

        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private Color[] ringColors;
        [SerializeField] private Key[] ringButtons;
        [SerializeField] private GameObject ringContainer, ring, circle, MainContainer;
        [SerializeField] private SpriteRenderer circleRenderer;
        [SerializeField] private Vector3 rotationDirection = Vector3.forward;
        [SerializeField] [Range(1, 5)] private float startTimeout = 3, spawnDelay;
        [SerializeField] [Range(10, 50)] private float rotationSpeed;
        [SerializeField] private bool stop = false;

        #endregion Serialized Fields

        #region Fields

        //private int _successes = 0, _fails = 0;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void OnEnable()
        {
            MinigameManager.OnDifficultyChanged += UpdateDifficulty; 
        }

        private void OnDisable()
        {
            MinigameManager.OnDifficultyChanged -= UpdateDifficulty; 
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());
            MainContainer.SetActive(true);
            ringButtons = _keys.All;
            Invoke(nameof(SpawnRings), startTimeout);
        }

        void Update()
        {
            circle.transform.Rotate(rotationSpeed * Time.deltaTime * rotationDirection);
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        private void UpdateDifficulty(Difficulty difficulty)
        {
            base.Difficulty = difficulty;
        }
        
        /// <summary>
        /// Spawns rings randomly forever.
        /// </summary>
        private void SpawnRings()
        {
            GameObject obj = Instantiate(ring, gameObject.transform.position, Quaternion.identity, ringContainer.transform);
            obj.SetActive(true);
            MagicRing script = obj.GetComponent<MagicRing>();

            if (Difficulty == Difficulty.MEDIUM || Difficulty == Difficulty.HARD)
            {
                int chance = Random.Range(0, 2);
                script.Setup(gameObject.transform.position, ringColors[chance], new[] { ringButtons[chance].Input, ringButtons[1-chance].Input });
            }
            else
            {
                script.Setup(gameObject.transform.position, ringColors[0], new[] { ringButtons[0].Input } );
            }

            if (ringContainer.transform.childCount == 1) script.ToggleListeners(true);

            spawnDelay = Random.Range(1, 5);
            Invoke(nameof(SpawnRings), spawnDelay);

            if (stop)
            {
                CancelInvoke(nameof(SpawnRings));
            }
        }

        /// <summary>
        /// Evaluates the result values sent by the ring.
        /// </summary>
        /// <param name="ring">The object, that invoked this method.</param>
        /// <param name="isWin">If the input was correct, this is true.</param>
        public void EvaluateResult(GameObject ring, bool isWin)
        {
            MagicRing script = ring.GetComponent<MagicRing>();
            script.ToggleListeners(false);
            Destroy(ring);

            if (ringContainer.transform.childCount != 1) ringContainer.transform.GetChild(1).GetComponent<MagicRing>().ToggleListeners(true);

            if (isWin)
            {
                StartCoroutine(AnimateColor(circleRenderer, circle.GetComponent<SpriteRenderer>().color, Color.green, 0.25f));
                //_correctGuesses++;
                //base.AnimateSuccess(circle.transform, 1, 5);
                //base.ScoreUp();
                
                base.Success();
            }
            else
            {
                StartCoroutine(AnimateColor(circleRenderer, circle.GetComponent<SpriteRenderer>().color, Color.red, 0.25f));
                //_fails++;
                //base.AnimateFail(circle.transform, 1, 3);
                
                base.Fail();
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private IEnumerator AnimateColor(SpriteRenderer sprite, Color original, Color target, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(original, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(target, original, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            sprite.color = original;
        }

        #endregion Overarching Methods / Helpers
    }
}