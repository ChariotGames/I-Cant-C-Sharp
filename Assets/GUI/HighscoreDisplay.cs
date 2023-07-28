using Scripts.Models;
using System;
using UnityEngine;

namespace Scripts.Controllers
{
    public class HighscoreDisplay : MonoBehaviour
    {
        [SerializeField] private HighscoreEntry[] entries;
        [SerializeField] private Settings settings;

        private const string VALUE_SPLIT = "_", ENTRY_SPLIT = ";", KEY_WORD = "HighscoreList";

        private void Awake()
        {
            LoadScores();
        }

        private void OnEnable()
        {
            UpdateScores();
        }

        private void OnDisable()
        {
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
                Debug.Log(allScores[i]);
                entries[i].Icon = settings.Characters[int.Parse(stats[0])].Icon;
                entries[i].Name = stats[1];
                entries[i].Score = stats[2];
                entries[i].Time = stats[3];
            }
        }

        public void UpdateScores()
        {
            if (settings.Score < int.Parse(entries[^1].Score)) return;
            HighscoreEntry incoming = new(settings.SelectedCharacter.Icon, settings.SelectedCharacter.Name, settings.Score, settings.Time);

            for (int i = 0; i < entries.Length; i++)
            {
                if (settings.Score < int.Parse(entries[i].Score)) continue;
                //swap
                HighscoreEntry temp = entries[i];
                entries[i] = incoming;
                incoming = temp;
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
            Debug.Log("Saved: " + result);
            PlayerPrefs.SetString(KEY_WORD, result);
            PlayerPrefs.Save();
        }

        private string FindCharIndex(Sprite icon)
        {
            for (int i = 0; i < settings.Characters.Count; i++)
            {
                if (settings.Characters[i].Icon.Equals(icon)) return i.ToString();
            }
            return null;
        }
    }
}
