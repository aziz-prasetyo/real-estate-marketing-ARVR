using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseListManager : MonoBehaviour
{
    public static HouseListManager Instance;

    [Header("House Settings")]
    public List<House> housePrefabs;
    public Transform houseDisplaySpot;
    public float zoomAnimationDuration = 0.3f;
    public float zoomScale = 2.0f;

    [Header("UI Elements")]
    [SerializeField] private GameObject mainUI;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [Header("House Display Settings")]
    [SerializeField] private float gap = 2.5f;

    private List<GameObject> activeHouses = new List<GameObject>();
    private int currentIndex = 0;
    private const int maxVisibleHouses = 3;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the buttons
        leftButton.onClick.AddListener(ShiftLeft);
        rightButton.onClick.AddListener(ShiftRight);

        // Initialize and position the houses
        InitializeHouses();
    }

    private void InitializeHouses()
    {
        foreach (House housePrefab in housePrefabs)
        {
            GameObject houseInstance = Instantiate(housePrefab.gameObject, houseDisplaySpot.position, houseDisplaySpot.rotation);
            houseInstance.SetActive(false);
            activeHouses.Add(houseInstance);
        }

        UpdateHouseVisibility();
    }

    public List<House> GetAllHouses()
    {
        List<House> houses = new List<House>();
        foreach (var house in housePrefabs)
        {
            houses.Add(house);
        }
        return houses;
    }

    public void UpdateVisibleHouses(List<House> filteredHouses)
    {
        // Destroy current active house instances
        foreach (var house in activeHouses)
        {
            Destroy(house);
        }
        activeHouses.Clear();

        // Instantiate new filtered house instances
        foreach (House house in filteredHouses)
        {
            GameObject houseInstance = Instantiate(house.gameObject, houseDisplaySpot.position, houseDisplaySpot.rotation);
            houseInstance.SetActive(false);
            activeHouses.Add(houseInstance);
        }

        // Reset index and update visibility
        currentIndex = 0;
        UpdateHouseVisibility();
    }

    private void ShiftLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateHouseVisibility();
        }
    }

    private void ShiftRight()
    {
        if (currentIndex < activeHouses.Count - maxVisibleHouses)
        {
            currentIndex++;
            UpdateHouseVisibility();
        }
    }

    private void UpdateHouseVisibility()
    {
        int visibleCount = Mathf.Min(maxVisibleHouses, activeHouses.Count - currentIndex);
        float totalWidth = (visibleCount - 1) * gap;
        float startX = houseDisplaySpot.position.x - totalWidth / 2;

        for (int i = 0; i < activeHouses.Count; i++)
        {
            if (i >= currentIndex && i < currentIndex + maxVisibleHouses)
            {
                activeHouses[i].SetActive(true);
                float positionX = startX + (i - currentIndex) * gap;
                Vector3 newPosition = new Vector3(positionX, houseDisplaySpot.position.y, houseDisplaySpot.position.z);
                activeHouses[i].transform.position = newPosition;
            }
            else
            {
                activeHouses[i].SetActive(false);
            }
        }

        // Enable or disable buttons based on the current index
        leftButton.interactable = currentIndex > 0;
        rightButton.interactable = currentIndex < activeHouses.Count - maxVisibleHouses;
    }

    public void HideAllHousesExcept(GameObject houseToKeep)
    {
        foreach (var house in activeHouses)
        {
            if (house != houseToKeep)
            {
                house.SetActive(false);
            }
        }

        leftButton.interactable = false;
        rightButton.interactable = false;
    }

    public void ShowAllHouses()
    {
        UpdateHouseVisibility();
    }

    public Vector3 GetHousePosition(GameObject house)
    {
        return house.transform.position;
    }

    public void HideMainUI()
    {
        mainUI.SetActive(false);
    }

    public void ShowMainUI()
    {
        mainUI.SetActive(true);
    }
}
