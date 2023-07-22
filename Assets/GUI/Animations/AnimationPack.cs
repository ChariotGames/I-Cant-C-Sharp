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
    
    public void Run(int starCount, float from, float to, string trigger)
    {
        audioSource.Play();
        particles.maxParticles = starCount;
        particles.Play();
        animator.SetTrigger(trigger);

        StartCoroutine(FillImage(from, to));
    }

    /// <summary>
    /// Fills the character Image 
    /// </summary>
    /// <param name="anim">The Object to run the animation on.</param>
    /// <param name="fraction">The fill amount.</param>
    /// <returns></returns>
    private IEnumerator FillImage(float from, float to)
    {
        emojiFace.fillAmount = from;
        while (emojiFace.fillAmount < to)
        {
            emojiFace.fillAmount += Time.deltaTime;
            //yield return new WaitUntil(() => emojiFace.fillAmount >= fraction);
        }
        yield return new WaitUntil(() => emojiFace.fillAmount >= to);
        Destroy(gameObject, 1f);
    }
}