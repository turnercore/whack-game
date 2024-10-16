using UnityEngine;

public class DisableJuiceOnHit : MonoBehaviour
{
    [SerializeField]
    private GameObject juice;

    [SerializeField]
    private bool onlyOnWeaponHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onlyOnWeaponHit && !collision.collider.CompareTag("Weapon"))
        {
            return;
        }

        juice.SetActive(false);
    }
}
