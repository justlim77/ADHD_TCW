using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Is Action Done?
    public static bool isCleanShelfDone = false;
    public static bool isBagDone = false;

    //Current Bar Level - 0 = Game Over
    public static float lvlPositivity = 2;
    public static float lvlCleanliness = 2; 
    public static float lvlStamina = 5; 

    //AI Progress Reward
    public static int[] gameReward = { 0, 0, 0, 0, 0, 0, 0, 0 };

    public static bool isGamePause = false;
    public static bool isTempPause = false;
    public static bool hasDayStarted = false;
    public static bool isInScenario = false;
    public static int dayScene = 1;

    public static float gameTime = 0;
    public static float gameHour = 0;

    public Button btnPause;
    public Button btnSettings;
    public Slider sliNegative;
    public Slider sliMessiness;
    public Slider sliTiredness;
    public SettingSequence gameOver;

    public static float maxBar = 2;
    public static float maxStamina = 5;

    static Button btnPause_;
    static Button btnSettings_;
    static Slider sliNegative_;
    static Slider sliMessiness_;
    static Slider sliTiredness_;
    static SettingSequence gameOver_;

    void OnDestroy()
    {
        ResetActionCheck();
        ResetDay();
        isTempPause = false;
        isInScenario = false;
        dayScene = 1;

        lvlPositivity = 2;
        lvlCleanliness = 2;
        lvlStamina = 5;

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

    void ResetDay()
    {
        gameTime = 0;
        gameHour = 0;
        hasDayStarted = false;
    }

    void CheckIsActionDone()
    {
        if (!isCleanShelfDone)
            lvlCleanliness--;

        if (!isBagDone)
            lvlPositivity--;

        UpdateMessinessBar();
        UpdateNegativeBar();
    }

    void Awake()
    {
        btnPause_ = btnPause;
        btnSettings_ = btnSettings;
        sliNegative_ = sliNegative;
        sliMessiness_ = sliMessiness;
        sliTiredness_ = sliTiredness;
        gameOver_ = gameOver;
    }

    void ResetActionCheck()
    {
        isCleanShelfDone = false;
        isBagDone = false;
    }

    public static void CheckifGameOver()
    {
        if ((lvlCleanliness <= 0) || (lvlPositivity <= 0))
            gameOver_.PauseGame(true);
    }

    public static void UpdateMessinessBar()
    {
        sliMessiness_.value = lvlCleanliness / maxBar;
    }

    public static void UpdateNegativeBar()
    {
        sliNegative_.value = lvlPositivity / maxBar;
    }

    public static void UpdateStaminaBar()
    {
        sliTiredness_.value = lvlStamina / maxStamina;
    }

    public static void SetInteractable(bool isInteractable)
    {
        btnPause_.interactable = isInteractable;
        btnSettings_.interactable = isInteractable;
    }

    public void SetInteractableWithoutScript(bool isInteractable)
    {
        btnPause.interactable = isInteractable;
        btnSettings.interactable = isInteractable;
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
}
