using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    private bool isDashOnColldown = false;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 _movementInput;
    private bool isDashing = false;
    private bool isMovementBlocked = false;

    // New Input System references
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;

    private PlayerController player;
    private Rigidbody2D weaponRb;

    public bool IsMovementBlocked => isMovementBlocked;
    public bool IsDashing => isDashing;
    public bool IsMoving => _movementInput.magnitude > 0;
    public Vector3 Direction => _movementInput.normalized;

    [SerializeField]
    private WeaponSlot weaponSlot;

    [SerializeField]
    private Health health;

    private Vector2 dashDirection;

    private void Awake()
    {
        playerInput = new PlayerInput();
        moveAction = playerInput.Player.Move;
        dashAction = playerInput.Player.Dash;

        player = GetComponentInParent<PlayerController>();

        dashAction.performed += OnDash;

        // Subscribe to weapon set event
        weaponSlot.OnWeaponSet += SetWeapon;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
    }

    private void FixedUpdate()
    {
        if (isMovementBlocked || health.IsDead)
            return;

        if (isDashing)
        {
            rb.MovePosition(rb.position + dashSpeed * Time.fixedDeltaTime * dashDirection);
            if (weaponRb != null)
            {
                weaponRb.MovePosition(rb.position);
            }
            return;
        }
        else
        {
            _movementInput = moveAction.ReadValue<Vector2>();

            Vector2 movement = _movementInput.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            if (weaponRb != null)
            {
                weaponRb.MovePosition(rb.position);
            }
        }
    }

    private void LateUpdate()
    {
        player.transform.position = rb.position;
        rb.transform.localPosition = Vector3.zero;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!isDashing && !isMovementBlocked && !isDashOnColldown && !health.IsDead)
        {
            health.IsInvincible = true;
            EventBus.Instance.TriggerPlayerDash();

            if (_movementInput.magnitude > 0)
            {
                // Dash in the direction of movement
                dashDirection = _movementInput.normalized;
            }
            else
            {
                // Dash towards the mouse cursor if not moving
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(
                    Mouse.current.position.ReadValue()
                );
                dashDirection = (mousePosition - rb.position).normalized;
            }

            StartDash();
        }
    }

    private void StartDash()
    {
        isDashing = true;
        Timercore.CreateTimer("DashTimer").SetLength(dashDuration).OnComplete(EndDash).Start();
    }

    private void EndDash()
    {
        isDashing = false;
        health.IsInvincible = false;
        EventBus.Instance.TriggerPlayerDashEnd();
        isDashOnColldown = true;
        Timercore
            .CreateTimer("DashCooldown")
            .SetLength(dashCooldown)
            .OnComplete(() => isDashOnColldown = false)
            .Start();
    }

    public void BlockMovement()
    {
        isMovementBlocked = true;
    }

    public void UnblockMovement()
    {
        isMovementBlocked = false;
    }

    public void SetWeapon(Weapon weapon)
    {
        weaponRb = weapon.GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        weaponSlot.OnWeaponSet -= SetWeapon;
    }

    private void EndDashCooldown()
    {
        isDashOnColldown = false;
    }
}
