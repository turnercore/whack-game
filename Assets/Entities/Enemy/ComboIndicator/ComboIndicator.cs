using TMPro;
using UnityEngine;

public class ComboIndicator : MonoBehaviour
{
    private float _comboNumber = 0.0f;

    [SerializeField]
    private TMP_Text comboText;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    public float comboNumber
    {
        get { return _comboNumber; }
        set
        {
            //Set combo number to the value, rounded to the first decimal place
            _comboNumber = Mathf.Round(value * 10) / 10;
        }
    }

    private void OnEnable()
    {
        if (comboNumber <= 1)
        {
            return;
        }
        // Change text to "X"##.# the combo number
        comboText.text = "X" + comboNumber.ToString();
        comboText.enabled = true;
        // Turn on sprite renderer
        spriteRenderer.enabled = true;
        animator.enabled = true;
        // The animator will disable itself when the animation is completed
    }

    private void OnAnimationComplete()
    {
        // Turn off sprite renderer
        spriteRenderer.enabled = false;
        animator.enabled = false;
        comboText.enabled = false;
        gameObject.SetActive(false);
    }
}
