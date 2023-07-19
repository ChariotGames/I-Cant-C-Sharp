using UnityEngine.InputSystem;
using UnityEngine;

namespace Scripts.Games
{
    public class EvadePlayer : MonoBehaviour
    {

        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] public InputActionReference stick;

        private void move()
        {
            //transform.position.x = new Vector3(_player, transform.position.y, transform.position.z);
            float playerPos = transform.localPosition.x;
            
            float moveAmount = stick.action.ReadValue<Vector2>().x * movementSpeed * Time.deltaTime;
            float newPos = Mathf.Clamp(playerPos + moveAmount, -2.3f, 2.3f);
            transform.localPosition = new Vector3(newPos, transform.localPosition.y, transform.localPosition.z);
        }

        // Update is called once per frame
        void Update()
        {
            move();
        }
    }
}
