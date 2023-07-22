using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Image))]
public class AnimationPack : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Animator animator;
    [SerializeField] Image emojiFace;
    
    /// <summary>
    /// Run chosen animation.
    /// </summary>
    /// <param name="starCount">Points won/lost.</param>
    /// <param name="numerator">Successes/Fails</param>
    /// <param name="denominator">Trys left.</param>
    /// <param name="trigger">Animation trigger.</param>
    public void Run(int starCount, float numerator, float denominator, string trigger)
    {
        audioSource.Play();
        particles.maxParticles = starCount;
        particles.Play();
        animator.SetTrigger(trigger);

        StartCoroutine(FillImage(numerator, denominator));
    }

    /// <summary>
    /// Fills the character Image 
    /// </summary>
    /// <param name="anim">The Object to run the animation on.</param>
    /// <param name="fraction">The fill amount.</param>
    /// <returns></returns>
    private IEnumerator FillImage(float numerator, float denominator)
    {
        float from = (numerator - 1) / denominator;
        float to = numerator / denominator;

        emojiFace.fillAmount = from;
        while (emojiFace.fillAmount < to)
        {
            emojiFace.fillAmount += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //yield return new WaitUntil(() => emojiFace.fillAmount >= to);
        Destroy(gameObject, audioSource.clip.length);
    }
}