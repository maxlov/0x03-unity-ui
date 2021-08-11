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
    InputAction charge;

    public Rigidbody rb;

    [System.NonSerialized]
    public Vector3 direction;
    Vector3 lastDirection;
    public float speed;

    [System.NonSerialized]
    public float durationHeld;
    bool isHeld = false;
    public float multiplier;

    private int score = 0;
    private int health = 5;

    private void Start()
    {
        var gameplayActionMap = playerControls.FindActionMap("Player");

        movement = gameplayActionMap.FindAction("Move");
        charge = gameplayActionMap.FindAction("Charge");

        movement.performed += OnMovementChanged;
        movement.canceled += OnMovementChanged;
        movement.Enable();

        charge.performed += chargeStart => { isHeld = true; };
        charge.canceled += chargeRelease;
        charge.Enable();
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
        if (!isHeld)
            rb.AddForce(direction * speed);
        else
            durationHeld += 1;
        Debug.Log(durationHeld);
    }

    private void OnMovementChanged(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();
        inputDirection = Vector2.ClampMagnitude(inputDirection, 1f);

        direction = new Vector3(inputDirection.x, 0f, inputDirection.y);
        if (direction.magnitude > 0)
            lastDirection = direction;
    }

    private void chargeRelease(InputAction.CallbackContext context)
    {
        

        rb.AddForce(lastDirection * durationHeld * multiplier);
        durationHeld = 0;
        isHeld = false;
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
