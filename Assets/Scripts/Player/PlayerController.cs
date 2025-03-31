using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("View")]
    [SerializeField, Tooltip("The main camera")]
    private GameObject mainCamera;

    [SerializeField, Tooltip("")]
    private float baseFOV = 60f;

    [SerializeField, Tooltip("")]
    private float maxFOV = 80f;

    [Header("Environment Control")]
    [SerializeField, Tooltip("The scale of gravity applied to the player")]
    private float gravityScale = 2f;

    [SerializeField, Tooltip("The force applied to the player's movement while in midair")]
    private float airControlForce = 15f;

    [SerializeField, Tooltip("How quickly the player can rotate while in midair")]
    private float airRotationSpeed = 15f;

    [SerializeField]
    private float groundDetectionRange = .6f;

    [SerializeField]
    private LayerMask groundMask;

    [Header("Movement")]
    [SerializeField, Tooltip("The speed at which the player moves")]
    private float speed = 45f;

    [SerializeField, Tooltip("The force applied to the player to make them jump")]
    private float jumpForce = 15f;

    [SerializeField, Tooltip("Time between jumps")]
    private float jumpCooldown = 0.5f;

    [SerializeField, Tooltip("How fast a player can change their direction")]
    private float turnSpeed = 5f;

    [SerializeField, Tooltip("Maximum speed the player can achieve")]
    private float maxSpeed = 200f;

    [SerializeField, Tooltip("The rate at which the player slows down when no input is applied")]
    private float decelerationRate = 1f;

    [SerializeField, Tooltip("How bouncy the player is when colliding with surfaces")]
    private float bounciness = .8f;

    [SerializeField]
    private float shakeIntensity = .3f;

    [SerializeField]
    private float shakeDuration = 1f;

    [SerializeField, Tooltip("Minimum collision magnitude before shaking camera")]
    private float shakeImpactThreshold = 35f;

    [Header("Default Conditions")]
    [SerializeField]
    private float defaultSpeed = 45f;

    [SerializeField]
    private float defaultTurnSpeed = 5f;

    [SerializeField]
    private float defaultDecelerationRate = 1f;

    [SerializeField]
    private float defaultTorque = 1f;

    [SerializeField, Tooltip("How bouncy the player is when colliding with surfaces")]
    private float defaultBounciness = .8f;

    [Header("Sand Conditions")]
    [SerializeField]
    private float sandSpeed = 30f;

    [SerializeField]
    private float sandTurnSpeed = 3.5f;

    [SerializeField]
    private float sandDecelerationRate = 4f;

    [SerializeField]
    private float sandTorque = 1f;

    [SerializeField]
    private float sandBounciness = 0f;

    [Header("Ice Conditions")]
    [SerializeField]
    private float iceSpeed = 15f;

    [SerializeField]
    private float iceTurnSpeed = 1f;

    [SerializeField]
    private float iceDecelerationRate = 0.2f;

    [SerializeField]
    private float iceTorque = 5f;

    [SerializeField]
    private float iceBounciness = .8f;

    [Header("Development")]
    [SerializeField]
    private bool debugRays;

    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private CameraShaker cameraShaker;
    private Camera cameraComponent;
    private Vector3 gravityDirection;
    private List<GravityDirection> gravityDirections;
    private float movementX;
    private float movementY;
    private float lastJumpTime = -Mathf.Infinity;
    private bool isGrounded;

    void Awake()
    {
        cameraComponent = mainCamera.GetComponent<Camera>();
        cameraShaker = mainCamera.GetComponent<CameraShaker>();
        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        gravityDirections = new();
        rb.angularDamping = 0;
        rb.linearDamping = 0;
        sphereCollider.sharedMaterial.bounciness = bounciness;
    }

    void FixedUpdate()
    {
        UpdateGravityDirection(); // Apply gravity changes first
        UpdateGravity();

        Vector3 movement = GetCameraRelativeMovement();
        UpdateFOV(movement);

        CheckGroundStatus();

        if (isGrounded)
        {
            if (movement.magnitude > 0)
                HandleGroundMovement(movement);
            else
                HandleIdleMovement();
        }
        else if (movement.magnitude > 0)
        {
            HandleAirMovement(movement);
        }

        EnforceSpeedLimit();
        HandleJump();
    }

    void HandleGroundMovement(Vector3 movement)
    {
        Vector3 movementForce = movement * speed;
        rb.AddForce(movementForce, ForceMode.Force);

        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.fixedDeltaTime
        );
    }

    void HandleIdleMovement()
    {
        // Decrease velocity when no input is applied
        Vector3 horizontalVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        horizontalVelocity = Vector3.Lerp(
            horizontalVelocity,
            Vector3.zero,
            decelerationRate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            horizontalVelocity.x,
            rb.linearVelocity.y,
            horizontalVelocity.z
        );

        // Decrease angular velocity when not moving
        rb.angularVelocity = Vector3.Lerp(
            rb.angularVelocity,
            Vector3.zero,
            decelerationRate * Time.fixedDeltaTime
        );
    }

    void HandleAirMovement(Vector3 movement)
    {
        // Add rotation and reduced velocity in midair
        rb.AddForce(movement * airControlForce, ForceMode.Force);

        Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement);
        rb.AddTorque(rotationAxis * airRotationSpeed, ForceMode.Force);
    }

    void EnforceSpeedLimit()
    {
        Vector3 horizontalVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        // Add excess force against player
        if (currentSpeed > maxSpeed)
        {
            Vector3 excessVelocity = horizontalVelocity.normalized * (currentSpeed - maxSpeed);

            rb.AddForce(-excessVelocity * rb.mass, ForceMode.Force);
        }
    }

    void CheckGroundStatus()
    {
        if (debugRays)
        {
            Debug.DrawRay(
                transform.position,
                gravityDirection.normalized * groundDetectionRange,
                Color.blue
            );
        }
        isGrounded = Physics.Raycast(
            transform.position,
            gravityDirection.normalized,
            groundDetectionRange,
            groundMask
        );
    }

    void UpdateFOV(Vector3 movement)
    {
        float targetFOV = Mathf.Lerp(baseFOV, maxFOV, movement.magnitude);
        cameraComponent.fieldOfView = Mathf.Lerp(
            cameraComponent.fieldOfView,
            targetFOV,
            Time.fixedDeltaTime
        );
    }

    void UpdateGravityDirection()
    {
        gravityDirection = Physics.gravity;

        // https://github.com/ThermiteFe8/Custom-Gravity-Physics-Unity/blob/main/Assets/Scripts/GravityUtils/CustomGravityAffected.cs
        if (gravityDirections.Count > 0)
        {
            int highestPriority = -100;
            Vector3 gravity = Vector3.zero;
            for (int i = 0; i < gravityDirections.Count; i++)
            {
                GravityDirection fallingRequest = gravityDirections[i];
                if (fallingRequest.priority > highestPriority)
                {
                    gravity = fallingRequest.gravity;
                    highestPriority = fallingRequest.priority;
                }
            }
            gravityDirection = gravity;
        }
        gravityDirections.Clear();
    }

    void UpdateGravity()
    {
        rb.linearVelocity += gravityScale * rb.mass * Time.fixedDeltaTime * gravityDirection;
    }

    // Rotate movement vector inline with camera rotation
    Vector3 GetCameraRelativeMovement()
    {
        Vector3 inputDirection = new Vector3(movementX, 0f, movementY).normalized;
        if (inputDirection.magnitude <= 0)
            return Vector3.zero;

        // Find the forward direction relative to the new gravity
        Vector3 cameraForward = Vector3
            .ProjectOnPlane(cameraComponent.transform.forward, -gravityDirection)
            .normalized;
        Vector3 cameraRight = Vector3
            .ProjectOnPlane(cameraComponent.transform.right, -gravityDirection)
            .normalized;

        Vector3 cameraRelativeMovement = (
            cameraRight * movementX + cameraForward * movementY
        ).normalized;

        return cameraRelativeMovement;
    }

    void HandleJump()
    {
        if (
            isGrounded
            && Keyboard.current.spaceKey.isPressed
            && Time.time - lastJumpTime > jumpCooldown
        )
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            rb.AddForce(-1 * jumpForce * gravityDirection.normalized, ForceMode.Impulse);

            lastJumpTime = Time.time;
            isGrounded = false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        ApplyMaterialConditions(collision.gameObject);

        if (impactForce > shakeImpactThreshold)
        {
            float intensity = Mathf.Clamp(impactForce / 40f, 0.05f, 1f) * shakeIntensity;
            float duration = Mathf.Clamp(impactForce / 50f, 0.1f, 0.5f) * shakeDuration;

            StartCoroutine(cameraShaker.Shake(duration, intensity));
        }
    }

    void ApplyMaterialConditions(GameObject gameObject)
    {
        if (gameObject.CompareTag("Sand"))
        {
            speed = sandSpeed;
            turnSpeed = sandTurnSpeed;
            decelerationRate = sandDecelerationRate;
            sphereCollider.sharedMaterial.bounciness = sandBounciness;
        }
        else if (gameObject.CompareTag("Ice"))
        {
            speed = iceSpeed;
            turnSpeed = iceTurnSpeed;
            decelerationRate = iceDecelerationRate;
            sphereCollider.sharedMaterial.bounciness = iceBounciness;
        }
        else
        {
            speed = defaultSpeed;
            turnSpeed = defaultTurnSpeed;
            decelerationRate = defaultDecelerationRate;
            sphereCollider.sharedMaterial.bounciness = defaultBounciness;
        }
    }

    // void HandleSurfaceAlignment() {
    //   if (Physics.Raycast(transform.position, gravityDirection, out RaycastHit hit, groundMask) && hit.collider.gameObject.layer == LayerMask.NameToLayer("AlignableSurface")) {
    //     gravityDirection = -hit.normal * 9.82f;

    //     Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
    //     transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.fixedDeltaTime);
    //   }
    // }

    public void AddGravityDirection(GravityDirection gravityDirection)
    {
        gravityDirections.Add(gravityDirection);
    }

    public void AdjustCameraOrientation(Vector3 newGravityDirection)
    {
        CameraController cameraController = mainCamera.GetComponent<CameraController>();
        if (cameraController != null)
        {
            cameraController.AlignWithGravity(newGravityDirection);
        }
    }
}
