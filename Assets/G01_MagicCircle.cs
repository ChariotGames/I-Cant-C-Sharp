using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Input;
using System;

public class G01_MagicCircle : MonoBehaviour
{
    //[SerializeField] private float time = 0f;

    [SerializeField] private GameObject ringContainer, ring, circle;
    [SerializeField] private bool stop = false;
    [SerializeField] private float sTime = 3;
    [SerializeField] private float sDelay;
    private Difficulty CurrentLevel { get; set; } = Difficulty.LVL2;

    //private MagicRing ringScript;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnRings", sTime);
    }

    private void SpawnRings()
    {
        //InputHandler.LeftShoulderBtnAction += checkmethod;

        GameObject obj = Instantiate(ring, ringContainer.transform);
        MagicRing objScript = obj.GetComponent<MagicRing>();
        obj.SetActive(true);
        objScript.live = true;
        objScript.level = CurrentLevel;
        if (stop)
        {
            CancelInvoke("SpawnRings");
        }
        sDelay = UnityEngine.Random.Range(1, 5);
        Invoke("SpawnRings", sDelay);
    }

    private void WinCondition(GameObject obj)
    {
        Debug.Log("win");
        if(obj.GetComponent<MagicRing>().live == true)
        {
            Destroy(obj);
        }
    }

    private void LoseCondition(GameObject obj)
    {
        Debug.Log("Lose");
        if (obj.GetComponent<MagicRing>().live == true)
        {
            Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
