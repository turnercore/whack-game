using UnityEngine;

public class XPBar : MonoBehaviour
{
    public float maxWidth = 1800f;
    private float height = 33f;
    //[SerializeField] private AnimationCurve curve;
    private float CurrentWidth{
        get{
            return Xp / MaxXP * maxWidth;
        }
    }
    public float Xp => GameManager.Instance.GetPlayerComponent().XP;
    public float MaxXP => GameManager.Instance.GetPlayerComponent().MaxXP;
    public Transform PlayerTransform => GameManager.Instance.GetPlayerComponent().transform;

    [SerializeField] private RectTransform rectTransform;

    void Start()
    {
        // Set height to current rect height
        height = rectTransform.sizeDelta.y;
        // Set width to 0 
        rectTransform.sizeDelta = new Vector2(0, height);
    }

    private void LateUpdate()
    {
        rectTransform.sizeDelta = new Vector2(Mathf.Lerp(rectTransform.sizeDelta.x, CurrentWidth, Time.deltaTime * 5), height);
    }
}
