using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 1f; // Speed at which the text floats upward
    public float fadeDuration = 1f; // Time until text fully fades away

    //Text mesh pro text component reference
    [SerializeField]
    private TMP_Text damageText;

    [SerializeField]
    private Animator anim;
    private GameObject target;

    public void Initialize(float damage, GameObject entity)
    {
        damageText.text = damage.ToString();
        target = entity;
        // Set trigger on animation controller
        anim.SetTrigger("Start");
    }

    // Called in the FloatingText animation event
    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }
        // Set the base transform position to the parent's position
        transform.position = target.transform.position;
    }
}
