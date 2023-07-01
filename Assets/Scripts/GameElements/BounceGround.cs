using System;
using UnityEngine;

namespace Scripts.GameElements
{
    public class BounceGround : MonoBehaviour
    {
        [SerializeField] private PhysicsMaterial2D groundMaterial;
        public static event Action HitGround;
        private BoxCollider2D groundCollider;
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            HitGround?.Invoke();
        }

        private void Awake()
        {
            groundCollider = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            groundCollider.sharedMaterial = groundMaterial;
        }
    }
}
