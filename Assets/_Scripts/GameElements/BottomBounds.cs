using System;
using UnityEngine;

namespace _Scripts.Pascal
{
    public class BottomBounds : MonoBehaviour
    {
        public static event Action DamageTaken;

        [SerializeField] private SpriteRenderer damageTakenIcon;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageIcon = Instantiate(damageTakenIcon, other.transform.position, Quaternion.identity);
            Destroy(damageIcon, 1);
            Destroy(other.gameObject);
            DamageTaken?.Invoke();
        }
    }
}
