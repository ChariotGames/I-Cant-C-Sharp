using Scripts.Models;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animation))]
public class AnimtionPlayer : MonoBehaviour
{
    [SerializeField] private GameObject win, lose;

    public IEnumerator Run(AnimType animType, int starCount, float fillFraction)
    {
        GameObject anim = win;
        if (animType == AnimType.Lose) anim = lose;

        AudioSource audioSource = anim.GetComponent<AudioSource>();
        ParticleSystem particles = anim.GetComponent<ParticleSystem>();
        Animator animator = anim.GetComponent<Animator>();
        Animation animationToPlay = anim.GetComponent<Animation>();

        audioSource.Play();
        particles.Emit(starCount);
        //animator.SetBool(param, true);
        animator.Play(animationToPlay.clip.name);
        yield return new WaitUntil(() => !animationToPlay.isPlaying);

        StartCoroutine(FillImage(anim, fillFraction));

        animator.Play("IdleUI");
        //anim.SetActive(false);
    }

    /// <summary>
    /// Fills the character Image 
    /// </summary>
    /// <param name="anim">The Object to run the animation on.</param>
    /// <param name="fraction">The fill amount.</param>
    /// <returns></returns>
    private IEnumerator FillImage(GameObject anim, float fraction)
    {
        Image character = anim.GetComponent<Image>();
        while (character.fillAmount < fraction)
        {
            character.fillAmount += Time.deltaTime;
            yield return new WaitUntil(() => character.fillAmount >= fraction);
        }
        yield return new WaitUntil(() => character.fillAmount >= fraction);
    }
}