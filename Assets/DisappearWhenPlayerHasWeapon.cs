using UnityEngine;

public class DisappearWhenPlayerHasWeapon : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image[] images;

    [SerializeField]
    private GameObject[] gameObjects;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Player.GetComponent<PlayerController>().HasWeapon())
        {
            foreach (UnityEngine.UI.Image image in images)
            {
                image.enabled = false;
            }
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (UnityEngine.UI.Image image in images)
            {
                image.enabled = true;
            }
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
