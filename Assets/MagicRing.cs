using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRing : MonoBehaviour
{
    
    [SerializeField] private float radius = 4f; //256
    [SerializeField] private LineRenderer ringRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //DrawRing();
        //int rnum = Random.Range(1, 5);
        //if (rnum == 1) thickness = Random.Range(45, 91);
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

            Vector3 currentPosition = new Vector3(x, y, 0);
            ringRenderer.SetPosition(i, currentPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (radius < 0.5) Destroy(gameObject);
        drawRing(1024, radius -= Time.deltaTime * 0.5f); //0.0001f);
    }
}
