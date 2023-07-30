using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyArea : MonoBehaviour
{
    public static event Action<int> OnDestroy;
    public static event Action OnFail;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnDestroy?.Invoke(0);
        OnFail?.Invoke();
        Destroy(collision.gameObject);
    }
}
