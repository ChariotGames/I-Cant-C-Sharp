using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts._Input;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;

public class G03_SAB : MonoBehaviour
{
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private int maxStepBacks;
    [SerializeField] private int timeToAsAnswer;
    [SerializeField] private List<GameObject> options;
    [SerializeField] private TMP_Text stepBackText;
    [SerializeField] private GameObject failText;

    private bool isYes;
    private bool isNo;
    private LinkedList<int> lastIndices = new();
    private int index;
    private int steps;
    private const string stepBackTextString = "Steps: ";
    
    
    
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
                steps = Random.Range(0, Math.Min(maxStepBacks, lastIndices.Count));
                break;
            default:
                steps = 0;
                break;
        }
    }
    
    private void UpdateStepBackText()
    {
        stepBackText.text = stepBackTextString + (steps + 1);
    }

    private IEnumerator GameStartCoroutine()
    {
        SpawnSymbol();
        lastIndices.AddFirst(index);
        if(lastIndices.Count > maxStepBacks){ lastIndices.RemoveLast();}
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
                yield return new WaitUntil(() => isYes || isNo || Time.unscaledTime - timer > timeToAsAnswer);
                if ((index == lastIndices.ElementAt(steps) && isYes && !isNo) || (index != lastIndices.ElementAt(steps) && isNo && !isYes))
                {
                    Debug.Log("Win");
                    options[index].SetActive(false);
                    isYes = false;
                    isNo = false;
                    lastIndices.AddFirst(index);
                    if(lastIndices.Count > maxStepBacks){ lastIndices.RemoveLast();}
                }
                else
                {
                    Debug.Log("Lose Condition");
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
        InputHandler.RightShoulderBtnAction += RightShoulderPressed;
        InputHandler.LeftShoulderBtnAction += LeftShoulderPressed;
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
        InputHandler.RightShoulderBtnAction -= RightShoulderPressed;
        InputHandler.LeftShoulderBtnAction -= LeftShoulderPressed;
    }
}
