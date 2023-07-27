using Scripts.Models;
using TMPro;
using UnityEngine;

namespace Scripts.Controllers
{
    public class HighscoreDisplay : MonoBehaviour
    {
        [SerializeField] private Transform namesContainer, scoresContainer, timesContainer;
        [SerializeField] private GameObject templateName, templateScore, templateTime;
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
                Destroy(namesContainer.GetChild(i).gameObject);
                Destroy(scoresContainer.GetChild(i).gameObject);
            }
        }

        private void DisplayHighscores()
        {
            for (int i = 1; i <= COUNT; i++)
            {

            }
            GameObject name = Instantiate(templateName, namesContainer);
            name.GetComponent<TMP_Text>().text = settings.SelectedCharacter.Name;
            GameObject score = Instantiate(templateScore, scoresContainer);
            score.GetComponent<TMP_Text>().text = settings.Highscore.ToString("D3");
        }
    }
}
