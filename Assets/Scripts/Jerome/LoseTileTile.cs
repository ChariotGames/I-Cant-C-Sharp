using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Models;
using UnityEngine;

namespace Scripts.Games
{
    public class LoseTileTile : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private LoseTile game;
        [SerializeField] private CircleCollider2D player;
        [SerializeField] private Color[] colors;
        [SerializeField][Range(0,2)] private float timer = 1f;



        #endregion Serialized Fields
        #region Fields

        public ElementType type = ElementType.CHECKPOINT;
        private bool visited = false;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            
        }

        void Update()
        {
            
        }
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.Equals(player)&& !visited)
            {
                visited = true;
                StartCoroutine(AnimateColor(gameObject.GetComponent<SpriteRenderer>(), colors[0], colors[1], timer));
                game.PlayerTouched(gameObject);
                
            }

            if (collision.Equals(player) && visited)
            {
                game.PlayerTouched(gameObject);
                //Destroy(gameObject,timer * 2f);
                gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            type = ElementType.ENEMY;
           
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

            //elapsedTime = 0f;
            //while (elapsedTime < duration)
            //{
            //    sprite.color = Color.Lerp(target, original, elapsedTime / duration);
            //    elapsedTime += Time.deltaTime;
            //    yield return null;
            //}
            //sprite.color = original;
            //Destroy(gameObject, timer/1.5f);
        }

        #endregion Overarching Methods / Helpers
    }
}