using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    DateTime epochStart;

    public Text gameLife;
    public Text gameTimer;

    int cur_time = 0;
    int nextRegen = 0;
    int baseTime = 0;
    int lifeCount = 0;
    int timeLeft = 0;

    int minutes, seconds;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        UpdateRegenTime();
    }

    public void UpdateRegenTime()
    {
        nextRegen = DataManager.ReadIntData("GAMETIME");
        lifeCount = DataManager.ReadIntData("LIFE");

        if ((nextRegen == 0) && (lifeCount != 3))
            CalculateNextRegenTime();

        Initialize();
    }

    public void Initialize()
    {
        gameLife.text = lifeCount.ToString() + "/3";

        if (lifeCount == 3)
        {
            DataManager.StoreIntData("GAMETIME", 0);
            gameTimer.gameObject.SetActive(false);
        }
        else
            gameTimer.gameObject.SetActive(true);

        //Debug.Log("INITIALIZED!");
    }

    public void DeductLife()
    {
        lifeCount = DataManager.ReadIntData("LIFE");

        if (lifeCount > 0)
        {
            //Calculate Next Time
            if (DataManager.ReadIntData("GAMETIME") == 0)
                nextRegen = AddEpochTime((int)(DateTime.UtcNow - epochStart).TotalSeconds, 1800);

            DataManager.StoreIntData("GAMETIME", nextRegen);

            //Calculate Life
            lifeCount--;
            DataManager.StoreIntData("LIFE", lifeCount);
            Initialize();
        }
    }

    public void IncreaseLife()
    {
        lifeCount++;
        DataManager.StoreIntData("LIFE", lifeCount);
        Initialize();
    }

    public void ResetLife()
    {
        lifeCount = 3;
        DataManager.StoreIntData("LIFE", 3);
        Initialize();
    }

    int AddEpochTime(int currTime, int seconds)
    {
        int nextTime = 0;
        nextTime = currTime + seconds;
        return nextTime;
    }

    void CalculateNextRegenTime()
    {
        DataManager.StoreIntData("GAMETIME", AddEpochTime((int)(DateTime.UtcNow - epochStart).TotalSeconds, 1800));
        UpdateRegenTime();
    }

    // Update is called once per frame
    void Update ()
    {
        cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;

        if ((nextRegen != 0) && (lifeCount <3))
        {
            timeLeft = nextRegen - cur_time;

            if(timeLeft <= 0)
            {
                IncreaseLife();
                CalculateNextRegenTime();
            }
            else
            {
                minutes = timeLeft / 60;
                seconds = timeLeft - minutes * 60;
                
                gameTimer.text = string.Format("{0:0}:{1:00}", minutes, seconds) + " to +1";
            }
        }
	}
}
