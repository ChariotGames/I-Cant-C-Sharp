using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    [SerializeField] private List<string> test;

    public void CheckInput(string input)
    {
        Debug.Log(input);
    }
}
