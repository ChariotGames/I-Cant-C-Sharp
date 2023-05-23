using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old : MonoBehaviour
{

    [SerializeField] private float size = 8f; //256
    [SerializeField] private float thickness = 15f;
    [SerializeField] private LineRenderer ringRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //DrawRing();
        //int rnum = Random.Range(1, 5);
        //if (rnum == 1) thickness = Random.Range(45, 91);
    }

    /*private void DrawRing()
    {
        //GameObject obj = Instantiate(ring, ringContainer.transform);
        //SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
    }*/

    /*private void ReDrawRing(int size, int thickness)
    {
        // create a new texture with transparent background
        Texture2D texture = new(size, size, TextureFormat.ARGB32, false);
        Color[] pixels = new Color[size * size];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        texture.SetPixels(pixels); 
        // TODO: SetPixels32 for faster performance.

        // draw the ring
        int radius = (size / 2);
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                float distance = Mathf.Sqrt(x * x + y * y);
                if (distance >= radius - thickness && distance <= radius - thickness / 2)
                {
                    texture.SetPixel(x + radius, y + radius, Color.white);
                }
            }
        }

        // apply changes to the texture and create a sprite
        texture.Apply();
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        gameObject.AddComponent<PolygonCollider2D>();
    }*/

    private void drawRing(int steps, float radius)
    {
        ringRenderer.positionCount = steps;

        for (int i = 0; i < steps; i++)
        {
            float cProg = (float)i / steps;
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
        //ReDrawRing((int)(size -= Time.deltaTime * 100), (int)(thickness));
        drawRing(1024, size -= 0.0001f);//Time.deltaTime * 0.2f));
    }

    //MagicCircle
    /* Vector3[] positions = new Vector3[lineRenderer.positionCount];
            float radius = 2.0f;

            for (int i = 0; i < positions.Length; i++)
            {
                float angle = Mathf.PI * 2 * i / positions.Length - 1;
                Vector3 position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
                positions[i] = position;
            }

            lineRenderer.SetPositions(positions);*/
    //[SerializeField] private LineRenderer lineRenderer;

    /*lineRenderer.startWidth = 1;
    lineRenderer.endWidth = 1;
    lineRenderer.positionCount = 200;

    obj.AddComponent<PolygonCollider2D>();*/

}
