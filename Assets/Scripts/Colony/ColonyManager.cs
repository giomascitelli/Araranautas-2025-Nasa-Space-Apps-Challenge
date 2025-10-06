using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColonyManager : MonoBehaviour
{
    [Header("UI - Materials")]
    [SerializeField] private Transform materialsParent;
    [SerializeField] private GameObject materialSlotPrefab;
    [SerializeField] private Sprite rockSprite;
    [SerializeField] private Sprite metalSprite;
    [SerializeField] private Sprite crystalSprite;

    private Dictionary<ResourceType, TMP_Text> materialTexts = new();

    [Header("UI - Bottom Stats")]
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text happinessText;

    [Header("UI - Alerts")]
    [SerializeField] private TMP_Text alertText;
    [SerializeField] private float alertDuration = 2f;

    private List<Building> buildings = new();

    private int currentRocks = 100;
    private int currentMetal = 50;
    private int currentCrystals = 25;

    private void Start()
    {
        CreateMaterialSlot(ResourceType.Rocks, rockSprite);
        CreateMaterialSlot(ResourceType.Metal, metalSprite);
        CreateMaterialSlot(ResourceType.Crystals, crystalSprite);

        if (alertText != null)
            alertText.gameObject.SetActive(false);

        UpdateUI();
    }

    private void CreateMaterialSlot(ResourceType type, Sprite icon)
    {
        var slot = Instantiate(materialSlotPrefab, materialsParent);
        var valueText = slot.transform.Find("ValueText").GetComponent<TMP_Text>();
        var iconImage = slot.transform.Find("Icon").GetComponent<Image>();

        if (icon != null)
            iconImage.sprite = icon;

        valueText.text = "0";
        materialTexts[type] = valueText;
    }

    public void RegisterBuilding(Building building)
    {
        buildings.Add(building);
        UpdateUI();
    }

    private void UpdateUI()
    {
        int water = 0, energy = 0, food = 0, happiness = 0;

        foreach (var b in buildings)
        {
            foreach (var stat in b.Data.ResourceStats)
            {
                switch (stat.type)
                {
                    case ResourceType.Water: water += stat.amountPerHour; break;
                    case ResourceType.Energy: energy += stat.amountPerHour; break;
                    case ResourceType.Food: food += stat.amountPerHour; break;
                    case ResourceType.Happiness: happiness += stat.amountPerHour; break;
                }
            }
        }

        if (waterText != null) waterText.text = $"WATER: {water} L/h";
        if (energyText != null) energyText.text = $"ENERGY: {energy} W/h";
        if (foodText != null) foodText.text = $"FOOD: {food} T/h";
        if (happinessText != null) happinessText.text = $"HAPPINESS: {happiness}";

        if (materialTexts.ContainsKey(ResourceType.Rocks)) materialTexts[ResourceType.Rocks].text = currentRocks.ToString();
        if (materialTexts.ContainsKey(ResourceType.Metal)) materialTexts[ResourceType.Metal].text = currentMetal.ToString();
        if (materialTexts.ContainsKey(ResourceType.Crystals)) materialTexts[ResourceType.Crystals].text = currentCrystals.ToString();
    }

    public bool HasEnoughResources(List<ResourceCost> costs)
    {
        foreach (var cost in costs)
        {
            switch (cost.type)
            {
                case ResourceType.Rocks: if (currentRocks < cost.amount) return false; break;
                case ResourceType.Metal: if (currentMetal < cost.amount) return false; break;
                case ResourceType.Crystals: if (currentCrystals < cost.amount) return false; break;
            }
        }
        return true;
    }

    public void SpendResources(List<ResourceCost> costs)
    {
        foreach (var cost in costs)
        {
            switch (cost.type)
            {
                case ResourceType.Rocks: currentRocks -= cost.amount; break;
                case ResourceType.Metal: currentMetal -= cost.amount; break;
                case ResourceType.Crystals: currentCrystals -= cost.amount; break;
            }
        }
        UpdateUI();
    }

    public bool CanSupportBuilding(BuildingData newBuilding)
    {
        // calcula produção atual
        int water = 0, energy = 0, food = 0, happiness = 0;

        foreach (var b in buildings)
        {
            foreach (var stat in b.Data.ResourceStats)
            {
                switch (stat.type)
                {
                    case ResourceType.Water: water += stat.amountPerHour; break;
                    case ResourceType.Energy: energy += stat.amountPerHour; break;
                    case ResourceType.Food: food += stat.amountPerHour; break;
                    case ResourceType.Happiness: happiness += stat.amountPerHour; break;
                }
            }
        }

        // adiciona os stats da nova construção
        foreach (var stat in newBuilding.ResourceStats)
        {
            switch (stat.type)
            {
                case ResourceType.Water: water += stat.amountPerHour; break;
                case ResourceType.Energy: energy += stat.amountPerHour; break;
                case ResourceType.Food: food += stat.amountPerHour; break;
                case ResourceType.Happiness: happiness += stat.amountPerHour; break;
            }
        }

        // se algum recurso básico ficar negativo, não pode construir
        if (water < 0 || energy < 0 || food < 0 || happiness < 0)
        {
            return false;
        }

        return true;
    }

    public void ShowAlert(string message)
    {
        if (alertText != null)
        {
            alertText.text = message;
            alertText.color = Color.red;
            alertText.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(HideAlertAfterTime());
        }
    }

    private IEnumerator HideAlertAfterTime()
    {
        yield return new WaitForSeconds(alertDuration);
        alertText.gameObject.SetActive(false);
    }

    public int GetMaterial(ResourceType type)
    {
        return type switch
        {
            ResourceType.Rocks => currentRocks,
            ResourceType.Metal => currentMetal,
            ResourceType.Crystals => currentCrystals,
            _ => 0
        };
    }

    public void AddMaterial(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Rocks: currentRocks += amount; break;
            case ResourceType.Metal: currentMetal += amount; break;
            case ResourceType.Crystals: currentCrystals += amount; break;
        }
        UpdateUI();
    }

    public void UnregisterBuilding(Building building)
    {
        if (buildings.Contains(building))
        {
            buildings.Remove(building);
            UpdateUI();
        }
    }
}
