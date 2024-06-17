using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int bedrooms;
    public int bathrooms;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isZoomed = false;

    private Coroutine currentCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Rotate the house smoothly
        transform.Rotate(Vector3.up, Time.deltaTime * 10);
    }

    // Handle house click
    private void OnMouseDown()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        if (isZoomed)
        {
            // Zoom out
            currentCoroutine = StartCoroutine(SmoothTransform(originalPosition, originalScale, HouseListManager.Instance.zoomAnimationDuration));

            // Wait for the animation to finish before showing all houses
            StartCoroutine(WaitForAnimation(HouseListManager.Instance.zoomAnimationDuration));
        }
        else
        {
            // Store current position before zooming in
            originalPosition = HouseListManager.Instance.GetHousePosition(this.gameObject);

            // Zoom in
            currentCoroutine = StartCoroutine(SmoothTransform(
                HouseListManager.Instance.houseDisplaySpot.position + new Vector3(0, 0.6f, 0),
                originalScale * HouseListManager.Instance.zoomScale,
                HouseListManager.Instance.zoomAnimationDuration
            ));

            // Hide all houses except this one
            HouseListManager.Instance.HideAllHousesExcept(this.gameObject);
            HouseListManager.Instance.HideFilterPanel();
        }
        isZoomed = !isZoomed;
    }

    private IEnumerator SmoothTransform(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.localScale = targetScale;
    }

    private IEnumerator WaitForAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        HouseListManager.Instance.ShowAllHouses();
        HouseListManager.Instance.ShowFilterPanel();
    }
}
