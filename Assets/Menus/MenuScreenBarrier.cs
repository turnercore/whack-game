using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EdgeCollider2D))]
public class MenuScreenBarrier : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;

    private Vector2 lastScreenSize;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();

        // Ensure the Rigidbody2D is set to Static
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        UpdateEdgeCollider();
    }

    private void OnEnable()
    {
        UpdateEdgeCollider();
    }

    private void Update()
    {
        if (ScreenSizeChanged())
        {
            UpdateEdgeCollider();
        }
    }

    private void UpdateEdgeCollider()
    {
        Vector2 screenBottomLeft = Camera.main.ScreenToWorldPoint(
            new Vector3(0, 0, Camera.main.nearClipPlane)
        );
        Vector2 screenTopRight = Camera.main.ScreenToWorldPoint(
            new Vector3(
                UnityEngine.Screen.width,
                UnityEngine.Screen.height,
                Camera.main.nearClipPlane
            )
        );

        Vector2[] edgePoints = new Vector2[]
        {
            screenBottomLeft, // Bottom-left corner
            new Vector2(screenBottomLeft.x, screenTopRight.y), // Top-left corner
            screenTopRight, // Top-right corner
            new Vector2(screenTopRight.x, screenBottomLeft.y), // Bottom-right corner
            screenBottomLeft // Closing the loop
            ,
        };

        edgeCollider.points = edgePoints;
    }

    private bool ScreenSizeChanged()
    {
        Vector2 currentScreenSize = new Vector2(
            UnityEngine.Screen.width,
            UnityEngine.Screen.height
        );
        if (currentScreenSize != lastScreenSize)
        {
            lastScreenSize = currentScreenSize;
            return true;
        }
        return false;
    }
}
