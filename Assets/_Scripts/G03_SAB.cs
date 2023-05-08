using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Input;
using UnityEngine;
using Random = UnityEngine.Random;

public class G03_SAB : MonoBehaviour
{

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
        StartCoroutine(GameStartCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int spawnSymbol()
    {
        index = Random.Range(0, sprites.Count);
        sprite.sprite = sprites[index];
        sprite.gameObject.SetActive(true);
        return index;
    }

    private IEnumerator GameStartCoroutine()
    {
        lastSprite = spawnSymbol();
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
                yield return new WaitUntil(() => isYes || isNo);
                Debug.Log(index + " " + lastSprite + " " + isYes + " " + isNo);
                if ((index == lastSprite && isYes && !isNo) || (index != lastSprite && isNo && !isYes))
                {
                    Debug.Log("Win");
                    sprite.gameObject.SetActive(false);
                    isYes = false;
                    isNo = false;
                }
                else
                {
                    Debug.Log("Lose Condition");
                    yield break;
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
                lastSprite = spawnSymbol();
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
