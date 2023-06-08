using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCheckpoint : MonoBehaviour, IarrowTouchable
{
    [SerializeField] private PolygonCollider2D playerCollider;

    public void touched()
    {
        if (playerCollider.IsTouching(gameObject.GetComponent<CircleCollider2D>()))
        {
            SendMessageUpwards("PlayerTouched", gameObject);
        }
    }
}
