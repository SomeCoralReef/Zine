using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float scrollSpeed = 5f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
		Debug.Log(scroll);
		// If the scroll value is positive, scroll up
        // Scroll down returns a negative value
        if (scroll != 0f)
        {
            transform.position += Vector3.down * scroll * scrollSpeed;
        }
    }
}
