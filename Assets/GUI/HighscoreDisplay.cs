using Scripts.Models;
using TMPro;
using UnityEngine;

namespace Scripts.Controllers
{
    public class HighscoreDisplay : MonoBehaviour
    {
        [SerializeField] private Transform namesContainer, scoresContainer;
        [SerializeField] private GameObject templateName, templateScore;
        [SerializeField] private Settings settings;

        public void DisplayHighscores()
        {
            GameObject name = Instantiate(templateName, namesContainer);
            name.GetComponent<TMP_Text>().text = settings.SelectedCharacter.Name;
            GameObject score = Instantiate(templateScore, scoresContainer);
            score.GetComponent<TMP_Text>().text = settings.Highscore.ToString("D3");
        }
    }
}
