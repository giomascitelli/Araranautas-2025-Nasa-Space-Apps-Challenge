using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public const float CellSize = 1f;

    [SerializeField] private BuildingData buildingData1;
    [SerializeField] private BuildingData buildingData2;
    [SerializeField] private BuildingData buildingData3;
    [SerializeField] private BuildingPreview previewPrefab;
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private BuildingGrid grid;
    [SerializeField] private ColonyManager colonyManager;
    private BuildingPreview preview;
    private Vector3 mouse;

    private void Update()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        mouse = mousePos;

        if (preview != null)
        {
            HandlePreview(mousePos);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // cancelar preview; se preview tinha SourceBuilding, restauramos a building no grid
                if (preview.SourceBuilding != null)
                {
                    // restaura posi��es originais no grid e visual
                    var original = preview.SourceBuilding.GetOriginalPositions();
                    grid.SetBuilding(preview.SourceBuilding, original);
                    preview.SourceBuilding.CancelMoveRestore();
                }

                Destroy(preview.gameObject);
                preview = null;
            }
        }
        else
        {
            // if (Input.GetKeyDown(KeyCode.Alpha1))
            //     preview = CreatePreview(buildingData1, mousePos);

            // if (Input.GetKeyDown(KeyCode.Alpha2))
            //     preview = CreatePreview(buildingData2, mousePos);

            // if (Input.GetKeyDown(KeyCode.Alpha3))
            //     preview = CreatePreview(buildingData3, mousePos);

            // clique direito para iniciar mover atrav�s do grid
            if (Input.GetMouseButtonDown(1))
                TryStartMove(mousePos);

            // delete por grid
            if (Input.GetKeyDown(KeyCode.Delete))
                TryDeleteAt(mousePos);
        }
    }

    public void BuildingPlacement(BuildingData data)
    {
        preview = CreatePreview(data, mouse);
    }

    private void HandlePreview(Vector3 mouseWorldPosition)
    {
        preview.transform.position = mouseWorldPosition;
        List<Vector3> buildPosition = preview.BuildingModel.GetAllBuildingPositions();
        bool canBuild = grid.CanBuild(buildPosition);
        if (canBuild)
        {
            preview.transform.position = GetSnappedCenterPosition(buildPosition);
            preview.ChangeState(BuildingPreview.BuildingPreviewState.POSITIVE);
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(buildPosition);
            }
        }
        else
        {
            preview.ChangeState(BuildingPreview.BuildingPreviewState.NEGATIVE);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            preview.Rotate(90);
        }
    }

    private void PlaceBuilding(List<Vector3> buildingPositions)
    {
        // Se estamos movendo uma building existente (preview.SourceBuilding != null)
        if (preview.SourceBuilding != null)
        {
            // confirmar movimento: posiciona a building original nas novas posi��es
            preview.SourceBuilding.FinishMove(buildingPositions, preview.transform.position);
            grid.SetBuilding(preview.SourceBuilding, buildingPositions);

            // N�O registrar novamente na colonyManager � stats permanecem os mesmos

            Destroy(preview.gameObject);
            preview = null;
            return;
        }

        // Caso normal de construir novo pr�dio
        if (colonyManager != null && preview != null)
        {
            BuildingData buildingData = preview.Data;

            if (!colonyManager.HasEnoughResources(buildingData.BuildCost) || !colonyManager.CanSupportBuilding(buildingData))
            {
                colonyManager.ShowAlert("Not enough resources!");
                return;
            }

            colonyManager.SpendResources(buildingData.BuildCost);
        }

        Building building = Instantiate(buildingPrefab, preview.transform.position, Quaternion.identity);
        building.Setup(preview.Data, preview.BuildingModel.Rotation, buildingPositions);
        grid.SetBuilding(building, buildingPositions);

        if (colonyManager != null)
        {
            colonyManager.RegisterBuilding(building);
        }

        Destroy(preview.gameObject);
        preview = null;
    }

    private void TryStartMove(Vector3 mouseWorldPosition)
    {
        (int x, int y) = grid.WorldToGridPosition(mouseWorldPosition);
        Building target = grid.GetBuildingAt(x, y);

        if (target == null) return;

        // 1) guarda posi��es originais dentro do building (BeginMove faz isso + esconde visual)
        target.BeginMove();

        // 2) limpa o grid (libera c�lulas)
        grid.ClearBuilding(target.GetOriginalPositions());

        // 3) cria preview vinculado ao building de origem
        preview = CreatePreview(target.Data, mouseWorldPosition, target);
    }

    private void TryDeleteAt(Vector3 mouseWorldPosition)
    {
        (int x, int y) = grid.WorldToGridPosition(mouseWorldPosition);
        Building target = grid.GetBuildingAt(x, y);
        if (target != null)
        {
            grid.ClearBuilding(target.OccupiedPositions);
            colonyManager.UnregisterBuilding(target);
            Destroy(target.gameObject);
        }
    }

    private BuildingPreview CreatePreview(BuildingData data, Vector3 position, Building sourceBuilding = null)
    {
        BuildingPreview buildingPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        buildingPreview.Setup(data, sourceBuilding);
        return buildingPreview;
    }

    private Vector3 GetSnappedCenterPosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
        List<int> zs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.z)).ToList();
        float centerX = (xs.Min() + xs.Max()) / 2f + CellSize / 2f;
        float centerZ = (zs.Min() + zs.Max()) / 2f + CellSize / 2f;
        return new(centerX, 0, centerZ);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
