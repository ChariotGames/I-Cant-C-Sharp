using Scripts.Models;
using UnityEngine;

namespace Scripts.Controllers
{
    public class HighscorePanel : MonoBehaviour
    {
        [SerializeField] private HighscoreEntry[] entries;
        [SerializeField] private HighscoreEntry incoming, temp;
        [SerializeField] private Settings settings;

        private const string VALUE_SPLIT = "_", ENTRY_SPLIT = ";", KEY_WORD = "HighscoreList";

        public void DisplayScores()
        {
            LoadScores();
            UpdateScores();
            SaveScores();
        }

        private void LoadScores()
        {
            string oneLine = PlayerPrefs.GetString(KEY_WORD);

            if (oneLine.Length == 0) return;

            string[] allScores = oneLine.Split(ENTRY_SPLIT);

            for (int i = 0; i < entries.Length; i++)
            {
                string[] stats = allScores[i].Split(VALUE_SPLIT);

                entries[i].Icon = settings.Characters[int.Parse(stats[0])].Icon;
                entries[i].Name = stats[1];
                entries[i].Score = stats[2];
                entries[i].Time = stats[3];
            }
        }

        private void UpdateScores()
        {
            if(settings.BaseDifficulty == Difficulty.TUTORIAL) return;
            if (settings.Score < int.Parse(entries[^1].Score)) return;

            incoming.SetValues(settings.SelectedCharacter.Icon, settings.SelectedCharacter.Name, settings.Score, settings.Time);

            for (int i = 0; i < entries.Length; i++)
            {
                if (settings.Score < int.Parse(entries[i].Score)) continue;
                //swap
                temp.CopyValues(entries[i]);
                entries[i].CopyValues(incoming);
                incoming.CopyValues(temp);
            }
        }

        private void SaveScores()
        {
            string result = "";

            for (int i = 0; i < entries.Length; i++)
            {
                result += FindCharIndex(entries[i].Icon) + VALUE_SPLIT;
                result += entries[i].Name + VALUE_SPLIT;
                result += entries[i].Score + VALUE_SPLIT;
                result += entries[i].Time;
                result += ENTRY_SPLIT;
            }
            result = result[0..^1];

            PlayerPrefs.SetString(KEY_WORD, result);
            PlayerPrefs.Save();
        }

        private string FindCharIndex(Sprite icon)
        {
            for (int i = 0; i < settings.Characters.Count; i++)
                if (settings.Characters[i].Icon.Equals(icon)) return i.ToString();

            return null;
        }
    }
}
