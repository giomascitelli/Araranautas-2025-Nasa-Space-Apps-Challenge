using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string BuildingName => data.BuildingName;
    public BuildingData Data => data;

    // Posi��es que essa inst�ncia ocupa no mundo (lista de world positions)
    public List<Vector3> OccupiedPositions { get; private set; } = new();

    // Backup para caso o jogador cancele o movimento
    private List<Vector3> originalOccupiedPositions = new();
    private Vector3 originalWorldPosition;

    private BuildingModel model;
    private BuildingData data;

    public void Setup(BuildingData data, float rotation, List<Vector3> occupiedPositions = null)
    {
        this.data = data;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);

        if (occupiedPositions != null)
        {
            OccupiedPositions = new List<Vector3>(occupiedPositions);
        }
    }

    // Chamado quando o jogador inicia "mover" esta constru��o
    public void BeginMove()
    {
        // guarda estado original pra poder voltar se cancelar
        originalOccupiedPositions = new List<Vector3>(OccupiedPositions);
        originalWorldPosition = transform.position;

        // liberar as posi��es ocupadas (a BuildingSystem vai chamar grid.ClearBuilding)
        OccupiedPositions.Clear();

        // esconder visual pra evitar duplicata com o preview
        if (model != null && model.gameObject != null)
            model.gameObject.SetActive(false);
    }

    // Cancela o movimento: restaura no grid (o BuildingSystem chamar� grid.SetBuilding usando GetOriginalPositions)
    public List<Vector3> GetOriginalPositions()
    {
        return new List<Vector3>(originalOccupiedPositions);
    }

    // Restaura visual e posi��o original (usado se o jogador cancelar)
    public void CancelMoveRestore()
    {
        OccupiedPositions = new List<Vector3>(originalOccupiedPositions);
        transform.position = originalWorldPosition;

        if (model != null && model.gameObject != null)
            model.gameObject.SetActive(true);

        originalOccupiedPositions.Clear();
    }

    // Finaliza o movimento: insere as novas posi��es e mostra visual
    public void FinishMove(List<Vector3> newPositions, Vector3 newWorldPosition)
    {
        OccupiedPositions = new List<Vector3>(newPositions);
        transform.position = newWorldPosition;

        if (model != null && model.gameObject != null)
            model.gameObject.SetActive(true);

        // limpa backup
        originalOccupiedPositions.Clear();
    }
}
