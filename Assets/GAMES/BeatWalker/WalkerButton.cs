using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Models;

public class WalkerButton : MonoBehaviour
{
    private float speed;

    private void Awake()
    {
        speed = PlayerPrefs.GetFloat("speed"); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.up * (speed * Time.deltaTime);
    }
}
