using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Input;
using UnityEngine;
using Random = UnityEngine.Random;

public class G03_SAB : MonoBehaviour
{
    [SerializeField] private int difficulty;
    
    private bool isYes;
    private bool isNo;
    private int lastSprite;
    private int index;
    private SpriteRenderer sprite;

    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private void Awake()
    {
        GameObject symbol = new GameObject();
        symbol.transform.Translate(0,0,0, Space.World);
        symbol.SetActive(true);
        sprite = symbol.AddComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        spawnUIElements();
        StartCoroutine(GameStartCoroutine());
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void spawnUIElements()
    {
        
    }

    private void spawnSymbol()
    {
        index = Random.Range(0, sprites.Count);
        sprite.sprite = sprites[index];
        sprite.gameObject.SetActive(true);
    }

    private IEnumerator GameStartCoroutine()
    {
        spawnSymbol();
        lastSprite = index;
        yield return new WaitForSeconds(1);
        sprite.gameObject.SetActive(false);
        StartCoroutine(SpawnCoroutine());
        yield break;
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (sprite.gameObject.activeSelf)
            {
                Debug.Log(index + " " + lastSprite);
                float timer = Time.unscaledTime;
                yield return new WaitUntil(() => isYes || isNo || Time.unscaledTime - timer > 5);
                if ((index == lastSprite && isYes && !isNo) || (index != lastSprite && isNo && !isYes))
                {
                    Debug.Log("Win");
                    sprite.gameObject.SetActive(false);
                    isYes = false;
                    isNo = false;
                    lastSprite = index;
                }
                else
                {
                    Debug.Log("Lose Condition");
                    sprite.gameObject.SetActive(false);
                    yield break;
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
                spawnSymbol();
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
