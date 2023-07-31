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

        #endregion Serialized Fields

        #region Fields

        private InputActionReference stick;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Update()
        {
            Vector2 input = stick.action.ReadValue<Vector2>().normalized;
            //float x = input.x;
            //float y = input.y;
            //Vector2 newInput;

            //if (Mathf.Abs(x) > Mathf.Abs(y))
            //{
            //    Debug.Log("x = " + (1.0f / Mathf.Abs(x)));
            //    newInput.x = (1.0f /Mathf.Abs(x)) * x;
            //    newInput.y = 0.0f;
            //}
            //else
            //{
            //    Debug.Log("y = " + (1.0f / Mathf.Abs(y)));
            //    newInput.y = (1.0f / Mathf.Abs(y)) * y;
            //    newInput.x = 0.0f;
            //}

            gameObject.transform.Translate(speed * Time.deltaTime * input);

            if (transform.localPosition.x >= x) transform.localPosition = new Vector2(x, transform.localPosition.y);
            if (transform.localPosition.x <= -x) transform.localPosition = new Vector2(-x, transform.localPosition.y);
            if (transform.localPosition.y >= y) transform.localPosition = new Vector2(transform.localPosition.x, y);
            if (transform.localPosition.y <= -y) transform.localPosition = new Vector2(transform.localPosition.x, -y);
        }


        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

        public InputActionReference Stick { get => stick; set => stick = value; }

        #endregion GetSets / Properties
        
    }
}