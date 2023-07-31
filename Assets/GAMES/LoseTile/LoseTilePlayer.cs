using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
{
    public class LoseTilePlayer : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] LoseTile MainGame;
        [SerializeField] float speed = 5f;
        [SerializeField][Range (2f, 4f)] float x = 2.5f;
        [SerializeField][Range (2f, 4f)] float y = 2.5f;
        [SerializeField] Rigidbody2D body;
        [SerializeField] AudioSource walkSound;

        #endregion Serialized Fields

        #region Fields

        private InputActionReference stick;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Update()
        {
            Vector2 input = stick.action.ReadValue<Vector2>().normalized;
            if (input.magnitude > 0)
            {
                Vector2 newInput;

                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                {
                    newInput.x = input.x;
                    newInput.y = 0.0f;
                }
                else
                {
                    newInput.y = input.y;
                    newInput.x = 0.0f;
                }

                gameObject.transform.Translate(speed * Time.deltaTime * newInput);

                if (!walkSound.isPlaying) walkSound.Play();
       
               
                if (transform.localPosition.x >= x) transform.localPosition = new Vector2(x, transform.localPosition.y);
                if (transform.localPosition.x <= -x) transform.localPosition = new Vector2(-x, transform.localPosition.y);
                if (transform.localPosition.y >= y) transform.localPosition = new Vector2(transform.localPosition.x, y);
                if (transform.localPosition.y <= -y) transform.localPosition = new Vector2(transform.localPosition.x, -y);
            }
             
        
        }


        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

        public InputActionReference Stick { get => stick; set => stick = value; }

        #endregion GetSets / Properties
        
    }
}