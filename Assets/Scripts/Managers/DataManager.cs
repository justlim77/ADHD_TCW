using UnityEngine;
using System.Collections;
using System;

public class DataManager : MonoBehaviour
{
    public static event Action<int> OnGemValueChanged;

    public static string totalGem = "GEM";
    public static string acOne = "AC_2";
    public static string acTwo = "AC_2";
    public static string acThree = "AC_3";

    //Read Int Data
    public static int ReadIntData(string data)
    {
        if (PlayerPrefs.GetInt("FIRSTLAUNCH") == 0)
        {
            PlayerPrefs.SetInt("GEM", 500);
            PlayerPrefs.SetInt("LIFE", 3);
            PlayerPrefs.SetInt("FIRSTLAUNCH", 1);
        }

        return PlayerPrefs.GetInt(data);
    }

    //Store Int Data
    public static void StoreIntData(string data, int value)
    {
        PlayerPrefs.SetInt(data, value);
    }

    public static void ResetToDefault()
    {
        PlayerPrefs.SetInt("GEM", 500);
        PlayerPrefs.SetInt("LIFE", 3);
        PlayerPrefs.SetInt("FIRSTLAUNCH", 1);
    }

    protected virtual void GemValueChanged()
    {
        if (OnGemValueChanged != null)
            OnGemValueChanged(ReadIntData(totalGem));
    }
}