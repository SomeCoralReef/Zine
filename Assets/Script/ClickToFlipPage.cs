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
        
        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            originalPosition = transform.position;
            originalSortingOrder = spriteRenderer.sortingOrder;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnMouseUp()
        {
            if (!isFlipped)
            {
                Debug.Log("to the front");
                    
                StartCoroutine(SlideAndFade(
                    originalPosition,
                    originalPosition + new Vector3(slideDistance, 0f, 0f),
                    1f, 0f,
                    () => {
                            
                        // bring to front
                        spriteRenderer.sortingOrder += 10;
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
            else
            {
                Debug.Log("set to back");
                    
                StartCoroutine(SlideAndFade(
                    originalPosition,
                    originalPosition + new Vector3(slideDistance, 0f, 0f),
                    1f, 0f,
                    () => {
                            
                        // back to original sorting order
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
