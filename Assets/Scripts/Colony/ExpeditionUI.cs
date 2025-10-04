using UnityEngine;
using UnityEngine.UI;

public class ExpeditionUI : MonoBehaviour
{
    [SerializeField] private ExpeditionManager expeditionManager;
    [SerializeField] private GameObject expeditionPanel;

    [Header("Buttons")]
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        easyButton.onClick.AddListener(() => OnExpeditionClicked("Easy"));
        mediumButton.onClick.AddListener(() => OnExpeditionClicked("Medium"));
        hardButton.onClick.AddListener(() => OnExpeditionClicked("Hard"));
        closeButton.onClick.AddListener(ClosePanel);

        expeditionPanel.SetActive(false);
    }

    public void OpenPanel()
    {
        expeditionPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        expeditionPanel.SetActive(false);
    }

    private void OnExpeditionClicked(string difficulty)
    {
        expeditionManager.StartExpedition(difficulty);
        ClosePanel();
    }
}
