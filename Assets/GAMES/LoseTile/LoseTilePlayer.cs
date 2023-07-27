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