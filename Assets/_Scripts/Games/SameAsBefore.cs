using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts._Input;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Serialization;

namespace _Scripts.Games
{
    public class SameAsBefore : BaseGame
    {
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private int maxStepsBack;
        [SerializeField] private int timeToAnswer;
        [SerializeField] private List<GameObject> options;
        [SerializeField] private GameObject startText;
        [SerializeField] private TMP_Text stepBackText;
        [SerializeField] private GameObject failText;

        private bool isYes;
        private bool isNo;
        private LinkedList<int> lastIndices = new();
        private int index;
        private int steps;
        private const string StepBackTextString = "Steps: ";



        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GameStartCoroutine());
        }

        private void SpawnSymbol()
        {
            index = Random.Range(0, options.Count);
            options[index].SetActive(true);
        }

        private void UpdateSteps()
        {
            switch (difficulty)
            {
                case Difficulty.LVL1:
                    steps = 0;
                    break;
                case Difficulty.LVL2:
                    steps = Math.Min(1, lastIndices.Count - 1);
                    break;
                case Difficulty.LVL3:
                    steps = Random.Range(0, Math.Min(maxStepsBack, lastIndices.Count));
                    break;
                default:
                    steps = 0;
                    break;
            }
        }

        private void UpdateStepBackText()
        {
            stepBackText.text = StepBackTextString + (steps + 1);
        }

        private IEnumerator AnimationStartText(float distance)
        {
            float offset = 0;
            float delta = 0.005f;
            while (offset < distance)
            {
                startText.transform.Translate(0, delta, 0, Space.Self);
                offset += delta;
                yield return new WaitForSeconds(0.001f);
            }
            yield return new WaitForSeconds(1);
            startText.SetActive(false);
        }

        private IEnumerator GameStartCoroutine()
        {
            StartCoroutine(AnimationStartText(2.1f));
            yield return new WaitForSeconds(1);
            SpawnSymbol();
            lastIndices.AddFirst(index);
            if (lastIndices.Count > maxStepsBack) { lastIndices.RemoveLast(); }
            yield return new WaitForSeconds(1);
            options[index].SetActive(false);
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                if (options[0].activeSelf || options[1].activeSelf)
                {
                    //Debug.Log(lastIndices.Count);
                    Debug.Log(index + " : " + lastIndices.ElementAt(steps) + "\tsteps: " + (steps + 1));

                    float timer = Time.unscaledTime;
                    yield return new WaitUntil(() => isYes || isNo || Time.unscaledTime - timer > timeToAnswer);
                    if ((index == lastIndices.ElementAt(steps) && isYes && !isNo) || (index != lastIndices.ElementAt(steps) && isNo && !isYes))
                    {
                        base.Win();
                        options[index].SetActive(false);
                        isYes = false;
                        isNo = false;
                        lastIndices.AddFirst(index);
                        if (lastIndices.Count > maxStepsBack) { lastIndices.RemoveLast(); }
                    }
                    else
                    {
                        base.Lose();
                        options[index].SetActive(false);
                        failText.SetActive(true);
                        yield break;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(1);
                    SpawnSymbol();
                    UpdateSteps();
                    UpdateStepBackText();
                }
            }
        }


        private void OnEnable()
        {
            InputHandler.ShoulderRight += RightShoulderPressed;
            InputHandler.ShoulderLeft += LeftShoulderPressed;
        }

        public void RightShoulderPressed()
        {
            isYes = true;
        }

        public void LeftShoulderPressed()
        {
            isNo = true;
        }

        private void OnDisable()
        {
            InputHandler.ShoulderRight -= RightShoulderPressed;
            InputHandler.ShoulderLeft -= LeftShoulderPressed;
        }
    }
}