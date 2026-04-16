using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharachterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Animator _animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    
    private void FixedUpdate()
    {
        // Using rb.MovePosition for smooth physics-based movement in Top-Down
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        _animator.SetFloat("speed", rb.linearVelocity.magnitude);

        _sr.flipX = rb.linearVelocityX > 0;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}

