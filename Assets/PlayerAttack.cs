using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region Variables
    public float maxSwingAngle = 90.0f; // Positive value for pullback angle
    public float minSwingAngle = 45.0f; // Positive value for swing forward

    public float swingTime = 0.1f; // Duration of the swing in seconds
    public float reloadTime = 0.2f; // Duration of the reload in seconds

    [SerializeField]
    private float rotateSpeed = 10f;

    [SerializeField]
    private Transform weaponPivot;

    [SerializeField]
    private AttackType attackType;

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Transform ghostWeaponPivot;

    private Rigidbody2D weaponRb;

    [SerializeField]
    private Rigidbody2D ghostWeaponRb;
    private bool IsDead => GetComponent<PlayerController>().IsDead;
    private Coroutine swingCoroutine;
    private Coroutine ghostSwingCoroutine;
    private bool isSwinging = false;

    private PlayerInput playerInput; // Class-level variable for input
    private float baseAngle = 0f; // Class-level variable for player's facing direction

    // Combo variables
    public float startComboMultiplier = 1.0f;
    public ComboMultiplierMode comboMultiplierMode = ComboMultiplierMode.Additive;
    public float comboIncrease = 1.0f;
    public float damage = 0.0f;
    public float force = 0.0f;
    public float addedWackedTime = 0.0f;
    #endregion

    private enum AttackType
    {
        Pivot,
        Swing,
        GhostPivot,
    }

    private void Awake()
    {
        // Initialize PlayerInput in Awake to ensure it's ready before Start
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        // Subscribe to input events
        playerInput.Player.Attack.performed += OnAttack;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        playerInput.Player.Attack.performed -= OnAttack;
        playerInput.Disable();
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        if (weaponRb != null)
            switch (attackType)
            {
                case AttackType.Pivot:
                    PivotWeapon();
                    break;
                case AttackType.GhostPivot:
                    GhostPivotWeapon();
                    break;
                case AttackType.Swing:
                    UpdateWeaponDirection();
                    break;
                default:
                    break;
            }
    }

    private void PivotWeapon()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we stay in 2D
        // Get direction from player to mouse
        Vector2 direction = (mousePos - weaponPivot.position).normalized;
        // Calculate the angle between the player and the mouse position
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Smoothly rotate the weapon using Rigidbody2D
        float currentAngle = weaponRb.rotation;
        float newAngle = Mathf.LerpAngle(
            currentAngle,
            targetAngle,
            rotateSpeed * Time.fixedDeltaTime
        );

        // Apply the new rotation using Rigidbody2D.MoveRotation

        // Add torque
        // weaponRb.AddTorque(rotateSpeed * Time.fixedDeltaTime * 1000);
        weaponRb.MoveRotation(newAngle);
    }

    private void GhostPivotWeapon()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we stay in 2D
        // Get direction from player to mouse
        Vector2 direction = (mousePos - weaponPivot.position).normalized;
        // Calculate the angle between the player and the mouse position
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Set the ghost weapon rotation
        ghostWeaponRb.MoveRotation(targetAngle);
    }

    private void UpdateWeaponDirection()
    {
        Vector2 direction = playerMovement.Direction;

        if (direction == Vector2.zero)
        {
            return;
        }

        // Update baseAngle continuously
        baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        baseAngle = (baseAngle + 360) % 360;

        if (!isSwinging)
        {
            // Only update weapon rotation when not swinging
            float pullbackAngle = (baseAngle - maxSwingAngle + 360) % 360;
            weaponRb.MoveRotation(pullbackAngle);
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        switch (attackType)
        {
            case AttackType.Pivot:
                break;
            case AttackType.GhostPivot:
                // Rotate the weapon to where the ghost weapon is pointing
                if (ghostSwingCoroutine != null) // Skip if it's already running
                {
                    StopCoroutine(ghostSwingCoroutine);
                }
                else
                {
                    ghostSwingCoroutine = StartCoroutine(
                        RotateWeaponToGhost(ghostWeaponRb.rotation)
                    );
                }

                break;
            case AttackType.Swing:
                if (swingCoroutine != null)
                {
                    StopCoroutine(swingCoroutine);
                }
                swingCoroutine = StartCoroutine(SwingWeapon());
                break;
            default:
                break;
        }
    }

    private IEnumerator RotateWeaponToGhost(float targetAngle)
    {
        while (true)
        {
            float weaponCurrentAngle = weaponRb.rotation;
            float weaponNewAngle = Mathf.MoveTowardsAngle(
                weaponCurrentAngle,
                targetAngle,
                rotateSpeed
            );
            weaponRb.MoveRotation(weaponNewAngle);

            // Stop rotating once close enough to target angle
            if (Mathf.Abs(Mathf.DeltaAngle(weaponCurrentAngle, targetAngle)) < 1.0f)
            {
                ghostSwingCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator SwingWeapon()
    {
        if (playerMovement.Direction == Vector2.zero)
        {
            yield break;
        }

        playerMovement.BlockMovement();
        isSwinging = true;

        float startAngle = (baseAngle - maxSwingAngle + 360) % 360;
        float targetAngle = (baseAngle - minSwingAngle + 360) % 360;

        float elapsedTime = 0f;

        // Swing weapon from startAngle to targetAngle over swingTime
        while (elapsedTime < swingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / swingTime);
            float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, t);
            weaponRb.MoveRotation(currentAngle);
            yield return null;
        }

        // Ensure the weapon reaches the target angle
        weaponRb.MoveRotation(targetAngle);

        playerMovement.UnblockMovement();

        // Reload weapon from targetAngle back to startAngle over reloadTime
        elapsedTime = 0f;

        // Check if another attack was initiated during reload
        while (elapsedTime < reloadTime)
        {
            // If a new attack is initiated, break out to restart the swing
            if (playerInput.Player.Attack.triggered)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / reloadTime);
            float currentAngle = Mathf.LerpAngle(targetAngle, startAngle, t);
            weaponPivot.localRotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // If an attack was initiated during reload, restart the swing
        if (playerInput.Player.Attack.triggered)
        {
            isSwinging = false;
            swingCoroutine = StartCoroutine(SwingWeapon());
            yield break;
        }

        // Ensure the weapon reaches the start angle
        weaponRb.MoveRotation(startAngle);

        isSwinging = false;
    }

    public void SetWeapon(Weapon weapon)
    {
        weaponRb = weapon.GetComponentInParent<Rigidbody2D>();
    }
}
