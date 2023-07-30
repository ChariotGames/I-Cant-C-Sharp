using System.Collections;
using Scripts.Games;
using UnityEngine;

public class ExpBomb : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Exploooosions parent;
    [SerializeField] private float size;
    [SerializeField] private SpriteRenderer spriteBomb;
    [SerializeField] private SpriteRenderer spriteBase;
    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioClip[] clips;

    private bool active = false;
    private bool danger = false;

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        InvokeRepeating(nameof(LoopAnimation), 0, 1);
        Invoke(nameof(SetDefault), 3);
    }

    public void SetDefault()
    {
        active = false;
        sound.time = 0.65f;
        sound.clip = clips[Random.Range(0, clips.Length)];
        sound.Play();
        spriteBomb.enabled = true;
        spriteBase.enabled = true;
    }

    private void LoopAnimation()
    {
        if (!active)
        {
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", colors[0]);
            CancelInvoke();
            
            Vector3 newSize = new(size, size, transform.position.z);
            StartCoroutine(AnimateSize(gameObject.transform.localScale, newSize, 0.1f));
            StartCoroutine(AnimateColor(gameObject.GetComponent<SpriteRenderer>(), colors[0], colors[1], 0.45f));
            StartCoroutine(AnimateColor(gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>(), colors[0], colors[1], 0.45f));
            Invoke(nameof(SetOff), 0.45f);
            
        }
        else
        {
            StartCoroutine(AnimateColor(gameObject.GetComponent<SpriteRenderer>(), colors[0], colors[1], 0.45f));
            StartCoroutine(AnimateColor(gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>(), colors[0], colors[1], 0.45f));
        }
    }

    private void SetOff()
    {
        
        parent.CheckWinCondition(parent.player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), gameObject);
        Destroy(gameObject);
    }

    private IEnumerator AnimateColor(SpriteRenderer sprite, Color original, Color target, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            sprite.color = Color.Lerp(original, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            sprite.color = Color.Lerp(target, original, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sprite.color = original;
    }

    private IEnumerator AnimateSize(Vector3 original, Vector3 target, float duration)
    {
        spriteBomb.enabled = false;
        spriteBase.enabled = false;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            gameObject.transform.localScale = Vector3.Lerp(original, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

        // Update is called once per frame
        void Update()
    {
        if (danger == true) parent.CheckWinCondition(parent.player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), gameObject);
    }
}
