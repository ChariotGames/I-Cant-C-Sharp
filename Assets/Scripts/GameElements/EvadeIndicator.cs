using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games
{
    public class EvadeIndicator : BaseGame
    {
        [SerializeField] private Color[] colors;

        private bool active = false;

        public void SetUp()
        {
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", colors[0]);
            active = true;
            InvokeRepeating(nameof(LoopAnimation), 0, 1);
        }

        public void SetDefault()
        {
            active = false;
        }

        private void LoopAnimation()
        {
            if (!active)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", colors[2]);
                CancelInvoke();
            }
            else
            {
                StartCoroutine(AnimateColor(gameObject.GetComponent<SpriteRenderer>(), colors[0], colors[1], 0.45f));
            }
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
    }
}

