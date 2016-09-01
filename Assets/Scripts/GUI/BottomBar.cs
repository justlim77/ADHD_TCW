using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BottomBar : MonoBehaviour
{
    public Text dayText;
    public string dayLabel = "Day";
    public Slider moodBar;
    public Slider hygieneBar;
    public Slider vitalityBar;

    void OnEnable()
    {
        GameManager.OnDayValueChanged += GameManager_OnDayValueChanged;
        GameManager.OnMoodValueChanged += GameManager_OnMoodValueChanged;
        GameManager.OnVitalityValueChanged += GameManager_OnVitalityValueChanged;
        GameManager.OnHygieneValueChanged += GameManager_OnHygieneValueChanged;
    }

    void OnDisable()
    {
        GameManager.OnDayValueChanged -= GameManager_OnDayValueChanged;
        GameManager.OnMoodValueChanged -= GameManager_OnMoodValueChanged;
        GameManager.OnVitalityValueChanged -= GameManager_OnVitalityValueChanged;
        GameManager.OnHygieneValueChanged -= GameManager_OnHygieneValueChanged;
    }

    private void GameManager_OnDayValueChanged(int obj)
    {
        dayText.text = string.Format("{0} {1}", dayLabel, obj);
    }

    private void GameManager_OnMoodValueChanged(float obj)
    {
        moodBar.value = obj;
    }

    private void GameManager_OnVitalityValueChanged(float obj)
    {
        vitalityBar.value = obj;
    }

    private void GameManager_OnHygieneValueChanged(float obj)
    {
        hygieneBar.value = obj;
    }
}
