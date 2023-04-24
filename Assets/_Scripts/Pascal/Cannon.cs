using _Scripts.Input;
using UnityEngine;

namespace _Scripts.Pascal
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private float maxRotation = 60f;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform gunBarrel;
        [SerializeField] private Transform gunSpawnPos;
        private Camera _mainCamera;
        
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnEnable()
        {
            InputHandler.UpKeyAction += ShootProjectile;
        }

        private void ShootProjectile()
        {
            var bullet = Instantiate(bulletPrefab, gunSpawnPos.position, Quaternion.identity);
            var bulletRb = bullet.GetComponent<Rigidbody2D>();
            var fireDirection = gunSpawnPos.transform.up;
            bulletRb.AddForce(fireDirection * bulletSpeed, ForceMode2D.Impulse);
            Destroy(bullet, 5f);
            
        }

        private void Update()
        {
           RotateTowardsMouse();
        }

        private void RotateTowardsMouse()
        {
            // get the mouse position in world space
            var mousePos = _mainCamera.ScreenToWorldPoint(InputHandler.CourserPos);
                     
            // calculate the direction from the barrel to the mouse
            var direction = mousePos - gunBarrel.transform.position;
                     
            // calculate the angle to rotate the barrel
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            // restrict the angle to the range of -180 to 180 degrees 
            // angle = angle % 360f;
            // if (angle > 180f) {
            //     angle -= 360f;
            // } else if (angle < -180f) {
            //     angle += 360f;
            // }
            angle = Mathf.Repeat(angle + 180f, 360f) - 180f; // new 
                     
            // clamp the angle to the range of -maxRotation to maxRotation
            angle = Mathf.Clamp(angle, -maxRotation, maxRotation);
          
            // smoothly adjust the rotation to the clamped angle using lerp
            var targetRotation = Quaternion.Euler(0f, 0f, angle);
            gunBarrel.rotation = targetRotation;
        }



        
        private void OnDisable()
        {
            InputHandler.UpKeyAction -= ShootProjectile;
        }

    }
    
}
