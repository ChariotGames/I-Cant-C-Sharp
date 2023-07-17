using Scripts.Models;
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


    public void Run(AnimType animType)
    {
        GameObject anim = win;
        if (animType == AnimType.Lose) anim = lose;

        anim.SetActive(true);
        audioSource = anim.GetComponent<AudioSource>();
        particles = anim.GetComponent<ParticleSystem>();
        animator = anim.GetComponent<Animator>();
        animationToPlay = anim.GetComponent<Animation>();
        audioSource.Play();
        particles.Emit(count);
        animator.Play(animationToPlay.clip.name);
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

        anim.SetActive(false);
    }

    private IEnumerator FillImage(GameObject anim, float part, float total)
    {
        Image character = anim.GetComponent<Image>();
        float toFill = part / total;
        while (character.fillAmount < toFill)
        {
            character.fillAmount += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        //anim.SetActive(false);
    }
}
