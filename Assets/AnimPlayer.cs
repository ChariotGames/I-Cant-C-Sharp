using Scripts.Models;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimPlayer : MonoBehaviour
{
    [SerializeField] private GameObject win, lose;

    private AudioSource audioSource;
    private ParticleSystem particles;
    private Animator animator;
    private Animation animationToPlay;

    private int count, winsMax, wins, fails, failsMax;

    public int WinsMax { set => winsMax = value; }
    public int FailsMax { set => failsMax = value; }
    public int Count { set => count = value; }


    public IEnumerator Run(AnimType animType)
    {
        GameObject anim = win;
        if (animType == AnimType.Lose) anim = lose;

        audioSource = anim.GetComponent<AudioSource>();
        particles = anim.GetComponent<ParticleSystem>();
        animator = anim.GetComponent<Animator>();
        animationToPlay = anim.GetComponent<Animation>();
        audioSource.Play();
        particles.Emit(count);
        //animator.SetBool(param, true);
        animator.Play(animationToPlay.clip.name);
        yield return new WaitForSeconds(1);
        if (animType == AnimType.Win)
        {
            wins++;
            StartCoroutine(FillImage(anim, wins, winsMax));
        }
        else
        {
            fails++;
            StartCoroutine(FillImage(anim, fails, failsMax));
        }
        animator.Play("IdleUI");
        //anim.SetActive(false);
    }

    private IEnumerator FillImage(GameObject anim, float part, float total)
    {
        Image character = anim.GetComponent<Image>();
        float toFill = part / total;
        while (character.fillAmount < toFill)
        {
            character.fillAmount += Time.deltaTime;
            yield return new WaitForSeconds(toFill);
        }
        yield return new WaitForSeconds(toFill);
        if (wins == winsMax) wins = 0;
        if (fails == failsMax) fails = 0;
    }
}