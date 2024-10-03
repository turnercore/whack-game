using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    [SerializeField] private GameObject floatingTextPrefab;
    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<Health>();
        if (health == null)
        {
            health = GetComponent<Health>();
        }

        if (health != null)
        {
            health.OnTakeDamage += CreateFloatingDamageText;
        }
      
    }
    public void CreateFloatingDamageText(float damage)
    {
        // Instantiate the floating text prefab
        GameObject floatingTextObject = Instantiate(floatingTextPrefab);
        // Set the text of the floating text
        floatingTextObject.GetComponent<FloatingDamageText>().Initialize(damage, gameObject);
    }
}
