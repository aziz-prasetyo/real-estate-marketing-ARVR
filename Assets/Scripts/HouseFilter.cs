using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseFilter : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button bedroom1Button;
    [SerializeField] private Button bedroom2Button;
    [SerializeField] private Button bedroom3Button;
    [SerializeField] private Button bathroom1Button;
    [SerializeField] private Button bathroom2Button;
    [SerializeField] private Button bathroom3Button;
    [SerializeField] private Button priceRange1Button;
    [SerializeField] private Button priceRange2Button;
    [SerializeField] private Button priceRange3Button;
    [SerializeField] private Button priceRange4Button;

    [Header("Button Colors")]
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color inactiveColor = Color.white;

    private List<House> allHouses = new List<House>();
    private int selectedBedrooms = -1;
    private int selectedBathrooms = -1;
    private int selectedPriceRange = -1;

    private void Start()
    {
        // Add listeners to bedroom buttons
        bedroom1Button.onClick.AddListener(delegate { ToggleBedrooms(1, bedroom1Button); });
        bedroom2Button.onClick.AddListener(delegate { ToggleBedrooms(2, bedroom2Button); });
        bedroom3Button.onClick.AddListener(delegate { ToggleBedrooms(3, bedroom3Button); });

        // Add listeners to bathroom buttons
        bathroom1Button.onClick.AddListener(delegate { ToggleBathrooms(1, bathroom1Button); });
        bathroom2Button.onClick.AddListener(delegate { ToggleBathrooms(2, bathroom2Button); });
        bathroom3Button.onClick.AddListener(delegate { ToggleBathrooms(3, bathroom3Button); });

        // Add listeners to price buttons
        priceRange1Button.onClick.AddListener(delegate { TogglePriceRange(1, priceRange1Button); });
        priceRange2Button.onClick.AddListener(delegate { TogglePriceRange(2, priceRange2Button); });
        priceRange3Button.onClick.AddListener(delegate { TogglePriceRange(3, priceRange3Button); });
        priceRange4Button.onClick.AddListener(delegate { TogglePriceRange(4, priceRange4Button); });

        // Get all houses
        allHouses = HouseListManager.Instance.GetAllHouses();

        // Initialize button colors
        SetButtonColor(bedroom1Button, false);
        SetButtonColor(bedroom2Button, false);
        SetButtonColor(bedroom3Button, false);
        SetButtonColor(bathroom1Button, false);
        SetButtonColor(bathroom2Button, false);
        SetButtonColor(bathroom3Button, false);
        SetButtonColor(priceRange1Button, false);
        SetButtonColor(priceRange2Button, false);
        SetButtonColor(priceRange3Button, false);
        SetButtonColor(priceRange4Button, false);
    }

    private void ToggleBedrooms(int bedrooms, Button button)
    {
        // If the button is already selected, deselect it
        if (selectedBedrooms == bedrooms)
        {
            selectedBedrooms = -1;
            SetButtonColor(button, false);
        }
        // If the button is not selected, select it
        else
        {
            DeselectAllBedroomButtons();
            selectedBedrooms = bedrooms;
            SetButtonColor(button, true);
        }
        FilterHouses();
    }

    private void ToggleBathrooms(int bathrooms, Button button)
    {
        // If the button is already selected, deselect it
        if (selectedBathrooms == bathrooms)
        {
            selectedBathrooms = -1;
            SetButtonColor(button, false);
        }
        // If the button is not selected, select it
        else
        {
            DeselectAllBathroomButtons();
            selectedBathrooms = bathrooms;
            SetButtonColor(button, true);
        }
        FilterHouses();
    }

    private void TogglePriceRange(int priceRange, Button button)
    {
        // If the button is already selected, deselect it
        if (selectedPriceRange == priceRange)
        {
            selectedPriceRange = -1;
            SetButtonColor(button, false);
        }
        // If the button is not selected, select it
        else
        {
            DeselectAllPriceButtons();
            selectedPriceRange = priceRange;
            SetButtonColor(button, true);
        }
        FilterHouses();
    }


    private void SetButtonColor(Button button, bool isActive)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = isActive ? activeColor : inactiveColor;
        colorBlock.highlightedColor = isActive ? activeColor : inactiveColor;
        colorBlock.pressedColor = isActive ? activeColor : inactiveColor;
        colorBlock.selectedColor = isActive ? activeColor : inactiveColor;
        button.colors = colorBlock;
    }

    private void DeselectAllBedroomButtons()
    {
        SetButtonColor(bedroom1Button, false);
        SetButtonColor(bedroom2Button, false);
        SetButtonColor(bedroom3Button, false);
    }

    private void DeselectAllBathroomButtons()
    {
        SetButtonColor(bathroom1Button, false);
        SetButtonColor(bathroom2Button, false);
        SetButtonColor(bathroom3Button, false);
    }

    private void DeselectAllPriceButtons()
    {
        SetButtonColor(priceRange1Button, false);
        SetButtonColor(priceRange2Button, false);
        SetButtonColor(priceRange3Button, false);
        SetButtonColor(priceRange4Button, false);
    }

    private void FilterHouses()
    {
        List<House> filteredHouses = new List<House>();

        foreach (House house in allHouses)
        {
            bool bedroomsMatch = selectedBedrooms == -1 || house.bedrooms == selectedBedrooms;
            bool bathroomsMatch = selectedBathrooms == -1 || house.bathrooms == selectedBathrooms;
            bool priceMatch = selectedPriceRange == -1 ||
                                (selectedPriceRange == 1 && house.price < 100000000) ||
                                (selectedPriceRange == 2 && house.price > 100000000 && house.price <= 500000000) ||
                                (selectedPriceRange == 3 && house.price > 500000000 && house.price <= 1000000000) ||
                                (selectedPriceRange == 4 && house.price > 1000000000);

            if (bedroomsMatch && bathroomsMatch && priceMatch)
            {
                filteredHouses.Add(house);
            }
        }

        HouseListManager.Instance.UpdateVisibleHouses(filteredHouses);
    }
}
