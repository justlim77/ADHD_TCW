using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region Events
    public delegate void GameStateChangedEventHandler(object sender, GameState e);
    public static event GameStateChangedEventHandler OnGameStateChanged;

    public static event Action<int> OnDayValueChanged;
    public static event Action<float> OnMoodValueChanged;
    public static event Action<float> OnHygieneValueChanged;
    public static event Action<float> OnVitalityValueChanged;
    #endregion
        
    //Is Action Done?
    public static bool isCleanShelfDone = false;
    public static bool isBagDone = false;

    #region Properties
    int m_dayScene = 1;
    public int dayScene
    {
        get { return m_dayScene; }
        set
        {
            m_dayScene = value;
            DayValueChanged();
        }
    }


    float m_mood = 5f;
    public float mood
    {
        get
        {
            return m_mood;
        }
        set
        {
            m_mood = value;
            MoodValueChanged();
        }
    }

    float m_hygiene = 5f;
    public float hygiene
    {
        get
        {
            return m_hygiene;
        }
        set
        {
            m_hygiene = value;
            HygieneValueChanged();
        }
    }

    float m_vitality = 5f;
    public float vitality
    {
        get
        {
            return m_vitality;
        }
        set
        {
            m_vitality = value;
            VitalityValueChanged();
        }
    }
    #endregion

    //AI Progress Reward
    public static int[] gameReward = { 0, 0, 0, 0, 0, 0, 0, 0 };

    public static bool isGamePause = false;
    public static bool isTempPause = false;
    public static bool hasDayStarted = false;
    public static bool isInScenario = false;

    public static float gameTime = 0;
    public static float gameHour = 0;

    public SettingSequence gameOver;

    public static float maxBar = 10;
    public static float maxStamina = 10;

    static SettingSequence gameOver_;

    GameState _gameState = GameState.None;
    GameState gameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            if (_gameState != value)
            {
                _gameState = value;
                Debug.Log("GameState changed to " + _gameState.ToString());
                GameStateChanged();
            }
        }
    }

    void OnEnable()
    {
        CameraControl.OnCameraReached += CameraControl_OnCameraReached;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    void OnDisable()
    {
        CameraControl.OnCameraReached -= CameraControl_OnCameraReached;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        //Debug.Log(string.Format("{0} scene loaded in {1} load scene mode", arg0.name, arg1.ToString()));
        Initialize();
    }

    private void CameraControl_OnCameraReached(object sender)
    {
        gameState = GameState.Pregame;
    }

    protected virtual void DayValueChanged()
    {
        if (OnDayValueChanged != null)
            OnDayValueChanged(dayScene);

        Debug.Log(string.Format("Day value changed to {0}", dayScene));
    }

    protected virtual void MoodValueChanged()
    {
        if (OnMoodValueChanged != null)
            OnMoodValueChanged(mood);

        Debug.Log(string.Format("Mood value changed to {0}", mood));
    }

    protected virtual void HygieneValueChanged()
    {
        if (OnHygieneValueChanged != null)
            OnHygieneValueChanged(hygiene);

        Debug.Log(string.Format("Hygiene value changed to {0}", hygiene));
    }

    protected virtual void VitalityValueChanged()
    {
        if (OnVitalityValueChanged != null)
            OnVitalityValueChanged(vitality);

        Debug.Log(string.Format("Vitality value changed to {0}", vitality));
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        ResetActionCheck();
        ResetDay();

        isTempPause = false;
        isInScenario = false;
        dayScene = 1;

        mood = 5;
        hygiene = 5;
        vitality = 5;

        for (int i = 0; i < gameReward.Length; i++)
            gameReward[i] = 0;

        isGamePause = false;
    }

    public void setGamePause(bool isPause)
    {
        if (isPause)
            isTempPause = true;
        else
            isTempPause = false;
    }

    public void IncreaseDayScene()
    {
        CheckIsActionDone();
        ResetActionCheck();
        ResetDay();
        dayScene++;
    }

    void CheckIsActionDone()
    {
        if (!isCleanShelfDone)
            hygiene--;

        if (!isBagDone)
            mood--;
    }

    void ResetActionCheck()
    {
        isCleanShelfDone = false;
        isBagDone = false;
    }

    void ResetDay()
    {
        gameTime = 0;
        gameHour = 0;
        hasDayStarted = false;
    }

    bool Initialize()
    {
        gameState = GameState.Default;
        return true;
    }

    public void CheckifGameOver()
    {
        if ((hygiene <= 0) || (mood <= 0))
            gameOver_.PauseGame(true);
    }

    public static int CalculateScore(int cat)
    {
        int score = 0;

        switch(cat)
        {
            case 0: //Calculate TotalScore
                for (int i = 0; i < gameReward.Length; i++)
                    score += gameReward[i];
                break;
            case 1: //Calculate E1
                score += (gameReward[2] + gameReward[6]); 
                break;
            case 2: //Calculate E2
                score += (gameReward[0] + gameReward[1] + gameReward[4]);
                break;
            case 3: //Calculate E3
                score += (gameReward[3] + gameReward[5] + gameReward[7]);
                break;
        }

        return score;
    }

    protected void GameStateChanged()
    {
        if (OnGameStateChanged != null)
            OnGameStateChanged(this, gameState);
    }

    public void PlayGame()
    {
        gameState = GameState.Playing;
    }
}

public enum GameState
{
    None,
    Default,
    Pregame,
    Playing,
    Minigame,
    End,
    Postgame
}
