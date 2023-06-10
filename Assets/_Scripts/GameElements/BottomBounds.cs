using System;
using UnityEngine;

public class BottomBounds : MonoBehaviour
{
    public static event Action DamageTaken;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
       Destroy(other.gameObject);
       DamageTaken?.Invoke();
    }
}
