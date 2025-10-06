using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building")]
public class BuildingData : ScriptableObject
{
    [field: SerializeField] public string BuildingName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public BuildingModel Model { get; private set; }

    [field: SerializeField] public List<ResourceCost> BuildCost { get; private set; } = new();

    [field: SerializeField] public List<ResourceStat> ResourceStats { get; private set; } = new();
}

[System.Serializable]
public class ResourceCost
{
    public ResourceType type;
    public int amount;
}

[System.Serializable]
public class ResourceStat
{
    public ResourceType type;
    public int amountPerHour;
}

public enum ResourceType
{
    Energy,
    Oxygen,
    Water,
    Food,
    Happiness,
    Rocks,
    Metal,
    Crystals
}
