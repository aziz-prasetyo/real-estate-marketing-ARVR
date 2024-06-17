using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseFilter : MonoBehaviour
{
    [SerializeField] private Button bedroom1Button;
    [SerializeField] private Button bedroom2Button;
    [SerializeField] private Button bedroom3Button;
    [SerializeField] private Button bathroom1Button;
    [SerializeField] private Button bathroom2Button;
    [SerializeField] private Button bathroom3Button;

    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color inactiveColor = Color.white;

    private List<House> allHouses = new List<House>();
    private int selectedBedrooms = -1;
    private int selectedBathrooms = -1;

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

        // Get all houses
        allHouses = HouseListManager.Instance.GetAllHouses();

        // Initialize button colors
        SetButtonColor(bedroom1Button, false);
        SetButtonColor(bedroom2Button, false);
        SetButtonColor(bedroom3Button, false);
        SetButtonColor(bathroom1Button, false);
        SetButtonColor(bathroom2Button, false);
        SetButtonColor(bathroom3Button, false);
    }

    private void ToggleBedrooms(int bedrooms, Button button)
    {
        if (selectedBedrooms == bedrooms)
        {
            selectedBedrooms = -1;
            SetButtonColor(button, false);
        }
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
        if (selectedBathrooms == bathrooms)
        {
            selectedBathrooms = -1;
            SetButtonColor(button, false);
        }
        else
        {
            DeselectAllBathroomButtons();
            selectedBathrooms = bathrooms;
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

    private void FilterHouses()
    {
        List<House> filteredHouses = new List<House>();

        foreach (House house in allHouses)
        {
            bool bedroomsMatch = selectedBedrooms == -1 || house.bedrooms == selectedBedrooms;
            bool bathroomsMatch = selectedBathrooms == -1 || house.bathrooms == selectedBathrooms;

            if (bedroomsMatch && bathroomsMatch)
            {
                filteredHouses.Add(house);
            }
        }

        HouseListManager.Instance.UpdateVisibleHouses(filteredHouses);
    }
}
