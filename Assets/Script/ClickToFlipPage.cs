using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vivian;

namespace vivian
{
    public class ClickToFlipPage : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private Vector3 originalPosition;
        [SerializeField] private int originalSortingOrder;

        public float slideDistance = 1f;
        public float slideDuration = 0.25f;

        [SerializeField] private bool isFlipped = false;
        
        [SerializeField] private Sprite spriteBeforeFlip; // default sprite
        [SerializeField] private Sprite spriteAfterFlip;  // flipped sprite
        
        // scrolling
        public CameraScroll cameraScroll;
        
        [SerializeField] private GameObject letterScrollContainer;
        public float scrollSpeed = 5f;
        public float minY = -2f;
        public float maxY = 2f;
        
        // regenerating the collider
        private BoxCollider2D letterCollider;
        
        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            originalPosition = transform.position;
            originalSortingOrder = spriteRenderer.sortingOrder;
            
            letterCollider = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isFlipped) {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0f) {
                    Vector3 pos = letterScrollContainer.transform.localPosition;
                    pos.y = Mathf.Clamp(pos.y + scroll * scrollSpeed, minY, maxY);
                    letterScrollContainer.transform.localPosition = pos;
                }
            }
        }

        private void OnMouseUp()
        {
            if (!isFlipped)
            {
                Debug.Log("to the front");
                
                // stop major scrolling
                cameraScroll.isLetterActive = true;
                letterCollider.size = new Vector2(17.8882f, 60f);
                
                Vector3 targetPosition = new Vector3(originalPosition.x, -211f, originalPosition.z);
                Vector3 slideOutFrom = targetPosition + new Vector3(slideDistance, 0f, 0f);
                    
                StartCoroutine(SlideAndFade(
                    slideOutFrom,
                    targetPosition + new Vector3(slideDistance, 0f, 0f),
                    1f, 0f,
                    () => {
                            
                        // bring to front
                        spriteRenderer.sprite = spriteAfterFlip;

                        spriteRenderer.sortingOrder += 10;
                        StartCoroutine(SlideAndFade(
                            targetPosition + new Vector3(slideDistance, 0f, 0f),
                            targetPosition,
                            0f, 1f,
                            null
                        ));
                            
                    }
                ));
                
                isFlipped = !isFlipped;
            }
            else
            {
                cameraScroll.isLetterActive = false;
                letterCollider.size = new Vector2(17.8882f, 36f);
                
                Debug.Log("set to back");
                    
                StartCoroutine(SlideAndFade(
                    originalPosition,
                    originalPosition + new Vector3(slideDistance, 0f, 0f),
                    1f, 0f,
                    () => {
                            
                        // back to original sorting order
                        spriteRenderer.sprite = spriteBeforeFlip;

                        spriteRenderer.sortingOrder = originalSortingOrder;
                        StartCoroutine(SlideAndFade(
                            originalPosition + new Vector3(slideDistance, 0f, 0f),
                            originalPosition,
                            0f, 1f,
                            null
                        ));
                            
                    }
                ));
                    
                isFlipped = !isFlipped;
            }
        }

        IEnumerator SlideAndFade(Vector3 from, Vector3 to, float alphaFrom, float alphaTo, System.Action onDone)
        {
            float t = 0f;
            Color color = spriteRenderer.color;

            while (t < slideDuration)
            {
                t += Time.deltaTime;
                float progress = t / slideDuration;

                transform.position = Vector3.Lerp(from, to, progress);
                color.a = Mathf.Lerp(alphaFrom, alphaTo, progress);
                spriteRenderer.color = color;

                yield return null;
            }

            transform.position = to;
            color.a = alphaTo;
            spriteRenderer.color = color;

            onDone?.Invoke();
        }
    }
}
