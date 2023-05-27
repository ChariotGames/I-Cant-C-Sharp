using System;
using TMPro;
using UnityEngine;

public class BottomBounds : MonoBehaviour
{
    
    public static event Action damageTaken;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
       Destroy(other.gameObject);
       damageTaken?.Invoke();
    }

   
}
