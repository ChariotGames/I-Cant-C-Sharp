using UnityEngine;

public class LineCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Hinzufügen des Edge Colliders
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // Holen der LineRenderer-Komponente des Line-Objekts
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        // Anpassen des Edge Colliders an die Positionen der Linienpunkte
        Vector3[] linePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePositions);
        Vector2[] colliderPoints = new Vector2[linePositions.Length];
        for (int i = 0; i < linePositions.Length; i++)
        {
            colliderPoints[i] = linePositions[i];
        }
        edgeCollider.points = colliderPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
