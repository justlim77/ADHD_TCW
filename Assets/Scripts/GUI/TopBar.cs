using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TopBar : MonoBehaviour
{
    public static event Action<string> OnShopButtonClicked;

    public Text energyText;
    public Text gemText;

    public AnimatedButton[] shopButtons;

    void OnEnable()
    {
        DataManager.OnGemValueChanged += DataManager_OnGemValueChanged;
        DataManager.OnEnergyValueChanged += DataManager_OnEnergyValueChanged;

        foreach (var button in shopButtons)
        {
            button.onClick.AddListener(() => ShopButtonClicked());
        }
    }

    private void DataManager_OnEnergyValueChanged(int obj)
    {
        energyText.text = string.Format("{0}/{1}", obj, 3);
    }

    private void DataManager_OnGemValueChanged(int obj)
    {
        gemText.text = obj.ToString();
    }

    protected virtual void ShopButtonClicked()
    {
        if (OnShopButtonClicked != null)
            OnShopButtonClicked("Shop button clicked");
    }

    void OnDisable()
    {
        DataManager.OnGemValueChanged -= DataManager_OnGemValueChanged;
        DataManager.OnEnergyValueChanged -= DataManager_OnEnergyValueChanged;

        foreach (var button in shopButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

}
