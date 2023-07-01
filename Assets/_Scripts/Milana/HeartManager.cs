
using UnityEngine;
using UnityEngine.UI;
using _Scripts.Models;

namespace _Scripts.Controllers
{
    public class HeartManager : MonoBehaviour
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

