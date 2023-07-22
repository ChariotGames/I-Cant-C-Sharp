using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private Image timer;

    public float Duration { get => duration; set => duration = value; }

    public void Run()
    {
        if (duration <= 0) return; 
        ToggleOnOff(true);
        StartCoroutine(AnimateTimer(duration));
    }

    public void ToggleOnOff(bool state)
    {
        gameObject.SetActive(state);
    }

    /// <summary>
    /// Animates the circular progess bar aka. timer.
    /// </summary>
    /// <param name="duration">The duration of timer.</param>
    /// <returns></returns>
    private IEnumerator AnimateTimer(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            timer.fillAmount = Mathf.Clamp01(progress);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        timer.fillAmount = 1f;
        ToggleOnOff(false);
    }
}
