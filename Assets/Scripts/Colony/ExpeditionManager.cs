using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ExpeditionData
{
    public string name;
    public float successChance;
    public int minReward;
    public int maxReward;
    public int timeCost;
    public ResourceType rewardType;
}

public class ExpeditionManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ColonyManager colonyManager;
    [SerializeField] private TimeCycleManager timeManager;

    [Header("Expedition Settings")]
    [SerializeField] private ExpeditionData easyExpedition;
    [SerializeField] private ExpeditionData mediumExpedition;
    [SerializeField] private ExpeditionData hardExpedition;

    [Header("UI Feedback")]
    [SerializeField] private TMP_Text expeditionResultText;
    [SerializeField] private float resultDisplayTime = 2f;

    private void Start()
    {
        if (expeditionResultText != null)
            expeditionResultText.gameObject.SetActive(false);
    }

    public void StartExpedition(string difficulty)
    {
        ExpeditionData data = difficulty switch
        {
            "Easy" => easyExpedition,
            "Medium" => mediumExpedition,
            "Hard" => hardExpedition,
            _ => easyExpedition
        };

        bool success = Random.value <= data.successChance;

        if (success)
        {
            int reward = Random.Range(data.minReward, data.maxReward + 1);
            colonyManager.AddMaterial(data.rewardType, reward);
            ShowResultText($"Expedition succeeded! +{reward} {data.rewardType}");
        }
        else
        {
            ShowResultText("Expedition failed...");
        }

        timeManager.AdvanceTurn(data.timeCost);
    }

    private void ShowResultText(string message)
    {
        if (expeditionResultText == null) return;

        expeditionResultText.text = message;
        expeditionResultText.color = Color.white;
        expeditionResultText.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideResultText());
    }

    private IEnumerator HideResultText()
    {
        yield return new WaitForSeconds(resultDisplayTime);
        expeditionResultText.gameObject.SetActive(false);
    }
}
