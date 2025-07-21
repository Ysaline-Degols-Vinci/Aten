using System;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;

    private Rigidbody rb;
    private bool isGrounded = true;

    public Vector3 rightDirection = new Vector3(1, 0, 0);  // droite
    public Vector3 upDirection = new Vector3(0, 0, 1);     // haut

    private Vector3 inputDirection = Vector3.zero;
    private float currentSpeed;
    public Transform imageTransform;
    private Vector3 originalScale;

    private Animator animator;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        originalScale = imageTransform.localScale;
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogError("Animator non trouvé sur le joueur ou ses enfants !");
    }

    void Update()
    {
        if (GameEventsManager.Instance.InputEventContext == InputEventContext.DIALOGUE || GameEventsManager.Instance.InputEventContext == InputEventContext.INVENTORY) {
            animator.SetBool("isWalking", false);
            inputDirection = Vector3.zero;
            return; }
            // Entrée uniquement dans Update
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        inputDirection = (rightDirection * inputX + upDirection * inputY).normalized;

        currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }

        if (inputX < 0)
        {
            imageTransform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (inputX > 0)
        {
            imageTransform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }


        bool isMoving = inputDirection.magnitude > 0;
        animator.SetBool("isWalking", isMoving);
    }

    void FixedUpdate()
    {
        if (GameEventsManager.Instance.InputEventContext == InputEventContext.DIALOGUE) return;

        // Appliquer le mouvement horizontal
        Vector3 horizontalVelocity = inputDirection * currentSpeed;
        Vector3 velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        rb.velocity = velocity;

        // Gravité renforcée si chute
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       // if (collision.gameObject.CompareTag("Ground"))
       // {
            isGrounded = true;
        //}
    }
}
