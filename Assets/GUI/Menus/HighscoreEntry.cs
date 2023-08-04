using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages content of the highscore entries.
/// By abstracting it, it makes access and readability better.
/// </summary>
public class HighscoreEntry : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text charName, score, time;

    public void CopyValues(HighscoreEntry that)
    {
        icon.sprite = that.icon.sprite;
        charName.text = that.charName.text;
        score.text = that.score.text;
        time.text = that.time.text;
    }

    public void SetValues(Sprite icon, string name, int score, int time)
    {
        this.icon.sprite = icon;
        this.charName.text = name;
        this.score.text = score.ToString("D3");
        this.time.text = TimeSpan.FromSeconds(time).ToString("mm':'ss");
    }

    public Sprite Icon { get => icon.sprite; set => icon.sprite = value; }
    public string Name { get => charName.text; set => charName.text = value; }
    public string Score { get => score.text; set => score.text = value; }
    public string Time { get => time.text; set => time.text = value; }
}