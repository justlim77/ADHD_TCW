#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

using System;

[Serializable]
public class InputManager : Singleton<InputManager>
{
    public KeyCode resetCurrencyKey = KeyCode.Delete;
    public KeyCode reduceEnergyKey = KeyCode.PageDown;
    public KeyCode addEnergyKey = KeyCode.PageUp;
    public KeyCode reduceGemKey = KeyCode.DownArrow;
    public KeyCode addGemKey = KeyCode.UpArrow;
    public int gemInterval = 100;

    // Set to protected to prevent calling constructor
    protected InputManager() { }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }

        if (Input.GetKeyDown(resetCurrencyKey))
        {
            DataManager.ResetToDefault();
        }

        if (Input.GetKeyDown(reduceEnergyKey))
        {
            int energy = DataManager.ReadIntData(DataManager.totalEnergy);
            energy--;
            DataManager.StoreIntData(DataManager.totalEnergy, energy);
        }

        if (Input.GetKeyDown(addEnergyKey))
        {
            int energy = DataManager.ReadIntData(DataManager.totalEnergy);
            energy++;
            DataManager.StoreIntData(DataManager.totalEnergy, energy);
        }

        if (Input.GetKeyDown(reduceGemKey))
        {
            int gem = DataManager.ReadIntData(DataManager.totalGem);
            gem -= gemInterval;
            DataManager.StoreIntData(DataManager.totalGem, gem);
        }

        if (Input.GetKeyDown(addGemKey))
        {
            int gem = DataManager.ReadIntData(DataManager.totalGem);
            gem += gemInterval;
            DataManager.StoreIntData(DataManager.totalGem, gem);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
