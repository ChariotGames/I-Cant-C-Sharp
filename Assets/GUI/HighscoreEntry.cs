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

    public Sprite Icon { get => icon.sprite; set => icon.sprite = value; }
    public string CharName { get => charName.text; set => charName.text = value; }
    public string Score { get => score.text; set => score.text = value; }
    public string Time { get => time.text; set => time.text = value; }
}
