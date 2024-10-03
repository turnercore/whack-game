using UnityEngine;

public class XPBar : MonoBehaviour
{
    public float maxWidth = 1800f;
    private float height = 33f;
    //[SerializeField] private AnimationCurve curve;
    private float currentWidth{
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

    // Cleanup
    // private void OnDestroy()
    // {
    //     EventBus.Instance.UnsubscribeFromXPChanged(UpdateXPBar);
    // }

    private void Update()
    {
        rectTransform.sizeDelta = new Vector2(Mathf.Lerp(rectTransform.sizeDelta.x, currentWidth, Time.deltaTime * 5), height);
    }
}
