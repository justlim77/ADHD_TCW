using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencyBar : MonoBehaviour
{
    public Text gemText;

    void OnEnable()
    {
        DataManager.OnGemValueChanged += DataManager_OnGemValueChanged;
    }

    void OnDisable()
    {
        DataManager.OnGemValueChanged -= DataManager_OnGemValueChanged;
    }

    private void DataManager_OnGemValueChanged(int obj)
    {
        gemText.text = obj.ToString();
    }
}
