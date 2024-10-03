using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBar : MonoBehaviour
{
    public float maxWidth = 1800f;
    static float height = 50f;
    public float currentWidth{
        get{
            return xp / maxXP * maxWidth;
        }
    }
    public float xp => GameManager.Instance.GetPlayerComponent().XP;
    public float maxXP => GameManager.Instance.GetPlayerComponent().MaxXP;

    [SerializeField] private RectTransform rectTransform;

    void Update()
    {
        print(rectTransform);
        // rectTransform.sizeDelta = new Vector2(currentWidth, height);
        
    }
}
