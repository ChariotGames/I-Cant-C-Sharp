using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Input;
using System;
using _Scripts;
using _Scripts.Games;

public class G01_MagicCircle : BaseGame
{
    //[SerializeField] private float time = 0f;

    [SerializeField] private GameObject ringContainer, ring, circle;
    [SerializeField] private bool stop = false;
    [SerializeField] private float sTime = 3;
    [SerializeField] private float sDelay;
    [SerializeField] private float rotationspeed;
    [SerializeField] private Vector3 rotationDir;
    private Difficulty CurrentLevel { get; set; } = Difficulty.LVL2;
    private SpriteRenderer circleRenderer;
    //private MagicRing ringScript;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnRings", sTime);
        circleRenderer= circle.GetComponent<SpriteRenderer>();
    }

    private void SpawnRings()
    {
        //InputHandler.ShoulderLeft += checkmethod;

        GameObject obj = Instantiate(ring, ringContainer.transform);
        MagicRing objScript = obj.GetComponent<MagicRing>();
        obj.SetActive(true);
        objScript.live = true;
        objScript.level = CurrentLevel;
        objScript.offset = gameObject.transform.position;

        if (ringContainer.transform.childCount == 1) objScript.activateInput();

        sDelay = UnityEngine.Random.Range(1, 5);
        Invoke("SpawnRings", sDelay);
        if (stop)
        {
            CancelInvoke("SpawnRings");
        }
    }

    private void WinCon(GameObject obj)
    {
        base.Win();
        Debug.Log("win");
        StartCoroutine(AnimateColor(circleRenderer, Color.blue, Color.green, 0.25f));
        if (obj.GetComponent<MagicRing>().live == true)
        {
            Destroy(obj);
            try
            {
                ringContainer.transform.GetChild(1).GetComponent<MagicRing>().activateInput();
            }
            catch
            {
                //nothing to catch
            }
        }
    }

    private void LoseCon(GameObject obj)
    {
        base.Lose();
        Debug.Log("Lose");
        StartCoroutine(AnimateColor(circleRenderer, Color.blue, Color.red, 0.25f));
        if (obj.GetComponent<MagicRing>().live == true)
        {
            Destroy(obj);
            try
            {
                ringContainer.transform.GetChild(1).GetComponent<MagicRing>().activateInput();
            }
            catch
            {
                //nothing to catch
            }
        }
    }

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

    // Update is called once per frame
    void Update()
    {
        circle.transform.Rotate(rotationspeed * Time.deltaTime * rotationDir);
    }
}
