using System;
using System.Collections;
using System.Numerics;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
     static PlayerController instance { get; set; }
    public static PlayerController Instance
    {
        get
        {
            return instance;
        }

        private set
        {
            instance = value;
            OnInitialize?.Invoke();
        }
    }
    
    
    public static event Action OnInitialize;
    
    [Header("States")]
    [SerializeField] private MovementParams walking;
    [SerializeField] private MovementParams inAir;
    [SerializeField] private MovementParams running;
    [SerializeField] private MovementParams crouching;
    [SerializeField] MovementParams currentMovement;
    
    [Header("General Parameters")]
    [SerializeField] public string characterName = "Player";
    [SerializeField] private float movementPauseTime = 1f;
    [SerializeField] float groundDist = 0.5f;
    
    [Header("Sliding Parameters")]
    [SerializeField] private float slideVelocity = 5f;
    
    [Header("Wall Hopping Parameters")]
    [SerializeField] private float wallHopForce = 50f;
    [SerializeField, Tooltip("Upwards bias for wall hopping")] 
    private float hopUpBias = 5f;
    
    [Header("Jumping Parameters")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] private float jumpApexSpeed = 0.1f;
    [SerializeField] private float jumpApexGravity = 0.5f;

    [SerializeField, Tooltip("How many times slower the player falls after reaching the apex")]
    private float fallSpeed = 0.5f;
    
    
    // components and tracker variables
    CapsuleCollider collider;
    private Rigidbody rb;
    private ConstantForce cf;
    private Animator animator;
    
    // State tracking
    [SerializeField] private Vector3 currentInput;
    private bool moving = false;
    private bool noMove = false;
    [SerializeField] private bool sliding = false;
    private Vector3 cachedWallNormal = Vector3.zero;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
        animator = transform.GetChild(1).GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        currentMovement = walking;
    }


    public void GoToDest(Vector3 dest)
    {
        // move to the destination
        rb.MovePosition(dest);
    }
    

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Debug.Log("Instance cleared");
        }
    }

    private Vector3 cachedAnimState;
    void OnMove(InputValue value)
    {
        currentInput = value.Get<Vector2>();
        currentInput = new Vector3(currentInput.x, 0, currentInput.y);
        
        if (cachedAnimState != currentInput)
        {
            cachedAnimState = currentInput;
            animator.SetFloat("VerticalInput", currentInput.z);
            animator.SetFloat("HorizontalInput", currentInput.x);

        }
        
        if (!moving)
        {
            moving = true;
            currentInput.Normalize();
            StartCoroutine(Move());
        }
        
        // allow the input to stop
        if (currentInput.magnitude < 0.1f)
        {
            moving = false;
        }

    }
    
    
    void OnSprint(InputValue value)
    {
        if (currentMovement == crouching)
        {
            StopCrouch();        
        }
        
        
        if (value.isPressed)
        {
            currentMovement = running;
            return;
        }
        currentMovement = walking;
        
    }


    bool IsGrounded()
    {
        Vector3 size = collider.bounds.extents;
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.down, out hit, size.y + groundDist);
    }   
    
    
    void OnJump(InputValue value)
    {
        if (IsGrounded())
        {
            StartCoroutine(Jump());
            return;
        }
        
        if (sliding)
        {
            StartCoroutine(WallHop());
            return;
        }
    }


    IEnumerator Jump()
    {
        float gravity = cf.force.y;
        rb.AddForce(transform.up * jumpForce);
    
        // Avoid triggering is grounded as soon as the jump starts
        yield return new WaitUntil(() => !IsGrounded());

        while (!IsGrounded())
        {
            yield return new WaitForFixedUpdate();

            // slowdown at the designated apex
            if (rb.linearVelocity.y > -jumpApexSpeed && rb.linearVelocity.y < jumpApexSpeed)
            {
                cf.force = new Vector3(0, jumpApexGravity, 0);
                continue;
            }
            
            cf.force = new Vector3(0, rb.linearVelocity.y > 0 ? gravity : gravity * fallSpeed, 0);
            
        }

        cf.force = new Vector3(0, gravity, 0);
    }


    void OnCrouch(InputValue value)
    {
        
        if (value.isPressed && !currentMovement.Equals(crouching))
        {
            float oldHeight;
            
            if (!collider || currentMovement == crouching)
            {
                Debug.LogError("Collider is not a BoxCollider");
                return;
            }
            
            currentMovement = crouching;
            oldHeight = collider.height;
            collider.height *= 0.5f;
            collider.center = new Vector3(collider.center.x,collider.center.y - (oldHeight * 0.15f), collider.center.z);
        
            
            return;
        }

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.up, groundDist))
        {
            StopCrouch();
        }

    }


    void StopCrouch()
    {
        collider.height *= 2f;
        collider.center = new Vector3(collider.center.x,collider.center.y + (collider.height * 0.15f), collider.center.z);
        
        currentMovement = walking;
    }


    IEnumerator Move()
    {
        while (moving && !sliding)
        {
            if (noMove)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }
            
            float acceleration = currentMovement.acceleration;
            float maxSpeed = currentMovement.maxSpeed;
            if (!IsGrounded())
            {
                acceleration = inAir.acceleration;
                maxSpeed = inAir.maxSpeed;
            }
            
            
            // Horizontal plane movement
            float xSpeed = rb.linearVelocity.x + (acceleration * currentInput.x * Time.deltaTime);
            float zSpeed = rb.linearVelocity.z + (acceleration * currentInput.z * Time.deltaTime);
            
            xSpeed = Mathf.Clamp(xSpeed, -maxSpeed, maxSpeed);
            zSpeed = Mathf.Clamp(zSpeed, -maxSpeed, maxSpeed);
            
            // perserves the vertical speed
            float ySpeed = rb.linearVelocity.y;
            rb.linearVelocity = new Vector3(xSpeed, ySpeed, zSpeed);
            
            yield return new WaitForFixedUpdate();
        }
        
        animator.SetFloat("VerticalInput", 0);
        animator.SetFloat("HorizontalInput", 0);

    }
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (IsGrounded())
        {
            sliding = false;
            return;
        }
        
        cachedWallNormal = other.GetContact(0).normal;
        if (!IsGrounded())
        {
            StartCoroutine(WallSlide());
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        sliding = false;
    }


    IEnumerator WallHop()
    {
        // hop off a wall
        if (cachedWallNormal.x != 0 || cachedWallNormal.z != 0)
        {
            currentInput = Vector3.zero;
            moving = false;
            yield return new WaitForSeconds(0.05f);
            rb.AddForce(wallHopForce * (cachedWallNormal), ForceMode.Acceleration);

            yield return new WaitForSeconds(0.05f);
            rb.AddForce(new Vector3(0, wallHopForce * hopUpBias, 0), ForceMode.Acceleration);

        }
        
        // pause the movement for a bit to let hop occur
        StartCoroutine(PauseMovement());
    }
    
    
    // allow the player to slide down a wall
    IEnumerator WallSlide()
    {
        // if the player is not on a wall, don't slide
        if (!(cachedWallNormal.x != 0 || cachedWallNormal.z != 0 || IsGrounded()))
            yield break;
        rb.linearVelocity = Vector3.zero;
        
        sliding = true;
        while (sliding && !IsGrounded())
        {
            // Check if player wants to leave slide
            bool x = Mathf.Abs(currentInput.x) > 0 && Mathf.Approximately(currentInput.x, cachedWallNormal.x);
            bool y = Mathf.Abs(currentInput.y) > 0 && Mathf.Approximately(currentInput.y, cachedWallNormal.z);
            if ( x || y )
            {
                sliding = false;
            }
            
            // set slide to set speed
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -slideVelocity, rb.linearVelocity.z);
            yield return new WaitForFixedUpdate();
        }
    }


    // pause the movement for a bit
    IEnumerator PauseMovement()
    {
        noMove = true;
        yield return new WaitForSeconds(movementPauseTime);
        noMove = false;
    }
    
    
    
}
