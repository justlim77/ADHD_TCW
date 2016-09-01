using UnityEngine;
using System.Collections;
using System;

public class DataManager : MonoBehaviour
{
    public static event Action<int> OnEnergyValueChanged;
    public static event Action<int> OnGemValueChanged;

    public const string totalGem = "GEM";
    public const string totalEnergy = "LIFE";
    public static string acOne = "AC_2";
    public static string acTwo = "AC_2";
    public static string acThree = "AC_3";

    //Read Int Data
    public static int ReadIntData(string data)
    {
        if (PlayerPrefs.GetInt("FIRSTLAUNCH") == 0)
        {
            ResetToDefault();
        }

        return PlayerPrefs.GetInt(data);
    }

    //Store Int Data
    public static void StoreIntData(string data, int value)
    {
        PlayerPrefs.SetInt(data, value);

        if (data == totalEnergy)
        {
            if (OnEnergyValueChanged != null)
                OnEnergyValueChanged(ReadIntData(totalEnergy));
        }

        else if (data == totalGem)
        {
            if (OnGemValueChanged != null)
                OnGemValueChanged(ReadIntData(totalGem));
        }
    }

    public static void ResetToDefault()
    {
        StoreIntData(totalEnergy, 3);
        StoreIntData(totalGem, 500);
        StoreIntData("FIRSTLAUNCH", 1);
    }

    protected virtual void EnergyValueChanged()
    {
        if (OnEnergyValueChanged != null)
            OnEnergyValueChanged(ReadIntData(totalEnergy));
    }

    protected virtual void GemValueChanged()
    {
        if (OnGemValueChanged != null)
            OnGemValueChanged(ReadIntData(totalGem));
    }
}