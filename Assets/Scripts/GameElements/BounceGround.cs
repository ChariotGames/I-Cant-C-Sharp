using System;
using UnityEngine;

namespace Scripts.GameElements
{
    public class BounceGround : MonoBehaviour
    {
        public static event Action HitGround;
        private void OnCollisionEnter2D(Collision2D col)
        {
            HitGround?.Invoke();
        }
    }
}
