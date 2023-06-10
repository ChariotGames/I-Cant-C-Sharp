using _Scripts._Input;
using UnityEngine;

namespace _Scripts.Pascal
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private float maxRotationSpeed = 2.5f;
        [SerializeField] private float maxRotation = 60f;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float attackSpeed;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform gunBarrel;
        [SerializeField] private Transform gunSpawnPos;

        private float _lastShootTime;
        
        
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            InputHandler.TriggerRight += ShootProjectile;
        }

        private void ShootProjectile()
        {
            
            // checking for "attack speed"
            if (Time.time - _lastShootTime < attackSpeed) return;
        

            var bullet = Instantiate(bulletPrefab, gunSpawnPos.position, Quaternion.identity);
            var bulletRb = bullet.GetComponent<Rigidbody2D>();
            var fireDirection = gunSpawnPos.transform.up;
            bulletRb.AddForce(fireDirection * bulletSpeed, ForceMode2D.Impulse);
            Destroy(bullet, 5f);

            _lastShootTime = Time.time; // Update the time of the last shot
        }

        private void Update()
        {
           RotateTowardsMouse();
        }

        private void RotateTowardsMouse()
        {
            // only use x-axis movement of input
            var inputDelta = InputHandler.StickLeft.x;
            
            //  Mathf.Sign will return 1,-1 or 0
            var direction = Vector2.right * Mathf.Sign(inputDelta);

            // calculate the angle to rotate the barrel
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            
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
            var rotationSpeed = maxRotationSpeed * Mathf.Abs(inputDelta);
            
            var targetRotation = Quaternion.Euler(0f, 0f, angle);
            gunBarrel.rotation = Quaternion.RotateTowards(gunBarrel.rotation, targetRotation, Time.deltaTime * rotationSpeed);;
            
        }
        
        
        private void OnDisable()
        {
            InputHandler.TriggerRight -= ShootProjectile;
        }

    }
    
}
