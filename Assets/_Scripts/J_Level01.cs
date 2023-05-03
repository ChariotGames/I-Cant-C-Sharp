using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class J_Level01 : MonoBehaviour { 

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Time delayInSeconds;
    

    private Camera _mainCamera;
    private Bounds _cameraViewportBounds;
    private Color[] buttonColors = new Color[] {
        new Color(1, 1, 0, 1), 
        new Color(1, 0, 0, 1), 
        new Color(0, 1, 0, 1), 
        new Color(0, 0, 1, 1) };
    private KeyCode[] keyCodes = new KeyCode[]
    {

    };
    private readonly List<GameObject> spawnedButtons = new ();
    private readonly List<SpriteRenderer> spawnedButtonSprites = new List<SpriteRenderer>();
 


    private void Awake()
    {
        _mainCamera = Camera.main;
        for(int i = 0; i < 4; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.transform.Translate(-1 * i, 0, 0, Space.Self);
            button.transform.Rotate(0, 0, 90 * i, Space.Self);
            SpriteRenderer buttonSprite = button.GetComponent<SpriteRenderer>();
            buttonSprite.color = buttonColors[i];
            spawnedButtons.Add(button);
            spawnedButtonSprites.Add(buttonSprite);
            button.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, 4);
            for(int i = 0; i < spawnedButtons.Count; i++)
            {
                if(i != randomIndex)
                {
                    yield return new WaitForSeconds(1);
                    spawnedButtons[i].SetActive(true);
                }
            }


            yield return waitForKeyPress(KeyCode.Space);
            spawnedButtons[randomIndex].SetActive(true);
            yield return new WaitForSeconds(.5f);
            for(int i = 0; i < spawnedButtons.Count; i++)
            {
                spawnedButtons[i].SetActive(false);
            }
            yield return new WaitForSeconds(.5f);
        }
    }


    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done)
        {
            if (Input.GetKeyDown(key))
            {
                done = true;
            }
            yield return null;
        }
    }
}