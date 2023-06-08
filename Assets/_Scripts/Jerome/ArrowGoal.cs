using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    public class ArrowGoal : Game, IarrowTouchable
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
}