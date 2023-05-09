using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Input;

public class EndlessRunnerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _jumpHeight = 5;
    private bool isJumping = false;
    

    private void OnEnable()
    {
        InputHandler.DownKeyAction += Jump;
    }

    private void OnDisable()
    {
        InputHandler.DownKeyAction -= Jump;
    }

    public void ButtonPressed()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("test");
        if (collision.gameObject.name == "Ground")
        {
            isJumping = false;
        }
    }

    void Jump()
    {
        
        if (isJumping == false)
        {
            Debug.Log("eey it worked");
            _rb.AddForce(transform.up * _jumpHeight, ForceMode2D.Impulse);
            isJumping = true;
        }

    }
}
