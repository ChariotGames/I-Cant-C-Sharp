using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private Settings settings;
        [SerializeField] private Image[] hearts;

        [SerializeField] private Sprite fullHeart;
        [SerializeField] private Sprite emptyHeart;

        

        private void Awake()
        {
            //health = hearts.Length;
        }

        // Update is called once per frame
        void Update()
        {
            foreach (Image img in hearts)
            {
                img.sprite = emptyHeart;
            }

            //depending on the health size, the amount of hearts will be filled
            for (int i = 0; i < settings.Lives; i++)
            {
                hearts[i].sprite = fullHeart;
            }
        }
    }

}
