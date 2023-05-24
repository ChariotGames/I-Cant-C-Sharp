using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G01_MagicCircle : MonoBehaviour
{

    
    [SerializeField] private GameObject ringContainer, ring, circle;

    [SerializeField] private float time = 0f;

    [SerializeField] private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < Random.Range(1, 2); i++) // 10, 16
        {
            GameObject obj = Instantiate(ring, ringContainer.transform);
        }
        */
        //InvokeRepeating("SpawnRings", 3, 4);
    }

    private void SpawnRings()
    {
        GameObject obj = Instantiate(ring, ringContainer.transform);
        if(stop)
        {
            CancelInvoke("SpawnRings");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
