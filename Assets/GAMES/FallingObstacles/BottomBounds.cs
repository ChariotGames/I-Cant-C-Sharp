using System;
using UnityEngine;

namespace Scripts.Pascal
{
    public class BottomBounds : MonoBehaviour
    {
        public static event Action DamageTaken;

        [SerializeField] private SpriteRenderer damageTakenIcon;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageIconGo = Instantiate(damageTakenIcon, other.transform.position, Quaternion.identity);
            damageIconGo.gameObject.SetActive(true);
            Destroy(damageIconGo.gameObject, 1);
            Destroy(other.gameObject);
            DamageTaken?.Invoke();
        }
    }
}
