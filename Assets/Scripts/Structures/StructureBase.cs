using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Structure", menuName = "Structures/New Structure")]
public class StructureBase : ScriptableObject
{
    [Header("Structure Info")]
    [SerializeField] string structureName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;

    [Header("Structure Types")]
    [SerializeField] StructureType type1;
    [SerializeField] StructureType type2;

    [Header("Resource Effects")]
    [Tooltip("Lista de efeitos no tempo. amount positivo = produção, negativo = consumo.")]
    [SerializeField] private List<ResourceStat> resourceStats = new List<ResourceStat>();

    [Header("Population")]
    [Tooltip("Capacidade populacional que essa estrutura adiciona. (ex: domes/housing)")]
    [SerializeField] private int populationCapacity;

    [Header("Build & Unlock")]
    [Tooltip("Custo em recursos para construir essa estrutura.")]
    [SerializeField] private List<ResourceStat> buildCost = new List<ResourceStat>();

    [Tooltip("Se true, essa estrutura só pode ser construída dentro de um dome/área controlada.")]
    [SerializeField] private bool requiresDome;

    [Tooltip("Se true, essa estrutura (ex: Dome) desbloqueia a possibilidade de construir casas comuns dentro dela.")]
    [SerializeField] private bool unlocksHousing;

    [Tooltip("Lista de requisitos adicionais para desbloqueio (ex: precisa de X recurso >= Y, ou precisa ter um tipo de estrutura).")]
    [SerializeField] private List<UnlockRequirement> unlockRequirements = new List<UnlockRequirement>();

    [Header("Designer notes")]
    [Tooltip("Campo livre para anotações/condições personalizadas")]
    [SerializeField] private string customRequirementNote;

    // Getters
    public string StructureName => structureName;
    public string Description => description;
    public Sprite FrontSprite => frontSprite;
    public StructureType Type1 => type1;
    public StructureType Type2 => type2;
    public List<ResourceStat> ResourceStats => resourceStats;
    public int PopulationCapacity => populationCapacity;
    public List<ResourceStat> BuildCost => buildCost;
    public bool RequiresDome => requiresDome;
    public bool UnlocksHousing => unlocksHousing;
    public List<UnlockRequirement> UnlockRequirements => unlockRequirements;
    public string CustomRequirementNote => customRequirementNote;
}

// Enums / Helpers
public enum StructureType
{
    None,
    Energy,
    Oxygen,
    Water,
    Food,
    Storage,
    Health,
    Temperature,
    Happiness,
    Dome,
    Housing,
    Miscellaneous
}

public enum ResourceType
{
    Energy,
    Oxygen,
    Water,
    Food,
    Health,
    Happiness,
    Metal,
    Circuit,
    Alloy
}

[System.Serializable]
public class ResourceStat
{
    [Tooltip("Tipo de recurso afetado")]
    public ResourceType type;

    [Tooltip("Valor por tick. Positivo = produz, Negativo = consome")]
    public int amount;
}

[System.Serializable]
public class UnlockRequirement
{
    [Tooltip("Estrutura necessária para construção (opcional)")]
    public StructureType requiredStructure = StructureType.None;

    [Tooltip("Recurso mínimo necessário (opcional)")]
    public ResourceType requiredResource = ResourceType.Energy;

    [Tooltip("Quantidade mínima necessária do recurso para desbloquear")]
    public int minResourceAmount = 0;

    [Tooltip("Notas adicionais para condições personalizadas")]
    public string note;
}
