using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    InputActionAsset playerControls;
    InputAction movement;

    public Rigidbody rb;

    [System.NonSerialized]
    public Vector3 direction;
    public float speed;

    private int score = 0;
    private int health = 5;

    private void Start()
    {
        var gameplayActionMap = playerControls.FindActionMap("Player");

        movement = gameplayActionMap.FindAction("Move");

        movement.performed += OnMovementChanged;
        movement.canceled += OnMovementChanged;
        movement.Enable();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(direction * speed);
    }

    private void OnMovementChanged(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();
        inputDirection = Vector2.ClampMagnitude(inputDirection, 1f);

        direction = new Vector3(inputDirection.x, 0f, inputDirection.y);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            score += 1;
            Debug.Log("Score: " + score);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Trap"))
        {
            health -= 1;
            Debug.Log("Health: " + health);
        }
        else if (other.CompareTag("Goal"))
        {
            Debug.Log("You win!");
        }
    }
}
