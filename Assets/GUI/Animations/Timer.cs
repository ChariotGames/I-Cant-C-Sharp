using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image timer;
    private float duration = 0f;
    private Coroutine routine;

    public float Duration { get => duration; set => duration = value; }

    /// <summary>
    /// Runs the respective timer.
    /// </summary>
    public void Run()
    {
        if (duration <= 0) return; 
        ToggleOnOff(true);
        routine = StartCoroutine(AnimateTimer(duration));
    }

    /// <summary>
    /// Stops the respective timer.
    /// </summary>
    public void Stop()
    {
        StopCoroutine(routine);
        timer.fillAmount = 0;
        ToggleOnOff(false);
    }

    public void ToggleOnOff(bool state) =>
        gameObject.SetActive(state);

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
            yield return new WaitForSeconds(Time.deltaTime);
            elapsedTime += Time.deltaTime;
        }
        Debug.Log(elapsedTime);

        Stop();
    }
}
