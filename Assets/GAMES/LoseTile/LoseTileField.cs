using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Models;
using UnityEngine;

namespace Scripts.Games
{
    public class LoseTileField : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private LoseTile game;
        [SerializeField] private CircleCollider2D player;
        [SerializeField] private SpriteRenderer outline;
        [SerializeField] private Color[] colors;
        [SerializeField][Range(1,3)] private float timer = 1f;
        [SerializeField] private BoxCollider2D box;

        #endregion Serialized Fields

        #region Fields

        public ElementType type = ElementType.CHECKPOINT;
        private bool visited = false;

        #endregion Fields

        #region Built-Ins / MonoBehaviours
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.Equals(player)&& !visited)
            {
                visited = true;
            
                StartCoroutine(AnimateTimes(3));
  
                game.PlayerTouched(gameObject);

            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.Equals(player) && visited && type == ElementType.ENEMY)
            {
                game.PlayerTouched(gameObject);
                //Destroy(gameObject,timer * 2f);
                box.isTrigger = false;
            }
        }
        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties


        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private IEnumerator AnimateColor(SpriteRenderer sprite, Color original, Color target, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(original, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

        }


        private IEnumerator AnimateTimes(int times)
        {
            SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();

            for (int i = 0; i < times; i++)
            {
                StartCoroutine(AnimateColor(sprite, colors[0], colors[1], timer/2.0f));
                yield return new WaitForSeconds(timer/2.0f);
                StartCoroutine(AnimateColor(sprite, colors[1], colors[0], timer/2.0f));
                yield return new WaitForSeconds(timer/2.0f);

            }
            yield return new WaitForSeconds(1f);
            sprite.color = Color.clear;
            outline.enabled = false;
            type = ElementType.ENEMY;
            
        }
        #endregion Overarching Methods / Helpers
    }
}