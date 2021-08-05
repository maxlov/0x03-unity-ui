using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Start()
    {
        var gameplayActionMap = playerControls.FindActionMap("Player");

        movement = gameplayActionMap.FindAction("Move");

        movement.performed += OnMovementChanged;
        movement.canceled += OnMovementChanged;
        movement.Enable();
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
    }
}
