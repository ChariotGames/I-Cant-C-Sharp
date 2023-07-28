using Scripts.Models;
using TMPro;
using UnityEngine;

namespace Scripts.Controllers
{
    public class HighscoreDisplay : MonoBehaviour
    {
        [SerializeField] private HighscoreEntry[] entries;
        [SerializeField] private Settings settings;

        private const int COUNT = 5;

        private struct Score
        {
            public string name;
            public int value;
        }

        private void OnEnable()
        {
            DisplayHighscores();
        }

        private void OnDisable()
        {
            ClearContainers();
        }

        private void ClearContainers()
        {
            for(int i = 0; i < COUNT; i++)
            {
                
            }
        }

        private void DisplayHighscores()
        {
            for (int i = 1; i <= COUNT; i++)
            {

            }
            
        }
    }
}
