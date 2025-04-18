using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float scrollSpeed = 5f;

    public GameObject canvasBase;
    
    public float topLimit;
    public float bottomLimit;

    private void Start()
    {
        topLimit = 0f;
        bottomLimit = -(canvasBase.transform.localScale.y - 10f);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
		Debug.Log(scroll);
		// If the scroll value is positive, scroll up
        // Scroll down returns a negative value
        if (scroll != 0f)
        {
            transform.position += Vector3.down * scroll * scrollSpeed;
            
            // clamping
            float clampedY = Mathf.Clamp(transform.position.y, bottomLimit, topLimit);
            
            transform.position = new Vector3(
                transform.position.x,
                clampedY,
                transform.position.z
            );
        }
    }
}
