using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementParams walking;
    [SerializeField] private MovementParams running;
    [SerializeField] private MovementParams crouching;
    [SerializeField] MovementParams currentMovement;
    
    [SerializeField] float jumpForce = 5f;

    private Rigidbody rb;
    Collider collider;
    [SerializeField] private Vector3 currentInput;
    private bool moving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        currentMovement = walking;
    }

    void OnMove(InputValue value)
    {
        currentInput = value.Get<Vector2>();
        currentInput = new Vector3(currentInput.x, 0, currentInput.y);
        
        if (!moving)
        {
            moving = true;
            StartCoroutine(Move());
            currentInput.Normalize();
        }
        
        if (currentInput.magnitude < 0.1f)
        {
            moving = false;
            //rb.velocity = Vector3.zero;
        }

    }
    
    
    void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            currentMovement = running;
            return;
        }
        currentMovement = walking;
        
    }
    
    
    void OnJump(InputValue value)
    {
        RaycastHit hit;
        Vector3 size = collider.bounds.extents;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, size.y + 0.1f))
            return;
        
        rb.AddForce(transform.up * jumpForce);
    }
    
    void OnCrouch(InputValue value)
    {
        
    }


    IEnumerator Move()
    {
        while (moving)
        {
            rb.AddForce(currentInput * (currentMovement.acceleration * Time.deltaTime), ForceMode.Acceleration);
            
            // perserves the vertical speed
            float ySpeed = rb.linearVelocity.y;
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, currentMovement.maxSpeed);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, ySpeed, rb.linearVelocity.z);
            
            yield return new WaitForFixedUpdate();
        }
    }
}
