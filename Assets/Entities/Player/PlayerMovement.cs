using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 _direction;
    public Vector2 Direction
    {
        get
        {
            Vector2 currentInput = moveAction.ReadValue<Vector2>();
            if (currentInput.magnitude > 0)
                _direction = currentInput.normalized;

            return _direction;
        }
        set { }
    }

    private bool isMovementBlocked = false;
    public bool IsMovementBlocked => isMovementBlocked;
    private Rigidbody2D weaponRb;

    // New Input System reference
    private PlayerInput playerInput;
    private InputAction moveAction;
    private PlayerController player;
    private WeaponSlot weaponSlot;

    private void Awake()
    {
        // Initialize the PlayerInput and get the movement action
        playerInput = new PlayerInput();
        moveAction = playerInput.Player.Move;
        player = GetComponentInParent<PlayerController>();
        weaponSlot = player.GetComponentInChildren<WeaponSlot>();
    }

    private void OnEnable()
    {
        // Enable the movement action
        moveAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the movement action
        moveAction.Disable();
    }

    private void FixedUpdate()
    {
        // If movement is blocked, don't move
        if (isMovementBlocked)
            return;

        // Get movement input from the new input system
        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        // Calculate movement based on input
        Vector2 movement = movementInput * speed;

        // Move the player using Rigidbody2D MovePosition
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        if (weaponRb != null)
        {
            weaponRb.MovePosition(rb.position);
        }

        // Update the player transform (the parent object's transform)
        // GetComponentInParent<PlayerController>().transform.position = rb.position;
    }

    private void LateUpdate()
    {
        // Update the player transform (the parent object's transform)
        player.transform.position = rb.position;
        // Update local position back to 0
        rb.transform.localPosition = Vector3.zero;
        // Update the weapon slots transform
        // weaponSlot.transform.localPosition = new(1.5f,);
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
}
