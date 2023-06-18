using _Scripts.Models;
using UnityEngine;

namespace _Scripts.Games
{
    public class ArrowObject : MonoBehaviour
    {
        [SerializeField] private ArrowTerror game;
        [SerializeField] private PolygonCollider2D player;
        [SerializeField] private ElementType type;

        public ElementType Type { get => type; set => type = value; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.Equals(player))
            {
                game.PlayerTouched(gameObject);
            }
        }
    }
}
