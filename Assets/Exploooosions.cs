using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    public class Exploooosions : BaseGame
    {
        [SerializeField] private GameObject bombBase, bombDonut, bombCross, bombContainer, player;

        private bool active = false;
        private GameObject[] bombs;
        // Start is called before the first frame update
        void Start()
        {
            active = true;
            bombs = new GameObject[] { bombBase, bombDonut, bombCross };
            Invoke(nameof(SpawnBombs), 3);
        }

        private void SpawnBombs()
        {
            GameObject bomb = Instantiate(bombs[0], gameObject.transform.position, Quaternion.identity, bombContainer.transform);
            bomb.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
