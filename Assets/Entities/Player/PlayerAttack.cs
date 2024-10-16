using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region Variables
    public float rotateSpeed = 10f;

    // private Weapon _weapon;
    // private Weapon weapon
    // {
    //     get { return _weapon; }
    //     set
    //     {
    //         _weapon = value;
    //         weaponRb = _weapon.GetComponent<Rigidbody2D>();
    //     }
    // }

    [SerializeField]
    private WeaponSlot weaponSlot;
    private Transform weaponPivot;
    public float damage = 10.0f;
    public float force = 5.0f;
    public float startComboMultiplier = 1.0f;
    public float comboIncrease = 0.5f;
    public float addedWackedTime = 1.0f;
    public ComboMultiplierMode comboMultiplierMode = ComboMultiplierMode.Additive;
    private Rigidbody2D weaponRb;
    private PlayerInput playerInput;
    private bool isSwinging = false;
    private bool IsDead => GetComponent<PlayerController>().IsDead;
    #endregion

    private void Awake()
    {
        // Initialize PlayerInput
        playerInput = new PlayerInput();
        weaponRb = weaponSlot.GetComponent<Rigidbody2D>();
        weaponPivot = weaponSlot.transform;
    }

    private void OnEnable()
    {
        // Subscribe to input events
        playerInput.Player.Attack.performed += OnAttack;
        playerInput.Enable();
        weaponSlot.OnWeaponSet += SetWeapon;
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        playerInput.Player.Attack.performed -= OnAttack;
        playerInput.Disable();
    }

    private void OnDestroy()
    {
        weaponSlot.OnWeaponSet -= SetWeapon;
    }

    private void SetWeapon(Weapon weapon)
    {
        Debug.Log("Setting weapon in attack");
    }

    private void FixedUpdate()
    {
        if (IsDead || isSwinging)
            return;

        PivotWeapon();
    }

    private void PivotWeapon()
    {
        if (weaponRb == null)
        {
            return;
        }
        // Get mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
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

        weaponRb.MoveRotation(newAngle);
    }

    private void OnAttack(InputAction.CallbackContext context) { }
}
