using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G01_MagicCircle : MonoBehaviour
{

    
    [SerializeField] private GameObject ringContainer, ring, circle;

    [SerializeField] private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Random.Range(10, 16); i++)
        {
            GameObject obj = Instantiate(ring, ringContainer.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

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
