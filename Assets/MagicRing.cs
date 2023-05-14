using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRing : MonoBehaviour
{
    
    [SerializeField] private float size = 256f;
    [SerializeField] private float thickness = 15f;

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

    private void ReDrawRing(int size, int thickness)
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
    }

    // Update is called once per frame
    void Update()
    {
        ReDrawRing((int)(size -= Time.deltaTime * 100), (int)(thickness));
    }
}
