using Scripts._Input;
using UnityEngine;

namespace Scripts.Pascal
{
    /// <summary>
    /// Represents the cannon object and its behaviour.
    /// </summary>
    public class Cannon : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private float maxRotationSpeed = 2.5f, maxRotation = 60f;
        [SerializeField] private float bulletSpeed, attackSpeed;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform gunBarrel, gunSpawnPos;

        #endregion Serialized Fields

        #region Fields

        private float _lastShootTime;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            InputHandler.TriggerRight += ShootProjectile;
        }

        private void Update()
        {
            RotateBarrel();
        }

        private void OnDisable()
        {
            InputHandler.TriggerRight -= ShootProjectile;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        /// <summary>
        /// Shopts a new bullet instance.
        /// </summary>
        private void ShootProjectile()
        {
            // checking for "attack speed"
            if (Time.time - _lastShootTime < attackSpeed) return;

            GameObject bullet = Instantiate(bulletPrefab, gunSpawnPos.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, gunSpawnPos.up) * Quaternion.Euler(0f, 0f, 90f);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            Vector3 fireDirection = gunSpawnPos.transform.up;
            bullet.SetActive(true);
            bulletRb.AddForce(fireDirection * bulletSpeed, ForceMode2D.Impulse);
            Destroy(bullet, 5f);

            _lastShootTime = Time.time; // Update the time of the last shot
        }

        /// <summary>
        /// Rotates the cannon barrel.
        /// </summary>
        private void RotateBarrel()
        {
            // only use x-axis movement of input
            float inputDelta = InputHandler.StickLeft.x;
            
            //  Mathf.Sign will return 1,-1 or 0
            Vector2 direction = Vector2.right * Mathf.Sign(inputDelta);

            // calculate the angle to rotate the barrel
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            
            // restrict the angle to the range of -180 to 180 degrees 
            // angle = angle % 360f;
            // if (angle > 180f) {
            //     angle -= 360f;
            // } else if (angle < -180f) {
            //     angle += 360f;
            // } == Mathf.Repeat
            
            angle = Mathf.Repeat(angle + 180f, 360f) - 180f; 
            
            // clamp the angle to the range of -maxRotation to maxRotation
            angle = Mathf.Clamp(angle, -maxRotation, maxRotation);
          
            // multiply the maxRotationSpeed by the inputDelta value
            float rotationSpeed = maxRotationSpeed * Mathf.Abs(inputDelta);
            
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            gunBarrel.rotation = Quaternion.RotateTowards(gunBarrel.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        #endregion Game Mechanics / Methods
    }
}
