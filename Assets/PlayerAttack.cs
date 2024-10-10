using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region Variables
    public float rotateSpeed = 20.0f;

    [SerializeField]
    private Transform weaponPivot;

    [SerializeField]
    private AttackType attackType;

    [SerializeField]
    private Rigidbody2D weaponRigidBody;

    [SerializeField]
    private Transform ghostWeaponPivot;

    private bool IsDead => GetComponent<PlayerController>().IsDead;
    private Coroutine rotateCoroutine;
    private bool isRotateRequested = false;
    #endregion

    private enum AttackType
    {
        Pivot,
        Swing,
        GhostPivot,
    }

    private void Start()
    {
        // Subscribe to mouse click event using Unity's new Input System
        var playerInput = new PlayerInput();
        playerInput.Player.Attack.performed += ctx => OnAttack();
        playerInput.Enable();
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        switch (attackType)
        {
            case AttackType.Pivot:
                PivotWeapon();
                break;
            case AttackType.GhostPivot:
                GhostPivotWeapon();
                break;
            default:
                break;
        }
    }

    public void PivotWeapon()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we stay in 2D
        // Get direction from player to mouse
        Vector2 direction = (mousePos - weaponPivot.position).normalized;
        // Calculate the angle between the player and the mouse position
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Smoothly rotate the weapon using Rigidbody2D
        float currentAngle = weaponRigidBody.rotation;
        float newAngle = Mathf.LerpAngle(
            currentAngle,
            targetAngle,
            rotateSpeed * Time.fixedDeltaTime
        );
        // Apply the new rotation using Rigidbody2D.MoveRotation
        weaponRigidBody.MoveRotation(newAngle);
    }

    public void GhostPivotWeapon()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we stay in 2D
        // Get direction from player to mouse
        Vector2 direction = (mousePos - ghostWeaponPivot.position).normalized;
        // Calculate the angle between the player and the mouse position
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Rotate the ghost weapon to the target angle
        ghostWeaponPivot.rotation = Quaternion.Euler(0, 0, targetAngle);

        // Rotate the actual weapon towards the ghost weapon's angle if requested
        if (isRotateRequested)
        {
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            rotateCoroutine = StartCoroutine(RotateWeaponToGhost(targetAngle));
            isRotateRequested = false;
        }
    }

    private void OnAttack()
    {
        isRotateRequested = true;
    }

    private IEnumerator RotateWeaponToGhost(float targetAngle)
    {
        while (true)
        {
            float weaponCurrentAngle = weaponRigidBody.rotation;
            float weaponNewAngle = Mathf.MoveTowardsAngle(
                weaponCurrentAngle,
                targetAngle,
                rotateSpeed
            );
            weaponRigidBody.MoveRotation(weaponNewAngle);

            // Stop rotating once close enough to target angle
            if (Mathf.Abs(Mathf.DeltaAngle(weaponCurrentAngle, targetAngle)) < 1.0f)
            {
                yield break;
            }

            yield return null;
        }
    }
}
