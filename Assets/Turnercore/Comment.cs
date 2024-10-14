using UnityEngine;

/// <summary>
/// Attach this script to any gameObject for which you want to put a note.
/// </summary>
public class Comment : MonoBehaviour
{
    [TextArea]
    public string Notes = "Comment Here."; // Do not place your note/comment here.

    // Enter your note in the Unity Editor.

    // Start is called before the first frame update
    void Start()
    {
        enabled = false; // Disable the script after the game starts.
    }
}
