using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public enum BuildingPreviewState { POSITIVE, NEGATIVE }

    [SerializeField] private Material positiveMaterial;
    [SerializeField] private Material negativeMaterial;

    public BuildingPreviewState State { get; private set; } = BuildingPreviewState.NEGATIVE;
    public BuildingData Data { get; private set; }
    public BuildingModel BuildingModel { get; private set; }

    // Se este preview veio de um building existente, setamos esta referência
    public Building SourceBuilding { get; private set; }

    private List<Renderer> renderers = new();
    private List<Collider> colliders = new();

    // Setup normal ou com sourceBuilding (movimento)
    public void Setup(BuildingData data, Building sourceBuilding = null)
    {
        Data = data;
        SourceBuilding = sourceBuilding;

        BuildingModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        renderers.AddRange(BuildingModel.GetComponentsInChildren<Renderer>());
        colliders.AddRange(BuildingModel.GetComponentsInChildren<Collider>());

        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        SetPreviewMaterial(State);
    }

    public void ChangeState(BuildingPreviewState newState)
    {
        if (newState == State) return;
        State = newState;
        SetPreviewMaterial(State);
    }

    public void Rotate(int rotationStep)
    {
        BuildingModel.Rotate(rotationStep);
    }

    private void SetPreviewMaterial(BuildingPreviewState newState)
    {
        Material previewMat = newState == BuildingPreviewState.POSITIVE ? positiveMaterial : negativeMaterial;
        foreach (var rend in renderers)
        {
            Material[] mats = new Material[rend.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = previewMat;
            rend.materials = mats;
        }
    }
}
