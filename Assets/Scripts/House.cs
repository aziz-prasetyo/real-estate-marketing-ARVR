using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Header("House Info")]
    public int bedrooms;
    public int bathrooms;
    public float price;
    [SerializeField] private string address;
    [SerializeField] private string size;

    [Header("House Model")]
    [SerializeField] private GameObject houseModel;

    [Header("UI Elements")]
    [SerializeField] private GameObject houseInfoPanel;
    [SerializeField] private TMPro.TextMeshProUGUI priceText;
    [SerializeField] private TMPro.TextMeshProUGUI addressText;
    [SerializeField] private TMPro.TextMeshProUGUI sizeText;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isZoomed = false;

    private Coroutine currentCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;

        priceText.text = "Rp" + price.ToString("N0");
        addressText.text = address;
        sizeText.text = size;
    }

    private void Update()
    {
        // Rotate the house model slowly
        houseModel.transform.Rotate(Vector3.up * Time.deltaTime * 10);

        // Handle mouse click on the house model
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == houseModel)
                {
                    OnModelClick();
                }
            }
        }
    }

    // Handle house click
    private void OnModelClick()
    {
        Debug.Log("Clicked on " + gameObject.name);
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
            HouseListManager.Instance.HideMainUI();
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
        HouseListManager.Instance.ShowMainUI();
    }
}
