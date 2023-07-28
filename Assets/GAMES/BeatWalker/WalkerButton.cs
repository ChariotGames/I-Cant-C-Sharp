using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Models;

public class WalkerButton : MonoBehaviour
{
    private float speed;

    private void Awake()
    {
        speed = 7f;//PlayerPrefs.GetFloat("speed"); //TODO wieder um?ndern!
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
