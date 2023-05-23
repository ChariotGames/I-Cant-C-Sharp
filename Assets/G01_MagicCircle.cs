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
        for (int i = 0; i < Random.Range(1, 2); i++) // 10, 16
        {
            GameObject obj = Instantiate(ring, ringContainer.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
