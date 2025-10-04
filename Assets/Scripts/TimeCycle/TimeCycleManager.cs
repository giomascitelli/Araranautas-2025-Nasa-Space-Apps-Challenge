using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public enum DayTime
{
    Morning,
    Afternoon,
    Night
}

public class TimeCycleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text dayText;

    [Header("Lighting")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Color morningColor = new(1f, 0.95f, 0.85f);
    [SerializeField] private Color afternoonColor = new(1f, 0.75f, 0.5f);
    [SerializeField] private Color nightColor = new(0.25f, 0.35f, 0.6f);
    [SerializeField] private float transitionSpeed = 2f;

    public int CurrentDay { get; private set; } = 1;
    public DayTime CurrentTime { get; private set; } = DayTime.Morning;

    private Color targetColor;

    private void Start()
    {
        UpdateLighting();
        UpdateUI();
    }

    private void Update()
    {
        if (globalLight != null)
        {
            globalLight.color = Color.Lerp(globalLight.color, targetColor, Time.deltaTime * transitionSpeed);
        }
    }

    public void AdvanceTurn(int steps = 1)
    {
        for (int i = 0; i < steps; i++)
        {
            switch (CurrentTime)
            {
                case DayTime.Morning:
                    CurrentTime = DayTime.Afternoon;
                    break;
                case DayTime.Afternoon:
                    CurrentTime = DayTime.Night;
                    break;
                case DayTime.Night:
                    CurrentTime = DayTime.Morning;
                    CurrentDay++;
                    break;
            }
        }

        UpdateLighting();
        UpdateUI();
    }

    public void SkipTurn()
    {
        AdvanceTurn(1);
    }

    private void UpdateLighting()
    {
        switch (CurrentTime)
        {
            case DayTime.Morning:
                targetColor = morningColor;
                break;
            case DayTime.Afternoon:
                targetColor = afternoonColor;
                break;
            case DayTime.Night:
                targetColor = nightColor;
                break;
        }
    }

    private void UpdateUI()
    {
        if (dayText != null)
        {
            dayText.text = $"DAY {CurrentDay}  {CurrentTime.ToString().ToUpper()}";
        }
    }
}
