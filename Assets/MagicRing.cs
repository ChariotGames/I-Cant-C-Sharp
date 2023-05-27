using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Input;
using System;
using _Scripts;

public class MagicRing : MonoBehaviour
{
    
    public float radius = 4f;
    public bool live = false;
    public Difficulty level;
    public Vector3 offset;

    [SerializeField] private float timer = 0.5f;
    [SerializeField] private LineRenderer ringRenderer;
    [SerializeField] private int chance = 0;
    [SerializeField] private Color ringColor= new Color(0, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(live);
        
        //ringRenderer = GetComponent<LineRenderer>();

        if (level == Difficulty.LVL2 || level == Difficulty.LVL3)
        {
            chance = UnityEngine.Random.Range(0, 2);
            if (chance == 1)
            {
                ringRenderer.startColor = ringColor;
                ringRenderer.endColor = ringColor;
            }
        }
    }

    public void activateInput()
    {
        InputHandler.LeftShoulderBtnAction += checkLeft;
        if (level == Difficulty.LVL2 || level == Difficulty.LVL3) InputHandler.RightShoulderBtnAction += checkRight;
    }

    private void checkLeft()
    {
        
        if (radius <= 0.75 && radius >= 0.25 && chance == 0)
        {
            SendMessageUpwards("WinCon", gameObject);
        }
        else
        {
            SendMessageUpwards("LoseCon", gameObject);
        }
        
    }

    private void checkRight()
    {

        if (radius <= 0.75 && radius >= 0.25 && chance == 1)
        {
            SendMessageUpwards("WinCon", gameObject);
        }
        else
        {
            SendMessageUpwards("LoseCon", gameObject);
        }
    }

    private void drawRing(int steps, float radius)
    {
        ringRenderer.positionCount = steps;

        for (int i = 0; i < steps; i++)
        {
            float cProg = (float) i / steps;
            float currentRadian = cProg * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0) + offset;
            ringRenderer.SetPosition(i, currentPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (radius < 0.24)
        {
            SendMessageUpwards("LoseCon", gameObject);
            //Destroy(gameObject);
        }
        if(live)
        {
            drawRing(1024, radius -= Time.deltaTime * timer);
        }
        
    }

    private void OnDestroy()
    {
        InputHandler.LeftShoulderBtnAction -= checkLeft;
        InputHandler.RightShoulderBtnAction -= checkRight;
    }
}
