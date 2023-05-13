using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Input;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class G03_SAB : MonoBehaviour
{
    [SerializeField] private int difficulty;
    [SerializeField] private List<GameObject> options;
    [SerializeField] private TMP_Text stepBackText;
    
    private bool isYes;
    private bool isNo;
    private int lastSprite;
    private int index;
    private int steps = 1;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        stepBackText.text += steps;
        StartCoroutine(GameStartCoroutine());
    }
    
    private void SpawnSymbol()
    {
        index = Random.Range(0, options.Count);
        options[index].SetActive(true);
    }

    private IEnumerator GameStartCoroutine()
    {
        SpawnSymbol();
        lastSprite = index;
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
                Debug.Log(index + " " + lastSprite);
                float timer = Time.unscaledTime;
                yield return new WaitUntil(() => isYes || isNo || Time.unscaledTime - timer > 5);
                if ((index == lastSprite && isYes && !isNo) || (index != lastSprite && isNo && !isYes))
                {
                    Debug.Log("Win");
                    options[index].SetActive(false);
                    isYes = false;
                    isNo = false;
                    lastSprite = index;
                }
                else
                {
                    Debug.Log("Lose Condition");
                    options[index].SetActive(false);
                    yield break;
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
                SpawnSymbol();
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
