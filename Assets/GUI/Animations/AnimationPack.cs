using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Image))]
public class AnimationPack : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Animator animator;
    [SerializeField] Animation anim;
    [SerializeField] Image emojiFace;
    [SerializeField] string[] animationOrder;

    public IEnumerator Run(int starCount, float from, float to)
    {
        audioSource.Play();
        particles.maxParticles = starCount;
        particles.Play();

        AnimationClip clip = anim.GetClip(animationOrder[0]);
        animator.Play(animationOrder[0]);
        yield return new WaitUntil(() => !anim.isPlaying);

        StartCoroutine(FillImage(from, to));
        clip = anim.GetClip(animationOrder[1]);
        animator.Play(animationOrder[1]);
        yield return new WaitUntil(() => !anim.isPlaying);

        clip = anim.GetClip(animationOrder[2]);
        animator.Play(animationOrder[2]);
        yield return new WaitUntil(() => !anim.isPlaying);

        yield return new WaitUntil(() => !audioSource.isPlaying);
        Destroy(gameObject, 1f);
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
    }
}