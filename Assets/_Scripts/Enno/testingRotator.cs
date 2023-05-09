using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector2 _rotator;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            print(Input.anyKeyDown);
            transform.Rotate(_rotator * Time.deltaTime);
        }
    }
}
